# Nemesis progression policy

**Status:** the rank-threshold schedule and immutable Domain progression state are implemented. Encounter attribution, event orchestration, and durable persistence remain Application/infrastructure responsibilities and are not implemented here.

## 1. Progression rules

- Any enemy category can become a Nemesis after its first qualifying victory over the player.
- That first defeat creates the persistent Nemesis at rank 1 and records the creation encounter. It does not also award growth progress.
- Later qualifying defeats by the same Nemesis can award one growth mark per distinct encounter.
- A promotion consumes the required marks and advances exactly one rank.
- The enemy's original category is immutable throughout Nemesis rank progression.
- At the category ceiling, no additional rank growth marks are accumulated.

Nemesis Growth Marks are not player souls, currency, enemy level, or encounter-scoped adaptive memory.

## 2. Category ceilings

| Original enemy category | Maximum Nemesis rank |
|---|---:|
| Normal | 20 |
| Special | 15 |
| Boss | 10 |

## 3. Escalating promotion threshold

For a Nemesis currently at rank r, the required marks for its next rank are:

T(r) = 1 + floor((r - 1) / 5)

| Current rank | Marks required |
|---:|---:|
| 1–5 | 1 |
| 6–10 | 2 |
| 11–15 | 3 |
| 16–20 | 4 |

The threshold is evaluated from the current rank. Growth marks persist when the Nemesis loses an encounter; they are not reset by an ordinary defeat of the Nemesis.

### Total later qualifying defeats to reach the ceiling

| Category | Maximum rank | Later qualifying defeats after creation |
|---|---:|---:|
| Normal | 20 | 46 |
| Special | 15 | 27 |
| Boss | 10 | 13 |

These totals are produced by the threshold schedule and are useful balance targets for later playtesting. They are not additional independent rules.

## 4. Idempotency

The Domain progression state stores credited encounter identifiers.

- The encounter that created the rank-1 Nemesis is already marked as credited.
- Reprocessing a credited encounter returns a duplicate status and leaves progression unchanged.
- Each new encounter identifier can add at most one growth mark and promote at most one rank.
- At the rank ceiling, new encounter identifiers are recorded but do not accumulate unused growth marks.

The state is immutable: recording a defeat returns a new state and does not mutate the previous instance.

## 5. Authoritative encounter credit boundary

The Domain progression method accepts a Nemesis and encounter identifier only after the Application layer has determined that this Nemesis is responsible for the player's authoritative defeat.

It does not infer responsibility from damage, participation, posture breaks, vitality-bar depletion, Muertebendita execution, boss phase transitions, or proximity to the player.

The following remain outside this Domain implementation:
- selecting a primary source when several enemies contribute to a lethal outcome;
- preserving causal source identity for delayed damage or environmental chains;
- ordering defeat recording against voluntary resurrection;
- durable atomic persistence of encounter credit and rank transitions;
- enemy retirement, respawn, and replacement lifecycle.

Application must supply a stable encounter identifier and persist credited identifiers with the Nemesis record. The encounter result must be processed before the resurrection choice can discard or alter the defeat consequence.

## 6. Relationship to other systems

- Nemesis rank does not change enemy category, Muertebendita resistance, vitality-bar count, or boss phase count.
- Nemesis rank does not automatically increase adaptive-combat capability level.
- Persistent Nemesis history and encounter-scoped adaptive memory retain separate lifecycles.
- Rank-specific evolution content follows the Nemesis rank evolution catalogue and must preserve player counterplay.

## 7. Implementation boundary

Implemented Domain types:
- Nemesis enemy category and category ceilings;
- immutable Nemesis progression state;
- growth marks and rank threshold evaluation;
- encounter-id idempotency;
- rank-cap behavior.

Not yet implemented:
- creation/advancement from the authoritative encounter outcome;
- multi-enemy and environmental attribution;
- persistence repository and transaction handling;
- world population, retirement, and respawn integration;
- applying rank evolution definitions to enemy archetypes.
