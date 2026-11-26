namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

/// <summary>
/// Immutable player resurrection charges. The initial charge is restored at a
/// rest point; additional charges are restored by enemy-defeat progress and
/// remain locked for use until the next rest point.
/// </summary>
public sealed record ResurrectionState
{
    public const int MaximumOpportunityLimit = 3;

    public ResurrectionState(int maximumOpportunities, decimal progressRequiredPerAdditionalCharge)
        : this(
            maximumOpportunities,
            initialChargeAvailable: true,
            additionalUsableCharges: maximumOpportunities - 1,
            additionalLockedCharges: 0,
            additionalSpentCharges: 0,
            defeatProgress: 0m,
            progressRequiredPerAdditionalCharge)
    {
        if (maximumOpportunities is < 1 or > MaximumOpportunityLimit)
            throw new ArgumentOutOfRangeException(nameof(maximumOpportunities));
        if (progressRequiredPerAdditionalCharge <= 0m)
            throw new ArgumentOutOfRangeException(nameof(progressRequiredPerAdditionalCharge));
    }

    private ResurrectionState(
        int maximumOpportunities,
        bool initialChargeAvailable,
        int additionalUsableCharges,
        int additionalLockedCharges,
        int additionalSpentCharges,
        decimal defeatProgress,
        decimal progressRequiredPerAdditionalCharge)
    {
        MaximumOpportunities = maximumOpportunities;
        InitialChargeAvailable = initialChargeAvailable;
        AdditionalUsableCharges = additionalUsableCharges;
        AdditionalLockedCharges = additionalLockedCharges;
        AdditionalSpentCharges = additionalSpentCharges;
        DefeatProgress = defeatProgress;
        ProgressRequiredPerAdditionalCharge = progressRequiredPerAdditionalCharge;
    }

    public int MaximumOpportunities { get; }
    public bool InitialChargeAvailable { get; }
    public int AdditionalUsableCharges { get; }
    public int AdditionalLockedCharges { get; }
    public int AdditionalSpentCharges { get; }
    public decimal DefeatProgress { get; }
    public decimal ProgressRequiredPerAdditionalCharge { get; }

    public Result<ResurrectionState> TryConsumeInitialCharge()
    {
        if (!InitialChargeAvailable)
        {
            return Result<ResurrectionState>.Failure(new GameError(
                "resurrection.initial_charge_unavailable",
                "The initial resurrection charge has already been spent."));
        }

        return Result<ResurrectionState>.Success(Copy(initialChargeAvailable: false));
    }

    public Result<ResurrectionState> TryConsumeAdditionalCharge()
    {
        if (AdditionalUsableCharges == 0)
        {
            return Result<ResurrectionState>.Failure(new GameError(
                "resurrection.additional_charge_unavailable",
                "No additional resurrection charge is currently usable."));
        }

        return Result<ResurrectionState>.Success(Copy(
            additionalUsableCharges: AdditionalUsableCharges - 1,
            additionalSpentCharges: AdditionalSpentCharges + 1));
    }

    /// <summary>
    /// The caller supplies the configured contribution for the defeated enemy.
    /// Recovered charges remain locked until the next rest point.
    /// </summary>
    public Result<ResurrectionState> TryRecordEnemyDefeat(decimal progress)
    {
        if (progress <= 0m)
        {
            return Result<ResurrectionState>.Failure(new GameError(
                "resurrection.progress.invalid_contribution",
                "Enemy defeat progress must be greater than zero."));
        }

        if (AdditionalSpentCharges == 0)
            return Result<ResurrectionState>.Success(this);

        var accumulatedProgress = DefeatProgress + progress;
        var spentCharges = AdditionalSpentCharges;
        var lockedCharges = AdditionalLockedCharges;

        while (spentCharges > 0 && accumulatedProgress >= ProgressRequiredPerAdditionalCharge)
        {
            accumulatedProgress -= ProgressRequiredPerAdditionalCharge;
            spentCharges--;
            lockedCharges++;
        }

        if (spentCharges == 0)
            accumulatedProgress = 0m;

        return Result<ResurrectionState>.Success(Copy(
            additionalLockedCharges: lockedCharges,
            additionalSpentCharges: spentCharges,
            defeatProgress: accumulatedProgress));
    }

    /// <summary>Restores the initial charge and unlocks already recovered additional charges.</summary>
    public ResurrectionState ReturnToRestPoint() => Copy(
        initialChargeAvailable: true,
        additionalUsableCharges: AdditionalUsableCharges + AdditionalLockedCharges,
        additionalLockedCharges: 0);

    public Result<ResurrectionState> TryUnlockAdditionalOpportunity()
    {
        if (MaximumOpportunities >= MaximumOpportunityLimit)
        {
            return Result<ResurrectionState>.Failure(new GameError(
                "resurrection.maximum_opportunities_reached",
                "The maximum number of resurrection opportunities is already unlocked."));
        }

        return Result<ResurrectionState>.Success(Copy(
            maximumOpportunities: MaximumOpportunities + 1,
            additionalUsableCharges: AdditionalUsableCharges + 1));
    }

    private ResurrectionState Copy(
        int? maximumOpportunities = null,
        bool? initialChargeAvailable = null,
        int? additionalUsableCharges = null,
        int? additionalLockedCharges = null,
        int? additionalSpentCharges = null,
        decimal? defeatProgress = null) =>
        new(
            maximumOpportunities ?? MaximumOpportunities,
            initialChargeAvailable ?? InitialChargeAvailable,
            additionalUsableCharges ?? AdditionalUsableCharges,
            additionalLockedCharges ?? AdditionalLockedCharges,
            additionalSpentCharges ?? AdditionalSpentCharges,
            defeatProgress ?? DefeatProgress,
            ProgressRequiredPerAdditionalCharge);
}
