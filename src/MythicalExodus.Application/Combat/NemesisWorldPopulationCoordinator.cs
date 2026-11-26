using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Selects persistent Nemesis identities that may be considered for world spawning.
/// It does not choose locations or instantiate enemy actors.
/// </summary>
public sealed class NemesisWorldPopulationCoordinator
{
    private readonly INemesisProgressionRepository _progressionRepository;
    private readonly INemesisWorldPresenceRegistry _presenceRegistry;

    public NemesisWorldPopulationCoordinator(
        INemesisProgressionRepository progressionRepository,
        INemesisWorldPresenceRegistry presenceRegistry)
    {
        ArgumentNullException.ThrowIfNull(progressionRepository);
        ArgumentNullException.ThrowIfNull(presenceRegistry);
        _progressionRepository = progressionRepository;
        _presenceRegistry = presenceRegistry;
    }

    /// <summary>
    /// Returns active Nemeses not currently bound to an actor in this registry.
    /// A recovered Nemesis becomes eligible again with its original identity.
    /// Kneeling and permanently retired Nemeses are never returned.
    /// </summary>
    public IReadOnlyList<NemesisProgressionState> FindSpawnCandidates() =>
        _progressionRepository.FindAll()
            .Where(state =>
                state.LifecycleStatus == NemesisLifecycleStatus.Active &&
                _presenceRegistry.Find(state.NemesisId) is null)
            .OrderBy(state => state.NemesisId.Value)
            .ToArray();
}
