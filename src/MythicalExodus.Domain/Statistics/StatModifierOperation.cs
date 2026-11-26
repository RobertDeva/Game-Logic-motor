namespace MythicalExodus.Domain.Statistics;

/// <summary>
/// Declares the intended operation of a statistic modifier.
/// Evaluation order and stacking policy are deliberately owned by a future domain policy.
/// </summary>
public enum StatModifierOperation
{
    Flat = 1,
    AdditivePercentage = 2,
    Multiplier = 3,
    Override = 4
}
