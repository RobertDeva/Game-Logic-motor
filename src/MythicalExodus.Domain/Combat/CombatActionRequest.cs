using MythicalExodus.Domain.Actions;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>An immutable request to attempt a combat action.</summary>
public sealed record CombatActionRequest
{
    public CombatActionRequest(ActionId actionId, EntityId actorId, EntityId? targetId = null)
    {
        if (!actionId.IsValid)
            throw new ArgumentException("A valid action identifier is required.", nameof(actionId));
        if (actorId.IsEmpty)
            throw new ArgumentException("An action actor identifier is required.", nameof(actorId));
        if (targetId is { IsEmpty: true })
            throw new ArgumentException("A target identifier cannot be empty.", nameof(targetId));

        ActionId = actionId;
        ActorId = actorId;
        TargetId = targetId;
    }

    public ActionId ActionId { get; }
    public EntityId ActorId { get; }
    public EntityId? TargetId { get; }
}
