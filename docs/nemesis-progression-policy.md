# Nemesis promotion and victory-credit policy

**Status:** proposed progression model for review. The category ceilings and rank-1 creation rule are established. The thresholds and encounter-credit rules below are a concrete design proposal, not yet approved gameplay canon. Do not implement them as fixed domain rules until approved.

## 1. Design goals

The promotion policy should make a Nemesis' repeated victories matter, require progressively more victories at higher ranks, preserve the original category and rank ceiling, prevent duplicate credit, remain deterministic and independent of combat damage formulas, and avoid automatic statistical inflation.

## 2. Proposed progression currency

Use a small integer counter named **Nemesis Growth Marks** (working name).

- The first qualifying defeat creates the Nemesis at rank 1. It does not also grant a growth mark.
- Each later qualifying defeat of the player by that same Nemesis grants one growth mark.
- A growth mark belongs to that Nemesis record, not to the enemy category, faction, or player globally.
- Growth marks are spent only on rank promotion.
- A promotion consumes the required marks and increases rank by exactly one.
- Each qualifying encounter grants at most one mark, so a single player defeat can never skip several ranks.
- At the category maximum, no further growth marks are needed or accumulated for rank progression. Non-rank evolution at the cap remains a separate content rule.

This is not XP, souls, enemy level, or adaptive-combat memory.

## 3. Proposed escalating threshold

For a Nemesis currently at rank r, the marks required for its next promotion are:

T(r) = 1 + floor((r - 1) / 5)

| Current rank | Marks required for next rank |
|---:|---:|
| 1–5 | 1 |
| 6–10 | 2 |
| 11–15 | 3 |
| 16–20 | 4 |

Examples:
- A normal Nemesis needs one later qualifying victory to move from rank 1 to 2. From rank 5 to 6 it still needs one; from rank 6 to 7 it needs two.
- A special Nemesis uses the same schedule but stops at rank 15.
- A boss uses the same schedule but stops at rank 10.

If the player defeats a Nemesis before it earns enough marks, its partial progress remains attached to that persistent record. It is not reset merely because the enemy lost one encounter, unless a separate lifecycle rule is later approved.

### Total repeated victories under this proposal

| Category | Maximum rank | Later qualifying victories from rank 1 to cap |
|---|---:|---:|
| Normal | 20 | 46 |
| Special | 15 | 27 |
| Boss | 10 | 13 |

These totals are consequences of the proposed formula, not independently chosen targets. They make the final normal ranks deliberately rare. If playtesting shows this is too slow, tune the threshold schedule as data; do not add hidden rank multipliers.

## 4. What counts as a qualifying victory

A growth mark should be awarded only when all of the following are true:

1. The player reaches the authoritative defeated state for the encounter.
2. The encounter result identifies this Nemesis as the enemy responsible for the lethal outcome.
3. The Nemesis record is active and below its category rank ceiling.
4. The defeat belongs to a completed encounter outcome, not an intermediate damage, vitality-bar, posture-break, execution, or boss-phase event.
5. The encounter outcome has not already been processed for this Nemesis.

Do not award marks merely because an enemy damaged the player, participated in combat, broke posture, depleted one vitality bar, performed a Muertebendita execution, triggered a boss phase transition, or was present when another enemy or environmental hazard defeated the player.

If damage-over-time or a delayed effect causes the lethal outcome after the enemy has left the immediate scene, the encounter result should retain the responsible source identity so the correct Nemesis can receive credit. This is a proposed attribution rule and requires support from the authoritative encounter event.

## 5. Idempotency and resurrection

The encounter result must carry a stable unique encounter identifier and a stable defeat-event identifier.

- Processing the same defeat event more than once must not create multiple Nemeses, add multiple growth marks, or promote multiple ranks.
- Choosing voluntary resurrection does not cancel the enemy's victory. The defeat outcome is recorded before the resurrection decision can erase or alter its consequences.
- A second lethal event within the same unresolved encounter must not grant a second growth mark for that encounter.
- A new growth mark requires a new, separately completed encounter and a new qualifying player defeat.

The Domain policy should be deterministic and idempotent. Application owns event ordering, encounter completion, and durable persistence.

## 6. Multiple-enemy and environmental attribution

This remains a design decision that must be explicit before implementation.

**Candidate default:** credit one primary Nemesis only—the authoritative source of the lethal event. If the lethal event has multiple simultaneous sources, the encounter coordinator must select one primary source using a deterministic rule and include that source in the result. Do not award a full growth mark to every enemy that participated.

Environmental damage, falls, hazards, and player-initiated deaths should not automatically credit a Nemesis. If an enemy directly caused the environmental chain, credit is possible only when the encounter system can preserve an explicit causal source; otherwise no Nemesis receives the mark.

This conservative policy avoids multiplying progression from crowd encounters and avoids attributing a death to an arbitrary nearby enemy.

## 7. Persistence and lifecycle

The persistent Nemesis record will eventually need:
- stable Nemesis identity;
- original enemy identity and category;
- current rank;
- current unspent growth marks;
- qualifying encounter identifiers or an equivalent durable idempotency record;
- promotion history, including previous rank and awarded evolution identifier;
- persistent Nemesis history and evolution state.

The persistence layer must commit the defeat credit and resulting rank transition atomically, or use an equivalent idempotent transaction/outbox strategy. A crash between recording the defeat and applying the promotion must not duplicate or lose the growth mark.

If the Nemesis is permanently retired, the record and its unused marks follow the eventual retirement policy. They must not transfer to a replacement enemy by accident.

## 8. Separation from other progression systems

- Nemesis Growth Marks are not player souls or currency.
- Nemesis rank does not change enemy category, enemy level, Muertebendita resistance, vitality-bar count, or boss phase count.
- Rank does not automatically raise the adaptive-combat level. Rank evolution content may explicitly unlock additional tactical rules, but the adaptive capability profile remains separately configured.
- Encounter-scoped adaptive memory may be informed by persistent Nemesis history; it does not own or spend growth marks.
- Rank evolutions must follow the Nemesis rank evolution catalogue and its counterplay constraints.

## 9. Decisions still required

Before this proposal becomes approved and is implemented, settle:
1. Whether the threshold formula and its resulting 46/27/13 later victories are acceptable.
2. Whether a primary lethal source is the correct attribution rule when several enemies contribute.
3. Whether a Nemesis that is defeated by the player but later returns may retain its rank and partial marks. The current proposal says yes, subject to final lifecycle rules.
4. Whether marks can ever be gained from non-lethal events, territory activity, or narrative events. This proposal says no.
5. Whether the system should cap the number of active Nemeses and what happens when that cap is reached.

Until these decisions are approved, treat this document as a proposed policy, not an implemented feature or final balance specification.
