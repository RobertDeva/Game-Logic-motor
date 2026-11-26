using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Selects persistent Nemesis identities that may be considered for world spawning.
/// It does not choose locations, instantiate enemy actors, or track host-owned world presence.
/// </summary>
public sealed class NemesisWorldPopulationCoordinator
{
    private readonly INemesisProgressionRepository _repository;

    public NemesisWorldPopulationCoordinator(INemesisProgressionRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <summary>
    /// Returns active Nemeses absent from the host's current world population.
    /// A recovered Nemesis becomes eligible again with its original persistent identity.
    /// Kneeling and permanently retired Nemeses are never returned.
    /// </summary>
    public IReadOnlyList<NemesisProgressionState> FindSpawnCandidates(
        IReadOnlySet<EntityId> currentlyPresentNemesisIds)
    {
        ArgumentNullException.ThrowIfNull(currentlyPresentNemesisIds);

        return _repository.FindAll()
            .Where(state =>
                state.LifecycleStatus == NemesisLifecycleStatus.Active &&
                !currentlyPresentNemesisIds.Contains(state.NemesisId))
            .OrderBy(state => state.NemesisId.Value)
            .ToArray();
    }
}
