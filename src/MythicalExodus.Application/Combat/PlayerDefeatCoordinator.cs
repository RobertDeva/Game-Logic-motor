using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Coordinates an already-authoritative player defeat. Nemesis victory credit
/// is persisted before the player's resurrection decision is opened.
/// </summary>
public sealed class PlayerDefeatCoordinator
{
    private readonly INemesisProgressionRepository _nemesisRepository;

    public PlayerDefeatCoordinator(INemesisProgressionRepository nemesisRepository)
    {
        ArgumentNullException.ThrowIfNull(nemesisRepository);
        _nemesisRepository = nemesisRepository;
    }

    public Result<PlayerDefeatProcessingResult> Process(
        PlayerDefeatOutcome outcome,
        PlayerResurrectionState resurrectionState)
    {
        ArgumentNullException.ThrowIfNull(outcome);
        ArgumentNullException.ThrowIfNull(resurrectionState);

        NemesisProgressionUpdate? nemesisUpdate = null;
        var nemesisCreated = false;

        if (outcome.NemesisCredit is { } credit)
        {
            var progression = RecordNemesisVictory(outcome.EncounterId, credit);
            if (progression.IsFailure)
                return Result<PlayerDefeatProcessingResult>.Failure(progression.Error!);

            nemesisUpdate = progression.Value.Update;
            nemesisCreated = progression.Value.Created;
        }

        var lethalTransition = resurrectionState.TryBeginLethalEvent();
        if (lethalTransition.IsFailure)
            return Result<PlayerDefeatProcessingResult>.Failure(lethalTransition.Error!);

        return Result<PlayerDefeatProcessingResult>.Success(new PlayerDefeatProcessingResult(
            lethalTransition.Value,
            nemesisUpdate,
            nemesisCreated));
    }

    private Result<(NemesisProgressionUpdate Update, bool Created)> RecordNemesisVictory(
        EntityId encounterId,
        NemesisDefeatCredit credit)
    {
        var existing = _nemesisRepository.Find(credit.EnemyId);
        if (existing is null)
        {
            var created = NemesisProgressionState.Create(
                credit.EnemyId,
                credit.OriginalCategory,
                encounterId);

            if (_nemesisRepository.TryAdd(created))
            {
                return Result<(NemesisProgressionUpdate, bool)>.Success((
                    new NemesisProgressionUpdate(
                        created,
                        NemesisProgressionUpdateStatus.NemesisCreated,
                        created.GetRequiredGrowthMarksForNextRank()),
                    true));
            }

            // Another concurrent result may have created this Nemesis. Reload
            // and process the same encounter; its creation ID makes this idempotent.
            existing = _nemesisRepository.Find(credit.EnemyId);
            if (existing is null)
            {
                return Failure<(NemesisProgressionUpdate, bool)>(
                    "nemesis.persistence.concurrent_create",
                    "The Nemesis record changed during creation; retry the same defeat outcome.");
            }
        }

        if (existing.Category != credit.OriginalCategory)
        {
            return Failure<(NemesisProgressionUpdate, bool)>(
                "nemesis.category_mismatch",
                "The credited enemy's original category does not match its persistent Nemesis record.");
        }

        var update = existing.RecordQualifyingDefeat(encounterId);
        if (update.Status == NemesisProgressionUpdateStatus.DuplicateEncounter ||
            update.Status == NemesisProgressionUpdateStatus.RankCapReached)
        {
            return Result<(NemesisProgressionUpdate, bool)>.Success((update, false));
        }

        if (!_nemesisRepository.TryUpdate(existing, update.State))
        {
            return Failure<(NemesisProgressionUpdate, bool)>(
                "nemesis.persistence.concurrent_update",
                "The Nemesis record changed while applying this defeat; retry the same defeat outcome.");
        }

        return Result<(NemesisProgressionUpdate, bool)>.Success((update, false));
    }

    private static Result<T> Failure<T>(string code, string message) =>
        Result<T>.Failure(new GameError(code, message));
}
