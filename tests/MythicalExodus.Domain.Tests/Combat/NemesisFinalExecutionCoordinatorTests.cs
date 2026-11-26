using MythicalExodus.Application.Combat;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class NemesisFinalExecutionCoordinatorTests
{
    [Fact]
    public void Missed_finisher_recovers_the_same_nemesis_for_a_later_encounter()
    {
        var nemesisId = EntityId.New();
        var firstEncounter = EntityId.New();
        var repository = new InMemoryNemesisProgressionRepository();
        var initial = NemesisProgressionState.Create(nemesisId, NemesisEnemyCategory.Normal, firstEncounter);
        Assert.True(repository.TryAdd(initial));
        var coordinator = new NemesisFinalExecutionCoordinator(repository);

        var firstExecutionId = EntityId.New();
        var opened = coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), firstExecutionId,
            CreateDefeatingExecution(), supportsFinalExecution: true);

        Assert.True(opened.IsSuccess);
        Assert.Equal(NemesisFinalExecutionUpdateStatus.OpportunityOpened, opened.Value.Status);
        Assert.Equal(NemesisLifecycleStatus.KneelingForFinalExecution, repository.Find(nemesisId)!.LifecycleStatus);

        var recovered = coordinator.Recover(nemesisId, firstExecutionId);

        Assert.True(recovered.IsSuccess);
        Assert.Equal(NemesisFinalExecutionUpdateStatus.Recovered, recovered.Value.Status);
        var survivor = repository.Find(nemesisId)!;
        Assert.Equal(NemesisLifecycleStatus.Active, survivor.LifecycleStatus);
        Assert.Equal(1, survivor.Rank);
        Assert.Equal(0, survivor.GrowthMarks);
        Assert.Equal(firstExecutionId, survivor.FinalExecutionId!.Value);

        var laterDefeat = survivor.RecordQualifyingDefeat(EntityId.New());
        Assert.Equal(firstExecutionId, laterDefeat.State.FinalExecutionId!.Value);
        Assert.True(repository.TryUpdate(survivor, laterDefeat.State));

        var laterExecutionId = EntityId.New();
        var laterOpportunity = coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), laterExecutionId,
            CreateDefeatingExecution(), supportsFinalExecution: true);

        Assert.True(laterOpportunity.IsSuccess);
        Assert.Equal(NemesisLifecycleStatus.KneelingForFinalExecution,
            repository.Find(nemesisId)!.LifecycleStatus);
        Assert.Equal(nemesisId, repository.Find(nemesisId)!.NemesisId);
    }

    [Fact]
    public void Completing_the_chained_finisher_permanently_retires_the_same_nemesis()
    {
        var nemesisId = EntityId.New();
        var repository = new InMemoryNemesisProgressionRepository();
        var initial = NemesisProgressionState.Create(nemesisId, NemesisEnemyCategory.Special, EntityId.New());
        Assert.True(repository.TryAdd(initial));
        var coordinator = new NemesisFinalExecutionCoordinator(repository);
        var executionId = EntityId.New();

        Assert.True(coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), executionId,
            CreateDefeatingExecution(), supportsFinalExecution: true).IsSuccess);

        var completed = coordinator.Complete(nemesisId, executionId);
        var duplicate = coordinator.Complete(nemesisId, executionId);

        Assert.True(completed.IsSuccess);
        Assert.Equal(NemesisFinalExecutionUpdateStatus.PermanentlyRetired, completed.Value.Status);
        Assert.True(duplicate.IsSuccess);
        Assert.Equal(NemesisFinalExecutionUpdateStatus.Duplicate, duplicate.Value.Status);
        Assert.True(repository.Find(nemesisId)!.IsPermanentlyRetired);
        Assert.Throws<InvalidOperationException>(() =>
            repository.Find(nemesisId)!.RecordQualifyingDefeat(EntityId.New()));
    }

    [Fact]
    public void Opportunity_requires_content_opt_in_and_a_defeating_normal_execution()
    {
        var nemesisId = EntityId.New();
        var repository = new InMemoryNemesisProgressionRepository();
        Assert.True(repository.TryAdd(
            NemesisProgressionState.Create(nemesisId, NemesisEnemyCategory.Boss, EntityId.New())));
        var coordinator = new NemesisFinalExecutionCoordinator(repository);

        var notConfigured = coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), EntityId.New(),
            CreateDefeatingExecution(), supportsFinalExecution: false);
        var notDefeated = coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), EntityId.New(),
            CreateNonDefeatingExecution(), supportsFinalExecution: true);

        Assert.True(notConfigured.IsFailure);
        Assert.Equal("nemesis.final_execution.not_supported", notConfigured.Error!.Code);
        Assert.True(notDefeated.IsFailure);
        Assert.Equal("nemesis.final_execution.target_not_defeated", notDefeated.Error!.Code);
        Assert.Equal(NemesisLifecycleStatus.Active, repository.Find(nemesisId)!.LifecycleStatus);
    }

    [Fact]
    public void Final_action_cannot_be_completed_for_another_execution_or_after_recovery()
    {
        var nemesisId = EntityId.New();
        var repository = new InMemoryNemesisProgressionRepository();
        Assert.True(repository.TryAdd(
            NemesisProgressionState.Create(nemesisId, NemesisEnemyCategory.Normal, EntityId.New())));
        var coordinator = new NemesisFinalExecutionCoordinator(repository);
        var executionId = EntityId.New();

        Assert.True(coordinator.OpenOpportunity(
            nemesisId, EntityId.New(), executionId,
            CreateDefeatingExecution(), supportsFinalExecution: true).IsSuccess);

        var wrongExecution = coordinator.Complete(nemesisId, EntityId.New());
        Assert.True(wrongExecution.IsFailure);
        Assert.Equal("nemesis.final_execution.opportunity_not_active", wrongExecution.Error!.Code);

        Assert.True(coordinator.Recover(nemesisId, executionId).IsSuccess);
        var afterRecovery = coordinator.Complete(nemesisId, executionId);
        Assert.True(afterRecovery.IsFailure);
        Assert.False(repository.Find(nemesisId)!.IsPermanentlyRetired);
    }

    private static MuertebenditaExecutionResult CreateDefeatingExecution() =>
        new(null!, EnemyDefeated: true, VitalityBarsConsumed: 0, PostureReset: false);

    private static MuertebenditaExecutionResult CreateNonDefeatingExecution() =>
        new(null!, EnemyDefeated: false, VitalityBarsConsumed: 1, PostureReset: true);

    private sealed class InMemoryNemesisProgressionRepository : INemesisProgressionRepository
    {
        private readonly Dictionary<EntityId, NemesisProgressionState> _states = [];

        public NemesisProgressionState? Find(EntityId nemesisId) =>
            _states.GetValueOrDefault(nemesisId);

        public IReadOnlyCollection<NemesisProgressionState> FindAll() => _states.Values.ToArray();

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
