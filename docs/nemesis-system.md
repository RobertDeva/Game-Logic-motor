# Nemesis system

**Status:** universal Nemesis creation eligibility, category-specific rank ceilings, rank progression, final-execution lifecycle, spawn-candidate selection, and process-local actor-presence exclusion are implemented. Application coordinates defeat credits, lifecycle transitions, and world population queries through repository contracts. The JSON repository persists rank and lifecycle state. Actual actor instantiation, location selection, host lifecycle hooks, and recovery scheduling remain host responsibilities.

## 1. Core design decisions

Any enemy in the game may become a Nemesis rival. Eligibility is not restricted to special enemies, elites, or bosses. An ordinary enemy that defeats the player may survive the encounter, become persistent, and ascend in rank through the Nemesis system.

This is an intentional death consequence: the player's defeat can create a more experienced and personally recurring threat. The system makes death matter beyond the player's resurrection resources, lost souls, or return to a rest point.

Enemy category and Nemesis rank are separate concepts. A common enemy can become a Nemesis; becoming one does not automatically change its original archetype or grant the same upgrades as every other rival.

## 2. Rank ladders

The maximum rank depends on the enemy's original category:

| Enemy category | Maximum Nemesis rank |
|---|---:|
| Normal | 20 |
| Special | 15 |
| Boss | 10 |

The first qualifying victory over the player creates the Nemesis identity at rank 1. Further qualifying victories by that same Nemesis allow progressive promotion, subject to its category ceiling.

These are category-specific ceilings, not a claim that every Nemesis must reach its maximum. A Nemesis at its cap cannot exceed the category's rank limit.

Rank numbers are canonical identifiers for progression. The enemy's category is retained when it becomes a Nemesis; rank progression does not by itself promote a normal enemy into the special or boss category.

## 3. Evolution catalogue

The companion [Nemesis rank evolution catalogue](nemesis-rank-evolution.md) defines a distinct design evolution for every rank across all three categories: 20 normal, 15 special, and 10 boss ranks.

The catalogue intentionally mixes combat capabilities with equipment, tactical behavior, visual identity, coordination, and world recognition. It does not assume that every promotion increases raw statistics. It also defines constraints for Muertebendita, posture, adaptive combat, boss phases, and player counterplay.

The catalogue is a content-design specification, not a universal moveset. Each enemy archetype must implement only compatible evolutions. Exact archetype-specific action values, timings, cooldowns, and resource costs remain content configuration work.

## 4. Promotion and victory credit

The immutable Domain progression state is documented in [Nemesis promotion and victory-credit policy](nemesis-progression-policy.md).

Implemented rules:
- first qualifying player defeat creates rank 1 and records the creation encounter;
- each later qualifying defeat in a distinct encounter grants one Nemesis Growth Mark;
- the original escalating threshold curve is rebased cumulatively to 70%, with at least one growth mark required for every promotion;
- a promotion consumes the threshold and advances exactly one rank;
- growth marks persist if the Nemesis is defeated by the player;
- repeated processing of the same encounter is idempotent;
- at the category cap, no further growth marks are accumulated.

The rebased later-defeat totals from rank 1 to maximum are 32 for normal enemies, 19 for special enemies, and 9 for bosses. This is approximately 30% fewer victories than the previous totals of 46, 27, and 13; integer rounding accounts for the small category differences.

Application receives a `PlayerDefeatOutcome` containing the encounter, player, and at most one authoritative enemy credit. A missing credit represents an unattributed lethal cause and does not create or advance a Nemesis. Application validates the enemy's original category, creates rank 1 on the first credited defeat, advances existing Nemesis records on later defeats, and persists the update before opening the resurrection decision. Compare-and-swap repository operations prevent concurrent updates from silently overwriting one another; a conflict is returned for retry with the same encounter ID. The exact attribution of a lethal source when several enemies or environmental effects contribute remains the responsibility of the encounter/combat host.

## 5. Separate memory responsibilities

Nemesis progression is a persistent world-level system and must remain distinct from encounter-scoped adaptive combat memory.

- **Adaptive combat memory** records tactical observations during an encounter and helps an enemy respond to player patterns. It can be used by any enemy with the configured capability.
- **Nemesis memory** records an individual enemy's identity and history across encounters, including its encounters with the player, victories, defeats, rank progression, and persistent evolution.

The two systems may inform one another, but neither should own the other's lifecycle. Nemesis history must not be erased when an encounter ends or when adaptive combat memory is reset.

## 6. Defeat and resurrection relationship

A Nemesis candidate is created or advanced because it defeated the player, not because it merely participated in combat or dealt damage.

