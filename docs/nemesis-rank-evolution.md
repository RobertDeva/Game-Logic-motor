# Nemesis rank evolution catalogue

**Status:** proposed evolution design delegated for the Nemesis system. The category ceilings and evolution direction are fixed; exact promotion thresholds and numerical tuning remain intentionally undefined.

This catalogue defines the identity and gameplay purpose of all 45 Nemesis ranks. It is a content-design specification, not an instruction to apply automatic numerical bonuses. A rank grants its listed evolution when reached, but its implementation must be adapted to the enemy's archetype, animation set, equipment, abilities, and encounter context.

## 1. Shared progression rules

1. The first qualifying victory over the player creates the Nemesis at rank 1.
2. Further qualifying victories by that same Nemesis allow progressive promotion. The exact victory thresholds are a separate balancing decision and are not implied by this catalogue.
3. Rank ceilings are category-specific: normal 20, special 15, boss 10.
4. Nemesis rank does not change the enemy's original category. A normal enemy at rank 20 remains a normal-category enemy; a special at rank 15 remains special; a boss at rank 10 remains a boss.
5. A rank evolution should have a recognizable expression in combat, world behavior, appearance, equipment, or identity. Not every rank needs to increase raw combat power.
6. Evolutions are cumulative only where compatible. A content profile may replace an earlier variant with a more advanced one rather than stacking incompatible moves, equipment, or behavior.
7. No rank grants blanket immunity, removes all counterplay, guarantees a hit, or silently changes the rules of Muertebendita, posture, resurrection, or boss vulnerability.
8. Numerical changes, where eventually justified, must use the existing combat/stat systems and be explicitly configured. This catalogue defines no damage multipliers, health multipliers, resistance values, or hidden level scaling.

## 2. Normal enemies — 20 ranks

Normal Nemeses are dangerous through experience, specialization, and persistence rather than by becoming disguised bosses. Their evolutions should remain compatible with their original enemy archetype.

| Rank | Evolution | Design effect |
|---:|---|---|
| 1 | Marked rival | Gains a persistent Nemesis identity and remembers the encounter in which it defeated the player. Its appearance or name gains a recognizable distinguishing mark where supported. |
| 2 | Combat recollection | Retains one notable observation about the player's behavior and can recognize that pattern in a later encounter. |
| 3 | Battle scar | Acquires a persistent visible scar, damaged piece of equipment, trophy, or other individualized visual detail. Primarily identity, not a stat bonus. |
| 4 | Guard instinct | Can use a configured defensive response against one frequently observed attack approach, with a readable tell and exploitable recovery. |
| 5 | Pursuer | Becomes more persistent in seeking a rematch when the world and encounter rules permit it. Does not teleport, bypass locked progression, or guarantee an ambush. |
| 6 | Field equipment | Gains one archetype-appropriate equipment variation that changes a tactical option without invalidating the player's counterplay. |
| 7 | First counter | Unlocks one specific countermeasure against a repeatedly observed player pattern. It must not counter every build or action in that category. |
| 8 | Positioning instinct | Uses terrain, spacing, and repositioning more deliberately instead of repeating its original approach. |
| 9 | Pack awareness | If its archetype supports allies, coordinates one simple behavior with nearby compatible enemies. Otherwise gains an equivalent self-preservation or spacing behavior. |
| 10 | Signature move | Develops one recognizable signature attack or maneuver with a clear telegraph, a defined range, and a punishable recovery. |
| 11 | Terrain familiarity | Uses one suitable environmental feature more effectively, such as cover, elevation, a narrow passage, or a hazard. It cannot create arbitrary environmental hazards. |
| 12 | Tempered defense | Gains a situational defensive technique against one known threat. This is a behavior or move, not passive immunity or an unconditional resistance increase. |
| 13 | Feint | May use a readable feint to bait premature attacks, rolls, or parries. The feint must have a real commitment or punish window. |
| 14 | Refined sequence | Gains a short, archetype-appropriate attack sequence with at least one meaningful response opportunity for the player. |
| 15 | Veteran composure | Recovers from disruption more intelligently by choosing when to disengage, defend, or resume pressure. Does not bypass posture break or execution rules. |
| 16 | Personalized loadout | Receives a distinctive equipment configuration or variant that reinforces its combat identity. It must remain valid for the archetype and world economy. |
| 17 | Adaptive answer | Can recognize a second distinct player pattern and select between its learned responses, subject to its configured adaptive-combat capability. |
| 18 | Battlefield coordination | Uses more deliberate coordination with compatible nearby enemies, such as spacing, flanking, or staggered pressure. It does not grant command over unrelated factions. |
| 19 | Infamous rival | Its reputation becomes visible in the world where supported: recognition by suitable NPCs, faction reactions, rumors, or a distinct encounter presentation. This is a world-identity evolution, not a guaranteed combat buff. |
| 20 | Apex of its kind | Reaches the highest expression of its original archetype: a final signature variant combining its established identity and learned tactics. It remains in its original category and gains no boss-only mechanics merely by reaching this rank. |

