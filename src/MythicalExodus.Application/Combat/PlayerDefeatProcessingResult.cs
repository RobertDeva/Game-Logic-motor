using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Application.Combat;

/// <summary>Combined result of persistent Nemesis credit and the resurrection decision phase.</summary>
public sealed record PlayerDefeatProcessingResult(
    PlayerResurrectionState ResurrectionState,
    NemesisProgressionUpdate? NemesisUpdate,
    bool NemesisCreated);
