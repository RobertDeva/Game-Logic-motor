# Nemesis system

**Status:** universal eligibility and rank ladders approved; promotion triggers, rank names, and detailed evolution rewards remain unspecified.

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

The first qualifying victory over the player creates the Nemesis identity at rank 1. Further qualifying victories can promote that Nemesis progressively, up to the maximum for its category.

These are category-specific ceilings, not a claim that every Nemesis must reach its maximum. A Nemesis at its cap may continue to remember encounters and develop in ways allowed by its evolution profile, but it cannot exceed the category's rank limit.

Rank numbers are canonical identifiers for progression. Rank names, presentation titles, promotion thresholds, and the exact effects of each rank are not defined yet. The enemy's category is retained when it becomes a Nemesis; rank progression does not by itself promote a normal enemy into the special or boss category.

## 3. Separate memory responsibilities

Nemesis progression is a persistent world-level system and must remain distinct from encounter-scoped adaptive combat memory.

- **Adaptive combat memory** records tactical observations during an encounter and helps an enemy respond to player patterns. It can be used by any enemy with the configured capability.
- **Nemesis memory** records an individual enemy's identity and history across encounters, including its encounters with the player, victories, defeats, rank progression, and persistent evolution.

The two systems may inform one another, but neither should own the other's lifecycle. Nemesis history must not be erased when an encounter ends or when adaptive combat memory is reset.

## 4. Defeat and resurrection relationship

A Nemesis candidate is created or advanced because it defeated the player, not because it merely participated in combat or dealt damage.

The resurrection flow is voluntary. The Nemesis system must therefore be integrated with the authoritative lethal-event/defeat outcome so that the enemy's victory is recorded consistently even when the player chooses to resurrect. A resurrection does not retroactively erase the enemy's victory or its memory of the encounter.

The exact encounter event contract and the handling of simultaneous/multiple enemy contributions remain implementation details to define in the application layer.

## 5. Rank and evolution

The first qualifying player defeat establishes the enemy as a rank-1 Nemesis. Later qualifying victories are progression opportunities and can increase its rank, subject to the category-specific cap.

Evolution is content-driven and need not mean a uniform increase to every statistic. It may include new tactics, equipment, scars, visual identity, personality, or other configured changes. A promotion must remain valid for the enemy's archetype and available capabilities.

The following are deliberately not fixed yet:
- rank names and presentation titles;
- exact promotion thresholds and whether every subsequent victory grants a rank;
- exact changes to attributes, equipment, appearance, moves, and tactical capabilities at each rank;
- whether a Nemesis at maximum rank can gain non-rank evolution;
- rules for retreat, permanent death, replacement, and population limits;
- how simultaneous or cooperative enemy contributions to a player defeat are attributed.

No numeric bonuses, promotion formulas, or automatic stat multipliers may be inferred from the rank ladder.

## 6. Persistence and lifecycle

A Nemesis needs a stable identity and persistent record independent of a single encounter. Its history should be available when the enemy reappears in a later encounter. The save system will eventually persist identity, category, rank, history, and evolution.

The Nemesis system must coordinate with enemy lifecycle and world population systems. Defeating a Nemesis permanently should retire its record according to an explicit rule; it must not silently respawn as the same rival unless a narrative or gameplay rule explicitly permits it.

## 7. Architectural boundaries

The intended responsibilities are:

1. **Nemesis eligibility and promotion policy:** decides whether an enemy can enter or advance in the system after a qualifying player defeat.
2. **Persistent Nemesis record:** owns identity, category, rank, encounter history, and evolution history.
3. **Evolution policy:** applies configured, archetype-valid changes.
4. **Encounter integration:** reports authoritative player defeat and connects the responsible enemy or enemies to the Nemesis policy.
5. **Adaptive combat integration:** exposes persistent Nemesis knowledge to encounter AI without merging persistent and encounter-scoped memories.
6. **Persistence:** saves and restores Nemesis records and world identity.

The domain should express deterministic rules and state transitions. Application coordinates encounter outcomes, world spawning, and persistence; presentation handles dialogue, animation, and visual rendering.

## 8. Remaining design decisions before implementation

The following still need to be settled before implementing the complete progression policy:
- rank names and presentation titles, if any;
- the promotion threshold for each subsequent victory;
- the evolution rewards and behavior changes associated with each rank;
- whether active Nemeses or simultaneous claims on one defeat are limited;
- retreat, permanent-death, replacement, and return rules;
- how bosses' Nemesis evolution interacts with their phase system.

The approved category ceilings are fixed at 20 for normal enemies, 15 for special enemies, and 10 for bosses. Until the remaining rules are agreed, implementation must not invent promotion thresholds, rank rewards, or population caps.
