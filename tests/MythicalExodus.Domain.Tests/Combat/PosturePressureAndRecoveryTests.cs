using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class PosturePressureAndRecoveryTests
{
    private static PostureRecoveryCurve CreateCurve() => new(new[]
    {
        new PostureRecoveryCurvePoint(0m, 0.20m),
        new PostureRecoveryCurvePoint(0.50m, 0.60m),
        new PostureRecoveryCurvePoint(1m, 1m)
    });

    [Fact]
    public void Pressure_uses_base_plus_scaling_then_multipliers_resistance_and_posture_factor()
    {
        var input = new PosturePressureInput(
            basePressure: 10m,
            scaledPressure: 5m,
            attackMultiplier: 1.2m,
            responseMultiplier: 1.5m,
            affinityMultiplier: 1.25m,
            postureResistance: 0.20m,
            postureMultiplier: 1.1m);

        var result = new PosturePressureCalculator().Calculate(input);

        Assert.True(result.IsSuccess);
        Assert.Equal(15m, result.Value.BaseAndScaledPressure);
        Assert.Equal(33.75m, result.Value.PressureBeforeResistance);
        Assert.Equal(27m, result.Value.PressureAfterResistance);
        Assert.Equal(29.7m, result.Value.FinalPressure);
    }

    [Fact]
    public void Full_posture_resistance_reduces_pressure_to_zero()
    {
        var result = new PosturePressureCalculator().Calculate(
            new PosturePressureInput(100m, 50m, postureResistance: 1m));

        Assert.Equal(0m, result.Value.FinalPressure);
    }

    [Fact]
    public void Pressure_input_rejects_resistance_outside_zero_to_one()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PosturePressureInput(10m, 0m, postureResistance: 1.01m));
    }

    [Fact]
    public void Recovery_curve_interpolates_and_covers_both_endpoints()
    {
        var curve = CreateCurve();

        Assert.Equal(0.20m, curve.Evaluate(0m));
        Assert.Equal(0.40m, curve.Evaluate(0.25m));
        Assert.Equal(0.60m, curve.Evaluate(0.50m));
        Assert.Equal(1m, curve.Evaluate(1m));
    }

    [Fact]
    public void Recovery_curve_rejects_decreasing_multiplier()
    {
        Assert.Throws<ArgumentException>(() => new PostureRecoveryCurve(new[]
        {
            new PostureRecoveryCurvePoint(0m, 0.5m),
            new PostureRecoveryCurvePoint(1m, 0.4m)
        }));
    }

    [Fact]
    public void Recovery_curve_requires_zero_and_one_vitality_endpoints()
    {
        Assert.Throws<ArgumentException>(() => new PostureRecoveryCurve(new[]
        {
            new PostureRecoveryCurvePoint(0.1m, 0.2m),
            new PostureRecoveryCurvePoint(1m, 1m)
        }));
    }

    [Fact]
    public void Recovery_is_slower_at_lower_vitality()
    {
        var calculator = new PostureRecoveryCalculator(CreateCurve());

        var low = calculator.Calculate(25m, 100m, 10m, false);
        var high = calculator.Calculate(100m, 100m, 10m, false);

        Assert.Equal(4m, low.Value.RecoveryAmount);
        Assert.Equal(10m, high.Value.RecoveryAmount);
        Assert.True(low.Value.RecoveryAmount < high.Value.RecoveryAmount);
    }

    [Fact]
    public void Recovery_stops_while_pressure_is_received()
    {
        var result = new PostureRecoveryCalculator(CreateCurve())
            .Calculate(100m, 100m, 10m, pressureReceivedThisTick: true);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.RecoveryAmount);
        Assert.True(result.Value.PressureReceivedThisTick);
    }

    [Fact]
    public void Recovery_calculation_rejects_invalid_vitality_and_rate()
    {
        var calculator = new PostureRecoveryCalculator(CreateCurve());

        Assert.True(calculator.Calculate(101m, 100m, 1m, false).IsFailure);
        Assert.True(calculator.Calculate(10m, 0m, 1m, false).IsFailure);
        Assert.True(calculator.Calculate(10m, 100m, -1m, false).IsFailure);
    }

    [Fact]
    public void Recovery_applier_reduces_posture_without_mutating_original_state()
    {
        var state = CreateCombatantState(postureCurrent: 30m);
        var result = new PostureRecoveryApplier().Apply(state, 12m);

        Assert.True(result.IsSuccess);
        Assert.Equal(18m, result.Value.State.Posture.Current);
        Assert.Equal(12m, result.Value.RecoveryApplied);
        Assert.Equal(30m, state.Posture.Current);
    }

    [Fact]
    public void Recovery_applier_never_reduces_posture_below_zero()
    {
        var state = CreateCombatantState(postureCurrent: 5m);
        var result = new PostureRecoveryApplier().Apply(state, 20m);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.State.Posture.Current);
        Assert.Equal(5m, result.Value.RecoveryApplied);
    }

    private static CombatantResourceState CreateCombatantState(decimal postureCurrent) =>
        new(
            new VitalityTrack(new[] { new ResourcePool(new ResourceId("vitality"), 50m, 100m) }),
            new ResourcePool(new ResourceId("posture"), postureCurrent, 50m));
}
