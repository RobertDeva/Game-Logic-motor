namespace MythicalExodus.Domain.Combat;

/// <summary>Stable content identifier for a recognizable player-behavior pattern.</summary>
public readonly record struct CombatPatternId
{
    public CombatPatternId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A combat pattern identifier cannot be empty.", nameof(value));
        Value = value.Trim();
    }

    public string Value { get; }
    public override string ToString() => Value;
}

public enum BossCombatTendency
{
    Defensive = 0,
    Aggressive = 1,
    Repositioning = 2
}

/// <summary>
/// A data-driven rule: after the observed pattern reaches its configured count,
/// the boss adopts the configured combat tendency.
/// </summary>
public sealed record BossAdaptationRule
{
    public BossAdaptationRule(
        CombatPatternId observedPattern,
        int minimumObservations,
        BossCombatTendency tendency,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(observedPattern.Value))
            throw new ArgumentException("A valid observed pattern is required.", nameof(observedPattern));
        if (minimumObservations < 1)
            throw new ArgumentOutOfRangeException(nameof(minimumObservations));
        if (!Enum.IsDefined(tendency))
            throw new ArgumentOutOfRangeException(nameof(tendency));
        if (priority < 0)
            throw new ArgumentOutOfRangeException(nameof(priority));

        ObservedPattern = observedPattern;
        MinimumObservations = minimumObservations;
        Tendency = tendency;
        Priority = priority;
    }

    public CombatPatternId ObservedPattern { get; }
    public int MinimumObservations { get; }
    public BossCombatTendency Tendency { get; }
    public int Priority { get; }
}

public sealed class BossAdaptationProfile
{
    private readonly IReadOnlyList<BossAdaptationRule> _rules;

    public BossAdaptationProfile(
        BossCombatTendency defaultTendency,
        IEnumerable<BossAdaptationRule> rules)
    {
        if (!Enum.IsDefined(defaultTendency))
            throw new ArgumentOutOfRangeException(nameof(defaultTendency));
        ArgumentNullException.ThrowIfNull(rules);

        var configuredRules = rules.ToArray();
        if (configuredRules.Any(rule => rule is null))
            throw new ArgumentException("Adaptation rules cannot contain null entries.", nameof(rules));
        if (configuredRules.Select(rule => rule.Priority).Distinct().Count() != configuredRules.Length)
            throw new ArgumentException("Adaptation rule priorities must be unique.", nameof(rules));

        DefaultTendency = defaultTendency;
        _rules = Array.AsReadOnly(configuredRules.OrderByDescending(rule => rule.Priority).ToArray());
    }

    public BossCombatTendency DefaultTendency { get; }
    public IReadOnlyList<BossAdaptationRule> Rules => _rules;
}

public sealed record BossAdaptationDecision(
    BossCombatTendency Tendency,
    CombatPatternId? MatchedPattern,
    int ObservedCount,
    bool UsedDefault);

/// <summary>
/// Encounter-scoped immutable memory. It is independent of BossPhaseState so
/// phase transitions do not erase learned player-behavior patterns.
/// </summary>
public sealed class BossCombatMemory
{
    private readonly Dictionary<CombatPatternId, int> _observations;
    private readonly IReadOnlyDictionary<CombatPatternId, int> _readOnlyObservations;

    public BossCombatMemory()
        : this(new Dictionary<CombatPatternId, int>())
    {
    }

    private BossCombatMemory(Dictionary<CombatPatternId, int> observations)
    {
        _observations = observations;
        _readOnlyObservations = new System.Collections.ObjectModel.ReadOnlyDictionary<CombatPatternId, int>(_observations);
    }

    public IReadOnlyDictionary<CombatPatternId, int> Observations => _readOnlyObservations;

    public int GetObservationCount(CombatPatternId pattern) =>
        _observations.TryGetValue(pattern, out var count) ? count : 0;

    public BossCombatMemory RecordObservation(CombatPatternId pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern.Value))
            throw new ArgumentException("A valid combat pattern is required.", nameof(pattern));

        var updated = new Dictionary<CombatPatternId, int>(_observations);
        updated[pattern] = checked(GetObservationCount(pattern) + 1);
        return new BossCombatMemory(updated);
    }

    public BossAdaptationDecision Resolve(BossAdaptationProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        foreach (var rule in profile.Rules)
        {
            var count = GetObservationCount(rule.ObservedPattern);
            if (count >= rule.MinimumObservations)
            {
                return new BossAdaptationDecision(
                    rule.Tendency,
                    rule.ObservedPattern,
                    count,
                    UsedDefault: false);
            }
        }

        return new BossAdaptationDecision(
            profile.DefaultTendency,
            null,
            0,
            UsedDefault: true);
    }

    /// <summary>Starts a new encounter memory. Phase changes must not call this.</summary>
    public BossCombatMemory ResetForNewEncounter() => new();
}