## 3. Special enemies — 15 ranks

Special Nemeses begin with a more capable archetype and should evolve toward highly individualized elite opponents. Their upgrades can be more mechanically distinctive, but must remain legible and answerable.

| Rank | Evolution | Design effect |
|---:|---|---|
| 1 | Named adversary | Gains a persistent Nemesis identity, a distinctive presentation, and a record of the encounter that created it. |
| 2 | Tactical archive | Retains multiple relevant observations from prior encounters and can select a response according to its configured adaptive-combat level. |
| 3 | Elite armament | Gains a signature weapon or equipment variant that changes its threat profile while preserving clear attack tells. |
| 4 | Stance discipline | Unlocks a second combat stance or tactical mode, with explicit transition cues and weaknesses. |
| 5 | Counter-specialist | Develops a focused counter to one recurring player strategy. It must have a defined scope and a viable alternative response for the player. |
| 6 | Zone controller | Uses spacing and one suitable arena feature to control an area more deliberately. It cannot arbitrarily alter arena geometry. |
| 7 | Defensive adaptation | Gains a conditional defensive action that responds to observed player behavior. It does not grant permanent immunity or bypass damage calculation. |
| 8 | Synergy leader | Can coordinate a small group of compatible enemies through configured behaviors. It cannot override encounter ownership or faction rules. |
| 9 | Signature technique | Unlocks a distinctive special technique with a readable startup, valid range, recovery, and explicit resource/cooldown policy supplied by content. |
| 10 | Relic or perfected gear | Acquires a unique equipment feature or relic-like modification. Its effects must be expressed through existing stat, ability, or action contracts. |
| 11 | Linked assault | Gains a multi-action sequence or stance-linked follow-up. Each stage must remain interruptible, avoidable, blockable, or otherwise answerable according to its attack definition. |
| 12 | Arena exploitation | Uses a second environmental interaction or combines positioning with its existing arena behavior. Hazards must be telegraphed and avoid unavoidable damage loops. |
| 13 | Veteran adaptation | Can retain and use a broader tactical repertoire across encounters, without merging persistent Nemesis memory with encounter-scoped adaptive memory. |
| 14 | Ascendant form | Gains a distinct visual/combat presentation that expresses its history and specialization. This may alter its move selection and equipment, but does not change it into a boss. |
| 15 | Master adversary | Reaches the fullest expression of its special archetype: a coherent signature kit combining its learned counters, stance, and defining technique. It remains a special enemy. |

## 4. Bosses — 10 ranks

Boss Nemeses evolve through mastery of their existing identity, arena, and phase design. Their Nemesis rank must not silently rewrite the boss-phase contract.

| Rank | Evolution | Design effect |
|---:|---|---|
| 1 | Remembered sovereign | Gains persistent Nemesis identity and retains the history of the player's defeat. Its recognition/presentation changes where supported. |
| 2 | Phase recollection | Retains tactical observations across future encounters and can select configured responses in suitable phases. |
| 3 | Transition signature | Develops a distinctive, telegraphed transition behavior using an already configured boss phase transition. It does not add a phase. |
| 4 | Arena dominion | Uses an existing arena feature more deliberately, with clear warning and counterplay. It cannot arbitrarily reshape the arena. |
| 5 | Adaptive strategy | Can switch between configured strategic responses based on observed player patterns, while respecting phase restrictions and the boss's adaptive-combat profile. |
| 6 | Signature sequence | Gains a boss-specific attack sequence with readable stages and defined punish windows. Its action timings and effects are content-authored. |
| 7 | Phase expression | Gives one or more existing phases a distinctive move-selection or behavior variant. Vulnerability windows and invulnerability transition rules remain intact. |
| 8 | Environmental mastery | Integrates a second configured arena interaction into its existing moveset. Environmental attacks must be telegraphed and avoid unavoidable lockout loops. |
| 9 | Complete battle memory | Retains a broader tactical history across encounters and uses it to vary its configured phase behavior. The memory remains separate from temporary encounter memory. |
| 10 | Apex Nemesis | Reaches the final Nemesis expression of that boss: a unique, coherent presentation and configured signature behavior across its existing phases. It does not gain extra phases, bypass declared special-transition phases, remove vulnerability windows, or invalidate established defeat conditions. |

