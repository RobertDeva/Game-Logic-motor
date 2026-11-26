# Boss adaptive combat

## Purpose

Bosses can observe recurring player behavior and select a configured response tendency:

- **Defensive:** favor defensive behavior against a recognized pattern.
- **Aggressive:** favor pressure and offense.
- **Repositioning:** favor changing position or engagement distance.

This domain component chooses a tendency. It does not invent concrete attacks, movement paths, animation timing, or tactical utility scores; those belong to the encounter/AI execution layer and content configuration.

## Observation model

The encounter coordinator records observations using content-defined `CombatPatternId` values. Examples of possible content tags include repeated heavy attacks, frequent healing, or long-range kiting. These are identifiers, not hard-coded detection algorithms: the coordinator decides when an observed event qualifies.

Each adaptation rule declares:
- observed pattern;
- minimum number of observations;
- resulting tendency;
- unique priority.

When several rules qualify, the highest-priority rule wins. If none qualifies, the profile's default tendency is returned. This makes decisions deterministic and tunable without changing domain code.

## Memory and phases

`BossCombatMemory` is encounter-scoped and immutable. It records pattern counts and is intentionally independent of `BossPhaseState`. The encounter coordinator should carry the same memory forward when a boss changes phase, so the boss does not forget what it learned about the player.

Memory is reset only when a new encounter begins. No decay, time window, forgetting curve, or phase-specific reset is assumed because those behaviors have not been specified.

## Boundaries and remaining integration

- The observation detector must classify player actions into content-defined patterns.
- The encounter AI must translate the selected tendency into concrete attack, defense, and repositioning choices.
- Boss-specific rules, thresholds, priorities, and fallback tendencies remain content data.
- The component does not bypass boss phase restrictions, transition vulnerability, invulnerability, or special transition-attack rules.
