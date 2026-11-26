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

/// <summary>
/// Adaptation capability, not enemy rank. Any enemy archetype can be assigned
/// any level by content configuration.
/// </summary>
public enum AdaptiveCombatLevel
{
    None = 0,
    Basic = 1,
    Advanced = 2,
    Expert = 3
}

public enum AdaptiveCombatTendency
{
    Defensive = 0,
    Aggressive = 1,
    Repositioning = 2
}

public sealed record AdaptiveCombatRule
{
    public AdaptiveCombatRule(
        CombatPatternId observedPattern,
        int minimumObservations,
        AdaptiveCombatTendency tendency,
        AdaptiveCombatLevel requiredLevel,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(observedPattern.Value))
            throw new ArgumentException("A valid observed pattern is required.", nameof(observedPattern));
        if (minimumObservations < 1)
            throw new ArgumentOutOfRangeException(nameof(minimumObservations));
        if (!Enum.IsDefined(tendency))
            throw new ArgumentOutOfRangeException(nameof(tendency));
        if (!Enum.IsDefined(requiredLevel) || requiredLevel == AdaptiveCombatLevel.None)
            throw new ArgumentOutOfRangeException(nameof(requiredLevel));
        if (priority < 0)
            throw new ArgumentOutOfRangeException(nameof(priority));

        ObservedPattern = observedPattern;
        MinimumObservations = minimumObservations;
        Tendency = tendency;
        RequiredLevel = requiredLevel;
        Priority = priority;
    }

    public CombatPatternId ObservedPattern { get; }
    public int MinimumObservations { get; }
    public AdaptiveCombatTendency Tendency { get; }
    public AdaptiveCombatLevel RequiredLevel { get; }
    public int Priority { get; }
}

public sealed class AdaptiveCombatProfile
{
    private readonly IReadOnlyList<AdaptiveCombatRule> _rules;

    public AdaptiveCombatProfile(
        AdaptiveCombatLevel level,
        AdaptiveCombatTendency defaultTendency,
        IEnumerable<AdaptiveCombatRule> rules)
    {
        if (!Enum.IsDefined(level))
            throw new ArgumentOutOfRangeException(nameof(level));
        if (!Enum.IsDefined(defaultTendency))
            throw new ArgumentOutOfRangeException(nameof(defaultTendency));
        ArgumentNullException.ThrowIfNull(rules);

        var configuredRules = rules.ToArray();
        if (configuredRules.Any(rule => rule is null))
            throw new ArgumentException("Adaptation rules cannot contain null entries.", nameof(rules));
        if (configuredRules.Select(rule => rule.Priority).Distinct().Count() != configuredRules.Length)
            throw new ArgumentException("Adaptation rule priorities must be unique.", nameof(rules));

        Level = level;
        DefaultTendency = defaultTendency;
        _rules = Array.AsReadOnly(configuredRules.OrderByDescending(rule => rule.Priority).ToArray());
    }

    public AdaptiveCombatLevel Level { get; }
    public AdaptiveCombatTendency DefaultTendency { get; }
    public IReadOnlyList<AdaptiveCombatRule> Rules => _rules;
}

public sealed record AdaptiveCombatDecision(
    AdaptiveCombatLevel Level,
    AdaptiveCombatTendency Tendency,
    CombatPatternId? MatchedPattern,
    int ObservedCount,
    bool UsedDefault);

/// <summary>
/// Encounter-scoped immutable memory, reusable by ordinary enemies, special
/// enemies and bosses. Keep the same instance across boss phase transitions.
/// </summary>
public sealed class AdaptiveCombatMemory
{
    private readonly Dictionary<CombatPatternId, int> _observations;
    private readonly IReadOnlyDictionary<CombatPatternId, int> _readOnlyObservations;

    public AdaptiveCombatMemory()
        : this(new Dictionary<CombatPatternId, int>())
    {
    }

    private AdaptiveCombatMemory(Dictionary<CombatPatternId, int> observations)
    {
        _observations = observations;
        _readOnlyObservations = new System.Collections.ObjectModel.ReadOnlyDictionary<CombatPatternId, int>(_observations);
    }

    public IReadOnlyDictionary<CombatPatternId, int> Observations => _readOnlyObservations;

    public int GetObservationCount(CombatPatternId pattern) =>
        _observations.TryGetValue(pattern, out var count) ? count : 0;

    public AdaptiveCombatMemory RecordObservation(CombatPatternId pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern.Value))
            throw new ArgumentException("A valid combat pattern is required.", nameof(pattern));

        var updated = new Dictionary<CombatPatternId, int>(_observations);
        updated[pattern] = checked(GetObservationCount(pattern) + 1);
        return new AdaptiveCombatMemory(updated);
    }

    public AdaptiveCombatDecision Resolve(AdaptiveCombatProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        if (profile.Level == AdaptiveCombatLevel.None)
            return new AdaptiveCombatDecision(profile.Level, profile.DefaultTendency, null, 0, UsedDefault: true);

        foreach (var rule in profile.Rules)
        {
            if (rule.RequiredLevel > profile.Level)
                continue;

            var count = GetObservationCount(rule.ObservedPattern);
            if (count >= rule.MinimumObservations)
            {
                return new AdaptiveCombatDecision(
                    profile.Level,
                    rule.Tendency,
                    rule.ObservedPattern,
                    count,
                    UsedDefault: false);
            }
        }

        return new AdaptiveCombatDecision(profile.Level, profile.DefaultTendency, null, 0, UsedDefault: true);
    }

    /// <summary>Starts a new encounter memory. Phase changes must not call this.</summary>
    public AdaptiveCombatMemory ResetForNewEncounter() => new();
}
