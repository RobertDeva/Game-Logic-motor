using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Persistence boundary for Nemesis progression. TryAdd and TryUpdate must be
/// atomic; TryUpdate is a compare-and-swap against the expected state instance.
/// </summary>
public interface INemesisProgressionRepository
{
    NemesisProgressionState? Find(EntityId nemesisId);

    /// <summary>Returns a stable snapshot of all persistent Nemesis identities.</summary>
    IReadOnlyCollection<NemesisProgressionState> FindAll();

    bool TryAdd(NemesisProgressionState state);

    bool TryUpdate(NemesisProgressionState expectedState, NemesisProgressionState updatedState);
}
