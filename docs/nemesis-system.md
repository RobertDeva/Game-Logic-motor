# Nemesis system

**Status:** universal eligibility and category-specific rank ceilings are approved. The 45-rank evolution catalogue is documented as a design proposal. A concrete escalating promotion and victory-credit policy is documented separately as a proposal pending approval. No promotion thresholds or numerical tuning are yet approved.

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

These are category-specific ceilings, not a claim that every Nemesis must reach its maximum. A Nemesis at its cap may continue to remember encounters and develop in ways allowed by its evolution profile, but it cannot exceed the category's rank limit.

Rank numbers are canonical identifiers for progression. The enemy's category is retained when it becomes a Nemesis; rank progression does not by itself promote a normal enemy into the special or boss category.

## 3. Evolution catalogue

The companion [Nemesis rank evolution catalogue](nemesis-rank-evolution.md) defines a proposed distinct evolution for every rank across all three categories: 20 normal, 15 special, and 10 boss ranks.

The catalogue intentionally mixes combat capabilities with equipment, tactical behavior, visual identity, coordination, and world recognition. It does not assume that every promotion increases raw statistics. It also defines constraints for Muertebendita, posture, adaptive combat, boss phases, and player counterplay.

The catalogue is a content-design specification, not a universal moveset. Each enemy archetype must implement only compatible evolutions. Exact numerical values, action timings, cooldowns, and resource costs remain open and must not be inferred from rank descriptions.

## 4. Promotion and victory credit

The proposed [Nemesis promotion and victory-credit policy](nemesis-progression-policy.md) introduces escalating growth-mark thresholds, one primary lethal-source attribution, and idempotent encounter credit. Its threshold schedule and attribution rules remain proposals pending approval; they are not implemented or binding gameplay rules.

## 5. Separate memory responsibilities

Nemesis progression is a persistent world-level system and must remain distinct from encounter-scoped adaptive combat memory.

- **Adaptive combat memory** records tactical observations during an encounter and helps an enemy respond to player patterns. It can be used by any enemy with the configured capability.
- **Nemesis memory** records an individual enemy's identity and history across encounters, including its encounters with the player, victories, defeats, rank progression, and persistent evolution.

The two systems may inform one another, but neither should own the other's lifecycle. Nemesis history must not be erased when an encounter ends or when adaptive combat memory is reset.

## 6. Defeat and resurrection relationship

A Nemesis candidate is created or advanced because it defeated the player, not because it merely participated in combat or dealt damage.

The resurrection flow is voluntary. The Nemesis system must therefore be integrated with the authoritative lethal-event/defeat outcome so that the enemy's victory is recorded consistently even when the player chooses to resurrect. A resurrection does not retroactively erase the enemy's victory or its memory of the encounter.

The exact encounter event contract and the handling of simultaneous/multiple enemy contributions remain implementation details to define in the application layer.

## 7. Persistence and lifecycle

A Nemesis needs a stable identity and persistent record independent of a single encounter. Its history should be available when the enemy reappears in a later encounter. The save system will eventually persist identity, category, rank, history, and evolution.

The Nemesis system must coordinate with enemy lifecycle and world population systems. Defeating a Nemesis permanently should retire its record according to an explicit rule; it must not silently respawn as the same rival unless a narrative or gameplay rule explicitly permits it.

## 8. Architectural boundaries

The intended responsibilities are:
1. **Nemesis eligibility and promotion policy:** decides whether an enemy can enter or advance in the system after a qualifying player defeat.
2. **Persistent Nemesis record:** owns identity, category, rank, encounter history, and evolution history.
3. **Evolution policy:** applies configured, archetype-valid changes from the rank evolution catalogue.
4. **Encounter integration:** reports authoritative player defeat and connects the responsible enemy or enemies to the Nemesis policy.
5. **Adaptive combat integration:** exposes persistent Nemesis knowledge to encounter AI without merging persistent and encounter-scoped memories.
6. **Persistence:** saves and restores Nemesis records and world identity.

The domain should express deterministic rules and state transitions. Application coordinates encounter outcomes, world spawning, and persistence; presentation handles dialogue, animation, and visual rendering.

## 9. Remaining design decisions before implementation

The following still need to be settled:
- approval or revision of the proposed escalating promotion thresholds;
- final attribution policy for simultaneous enemy contributions and environmental deaths;
- limits on simultaneous active Nemeses;
- retreat, permanent-death, replacement, and return rules;
- whether a Nemesis can lose rank or evolve through events other than defeating the player.

The category ceilings and the 45-rank evolution catalogue are documented. The promotion model is specified as a reviewable proposal, not approved balance. No code should hard-code its thresholds or attribution assumptions before those decisions are accepted.
