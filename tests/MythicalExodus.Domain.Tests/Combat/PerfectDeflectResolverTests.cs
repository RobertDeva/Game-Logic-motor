using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class PerfectDeflectResolverTests
{
    private static CombatantResourceState CreateState(string prefix, decimal vitality, decimal posture) =>
        new(
            new VitalityTrack(new[] { new ResourcePool(new ResourceId($"{prefix}-vitality"), vitality, 100m) }),
            new ResourcePool(new ResourceId($"{prefix}-posture"), posture, 100m));

    private static PerfectDeflectProfile CreateProfile(
        decimal vitalityMultiplier = 0m,
        decimal pressureMultiplier = 0m,
        decimal weaponReduction = 20m,
        decimal attackMultiplier = 1m,
        decimal skillBonus = 0m,
        decimal masteryBonus = 0m,
        decimal maximumReduction = 100m) =>
        new(vitalityMultiplier, pressureMultiplier, weaponReduction, attackMultiplier,
            skillBonus, masteryBonus, maximumReduction);

    [Fact]
    public void Perfect_deflect_neutralizes_incoming_attack_and_reduces_attacker_posture()
    {
        var defender = CreateState("defender", 100m, 25m);
        var attacker = CreateState("attacker", 100m, 60m);

        var result = new PerfectDeflectResolver().Resolve(new PerfectDeflectInput(
            true, defender, attacker, new CombatResourceImpact(40m, 30m), CreateProfile()));

        Assert.True(result.IsSuccess);
        Assert.Equal(100m, result.Value.DefenderResources.Vitality.CurrentBar!.Current);
        Assert.Equal(25m, result.Value.DefenderResources.Posture.Current);
        Assert.Equal(40m, result.Value.AttackerResources.Posture.Current);
        Assert.Equal(20m, result.Value.AttackerPostureReductionApplied);
        Assert.Equal(0m, result.Value.IncomingVitalityDamageApplied);
        Assert.Equal(0m, result.Value.IncomingPosturePressureApplied);
        Assert.Equal(60m, attacker.Posture.Current);
    }

    [Fact]
    public void Deflect_can_mitigate_instead_of_fully_canceling_the_attack()
    {
        var defender = CreateState("defender", 100m, 25m);
        var attacker = CreateState("attacker", 100m, 60m);

        var result = new PerfectDeflectResolver().Resolve(new PerfectDeflectInput(
            true, defender, attacker, new CombatResourceImpact(40m, 30m),
            CreateProfile(0.25m, 0.5m)));

        Assert.True(result.IsSuccess);
        Assert.Equal(90m, result.Value.DefenderResources.Vitality.CurrentBar!.Current);
        Assert.Equal(40m, result.Value.DefenderResources.Posture.Current);
        Assert.Equal(20m, result.Value.AttackerPostureReductionApplied);
    }

    [Fact]
    public void Skill_and_mastery_bonuses_are_capped_by_profile_limit()
    {
        var profile = CreateProfile(
            weaponReduction: 30m,
            attackMultiplier: 2m,
            skillBonus: 15m,
            masteryBonus: 10m,
            maximumReduction: 50m);

        Assert.Equal(50m, profile.CalculateAttackerPostureReduction());
    }

    [Fact]
    public void Posture_reduction_cannot_exceed_attacker_current_posture()
    {
        var result = new PerfectDeflectResolver().Resolve(new PerfectDeflectInput(
            true,
            CreateState("defender", 100m, 0m),
            CreateState("attacker", 100m, 5m),
            new CombatResourceImpact(0m, 0m),
            CreateProfile(weaponReduction: 20m)));

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.AttackerResources.Posture.Current);
        Assert.Equal(5m, result.Value.AttackerPostureReductionApplied);
    }

    [Fact]
    public void Response_outside_perfect_window_is_rejected()
    {
        var result = new PerfectDeflectResolver().Resolve(new PerfectDeflectInput(
            false,
            CreateState("defender", 100m, 0m),
            CreateState("attacker", 100m, 20m),
            new CombatResourceImpact(10m, 10m),
            CreateProfile()));

        Assert.True(result.IsFailure);
        Assert.Equal("combat.deflect.outside_perfect_window", result.Error!.Code);
    }

    [Fact]
    public void Profile_rejects_mitigation_outside_zero_to_one()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateProfile(vitalityMultiplier: 1.1m));
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateProfile(pressureMultiplier: -0.1m));
    }
}
