using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Entities;

/// <summary>
/// Base for entities with stable identity and an explicit simulation lifecycle.
/// Gameplay states such as death, stagger, or defeat belong to specialized domain models.
/// </summary>
public abstract class DomainEntity : IEquatable<DomainEntity>
{
    protected DomainEntity(EntityId id)
    {
        if (id.Value == Guid.Empty)
            throw new ArgumentException("An entity must have a non-empty identifier.", nameof(id));

        Id = id;
    }

    public EntityId Id { get; }
    public EntityLifecycleState LifecycleState { get; private set; } = EntityLifecycleState.Active;
    public bool IsActive => LifecycleState == EntityLifecycleState.Active;

    /// <summary>Removes the entity from simulation participation. Repeated calls are harmless.</summary>
    public bool Remove()
    {
        if (!IsActive)
            return false;

        LifecycleState = EntityLifecycleState.Removed;
        return true;
    }

    public bool Equals(DomainEntity? other) =>
        other is not null && Id == other.Id;

    public override bool Equals(object? obj) =>
        obj is DomainEntity other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(DomainEntity? left, DomainEntity? right) =>
        Equals(left, right);

    public static bool operator !=(DomainEntity? left, DomainEntity? right) =>
        !Equals(left, right);
}
