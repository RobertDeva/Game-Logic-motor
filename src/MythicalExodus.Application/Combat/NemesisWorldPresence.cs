using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Runtime binding between one persistent Nemesis identity and its currently
/// instantiated host actor. This record is intentionally not save-game state.
/// </summary>
public sealed record NemesisWorldPresence
{
    public NemesisWorldPresence(
        EntityId nemesisId,
        EntityId actorEntityId,
        EntityId worldInstanceId,
        EntityId zoneId)
    {
        if (nemesisId.IsEmpty)
            throw new ArgumentException("A valid Nemesis identifier is required.", nameof(nemesisId));
        if (actorEntityId.IsEmpty)
            throw new ArgumentException("A valid actor identifier is required.", nameof(actorEntityId));
        if (worldInstanceId.IsEmpty)
            throw new ArgumentException("A valid world-instance identifier is required.", nameof(worldInstanceId));
        if (zoneId.IsEmpty)
            throw new ArgumentException("A valid zone identifier is required.", nameof(zoneId));

        NemesisId = nemesisId;
        ActorEntityId = actorEntityId;
        WorldInstanceId = worldInstanceId;
        ZoneId = zoneId;
    }

    public EntityId NemesisId { get; }
    public EntityId ActorEntityId { get; }
    public EntityId WorldInstanceId { get; }
    public EntityId ZoneId { get; }
}

/// <summary>
/// Runtime-only presence index. Implementations must serialize registration and
/// removal so one Nemesis cannot be instantiated twice in the same registry scope.
/// </summary>
public interface INemesisWorldPresenceRegistry
{
    NemesisWorldPresence? Find(EntityId nemesisId);
    IReadOnlyCollection<NemesisWorldPresence> FindAll();

    /// <summary>
    /// Registers a presence, returning true for a new registration or an exact replay.
    /// Returns false if either identity is already bound to a different presence.
    /// </summary>
    bool TryRegister(NemesisWorldPresence presence);

    /// <summary>Removes only the registration owned by the supplied actor instance.</summary>
    bool TryUnregister(EntityId nemesisId, EntityId actorEntityId);
}

/// <summary>
/// Thread-safe, process-local presence registry. It is rebuilt by the world host
/// after startup and does not provide cross-process or distributed exclusion.
/// </summary>
public sealed class InMemoryNemesisWorldPresenceRegistry : INemesisWorldPresenceRegistry
{
    private readonly object _gate = new();
    private readonly Dictionary<EntityId, NemesisWorldPresence> _byNemesis = [];

    public NemesisWorldPresence? Find(EntityId nemesisId)
    {
        lock (_gate)
            return _byNemesis.GetValueOrDefault(nemesisId);
    }

    public IReadOnlyCollection<NemesisWorldPresence> FindAll()
    {
        lock (_gate)
            return Array.AsReadOnly(_byNemesis.Values
                .OrderBy(presence => presence.NemesisId.Value)
                .ToArray());
    }

    public bool TryRegister(NemesisWorldPresence presence)
    {
        ArgumentNullException.ThrowIfNull(presence);

        lock (_gate)
        {
            if (_byNemesis.TryGetValue(presence.NemesisId, out var existing))
                return existing == presence;

            if (_byNemesis.Values.Any(item => item.ActorEntityId == presence.ActorEntityId))
                return false;

            _byNemesis.Add(presence.NemesisId, presence);
            return true;
        }
    }

    public bool TryUnregister(EntityId nemesisId, EntityId actorEntityId)
    {
        if (nemesisId.IsEmpty)
            throw new ArgumentException("A valid Nemesis identifier is required.", nameof(nemesisId));
        if (actorEntityId.IsEmpty)
            throw new ArgumentException("A valid actor identifier is required.", nameof(actorEntityId));

        lock (_gate)
        {
            if (!_byNemesis.TryGetValue(nemesisId, out var current) ||
                current.ActorEntityId != actorEntityId)
                return false;

            return _byNemesis.Remove(nemesisId);
        }
    }
}
