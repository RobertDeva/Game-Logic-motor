using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class CombatantResourceStateTests
{
    [Fact]
    public void Vitality_and_posture_are_stored_as_separate_resources()
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("vitality-1"), 100m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("posture"), 30m, 100m);

        var state = new CombatantResourceState(vitality, posture);

        Assert.Same(vitality, state.Vitality);
        Assert.Same(posture, state.Posture);
    }

    [Fact]
    public void Posture_cannot_share_identifier_with_vitality_bar()
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("shared"), 100m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("shared"), 20m, 100m);

        Assert.Throws<ArgumentException>(() => new CombatantResourceState(vitality, posture));
    }

    [Fact]
    public void Posture_requires_a_positive_maximum()
    {
        var vitality = new VitalityTrack([]);
        var posture = new ResourcePool(new ResourceId("posture"), 0m, 0m);

        Assert.Throws<ArgumentException>(() => new CombatantResourceState(vitality, posture));
    }
}
