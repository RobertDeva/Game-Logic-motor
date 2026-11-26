namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

public sealed record PosturePressureInput
{
    public PosturePressureInput(
        decimal basePressure,
        decimal scaledPressure,
        decimal attackMultiplier = 1m,
        decimal responseMultiplier = 1m,
        decimal affinityMultiplier = 1m,
        decimal postureResistance = 0m,
        decimal postureMultiplier = 1m)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(basePressure);
        ArgumentOutOfRangeException.ThrowIfNegative(scaledPressure);
        ArgumentOutOfRangeException.ThrowIfNegative(attackMultiplier);
        ArgumentOutOfRangeException.ThrowIfNegative(responseMultiplier);
        ArgumentOutOfRangeException.ThrowIfNegative(affinityMultiplier);
        if (postureResistance < 0m || postureResistance > 1m)
            throw new ArgumentOutOfRangeException(nameof(postureResistance));
        ArgumentOutOfRangeException.ThrowIfNegative(postureMultiplier);

        BasePressure = basePressure;
        ScaledPressure = scaledPressure;
        AttackMultiplier = attackMultiplier;
        ResponseMultiplier = responseMultiplier;
        AffinityMultiplier = affinityMultiplier;
        PostureResistance = postureResistance;
        PostureMultiplier = postureMultiplier;
    }

    public decimal BasePressure { get; }
    public decimal ScaledPressure { get; }
    public decimal AttackMultiplier { get; }
    public decimal ResponseMultiplier { get; }
    public decimal AffinityMultiplier { get; }
    public decimal PostureResistance { get; }
    public decimal PostureMultiplier { get; }
}

public sealed record PosturePressureCalculationResult(
    decimal BaseAndScaledPressure,
    decimal PressureBeforeResistance,
    decimal PressureAfterResistance,
    decimal FinalPressure);

/// <summary>Implements the documented posture pressure equation without inventing scaling coefficients.</summary>
public sealed class PosturePressureCalculator
{
    public Result<PosturePressureCalculationResult> Calculate(PosturePressureInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        var baseAndScaled = input.BasePressure + input.ScaledPressure;
        var pressureBeforeResistance = baseAndScaled *
                                      input.AttackMultiplier *
                                      input.ResponseMultiplier *
                                      input.AffinityMultiplier;
        var pressureAfterResistance = pressureBeforeResistance * (1m - input.PostureResistance);
        var finalPressure = pressureAfterResistance * input.PostureMultiplier;

        return Result<PosturePressureCalculationResult>.Success(new PosturePressureCalculationResult(
            baseAndScaled,
            pressureBeforeResistance,
            pressureAfterResistance,
            finalPressure));
    }
}

public readonly record struct PostureRecoveryCurvePoint
{
    public PostureRecoveryCurvePoint(decimal vitalityRatio, decimal recoveryMultiplier)
    {
        if (vitalityRatio < 0m || vitalityRatio > 1m)
            throw new ArgumentOutOfRangeException(nameof(vitalityRatio));
        ArgumentOutOfRangeException.ThrowIfNegative(recoveryMultiplier);
        VitalityRatio = vitalityRatio;
        RecoveryMultiplier = recoveryMultiplier;
    }

    public decimal VitalityRatio { get; }
    public decimal RecoveryMultiplier { get; }
}

/// <summary>
/// Configurable piecewise-linear multiplier curve. Points must cover 0..1 and
/// be non-decreasing so recovery never accelerates as Vitality falls.
/// </summary>
public sealed class PostureRecoveryCurve
{
    private readonly PostureRecoveryCurvePoint[] _points;
    private readonly IReadOnlyList<PostureRecoveryCurvePoint> _readOnlyPoints;

    public PostureRecoveryCurve(IEnumerable<PostureRecoveryCurvePoint> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        _points = points.ToArray();
        if (_points.Length < 2)
            throw new ArgumentException("A posture recovery curve requires at least two points.", nameof(points));
        if (_points[0].VitalityRatio != 0m || _points[^1].VitalityRatio != 1m)
            throw new ArgumentException("A posture recovery curve must define both vitality ratio endpoints 0 and 1.", nameof(points));

        for (var index = 1; index < _points.Length; index++)
        {
            if (_points[index].VitalityRatio <= _points[index - 1].VitalityRatio)
                throw new ArgumentException("Vitality ratios must be strictly increasing.", nameof(points));
            if (_points[index].RecoveryMultiplier < _points[index - 1].RecoveryMultiplier)
                throw new ArgumentException("Recovery multipliers must be non-decreasing with Vitality.", nameof(points));
        }

        _readOnlyPoints = Array.AsReadOnly(_points);
    }

