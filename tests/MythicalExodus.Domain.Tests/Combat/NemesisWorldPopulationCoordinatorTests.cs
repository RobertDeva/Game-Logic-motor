using MythicalExodus.Application.Combat;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class NemesisWorldPopulationCoordinatorTests
{
    [Fact]
    public void Spawn_candidates_exclude_kneeling_and_permanently_retired_nemeses()
    {
        var repository = new InMemoryNemesisProgressionRepository();
        var activeId = EntityId.New();
        var kneelingId = EntityId.New();
        var retiredId = EntityId.New();

        repository.Add(NemesisProgressionState.Create(
            activeId, NemesisEnemyCategory.Normal, EntityId.New()));

        var kneeling = NemesisProgressionState.Create(
            kneelingId, NemesisEnemyCategory.Special, EntityId.New());
        var executionId = EntityId.New();
        var opened = kneeling.OpenFinalExecutionOpportunity(EntityId.New(), executionId);
        Assert.True(opened.IsSuccess);
        repository.Add(opened.Value.State);

        var retired = NemesisProgressionState.Create(
            retiredId, NemesisEnemyCategory.Boss, EntityId.New());
        var retirementOpportunity = retired.OpenFinalExecutionOpportunity(EntityId.New(), executionId);
        Assert.True(retirementOpportunity.IsSuccess);
        var completed = retirementOpportunity.Value.State.CompleteFinalExecution(executionId);
        Assert.True(completed.IsSuccess);
        repository.Add(completed.Value.State);

        var coordinator = CreateCoordinator(repository);

        var candidates = coordinator.FindSpawnCandidates();

        Assert.Single(candidates);
        Assert.Equal(activeId, candidates[0].NemesisId);
    }

    [Fact]
    public void Recovered_nemesis_becomes_spawn_candidate_again_under_the_same_identity()
    {
        var repository = new InMemoryNemesisProgressionRepository();
        var nemesisId = EntityId.New();
        var state = NemesisProgressionState.Create(
            nemesisId, NemesisEnemyCategory.Normal, EntityId.New());
        var executionId = EntityId.New();
        var opened = state.OpenFinalExecutionOpportunity(EntityId.New(), executionId);
        Assert.True(opened.IsSuccess);
        var kneelingState = opened.Value.State;
        repository.Add(kneelingState);

        var coordinator = CreateCoordinator(repository);
        Assert.Empty(coordinator.FindSpawnCandidates());

        var recovered = kneelingState.RecoverFromFinalExecutionOpportunity(executionId);
        Assert.True(recovered.IsSuccess);
        Assert.True(repository.TryUpdate(kneelingState, recovered.Value.State));

        var candidates = coordinator.FindSpawnCandidates();

        Assert.Single(candidates);
        Assert.Equal(nemesisId, candidates[0].NemesisId);
        Assert.Equal(NemesisLifecycleStatus.Active, candidates[0].LifecycleStatus);
    }

    [Fact]
    public void Registered_nemesis_is_not_returned_as_a_spawn_candidate()
    {
        var repository = new InMemoryNemesisProgressionRepository();
        var nemesisId = EntityId.New();
        repository.Add(NemesisProgressionState.Create(
            nemesisId, NemesisEnemyCategory.Boss, EntityId.New()));
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var presence = new NemesisWorldPresence(
            nemesisId, EntityId.New(), EntityId.New(), EntityId.New());
        Assert.True(registry.TryRegister(presence));
        var coordinator = new NemesisWorldPopulationCoordinator(repository, registry);

        Assert.Empty(coordinator.FindSpawnCandidates());
    }

    [Fact]
    public void Unregistering_a_world_actor_makes_its_active_nemesis_a_candidate_again()
    {
        var repository = new InMemoryNemesisProgressionRepository();
        var nemesisId = EntityId.New();
        repository.Add(NemesisProgressionState.Create(
            nemesisId, NemesisEnemyCategory.Normal, EntityId.New()));
        var registry = new InMemoryNemesisWorldPresenceRegistry();
        var presence = new NemesisWorldPresence(
            nemesisId, EntityId.New(), EntityId.New(), EntityId.New());
        Assert.True(registry.TryRegister(presence));
        var coordinator = new NemesisWorldPopulationCoordinator(repository, registry);

        Assert.True(registry.TryUnregister(nemesisId, presence.ActorEntityId));

        var candidates = coordinator.FindSpawnCandidates();
        Assert.Single(candidates);
        Assert.Equal(nemesisId, candidates[0].NemesisId);
    }

    private static NemesisWorldPopulationCoordinator CreateCoordinator(
        InMemoryNemesisProgressionRepository repository) =>
        new(repository, new InMemoryNemesisWorldPresenceRegistry());

    private sealed class InMemoryNemesisProgressionRepository : INemesisProgressionRepository
    {
        private readonly Dictionary<EntityId, NemesisProgressionState> _states = [];

        public NemesisProgressionState? Find(EntityId nemesisId) =>
            _states.GetValueOrDefault(nemesisId);

        public IReadOnlyCollection<NemesisProgressionState> FindAll() =>
            _states.Values.ToArray();

        public void Add(NemesisProgressionState state) =>
            _states.Add(state.NemesisId, state);

        public bool TryAdd(NemesisProgressionState state) =>
            _states.TryAdd(state.NemesisId, state);

        public bool TryUpdate(NemesisProgressionState expectedState, NemesisProgressionState updatedState)
        {
            if (!_states.TryGetValue(expectedState.NemesisId, out var current) ||
                !ReferenceEquals(current, expectedState))
                return false;

            _states[updatedState.NemesisId] = updatedState;
            return true;
        }
    }
}
