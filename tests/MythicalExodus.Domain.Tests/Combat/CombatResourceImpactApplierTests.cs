using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class CombatResourceImpactApplierTests
{
    private readonly CombatResourceImpactApplier _applier = new();

    [Fact]
    public void Impact_applies_vitality_damage_and_posture_pressure_independently()
    {
        var initial = CreateState([100m], vitalityCurrent: 80m, postureCurrent: 10m);
        var impact = new CombatResourceImpact(vitalityDamage: 25m, posturePressure: 15m);

        var result = _applier.Apply(initial, impact);

        Assert.True(result.IsSuccess);
        Assert.Equal(55m, result.Value.State.Vitality.CurrentBar!.Current);
        Assert.Equal(25m, result.Value.VitalityDamageApplied);
        Assert.Equal(0m, result.Value.VitalityDamageOverflow);
        Assert.Equal(25m, result.Value.State.Posture.Current);
        Assert.Equal(15m, result.Value.PosturePressureApplied);
        Assert.Equal(0, result.Value.VitalityBarsDepleted);
        Assert.Equal(80m, initial.Vitality.CurrentBar!.Current);
        Assert.Equal(10m, initial.Posture.Current);
    }

    [Fact]
    public void Damage_overflow_depletes_multiple_bars_in_order()
    {
        var initial = CreateState([100m, 80m, 60m], vitalityCurrent: 30m, postureCurrent: 0m);

        var result = _applier.Apply(initial, new CombatResourceImpact(110m, 0m));

        Assert.True(result.IsSuccess);
        Assert.Equal(110m, result.Value.VitalityDamageApplied);
        Assert.Equal(0m, result.Value.VitalityDamageOverflow);
        Assert.Equal(2, result.Value.VitalityBarsDepleted);
        Assert.Equal(60m, result.Value.State.Vitality.CurrentBar!.Maximum);
        Assert.Equal(60m, result.Value.State.Vitality.CurrentBar.Current);
        Assert.Equal(30m, initial.Vitality.CurrentBar!.Current);
    }

    [Fact]
    public void Damage_beyond_all_remaining_vitality_is_reported_as_overflow()
    {
        var initial = CreateState([40m, 30m], vitalityCurrent: 10m, postureCurrent: 0m);

        var result = _applier.Apply(initial, new CombatResourceImpact(100m, 0m));

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.VitalityDepleted);
        Assert.Equal(40m, result.Value.VitalityDamageApplied);
        Assert.Equal(60m, result.Value.VitalityDamageOverflow);
        Assert.Equal(2, result.Value.VitalityBarsDepleted);
    }

    [Fact]
    public void Posture_pressure_is_capped_at_remaining_capacity()
    {
        var initial = CreateState([100m], vitalityCurrent: 100m, postureCurrent: 40m);

        var result = _applier.Apply(initial, new CombatResourceImpact(0m, 25m));

        Assert.True(result.IsSuccess);
        Assert.Equal(50m, result.Value.State.Posture.Current);
        Assert.Equal(10m, result.Value.PosturePressureApplied);
        Assert.True(result.Value.PostureBroken);
    }

    [Fact]
    public void Zero_impact_preserves_resource_values_and_depletes_no_bars()
    {
        var initial = CreateState([100m, 50m], vitalityCurrent: 75m, postureCurrent: 12m);

        var result = _applier.Apply(initial, new CombatResourceImpact(0m, 0m));

        Assert.True(result.IsSuccess);
        Assert.Equal(75m, result.Value.State.Vitality.CurrentBar!.Current);
        Assert.Equal(12m, result.Value.State.Posture.Current);
        Assert.Equal(0, result.Value.VitalityBarsDepleted);
    }

    [Fact]
    public void Impact_source_is_preserved_in_the_resolved_result()
    {
        var sourceId = EntityId.New();
        var initial = CreateState([10m], vitalityCurrent: 10m, postureCurrent: 0m);

        var result = _applier.Apply(initial, new CombatResourceImpact(10m, 0m, sourceId));

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.VitalityDepleted);
        Assert.Equal(sourceId, result.Value.SourceEntityId);
    }

    [Fact]
    public void Impact_rejects_an_empty_source_identifier()
    {
        Assert.Throws<ArgumentException>(() => new CombatResourceImpact(1m, 0m, default(EntityId)));
    }

    [Fact]
    public void Impact_constructor_rejects_negative_magnitudes()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CombatResourceImpact(-1m, 0m));
        Assert.Throws<ArgumentOutOfRangeException>(() => new CombatResourceImpact(0m, -1m));
    }

    private static CombatantResourceState CreateState(
        decimal[] maxima,
        decimal vitalityCurrent,
        decimal postureCurrent)
    {
        var bars = maxima.Select((maximum, index) =>
            new ResourcePool(new ResourceId($"vitality-{index}"),
                index == 0 ? vitalityCurrent : maximum, maximum));

        return new CombatantResourceState(
            new VitalityTrack(bars),
            new ResourcePool(new ResourceId("posture"), postureCurrent, 50m));
    }
}
