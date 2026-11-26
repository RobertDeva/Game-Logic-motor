namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

/// <summary>The enemy's original category. Nemesis rank never changes this value.</summary>
public enum NemesisEnemyCategory
{
    Normal = 0,
    Special = 1,
    Boss = 2
}

/// <summary>Persistent lifecycle of one Nemesis identity.</summary>
public enum NemesisLifecycleStatus
{
    Active = 0,
    KneelingForFinalExecution = 1,
    PermanentlyRetired = 2
}

/// <summary>Outcome of a Nemesis final-execution lifecycle transition.</summary>
public enum NemesisFinalExecutionUpdateStatus
{
    OpportunityOpened = 0,
    Recovered = 1,
    PermanentlyRetired = 2,
    Duplicate = 3
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
        HashSet<EntityId> creditedEncounterIds,
        NemesisLifecycleStatus lifecycleStatus = NemesisLifecycleStatus.Active,
        EntityId? finalExecutionEncounterId = null,
        EntityId? finalExecutionId = null)
    {
        NemesisId = nemesisId;
        Category = category;
        Rank = rank;
        GrowthMarks = growthMarks;
        LifecycleStatus = lifecycleStatus;
        FinalExecutionEncounterId = finalExecutionEncounterId;
        FinalExecutionId = finalExecutionId;
        _creditedEncounterIds = creditedEncounterIds;
        _readOnlyCreditedEncounterIds = Array.AsReadOnly(
            creditedEncounterIds.OrderBy(id => id.Value).ToArray());
    }

    public EntityId NemesisId { get; }
    public NemesisEnemyCategory Category { get; }
    public int Rank { get; }
    public int GrowthMarks { get; }
    public NemesisLifecycleStatus LifecycleStatus { get; }
    public EntityId? FinalExecutionEncounterId { get; }
    public EntityId? FinalExecutionId { get; }
    public bool IsPermanentlyRetired => LifecycleStatus == NemesisLifecycleStatus.PermanentlyRetired;
    public bool HasFinalExecutionOpportunity => LifecycleStatus == NemesisLifecycleStatus.KneelingForFinalExecution;
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
        IEnumerable<EntityId> creditedEncounterIds,
        NemesisLifecycleStatus lifecycleStatus = NemesisLifecycleStatus.Active,
        EntityId? finalExecutionEncounterId = null,
        EntityId? finalExecutionId = null)
    {
        if (nemesisId.IsEmpty)
            throw new ArgumentException("A valid Nemesis identifier is required.", nameof(nemesisId));
        if (!Enum.IsDefined(category))
            throw new ArgumentOutOfRangeException(nameof(category));
        if (!Enum.IsDefined(lifecycleStatus))
            throw new ArgumentOutOfRangeException(nameof(lifecycleStatus));
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

        if (finalExecutionEncounterId.HasValue != finalExecutionId.HasValue ||
            (finalExecutionEncounterId.HasValue &&
             (finalExecutionEncounterId.Value.IsEmpty || finalExecutionId!.Value.IsEmpty)))
            throw new ArgumentException("Final-execution identifiers must either both be valid or both be absent.");
        if (lifecycleStatus != NemesisLifecycleStatus.Active &&
            (!finalExecutionEncounterId.HasValue || !finalExecutionId.HasValue))
            throw new ArgumentException("A Nemesis outside the active state must retain its final-execution identifiers.");

        return new NemesisProgressionState(
            nemesisId, category, rank, growthMarks, ids.ToHashSet(),
            lifecycleStatus, finalExecutionEncounterId, finalExecutionId);
    }


    /// <summary>
    /// Opens the optional kneeling opportunity after a completed normal execution.
    /// Eligibility and the authoritative execution result are checked by Application.
    /// </summary>
    public Result<NemesisFinalExecutionUpdate> OpenFinalExecutionOpportunity(
        EntityId encounterId,
        EntityId executionId)
    {
        ValidateFinalExecutionIdentifiers(encounterId, executionId);

        if (IsPermanentlyRetired)
            return FinalExecutionFailure("nemesis.final_execution.retired",
                "A permanently executed Nemesis cannot enter another final-execution opportunity.");

        if (LifecycleStatus == NemesisLifecycleStatus.KneelingForFinalExecution)
        {
            if (FinalExecutionEncounterId == encounterId && FinalExecutionId == executionId)
                return Result<NemesisFinalExecutionUpdate>.Success(
                    new NemesisFinalExecutionUpdate(this, NemesisFinalExecutionUpdateStatus.Duplicate));
            return FinalExecutionFailure("nemesis.final_execution.opportunity_already_open",
                "This Nemesis already has an unresolved final-execution opportunity.");
        }

        if (FinalExecutionEncounterId == encounterId && FinalExecutionId == executionId)
            return Result<NemesisFinalExecutionUpdate>.Success(
                new NemesisFinalExecutionUpdate(this, NemesisFinalExecutionUpdateStatus.Duplicate));

        var updated = CopyWithLifecycle(
            NemesisLifecycleStatus.KneelingForFinalExecution, encounterId, executionId);
        return Result<NemesisFinalExecutionUpdate>.Success(
            new NemesisFinalExecutionUpdate(updated, NemesisFinalExecutionUpdateStatus.OpportunityOpened));
    }

    /// <summary>
    /// Recovers the Nemesis after the player leaves or declines the optional finisher.
    /// The host decides when recovery occurs; this transition has no built-in timer.
    /// </summary>
    public Result<NemesisFinalExecutionUpdate> RecoverFromFinalExecutionOpportunity(EntityId executionId)
    {
        if (executionId.IsEmpty)
            throw new ArgumentException("A valid execution identifier is required.", nameof(executionId));

        if (LifecycleStatus == NemesisLifecycleStatus.Active && FinalExecutionId == executionId)
            return Result<NemesisFinalExecutionUpdate>.Success(
                new NemesisFinalExecutionUpdate(this, NemesisFinalExecutionUpdateStatus.Duplicate));

        if (LifecycleStatus != NemesisLifecycleStatus.KneelingForFinalExecution || FinalExecutionId != executionId)
            return FinalExecutionFailure("nemesis.final_execution.opportunity_not_active",
                "The requested final-execution opportunity is not active for this Nemesis.");

        var updated = CopyWithLifecycle(
            NemesisLifecycleStatus.Active, FinalExecutionEncounterId, FinalExecutionId);
        return Result<NemesisFinalExecutionUpdate>.Success(
            new NemesisFinalExecutionUpdate(updated, NemesisFinalExecutionUpdateStatus.Recovered));
    }

    /// <summary>
    /// Permanently retires this Nemesis after the host confirms the contextual final action.
    /// Replaying the same completion is idempotent.
    /// </summary>
    public Result<NemesisFinalExecutionUpdate> CompleteFinalExecution(EntityId executionId)
    {
        if (executionId.IsEmpty)
            throw new ArgumentException("A valid execution identifier is required.", nameof(executionId));

        if (IsPermanentlyRetired && FinalExecutionId == executionId)
            return Result<NemesisFinalExecutionUpdate>.Success(
                new NemesisFinalExecutionUpdate(this, NemesisFinalExecutionUpdateStatus.Duplicate));

        if (LifecycleStatus != NemesisLifecycleStatus.KneelingForFinalExecution || FinalExecutionId != executionId)
            return FinalExecutionFailure("nemesis.final_execution.opportunity_not_active",
                "The final action can only complete for the Nemesis currently kneeling for this execution.");

        var updated = CopyWithLifecycle(
            NemesisLifecycleStatus.PermanentlyRetired, FinalExecutionEncounterId, FinalExecutionId);
        return Result<NemesisFinalExecutionUpdate>.Success(
            new NemesisFinalExecutionUpdate(updated, NemesisFinalExecutionUpdateStatus.PermanentlyRetired));
    }

    private NemesisProgressionState CopyWithLifecycle(
        NemesisLifecycleStatus lifecycleStatus,
        EntityId? finalExecutionEncounterId,
        EntityId? finalExecutionId) =>
        new(NemesisId, Category, Rank, GrowthMarks, new HashSet<EntityId>(_creditedEncounterIds),
            lifecycleStatus, finalExecutionEncounterId, finalExecutionId);

    private static void ValidateFinalExecutionIdentifiers(EntityId encounterId, EntityId executionId)
    {
        if (encounterId.IsEmpty)
            throw new ArgumentException("A valid encounter identifier is required.", nameof(encounterId));
        if (executionId.IsEmpty)
            throw new ArgumentException("A valid execution identifier is required.", nameof(executionId));
    }

    private static Result<NemesisFinalExecutionUpdate> FinalExecutionFailure(string code, string message) =>
        Result<NemesisFinalExecutionUpdate>.Failure(new GameError(code, message));

    private static int GetRequiredMarksForRank(int rank)
    {
        var cumulativeOriginalMarks = 0;
        var cumulativeScaledMarks = 0;
        var requiredMarks = 0;

        for (var sourceRank = 1; sourceRank <= rank; sourceRank++)
        {
            cumulativeOriginalMarks += 1 + ((sourceRank - 1) / 5);
            var scaledCumulativeTarget = (int)Math.Round(
                cumulativeOriginalMarks * 0.70m,
                MidpointRounding.AwayFromZero);
            requiredMarks = Math.Max(1, scaledCumulativeTarget - cumulativeScaledMarks);
            cumulativeScaledMarks += requiredMarks;
        }

        return requiredMarks;
    }

    /// <summary>
    /// Records one qualifying defeat from a distinct encounter. Reprocessing an
    /// encounter is idempotent. Each encounter can promote at most one rank.
    /// </summary>
    public NemesisProgressionUpdate RecordQualifyingDefeat(EntityId encounterId)
    {
        if (LifecycleStatus != NemesisLifecycleStatus.Active)
            throw new InvalidOperationException("A Nemesis must be active to receive further progression.");
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
                updatedEncounterIds,
                LifecycleStatus,
                FinalExecutionEncounterId,
                FinalExecutionId);

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
                updatedEncounterIds,
                LifecycleStatus,
                FinalExecutionEncounterId,
                FinalExecutionId);

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
            updatedEncounterIds,
            LifecycleStatus,
            FinalExecutionEncounterId,
            FinalExecutionId);

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

/// <summary>Immutable result of a Nemesis final-execution lifecycle transition.</summary>
public sealed record NemesisFinalExecutionUpdate(
    NemesisProgressionState State,
    NemesisFinalExecutionUpdateStatus Status);