    public IReadOnlyList<PostureRecoveryCurvePoint> Points => _readOnlyPoints;

    public decimal Evaluate(decimal vitalityRatio)
    {
        if (vitalityRatio < 0m || vitalityRatio > 1m)
            throw new ArgumentOutOfRangeException(nameof(vitalityRatio));

        for (var index = 1; index < _points.Length; index++)
        {
            var right = _points[index];
            if (vitalityRatio > right.VitalityRatio)
                continue;

            var left = _points[index - 1];
            var segmentRatio = (vitalityRatio - left.VitalityRatio) /
                               (right.VitalityRatio - left.VitalityRatio);
            return left.RecoveryMultiplier +
                   ((right.RecoveryMultiplier - left.RecoveryMultiplier) * segmentRatio);
        }

        return _points[^1].RecoveryMultiplier;
    }
}

public sealed record PostureRecoveryCalculationResult(
    decimal VitalityRatio,
    decimal RecoveryMultiplier,
    decimal BaseRecoveryPerTick,
    bool PressureReceivedThisTick,
    decimal RecoveryAmount);

public sealed class PostureRecoveryCalculator
{
    private readonly PostureRecoveryCurve _curve;

    public PostureRecoveryCalculator(PostureRecoveryCurve curve)
    {
        ArgumentNullException.ThrowIfNull(curve);
        _curve = curve;
    }

    public Result<PostureRecoveryCalculationResult> Calculate(
        decimal currentVitality,
        decimal maximumVitality,
        decimal baseRecoveryPerTick,
        bool pressureReceivedThisTick)
    {
        if (maximumVitality <= 0m)
            return Failure("combat.posture_recovery.invalid_maximum_vitality", "Maximum Vitality must be greater than zero.");
        if (currentVitality < 0m || currentVitality > maximumVitality)
            return Failure("combat.posture_recovery.vitality_out_of_range", "Current Vitality must be between zero and its maximum.");
        if (baseRecoveryPerTick < 0m)
            return Failure("combat.posture_recovery.negative_base_rate", "Base posture recovery cannot be negative.");

        var vitalityRatio = currentVitality / maximumVitality;
        var multiplier = _curve.Evaluate(vitalityRatio);
        var recovery = pressureReceivedThisTick ? 0m : baseRecoveryPerTick * multiplier;

        return Result<PostureRecoveryCalculationResult>.Success(new PostureRecoveryCalculationResult(
            vitalityRatio,
            multiplier,
            baseRecoveryPerTick,
            pressureReceivedThisTick,
            recovery));
    }

    private static Result<PostureRecoveryCalculationResult> Failure(string code, string message) =>
        Result<PostureRecoveryCalculationResult>.Failure(new GameError(code, message));
}

public sealed record PostureRecoveryApplicationResult(
    CombatantResourceState State,
    decimal RecoveryRequested,
    decimal RecoveryApplied);

public sealed class PostureRecoveryApplier
{
    public Result<PostureRecoveryApplicationResult> Apply(
        CombatantResourceState currentState,
        decimal recoveryAmount)
    {
        ArgumentNullException.ThrowIfNull(currentState);
        if (recoveryAmount < 0m)
        {
            return Result<PostureRecoveryApplicationResult>.Failure(new GameError(
                "combat.posture_recovery.negative_amount",
                "Posture recovery amount cannot be negative."));
        }

        var applied = Math.Min(currentState.Posture.Current, recoveryAmount);
        var updated = currentState.TrySetPostureValue(currentState.Posture.Current - applied);
        if (updated.IsFailure)
            return Result<PostureRecoveryApplicationResult>.Failure(updated.Error!);

        return Result<PostureRecoveryApplicationResult>.Success(new PostureRecoveryApplicationResult(
            updated.Value,
            recoveryAmount,
            applied));
    }
}
