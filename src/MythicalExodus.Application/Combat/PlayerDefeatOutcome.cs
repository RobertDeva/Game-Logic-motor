using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Authoritative encounter result for a player's lethal defeat. A null Nemesis
/// credit means the lethal cause was not attributable to a qualifying enemy.
/// </summary>
public sealed record PlayerDefeatOutcome
{
    public PlayerDefeatOutcome(
        EntityId encounterId,
        EntityId playerId,
        NemesisDefeatCredit? nemesisCredit)
    {
        if (encounterId.IsEmpty)
            throw new ArgumentException("A valid encounter identifier is required.", nameof(encounterId));
        if (playerId.IsEmpty)
            throw new ArgumentException("A valid player identifier is required.", nameof(playerId));

        EncounterId = encounterId;
        PlayerId = playerId;
        NemesisCredit = nemesisCredit;
    }

    public EntityId EncounterId { get; }
    public EntityId PlayerId { get; }
    public NemesisDefeatCredit? NemesisCredit { get; }
}

/// <summary>Identifies the one enemy authoritatively credited with the lethal defeat.</summary>
public sealed record NemesisDefeatCredit
{
    public NemesisDefeatCredit(EntityId enemyId, NemesisEnemyCategory originalCategory)
    {
        if (enemyId.IsEmpty)
            throw new ArgumentException("A valid enemy identifier is required.", nameof(enemyId));
        if (!Enum.IsDefined(originalCategory))
            throw new ArgumentOutOfRangeException(nameof(originalCategory));

        EnemyId = enemyId;
        OriginalCategory = originalCategory;
    }

    public EntityId EnemyId { get; }
    public NemesisEnemyCategory OriginalCategory { get; }
}
