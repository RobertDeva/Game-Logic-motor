# Nemesis system

**Status:** design scope approved; rank progression and detailed evolution rules remain unspecified.

## 1. Core design decision

Any enemy in the game may become a Nemesis rival. Eligibility is not restricted to special enemies, elites, or bosses. An ordinary enemy that defeats the player may survive the encounter, become persistent, and ascend in rank through the Nemesis system.

This is an intentional death consequence: the player's defeat can create a stronger, more experienced, and personally recurring threat. The system should make death matter beyond the player's resurrection resources, lost souls, or return to a rest point.

Enemy category and Nemesis rank are separate concepts. A common enemy can become a Nemesis; becoming one does not automatically change its original archetype or grant the same upgrades as every other rival.

## 2. Separate memory responsibilities

Nemesis progression is a persistent world-level system and must remain distinct from encounter-scoped adaptive combat memory.

- **Adaptive combat memory** records tactical observations during an encounter and helps an enemy respond to player patterns. It can be used by any enemy with the configured capability.
- **Nemesis memory** records an individual enemy's identity and history across encounters, including its encounters with the player, victories, defeats, rank progression, and persistent evolution.

The two systems may inform one another, but neither should own the other's lifecycle. Nemesis history must not be erased when an encounter ends or when adaptive combat memory is reset.

## 3. Defeat and resurrection relationship

A Nemesis candidate is created or advanced because it defeated the player, not because it merely participated in combat or dealt damage.

The resurrection flow is voluntary. The Nemesis system must therefore be integrated with the authoritative lethal-event/defeat outcome so that the enemy's victory is recorded consistently even when the player chooses to resurrect. A resurrection does not retroactively erase the enemy's victory or its memory of the encounter.

The exact encounter event contract and the handling of simultaneous/multiple enemy contributions remain implementation details to define in the application layer.

## 4. Rank and evolution

Rank is a persistent progression dimension for Nemesis enemies. The approved direction is that ordinary enemies can ascend in rank by defeating the player.

The following are deliberately not fixed yet:
- rank names, count, and ordering;
- promotion thresholds and whether one defeat always grants a rank;
- whether repeated victories are required for later promotions;
- exact changes to attributes, equipment, appearance, moves, and tactical capabilities;
- whether every Nemesis can reach the same maximum rank;
- rules for retreat, permanent death, replacement, and population limits;
- whether bosses use the same rank ladder or a distinct narrative variant.

No numeric bonuses, rank thresholds, or automatic stat multipliers may be inferred from this document.

Evolution should be content-driven and need not mean a uniform increase to every statistic. It may include new tactics, equipment, scars, visual identity, personality, or other configured changes. A promotion must remain valid for the enemy's archetype and available capabilities.

## 5. Persistence and lifecycle

A Nemesis needs a stable identity and persistent record independent of a single encounter. Its history should be available when the enemy reappears in a later encounter. The save system will eventually persist identity, history, rank, and evolution.

The Nemesis system must coordinate with enemy lifecycle and world population systems. Defeating a Nemesis permanently should retire its record according to an explicit rule; it must not silently respawn as the same rival unless a narrative or gameplay rule explicitly permits it.

## 6. Architectural boundaries

The intended responsibilities are:

1. **Nemesis eligibility and promotion policy:** decides whether an enemy can enter or advance in the system after a qualifying player defeat.
2. **Persistent Nemesis record:** owns identity, rank, encounter history, and evolution history.
3. **Evolution policy:** applies configured, archetype-valid changes.
4. **Encounter integration:** reports authoritative player defeat and connects the responsible enemy or enemies to the Nemesis policy.
5. **Adaptive combat integration:** exposes persistent Nemesis knowledge to encounter AI without merging persistent and encounter-scoped memories.
6. **Persistence:** saves and restores Nemesis records and world identity.

The domain should express deterministic rules and state transitions. Application coordinates encounter outcomes, world spawning, and persistence; presentation handles dialogue, animation, and visual rendering.

## 7. Open design decisions before rank implementation

The universal eligibility decision is approved: any ordinary enemy may become a Nemesis after defeating the player.

Before implementing rank progression, the design still needs to settle:
- how many ranks exist and what they are called;
- what each promotion grants and what triggers it;
- whether there are limits on active Nemeses or on how many enemies can claim the same defeat;
- how an enemy's permanent death and return are handled;
- whether bosses follow the same system or a separate one.

Until these decisions are made, implementation must not invent a rank ladder, promotion formula, stat scaling, or population cap.
