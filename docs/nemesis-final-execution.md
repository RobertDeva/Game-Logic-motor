# Nemesis final execution

**Status:** approved gameplay rule; lifecycle implementation pending.

## 1. Purpose

A Nemesis can be permanently eliminated through a special second finisher chained to a normal Muertebendita execution. This is distinct from ordinary enemy defeat and from the normal execution resolver.

The sequence is a deliberate double finisher: the first execution defeats the Nemesis in the encounter; the second, optional contextual action ends that individual's persistent Nemesis identity.

## 2. Approved sequence

1. **Vulnerability:** the Nemesis becomes eligible for a normal Muertebendita execution under the existing posture/vitality rules.
2. **Normal execution:** the player explicitly performs the ordinary Muertebendita action. It resolves according to the target's existing resistance and vitality-bar rules.
3. **Kneeling state:** for a Nemesis eligible for this special elimination, the execution transitions into a distinct defeated-but-finishable presentation. The Nemesis falls to its knees. This state is the condition that enables the final finisher.
4. **Final-finisher opportunity:** a contextual action becomes available while the Nemesis remains in that kneeling state.
5. **Permanent execution:** if the player performs the final finisher (for example, a decapitation), the Nemesis identity is permanently retired and cannot return as the same individual.

The final finisher is voluntary. The kneeling animation enables the action; it does not execute automatically.

## 3. Separation of outcomes

These outcomes must remain distinct:

- **Normal encounter defeat:** the Nemesis has lost the encounter. This alone does not erase its persistent identity.
- **Kneeling / final-finisher opportunity:** a temporary post-execution state in which the special action can be performed.
- **Recovery without final execution:** if the player does not perform the final action, the Nemesis survives, recovers from the kneeling/defeated state, and may appear again in a later encounter. Its identity, rank, and accumulated history remain intact.
- **Permanent Nemesis execution:** the final action succeeds and the persistent identity is irreversibly retired.

The normal Muertebendita resolver remains responsible for ordinary execution eligibility and resistance behavior. It must not silently perform the final finisher or retire a Nemesis record. The Nemesis lifecycle/application layer coordinates the chained state and persistent retirement.

## 4. Invariants

- Only the Nemesis targeted by the normal execution can receive its chained final finisher.
- The kneeling state is required to enable the final action.
- The final action cannot be invoked against an active, merely vulnerable, or unrelated enemy.
- A normal execution without the final action does not count as permanent Nemesis elimination.
- Declining or missing the final-finisher opportunity is not an automatic kill: the Nemesis can recover and return in a later encounter as the same individual.
- Permanent retirement is idempotent and persisted; replaying the same finalization event cannot create duplicate rewards or restore the Nemesis.
- Once permanently executed, the same Nemesis identity cannot respawn or re-enter the active world population.
- Nemesis rank, original enemy category, and the ordinary Muertebendita resistance rules are not changed by this mechanic.
- The final finisher is not a second ordinary Muertebendita execution and must not consume another vitality bar or award a second ordinary enemy-defeat reward merely because it is a second animation/action.

## 5. Presentation and content

Decapitation is an example, not a mandatory universal animation. The final action should be authored to fit the enemy's anatomy, category, equipment, and presentation. Different Nemeses may have different finishing animations while sharing the same lifecycle contract.

The Domain/Application contract should represent the kneeling opportunity and permanent retirement independently from animation playback. Presentation reports completion of the contextual final-finisher action; the authoritative lifecycle transition then persists the retirement.

## 6. Decisions still open

The following details are not established by the approved rule and must not be guessed during implementation:

- how long the kneeling opportunity remains available;
- the exact timing and presentation of recovery if the player walks away, is interrupted, or the encounter unloads;
- whether every Nemesis supports a final finisher or only configured individuals;
- whether any special contextual positioning or input is required beyond the kneeling state;
- exact animation, camera, audio, loot, and narrative treatment.

The survival outcome is now established: without the final execution, the Nemesis recovers and may return later. The precise recovery timing, interruption handling, and encounter-unload behavior remain open. Until those are decided, the implementation must not invent a timeout, automatic death, or universal eligibility policy. It may model the approved states and action contract while keeping these policies configurable or explicitly unresolved.
