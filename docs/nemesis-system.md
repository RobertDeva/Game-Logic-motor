# Nemesis system

**Status:** universal eligibility, category-specific rank ceilings, and the escalating rank-promotion schedule are implemented in the Domain. The 45-rank evolution catalogue remains a content-design specification awaiting archetype-specific content integration. Encounter attribution, durable persistence, and world lifecycle integration remain unfinished.

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
- next-rank threshold is 1 + floor((current rank - 1) / 5);
- a promotion consumes the threshold and advances exactly one rank;
- growth marks persist if the Nemesis is defeated by the player;
- repeated processing of the same encounter is idempotent;
- at the category cap, no further growth marks are accumulated.

The resulting later-defeat totals from rank 1 to maximum are 46 for normal enemies, 27 for special enemies, and 13 for bosses. These values are consequences of the implemented threshold schedule and should be assessed through playtesting.

The Domain receives an already-authoritative Nemesis/encounter attribution. It does not yet decide which enemy caused a defeat when several enemies contribute, preserve causal identity for environmental chains, or coordinate the defeat event with resurrection. Those responsibilities belong to Application.

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

The Nemesis system must coordinate with enemy lifecycle and world population systems. Defeating a Nemesis permanently should retire its record according to an explicit rule; it must not silently respawn as the same rival unless a narrative or gameplay rule explicitly permits it.

## 8. Architectural boundaries

1. **Nemesis progression state (Domain):** owns category, rank, growth marks, category ceiling, and idempotent encounter credits.
2. **Encounter integration (Application):** reports authoritative player defeat and identifies the responsible Nemesis.
3. **Persistent Nemesis record:** owns identity, encounter history, and evolution history.
4. **Evolution policy:** applies configured, archetype-valid changes from the rank evolution catalogue.
5. **Adaptive combat integration:** exposes persistent Nemesis knowledge to encounter AI without merging persistent and encounter-scoped memories.
6. **Persistence:** saves and restores Nemesis records and world identity.

The domain expresses deterministic rules and immutable state transitions. Application coordinates encounter outcomes, world spawning, and persistence; presentation handles dialogue, animation, and visual rendering.

## 9. Remaining work

- authoritative encounter-to-Nemesis attribution, including simultaneous sources and environmental causality;
- ordering defeat credit with voluntary resurrection;
- durable persistence of credited encounter IDs and progression state;
- active Nemesis limits, retreat, permanent-death, replacement, and respawn rules;
- applying rank evolution definitions to actual enemy archetypes;
- playtesting the promotion curve and adjusting it through explicit balance configuration if needed.
