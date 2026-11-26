namespace MythicalExodus.Domain.Combat;

/// <summary>Explicit immutable actor/target context for combat preconditions.</summary>
public sealed record CombatActionContext
{
    public CombatActionContext(CombatantSnapshot actor, CombatantSnapshot? target)
    {
        ArgumentNullException.ThrowIfNull(actor);
        Actor = actor;
        Target = target;
    }

    public CombatantSnapshot Actor { get; }
    public CombatantSnapshot? Target { get; }
}
