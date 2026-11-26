namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

/// <summary>The enemy's original category. Nemesis rank never changes this value.</summary>
public enum NemesisEnemyCategory
{
    Normal = 0,
    Special = 1,
    Boss = 2
}

/// <summary>Outcome of applying one authoritative player-defeat credit to a Nemesis.</summary>
public enum NemesisProgressionUpdateStatus
{
    DuplicateEncounter = 0,
    GrowthMarkRecorded = 1,
    RankPromoted = 2,
    RankCapReached = 3,
    NemesisCreated = 4
}

/// <summary>
/// Immutable persistent rank progression for one Nemesis. The caller must only
/// submit an encounter after the application layer has authoritatively credited
/// this Nemesis as the source of the player's defeat.
/// </summary>
public sealed class NemesisProgressionState
{
    private readonly HashSet<EntityId> _creditedEncounterIds;
    private readonly IReadOnlyCollection<EntityId> _readOnlyCreditedEncounterIds;

    private NemesisProgressionState(
        EntityId nemesisId,
        NemesisEnemyCategory category,
        int rank,
        int growthMarks,
        HashSet<EntityId> creditedEncounterIds)
    {
        NemesisId = nemesisId;
        Category = category;
        Rank = rank;
        GrowthMarks = growthMarks;
        _creditedEncounterIds = creditedEncounterIds;
        _readOnlyCreditedEncounterIds = Array.AsReadOnly(
            creditedEncounterIds.OrderBy(id => id.Value).ToArray());
    }

    public EntityId NemesisId { get; }
    public NemesisEnemyCategory Category { get; }
    public int Rank { get; }
    public int GrowthMarks { get; }
    public int MaximumRank => GetMaximumRank(Category);
    public bool IsAtMaximumRank => Rank == MaximumRank;
    public IReadOnlyCollection<EntityId> CreditedEncounterIds => _readOnlyCreditedEncounterIds;

    /// <summary>
    /// Creates rank 1 from the first authoritative player defeat. The creation
    /// encounter is recorded so it cannot be credited again as later growth.
    /// </summary>
    public static NemesisProgressionState Create(
        EntityId nemesisId,
        NemesisEnemyCategory category,
        EntityId firstDefeatEncounterId)
    {
        if (nemesisId.IsEmpty)
            throw new ArgumentException("A valid Nemesis identifier is required.", nameof(nemesisId));
        if (!Enum.IsDefined(category))
            throw new ArgumentOutOfRangeException(nameof(category));
        if (firstDefeatEncounterId.IsEmpty)
            throw new ArgumentException("A valid encounter identifier is required.", nameof(firstDefeatEncounterId));

        return new NemesisProgressionState(
            nemesisId,
            category,
            rank: 1,
            growthMarks: 0,
            new HashSet<EntityId> { firstDefeatEncounterId });
    }

    /// <summary>Rehydrates a persisted progression snapshot after validating its invariants.</summary>
    public static NemesisProgressionState Restore(
        EntityId nemesisId,
        NemesisEnemyCategory category,
        int rank,
        int growthMarks,
        IEnumerable<EntityId> creditedEncounterIds)
    {
        if (nemesisId.IsEmpty)
            throw new ArgumentException("A valid Nemesis identifier is required.", nameof(nemesisId));
        if (!Enum.IsDefined(category))
            throw new ArgumentOutOfRangeException(nameof(category));
        if (rank < 1 || rank > GetMaximumRank(category))
            throw new ArgumentOutOfRangeException(nameof(rank));
        if (growthMarks < 0)
            throw new ArgumentOutOfRangeException(nameof(growthMarks));
        ArgumentNullException.ThrowIfNull(creditedEncounterIds);

        var ids = creditedEncounterIds.ToArray();
        if (ids.Length == 0 || ids.Any(id => id.IsEmpty) || ids.Distinct().Count() != ids.Length)
            throw new ArgumentException("Persisted encounter identifiers must be non-empty and unique.", nameof(creditedEncounterIds));

        if (rank == GetMaximumRank(category))
        {
            if (growthMarks != 0)
                throw new ArgumentException("A Nemesis at its rank ceiling cannot retain growth marks.", nameof(growthMarks));
        }
        else if (growthMarks >= GetRequiredMarksForRank(rank))
        {
            throw new ArgumentException("Persisted growth marks must be below the next promotion threshold.", nameof(growthMarks));
        }

        return new NemesisProgressionState(nemesisId, category, rank, growthMarks, ids.ToHashSet());
    }