The resurrection flow is voluntary. The Nemesis system must therefore be integrated with the authoritative lethal-event/defeat outcome so that the enemy's victory is recorded consistently even when the player chooses to resurrect. A resurrection does not retroactively erase the enemy's victory or its memory of the encounter.

The Domain progression state is idempotent for encounter identifiers, but Application must still ensure that defeat credit is recorded before the resurrection choice can alter the player's life-state flow.

## 7. Persistence and lifecycle

A Nemesis needs a stable identity and persistent record independent of a single encounter. Its history should be available when the enemy reappears in a later encounter. The save system will eventually persist identity, category, rank, growth marks, credited encounter identifiers, history, and evolution.

The Nemesis system coordinates persistent identity with runtime world presence through `INemesisWorldPresenceRegistry`. Each instantiated host actor must be registered with its persistent `NemesisId`, its own `ActorEntityId`, a `WorldInstanceId`, and a `ZoneId`. The registry rejects a second actor for the same Nemesis and rejects reuse of one actor ID for another Nemesis. Exact registration replay is idempotent.

The registry is process-local runtime state, deliberately separate from the JSON progression save. It starts empty after process restart and must be reconstructed as the host loads actors. When unloading an actor, the host unregisters the exact pair of Nemesis and actor IDs. An old actor cannot unregister a replacement instance. Unloading only removes runtime presence: it does not recover, retire, or otherwise change the Nemesis lifecycle. After unregistering, an active Nemesis can become a spawn candidate again.

`NemesisWorldPopulationCoordinator.FindSpawnCandidates()` reads the persistent progression snapshot and excludes Nemeses that are kneeling, permanently retired, or already registered as present. The host remains responsible for choosing a location, creating the actor with the original Nemesis identity, registering it, and cleaning up presence on unload/removal. Candidate lookup and registration are separate operations; the host must treat registration failure as a duplicate/conflict and must not leave the newly created actor active.

The in-memory registry guarantees exclusion only within one process and one shared registry instance. A multi-process or distributed world deployment will require a shared atomic registry or lease mechanism; this implementation does not claim global exclusion.

## 8. Nemesis final execution

A Nemesis may be permanently eliminated through a special second finisher chained to a normal Muertebendita execution. The normal execution is followed by a distinct kneeling state that enables an optional contextual final finisher, such as a decapitation. The first execution defeats the Nemesis in the encounter but does not by itself retire its persistent identity; if the player does not perform the second action, the Nemesis recovers and may return in a later encounter as the same individual. Only successful completion of the second action permanently retires that identity. The Domain stores the active, kneeling, and permanently retired states; recovery is an explicit host-triggered transition with no hard-coded timer. Application opens the opportunity only when the normal execution defeated the Nemesis and content explicitly marks that individual as supporting the final finisher. The JSON progression repository persists these transitions with compare-and-swap. See [Nemesis final execution](nemesis-final-execution.md).

## 9. Architectural boundaries

1. **Nemesis progression state (Domain):** owns category, rank, growth marks, category ceiling, and idempotent encounter credits.
2. **Encounter integration (Application):** reports authoritative player defeat and identifies the responsible Nemesis.
3. **Persistent Nemesis record:** owns identity, encounter history, and evolution history.
4. **Evolution policy:** applies configured, archetype-valid changes from the rank evolution catalogue.
5. **Adaptive combat integration:** exposes persistent Nemesis knowledge to encounter AI without merging persistent and encounter-scoped memories.
6. **Persistence:** saves and restores Nemesis records and world identity; runtime actor presence is intentionally not persisted.
7. **World host integration:** creates actors, assigns the stored Nemesis identity, registers/unregisters presence, and handles locations and zone lifecycle.

The domain expresses deterministic rules and immutable state transitions. Application coordinates encounter outcomes, world spawning, and persistence; presentation handles dialogue, animation, and visual rendering.

## 10. Remaining work

- host integration that instantiates actors with the stored Nemesis ID and registers/unregisters their presence on load, unload, and removal;
- authoritative encounter-to-Nemesis attribution, including simultaneous sources and environmental causality;
- recovery scheduling and the unresolved duration/interruption policy for the final-execution opportunity;
- active Nemesis limits, retreat, non-execution defeat, and replacement rules;
- applying rank evolution definitions to actual enemy archetypes;
- a shared atomic presence registry/lease if the world is deployed across multiple processes or servers;
- playtesting the promotion curve and adjusting it through explicit balance configuration if needed.

Spawn locations, cooldowns, population caps, and recovery timing are not specified by this implementation.
