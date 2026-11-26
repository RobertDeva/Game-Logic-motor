# Adaptive enemy combat

## Shared system

Adaptive combat is a shared enemy capability, not a boss-only feature. Ordinary enemies, special enemies and bosses use the same domain model. Content assigns each enemy an `AdaptiveCombatLevel`; enemy category does not hard-code the level.

## Capability levels

| Level | Capability | Typical use |
|---|---|---|
| 0 — None | Does not learn or change its response based on observed patterns | Enemies with fixed/basic behavior |
| 1 — Basic | Recognizes simple recurring patterns and selects a configured response | Ordinary enemies |
| 2 — Advanced | Recognizes a broader set of patterns and can use more tactical responses | Special enemies |
| 3 — Expert | Uses the broadest configured pattern set and can preserve learned behavior across boss phases | Bosses |

These are capability tiers, not mandatory category assignments. A special enemy may be configured at level 1 or 3; a boss may use level 2 if appropriate.

## Rules and resolution

Each rule declares a player-behavior pattern, its observation threshold, the response tendency, the minimum adaptation level required, and a unique priority.

The resolver considers only rules whose required level is at or below the enemy's configured level. Among matching rules, the highest priority wins. If no rule qualifies, the profile's default tendency is used. Level 0 always uses the default tendency and never adapts.

Tendencies currently represented are defensive, aggressive and repositioning. They express strategic intent; the encounter AI must map them to concrete attacks, defenses and movement.

## Memory

`AdaptiveCombatMemory` is immutable and encounter-scoped. It is generic and can be used by any enemy archetype. The same memory can be retained across boss phase transitions; it is reset when a new encounter begins.

No observation decay, time windows, forgetting behavior, or exact pattern-detection rules are assumed. The encounter coordinator is responsible for classifying observed player behavior into content-defined pattern identifiers.

## Integration boundaries

- Enemy content chooses the adaptation level and profile.
- The encounter coordinator detects and records patterns.
- The adaptive resolver selects a tendency.
- Enemy-specific AI executes the tendency using available moves and movement options.
- Boss phase restrictions, invulnerability, transition windows and special attacks remain authoritative and cannot be bypassed by adaptation.
- Persistent Nemesis identity, rank, history and evolution are a separate system; see [Nemesis system](nemesis-system.md). Nemesis knowledge may inform encounter AI, but it must not be merged into encounter-scoped adaptive memory.