## 5. Combat fairness and integration constraints

### 5.1 Muertebendita and execution

- Nemesis rank does not automatically change an enemy's Muertebendita resistance category.
- A normal Nemesis does not become resistant solely because it has a high rank.
- A resistant Nemesis continues to follow the established execution rule: each valid execution consumes its active vitality bar and resets its single posture bar, unless vitality was already fully depleted.
- Bosses continue to follow their boss-phase and defeat rules. Nemesis rank cannot add vitality bars or override phase transitions unless a separate boss-design decision explicitly authorizes it.

### 5.2 Nemesis final execution

- Nemesis rank does not automatically unlock or disable the special final finisher.
- The approved sequence is normal Muertebendita execution, a kneeling state that enables a contextual final action, and optional permanent execution of that same Nemesis identity.
- The final action is not a second ordinary Muertebendita execution and must not consume another vitality bar or duplicate ordinary defeat rewards.
- Eligibility, timing, interruption, and presentation are lifecycle/content policies, not rank bonuses. See [Nemesis final execution](nemesis-final-execution.md).

### 5.3 Posture and recovery

- Rank evolutions may introduce new posture-pressure actions or defensive behaviors, but must use the established posture-pressure and recovery pipeline.
- An evolution cannot silently disable posture recovery, posture break, or the player's ability to pressure an enemy.
- Enemy posture recovery remains governed by the existing vitality-dependent recovery curve and encounter timing.

### 5.4 Adaptive combat

- Rank can unlock or configure additional tactical options, but does not automatically increase AdaptiveCombatLevel.
- The content profile must explicitly declare which adaptive capabilities a Nemesis receives.
- Persistent Nemesis history may inform the encounter's adaptive memory; the two records retain separate lifecycles.

### 5.5 Boss phases

- Boss Nemesis evolution must preserve the configured phase count and transition contract.
- Existing vulnerability and invulnerability durations remain authoritative.
- Special transition attacks remain restricted to phases explicitly declared by the boss profile.
- One impact must not skip multiple boss bars/phases as a side effect of Nemesis evolution.

### 5.6 Player counterplay

Every combat-affecting evolution must define:
- what player behavior or situation triggers it;
- how the player can recognize it;
- at least one practical response or counterplay path;
- its commitment, recovery, limitation, or cost;
- how it interacts with posture, vitality, and existing action validation.

Cosmetic, narrative, or world-reputation evolutions do not need combat counterplay, but must not secretly modify combat values.

## 6. Content authoring model

The catalogue is a design taxonomy, not a universal hard-coded moveset. Each enemy archetype should author compatible evolution definitions. An evolution definition should identify:

- category and rank;
- stable evolution identifier;
- evolution kind (identity, visual, equipment, tactical behavior, action, coordination, or world reaction);
- prerequisites and incompatibilities;
- granted or replaced capabilities;
- explicit combat effects, if any, expressed through existing domain contracts;
- telegraph and counterplay metadata for combat-affecting actions;
- persistence data needed to restore the evolution.

A rank may have multiple archetype-specific implementations of the same design intent. For example, a ranged enemy's signature move differs from a heavy melee enemy's signature move, while both can fulfill the rank's signature-technique role.

## 7. Deliberately unresolved balancing decisions

This catalogue does not decide:
- how many repeat victories are required for each promotion;
- whether the threshold is fixed, escalating, or customized by category/rank;
- whether one defeat can promote several enemies or how responsibility is assigned when multiple enemies contribute;
- exact numerical changes, action values, durations, cooldowns, and resource costs;
- limits on simultaneous active Nemeses;
- how retreat, permanent defeat, replacement, and world population are handled;
- whether a Nemesis can lose rank or evolve through non-victory events.

Those rules must be designed separately and then connected to this catalogue. Do not infer them from rank descriptions.
