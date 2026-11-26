using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Content-driven effects of a perfect deflect. Window timing is deliberately
/// excluded until the game adopts its canonical combat-time unit.
/// </summary>
public sealed record PerfectDeflectProfile
{
    public PerfectDeflectProfile(
        decimal incomingVitalityDamageMultiplier,
        decimal incomingPosturePressureMultiplier,
        decimal weaponBasePostureReduction,
        decimal attackPostureMultiplier,
        decimal skillPostureBonus,
        decimal masteryPostureBonus,
        decimal maximumAttackerPostureReduction)
    {
        ValidateMitigation(incomingVitalityDamageMultiplier, nameof(incomingVitalityDamageMultiplier));
        ValidateMitigation(incomingPosturePressureMultiplier, nameof(incomingPosturePressureMultiplier));
        ArgumentOutOfRangeException.ThrowIfNegative(weaponBasePostureReduction);
        ArgumentOutOfRangeException.ThrowIfNegative(attackPostureMultiplier);
        ArgumentOutOfRangeException.ThrowIfNegative(skillPostureBonus);
        ArgumentOutOfRangeException.ThrowIfNegative(masteryPostureBonus);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumAttackerPostureReduction);

        IncomingVitalityDamageMultiplier = incomingVitalityDamageMultiplier;
        IncomingPosturePressureMultiplier = incomingPosturePressureMultiplier;
        WeaponBasePostureReduction = weaponBasePostureReduction;
        AttackPostureMultiplier = attackPostureMultiplier;
        SkillPostureBonus = skillPostureBonus;
        MasteryPostureBonus = masteryPostureBonus;
        MaximumAttackerPostureReduction = maximumAttackerPostureReduction;
    }

    public decimal IncomingVitalityDamageMultiplier { get; }
    public decimal IncomingPosturePressureMultiplier { get; }
    public decimal WeaponBasePostureReduction { get; }
    public decimal AttackPostureMultiplier { get; }
    public decimal SkillPostureBonus { get; }
    public decimal MasteryPostureBonus { get; }
    public decimal MaximumAttackerPostureReduction { get; }

    public decimal CalculateAttackerPostureReduction()
    {
        var requested = (WeaponBasePostureReduction * AttackPostureMultiplier) +
                        SkillPostureBonus +
                        MasteryPostureBonus;
        return Math.Min(requested, MaximumAttackerPostureReduction);
    }

    private static void ValidateMitigation(decimal value, string parameterName)
    {
        if (value < 0m || value > 1m)
            throw new ArgumentOutOfRangeException(parameterName, "Mitigation multipliers must be between zero and one.");
    }
}

public sealed record PerfectDeflectInput(
    bool WasWithinPerfectWindow,
    CombatantResourceState DefenderResources,
    CombatantResourceState AttackerResources,
    CombatResourceImpact IncomingImpact,
    PerfectDeflectProfile Profile);

public sealed record PerfectDeflectResult(
    CombatantResourceState DefenderResources,
    CombatantResourceState AttackerResources,
    decimal IncomingVitalityDamageApplied,
    decimal IncomingPosturePressureApplied,
    decimal AttackerPostureReductionApplied);

/// <summary>
/// Resolves the resource consequences of a perfect deflect in either combat
/// direction. The caller determines timing and supplies the appropriate
/// defender-weapon/attacker-move profile.
/// </summary>
public sealed class PerfectDeflectResolver
{
    private readonly CombatResourceImpactApplier _impactApplier = new();

    public Result<PerfectDeflectResult> Resolve(PerfectDeflectInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(input.DefenderResources);
        ArgumentNullException.ThrowIfNull(input.AttackerResources);
        ArgumentNullException.ThrowIfNull(input.IncomingImpact);
        ArgumentNullException.ThrowIfNull(input.Profile);

        if (!input.WasWithinPerfectWindow)
        {
            return Result<PerfectDeflectResult>.Failure(new GameError(
                "combat.deflect.outside_perfect_window",
                "The response did not occur inside the perfect-deflect window."));
        }

        var mitigatedImpact = new CombatResourceImpact(
            input.IncomingImpact.VitalityDamage * input.Profile.IncomingVitalityDamageMultiplier,
            input.IncomingImpact.PosturePressure * input.Profile.IncomingPosturePressureMultiplier);
        var defenderUpdate = _impactApplier.Apply(input.DefenderResources, mitigatedImpact);
        if (defenderUpdate.IsFailure)
            return Result<PerfectDeflectResult>.Failure(defenderUpdate.Error!);

        var requestedReduction = input.Profile.CalculateAttackerPostureReduction();
        var appliedReduction = Math.Min(requestedReduction, input.AttackerResources.Posture.Current);
        var attackerUpdate = input.AttackerResources.TrySetPostureValue(
            input.AttackerResources.Posture.Current - appliedReduction);
        if (attackerUpdate.IsFailure)
            return Result<PerfectDeflectResult>.Failure(attackerUpdate.Error!);

        return Result<PerfectDeflectResult>.Success(new PerfectDeflectResult(
            defenderUpdate.Value.State,
            attackerUpdate.Value,
            defenderUpdate.Value.VitalityDamageApplied,
            defenderUpdate.Value.PosturePressureApplied,
            appliedReduction));
    }
}
