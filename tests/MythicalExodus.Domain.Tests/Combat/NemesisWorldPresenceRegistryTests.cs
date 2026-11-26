using MythicalExodus.Application.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class NemesisWorldPresenceRegistryTests
{
    [Fact]
    public void Registering_the_same_presence_twice_is_idempotent()
    {
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var presence = CreatePresence();

        Assert.True(registry.TryRegister(presence));
        Assert.True(registry.TryRegister(presence));
        Assert.Same(presence, registry.Find(presence.NemesisId));
        Assert.Single(registry.FindAll());
    }

    [Fact]
    public void Different_actor_or_zone_cannot_register_for_an_already_present_nemesis()
    {
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var first = CreatePresence();
        var second = new NemesisWorldPresence(
            first.NemesisId, EntityId.New(), first.WorldInstanceId, EntityId.New());

        Assert.True(registry.TryRegister(first));
        Assert.False(registry.TryRegister(second));
        Assert.Same(first, registry.Find(first.NemesisId));
    }

    [Fact]
    public void One_actor_cannot_be_bound_to_multiple_nemesis_identities()
    {
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var first = CreatePresence();
        var second = new NemesisWorldPresence(
            EntityId.New(), first.ActorEntityId, first.WorldInstanceId, first.ZoneId);

        Assert.True(registry.TryRegister(first));
        Assert.False(registry.TryRegister(second));
        Assert.Single(registry.FindAll());
    }

    [Fact]
    public void Stale_actor_cannot_unregister_a_replacement_instance()
    {
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var oldPresence = CreatePresence();
        var replacement = new NemesisWorldPresence(
            oldPresence.NemesisId, EntityId.New(), EntityId.New(), EntityId.New());

        Assert.True(registry.TryRegister(oldPresence));
        Assert.True(registry.TryUnregister(oldPresence.NemesisId, oldPresence.ActorEntityId));
        Assert.True(registry.TryRegister(replacement));

        Assert.False(registry.TryUnregister(oldPresence.NemesisId, oldPresence.ActorEntityId));
        Assert.Same(replacement, registry.Find(oldPresence.NemesisId));
    }

    [Fact]
    public void Unloading_an_actor_allows_the_same_nemesis_to_register_in_another_zone()
    {
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var first = CreatePresence();
        var nextZonePresence = new NemesisWorldPresence(
            first.NemesisId, EntityId.New(), first.WorldInstanceId, EntityId.New());

        Assert.True(registry.TryRegister(first));
        Assert.True(registry.TryUnregister(first.NemesisId, first.ActorEntityId));
        Assert.True(registry.TryRegister(nextZonePresence));
        Assert.Equal(nextZonePresence, registry.Find(first.NemesisId));
    }

    private static NemesisWorldPresence CreatePresence() =>
        new(EntityId.New(), EntityId.New(), EntityId.New(), EntityId.New());
}
