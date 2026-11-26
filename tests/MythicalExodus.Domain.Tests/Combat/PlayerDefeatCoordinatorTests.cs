using MythicalExodus.Application.Combat;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using Xunit;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class PlayerDefeatCoordinatorTests
{
    [Fact]
    public void First_authoritative_defeat_creates_rank_one_before_resurrection_decision()
    {
        var repository = new InMemoryNemesisRepository();
        var coordinator = new PlayerDefeatCoordinator(repository);
        var enemyId = EntityId.New();
        var encounterId = EntityId.New();

        var result = coordinator.Process(
            Outcome(encounterId, enemyId, NemesisEnemyCategory.Normal),
            CreatePlayerState());

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.NemesisCreated);
        Assert.Equal(NemesisProgressionUpdateStatus.NemesisCreated, result.Value.NemesisUpdate!.Status);
        Assert.Equal(1, result.Value.NemesisUpdate.State.Rank);
        Assert.Equal(PlayerResurrectionPhase.AwaitingDecision, result.Value.ResurrectionState.Phase);
        Assert.True(repository.WasWrittenBeforeResurrectionDecision);
    }

    [Fact]
    public void Later_defeat_of_same_enemy_advances_its_existing_nemesis_record()
    {
        var repository = new InMemoryNemesisRepository();
        var coordinator = new PlayerDefeatCoordinator(repository);
        var enemyId = EntityId.New();

        coordinator.Process(Outcome(EntityId.New(), enemyId, NemesisEnemyCategory.Special), CreatePlayerState());
        var later = coordinator.Process(
            Outcome(EntityId.New(), enemyId, NemesisEnemyCategory.Special),
            CreatePlayerState());

        Assert.True(later.IsSuccess);
        Assert.False(later.Value.NemesisCreated);
        Assert.Equal(NemesisProgressionUpdateStatus.RankPromoted, later.Value.NemesisUpdate!.Status);
        Assert.Equal(2, later.Value.NemesisUpdate.State.Rank);
    }

    [Fact]
    public void Replaying_same_defeat_does_not_grant_another_growth_mark()
    {
        var repository = new InMemoryNemesisRepository();
        var coordinator = new PlayerDefeatCoordinator(repository);
        var enemyId = EntityId.New();
        var encounterId = EntityId.New();
        var outcome = Outcome(encounterId, enemyId, NemesisEnemyCategory.Boss);

        coordinator.Process(outcome, CreatePlayerState());
        var replay = coordinator.Process(outcome, CreatePlayerState());

        Assert.True(replay.IsSuccess);
        Assert.False(replay.Value.NemesisCreated);
        Assert.Equal(NemesisProgressionUpdateStatus.DuplicateEncounter, replay.Value.NemesisUpdate!.Status);
        Assert.Equal(1, replay.Value.NemesisUpdate.State.Rank);
    }

    [Fact]
    public void Unattributed_lethal_defeat_still_enters_resurrection_flow_without_nemesis_credit()
    {
        var coordinator = new PlayerDefeatCoordinator(new InMemoryNemesisRepository());
        var outcome = new PlayerDefeatOutcome(EntityId.New(), EntityId.New(), null);

        var result = coordinator.Process(outcome, CreatePlayerState());

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value.NemesisUpdate);
        Assert.Equal(PlayerResurrectionPhase.AwaitingDecision, result.Value.ResurrectionState.Phase);
    }

    [Fact]
    public void Category_mismatch_is_rejected_without_changing_persistent_record()
    {
        var repository = new InMemoryNemesisRepository();
        var coordinator = new PlayerDefeatCoordinator(repository);
        var enemyId = EntityId.New();

        coordinator.Process(Outcome(EntityId.New(), enemyId, NemesisEnemyCategory.Normal), CreatePlayerState());
        var result = coordinator.Process(
            Outcome(EntityId.New(), enemyId, NemesisEnemyCategory.Boss),
            CreatePlayerState());

        Assert.True(result.IsFailure);
        Assert.Equal("nemesis.category_mismatch", result.Error!.Code);
        Assert.Equal(1, repository.Find(enemyId)!.Rank);
    }

    private static PlayerDefeatOutcome Outcome(
        EntityId encounterId,
        EntityId enemyId,
        NemesisEnemyCategory category) =>
        new(encounterId, EntityId.New(), new NemesisDefeatCredit(enemyId, category));

    private static PlayerResurrectionState CreatePlayerState() =>
        new(new ResurrectionState(1, 100m), EntityId.New());

    private sealed class InMemoryNemesisRepository : INemesisProgressionRepository
    {
        private readonly Dictionary<EntityId, NemesisProgressionState> _states = [];

        public bool WasWrittenBeforeResurrectionDecision { get; private set; }

        public NemesisProgressionState? Find(EntityId nemesisId)
        {
            WasWrittenBeforeResurrectionDecision |= _states.Count > 0;
            return _states.GetValueOrDefault(nemesisId);
        }

        public bool TryAdd(NemesisProgressionState state)
        {
            if (_states.ContainsKey(state.NemesisId))
                return false;
            _states.Add(state.NemesisId, state);
            WasWrittenBeforeResurrectionDecision = true;
            return true;
        }

        public bool TryUpdate(NemesisProgressionState expectedState, NemesisProgressionState updatedState)
        {
            if (!_states.TryGetValue(expectedState.NemesisId, out var current) ||
                !ReferenceEquals(current, expectedState))
                return false;
            _states[expectedState.NemesisId] = updatedState;
            WasWrittenBeforeResurrectionDecision = true;
            return true;
        }
    }
}