    private static int GetRequiredMarksForRank(int rank)
    {
        var state = Create(EntityId.New(), category, EntityId.New());
        for (var currentRank = 1; currentRank < rank; currentRank++)
        {
            var required = state.GetRequiredGrowthMarksForNextRank()
                ?? throw new InvalidOperationException("Unexpected rank ceiling while validating persisted progression.");
            state = Restore(state.NemesisId, category, currentRank + 1, 0, state.CreditedEncounterIds);
            _ = required;
        }

        return state.GetRequiredGrowthMarksForNextRank()
            ?? throw new InvalidOperationException("A non-maximum rank must have a promotion threshold.");
    }

    /// <summary>
    /// Records one qualifying defeat from a distinct encounter. Reprocessing an
    /// encounter is idempotent. Each encounter can promote at most one rank.
    /// </summary>
    public NemesisProgressionUpdate RecordQualifyingDefeat(EntityId encounterId)
    {
        if (encounterId.IsEmpty)
            throw new ArgumentException("A valid encounter identifier is required.", nameof(encounterId));

        if (_creditedEncounterIds.Contains(encounterId))
        {
            return new NemesisProgressionUpdate(
                this,
                NemesisProgressionUpdateStatus.DuplicateEncounter,
                GetRequiredGrowthMarksForNextRank());
        }

        var updatedEncounterIds = new HashSet<EntityId>(_creditedEncounterIds)
        {
            encounterId
        };

        if (IsAtMaximumRank)
        {
            var cappedState = new NemesisProgressionState(
                NemesisId,
                Category,
                Rank,
                GrowthMarks,
                updatedEncounterIds);

            return new NemesisProgressionUpdate(
                cappedState,
                NemesisProgressionUpdateStatus.RankCapReached,
                RequiredGrowthMarks: 0);
        }

        var requiredMarks = GetRequiredGrowthMarksForNextRank()
            ?? throw new InvalidOperationException("A Nemesis below its rank ceiling must have a promotion threshold.");
        var updatedMarks = checked(GrowthMarks + 1);

        if (updatedMarks >= requiredMarks)
        {
            var promotedState = new NemesisProgressionState(
                NemesisId,
                Category,
                Rank + 1,
                updatedMarks - requiredMarks,
                updatedEncounterIds);

            return new NemesisProgressionUpdate(
                promotedState,
                NemesisProgressionUpdateStatus.RankPromoted,
                requiredMarks);
        }

        var progressedState = new NemesisProgressionState(
            NemesisId,
            Category,
            Rank,
            updatedMarks,
            updatedEncounterIds);

        return new NemesisProgressionUpdate(
            progressedState,
            NemesisProgressionUpdateStatus.GrowthMarkRecorded,
            requiredMarks);
    }

    /// <summary>
    /// Returns the marks required for the next rank. The original escalating
    /// schedule is scaled cumulatively to 70%, with at least one mark per rank.
    /// Cumulative scaling keeps the final victory targets close to a 30% reduction
    /// despite integer-only encounter credits.
    /// </summary>
    public int? GetRequiredGrowthMarksForNextRank()
    {
        if (IsAtMaximumRank)
            return null;

        var cumulativeScaledMarks = 0;
        for (var sourceRank = 1; sourceRank < Rank; sourceRank++)
        {
            cumulativeScaledMarks += GetScaledThreshold(sourceRank, cumulativeScaledMarks);
        }

        return GetScaledThreshold(Rank, cumulativeScaledMarks);
    }

    private int GetScaledThreshold(int sourceRank, int cumulativeMarksBeforePromotion)
    {
        var originalCumulativeMarks = 0;
        for (var rank = 1; rank <= sourceRank; rank++)
            originalCumulativeMarks += 1 + ((rank - 1) / 5);

        var scaledCumulativeTarget = (int)Math.Round(
            originalCumulativeMarks * 0.70m,
            MidpointRounding.AwayFromZero);

        return Math.Max(1, scaledCumulativeTarget - cumulativeMarksBeforePromotion);
    }

    public static int GetMaximumRank(NemesisEnemyCategory category) =>
        category switch
        {
            NemesisEnemyCategory.Normal => 20,
            NemesisEnemyCategory.Special => 15,
            NemesisEnemyCategory.Boss => 10,
            _ => throw new ArgumentOutOfRangeException(nameof(category))
        };
}

/// <summary>Immutable result of recording a qualifying Nemesis defeat.</summary>
public sealed record NemesisProgressionUpdate(
    NemesisProgressionState State,
    NemesisProgressionUpdateStatus Status,
    int? RequiredGrowthMarks);
