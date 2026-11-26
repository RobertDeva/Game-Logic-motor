using System.Text.Json;
using MythicalExodus.Application.Combat;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Infrastructure.Persistence;

/// <summary>
/// File-backed Nemesis progression store. Updates are serialized within this process
/// and committed by atomic file replacement; it is not a multi-process locking mechanism.
/// </summary>
public sealed class JsonNemesisProgressionRepository : INemesisProgressionRepository
{
    private readonly object _gate = new();
    private readonly string _filePath;
    private readonly Dictionary<EntityId, NemesisProgressionState> _states;
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public JsonNemesisProgressionRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = Path.GetFullPath(filePath);
        _states = Load(_filePath);
    }

    public NemesisProgressionState? Find(EntityId nemesisId)
    {
        lock (_gate)
            return _states.GetValueOrDefault(nemesisId);
    }

    public bool TryAdd(NemesisProgressionState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        lock (_gate)
        {
            if (_states.ContainsKey(state.NemesisId))
                return false;

            var updated = new Dictionary<EntityId, NemesisProgressionState>(_states)
            {
                [state.NemesisId] = state
            };
            Persist(updated);
            _states.Add(state.NemesisId, state);
            return true;
        }
    }

    public bool TryUpdate(NemesisProgressionState expectedState, NemesisProgressionState updatedState)
    {
        ArgumentNullException.ThrowIfNull(expectedState);
        ArgumentNullException.ThrowIfNull(updatedState);
        if (expectedState.NemesisId != updatedState.NemesisId)
            throw new ArgumentException("A Nemesis update cannot change its identity.", nameof(updatedState));

        lock (_gate)
        {
            if (!_states.TryGetValue(expectedState.NemesisId, out var current) ||
                !ReferenceEquals(current, expectedState))
                return false;

            var updated = new Dictionary<EntityId, NemesisProgressionState>(_states)
            {
                [updatedState.NemesisId] = updatedState
            };
            Persist(updated);
            _states[updatedState.NemesisId] = updatedState;
            return true;
        }
    }

    private static Dictionary<EntityId, NemesisProgressionState> Load(string path)
    {
        if (!File.Exists(path))
            return new Dictionary<EntityId, NemesisProgressionState>();

        var json = File.ReadAllText(path);
        var document = JsonSerializer.Deserialize<PersistedDocument>(json, JsonOptions)
            ?? throw new InvalidDataException("The Nemesis persistence document is empty or invalid.");
        var states = new Dictionary<EntityId, NemesisProgressionState>();

        foreach (var item in document.Records)
        {
            var id = new EntityId(item.NemesisId);
            var state = NemesisProgressionState.Restore(
                id,
                item.Category,
                item.Rank,
                item.GrowthMarks,
                item.CreditedEncounterIds.Select(value => new EntityId(value)),
                item.LifecycleStatus,
                item.FinalExecutionEncounterId is Guid encounterId ? new EntityId(encounterId) : null,
                item.FinalExecutionId is Guid executionId ? new EntityId(executionId) : null);
            if (!states.TryAdd(id, state))
                throw new InvalidDataException($"Duplicate Nemesis identifier '{id}' in persistence document.");
        }

        return states;
    }

    private void Persist(Dictionary<EntityId, NemesisProgressionState> states)
    {
        var directory = Path.GetDirectoryName(_filePath)!;
        Directory.CreateDirectory(directory);
        var document = new PersistedDocument(states.Values
            .OrderBy(state => state.NemesisId.Value)
            .Select(state => new PersistedRecord(
                state.NemesisId.Value,
                state.Category,
                state.Rank,
                state.GrowthMarks,
                state.CreditedEncounterIds.Select(id => id.Value).ToArray(),
                state.LifecycleStatus,
                state.FinalExecutionEncounterId?.Value,
                state.FinalExecutionId?.Value))
            .ToArray());

        var temporaryPath = _filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(document, JsonOptions));
            File.Move(temporaryPath, _filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private sealed record PersistedDocument(PersistedRecord[] Records);
    private sealed record PersistedRecord(
        Guid NemesisId,
        NemesisEnemyCategory Category,
        int Rank,
        int GrowthMarks,
        Guid[] CreditedEncounterIds,
        NemesisLifecycleStatus LifecycleStatus = NemesisLifecycleStatus.Active,
        Guid? FinalExecutionEncounterId = null,
        Guid? FinalExecutionId = null);
}
