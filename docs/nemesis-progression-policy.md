# Nemesis progression policy

**Status:** rank progression is implemented in Domain. Application coordinates authoritative defeat outcomes before resurrection and persists through a repository contract. Combat impacts preserve optional causal source identity. A concrete durable adapter and encounter-side attribution policy remain pending.

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

The previous schedule is retained as the baseline, but its cumulative cost is reduced to 70%:

1. Calculate the baseline cumulative marks required to reach the next rank using the original schedule.
2. Multiply that cumulative total by 0.70 and round to the nearest integer (midpoints away from zero).
3. Subtract the marks already required by the rebased schedule to reach the current rank.
4. Require at least one mark for every promotion.

This cumulative method avoids fractional victories and ensures the total cost at each category ceiling is the rounded 70% target. The threshold can vary by category because each category has a different ceiling. Growth marks persist when the Nemesis loses an encounter; they are not reset by an ordinary defeat of the Nemesis.

### Total later qualifying defeats to reach the ceiling

| Category | Maximum rank | Later qualifying defeats after creation |
|---|---:|---:|
| Normal | 20 | 46 | **32** |
| Special | 15 | 27 | **19** |
| Boss | 10 | 13 | **9** |

The rebased totals are respectively 30.4%, 29.6%, and 30.8% lower than the baseline; the small differences from exactly 30% are caused by integer rounding.

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

Combat impact results preserve an optional source entity ID, allowing delayed damage and environmental chains to retain their initiating enemy when the host supplies that provenance. Application accepts at most one already-authoritative enemy credit; the encounter host must resolve attribution when multiple sources contribute. The coordinator records the credit before opening the resurrection decision and uses atomic repository contract operations. A concrete durable adapter and enemy retirement/respawn lifecycle remain pending.

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
- growth marks and cumulative 70%-scaled rank threshold evaluation;
- encounter-id idempotency;
- rank-cap behavior;
- Nemesis lifecycle states for active, kneeling for a final execution, and permanently retired;
- explicit recovery transition that preserves the same Nemesis identity and remembers the previous execution identifier for replay protection;
- final-action completion that is idempotent and blocks further rank progression after permanent retirement.

Application and persistence also coordinate and store these lifecycle transitions in the same Nemesis progression snapshot. Opening the opportunity requires both an actual `EnemyDefeated` result from normal Muertebendita and explicit per-content opt-in. Recovery timing remains owned by the encounter/world host; no timeout is hard-coded. The world-population coordinator returns active identities that are not currently present, excluding kneeling and permanently retired Nemeses. After the host records recovery, the same identity becomes eligible for a later spawn query.

Not yet implemented:
- authoritative attribution policy for simultaneous lethal sources and environmental causes;
- concrete durable persistence adapter and transaction handling;
- actual actor creation, world spawn-point selection, encounter placement, and host-side world presence tracking;
- applying rank evolution definitions to enemy archetypes.
