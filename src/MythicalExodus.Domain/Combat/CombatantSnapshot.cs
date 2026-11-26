using MythicalExodus.Domain.Entities;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Immutable combat-facing view of an entity. It is a supplied snapshot,
/// not a second owner of entity identity or mutable gameplay state.
/// </summary>
public sealed record CombatantSnapshot
{
    public CombatantSnapshot(
        EntityId entityId,
        EntityLifecycleState lifecycleState,
        CombatantResourceState resources)
    {
        if (entityId.IsEmpty)
            throw new ArgumentException("A combatant snapshot requires a valid entity identifier.", nameof(entityId));
        if (!Enum.IsDefined(lifecycleState))
            throw new ArgumentOutOfRangeException(nameof(lifecycleState));
        ArgumentNullException.ThrowIfNull(resources);

        EntityId = entityId;
        LifecycleState = lifecycleState;
        Resources = resources;
    }

    public EntityId EntityId { get; }
    public EntityLifecycleState LifecycleState { get; }
    public bool IsActive => LifecycleState == EntityLifecycleState.Active;
    public CombatantResourceState Resources { get; }
}
