using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Infrastructure.Persistence;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class JsonNemesisProgressionRepositoryTests
{
    [Fact]
    public void State_survives_repository_reconstruction_and_duplicate_credit_is_preserved()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "nemesis.json");
        try
        {
            var nemesisId = EntityId.New();
            var firstEncounter = EntityId.New();
            var secondEncounter = EntityId.New();
            var initial = NemesisProgressionState.Create(nemesisId, NemesisEnemyCategory.Normal, firstEncounter);
            var repository = new JsonNemesisProgressionRepository(path);
            Assert.True(repository.TryAdd(initial));

            var update = initial.RecordQualifyingDefeat(secondEncounter);
            Assert.True(repository.TryUpdate(initial, update.State));

            var reloaded = new JsonNemesisProgressionRepository(path);
            var restored = reloaded.Find(nemesisId);
            Assert.NotNull(restored);
            Assert.Equal(update.State.Rank, restored.Rank);
            Assert.Equal(update.State.GrowthMarks, restored.GrowthMarks);
            Assert.Equal(2, restored.CreditedEncounterIds.Count);
            Assert.Equal(NemesisProgressionUpdateStatus.DuplicateEncounter,
                restored.RecordQualifyingDefeat(secondEncounter).Status);
        }
        finally
        {
            var directory = Path.GetDirectoryName(path)!;
            if (Directory.Exists(directory))
                Directory.Delete(directory, recursive: true);
        }
    }

    [Fact]
    public void Compare_and_swap_rejects_stale_state_instance()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "nemesis.json");
        try
        {
            var state = NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Special, EntityId.New());
            var repository = new JsonNemesisProgressionRepository(path);
            Assert.True(repository.TryAdd(state));
            var staleCopy = NemesisProgressionState.Restore(
                state.NemesisId, state.Category, state.Rank, state.GrowthMarks, state.CreditedEncounterIds);
            var updated = state.RecordQualifyingDefeat(EntityId.New()).State;

            Assert.False(repository.TryUpdate(staleCopy, updated));
            Assert.True(repository.TryUpdate(state, updated));
        }
        finally
        {
            var directory = Path.GetDirectoryName(path)!;
            if (Directory.Exists(directory))
                Directory.Delete(directory, recursive: true);
        }
    }
}
