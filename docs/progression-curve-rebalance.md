# Rebalance proposal: first-run progression to level 100 in 40–60 hours

**Status:** revised mathematical proposal for simulation and playtesting. It replaces the earlier level-cost curve only if approved after testing; it is not yet implemented in C#.

## 1. Design targets

- Reach level 100 in approximately 40–60 active hours during a normal first playthrough.
- Level 225 is the recommended preparation level for completing the main campaign on the first world cycle.
- Level 225 is guidance, not a hard gate: skilled players may complete the campaign below it.
- Preserve the very long-term progression beyond the campaign and the high maximum levels:
  - Magic Swordsman: 752.
  - Warrior: 753.
  - Reaper: 753.
- Keep NG+ as the long-term challenge for highly dedicated players.
- Do not reduce all costs uniformly; accelerate early/mid progression and preserve a demanding endgame.

## 2. Proposed cost adjustment

Keep the existing base cost function C(x):
- For target levels 2–12, use the existing Dark Souls III reference table.
- For target levels 13 and above, use round(0.02*x^3 + 3.06*x^2 + 105.6*x - 895).

Apply a level-dependent multiplier F(x):

- F(x)=1 for x<=12.
- For 12<x<=50: F(x)=1 - 0.45*(x-12)/38 (linear transition from 1.00 to 0.55).
- For 50<x<=225: F(x)=0.55.
- For 225<x<=500: F(x)=0.55 + 0.45*(x-225)/275 (linear return from 0.55 to 1.00).
- For x>500: F(x)=1.

New level-up cost: C_new(x)=max(1, round(C(x)*F(x))).

This makes the discount gradual rather than creating a sudden price drop immediately after level 12. It preserves the original curve beyond level 500, retaining the long-term aspiration.

## 3. Cumulative cost results

Costs are summed from target level 2 through the listed target level.

| Target level | Original cumulative cost | Proposed cumulative cost |
|---:|---:|---:|
| 50 | 262,488 | 184,201 |
| 100 | 1,997,916 | 1,138,689 |
| 150 | 7,112,519 | 3,951,722 |
| 200 | 18,253,797 | 10,079,423 |
| 225 | 27,116,861 | 14,954,109 |
| 300 | 72,956,378 | 43,354,558 |
| 400 | 181,094,? | 142,361,199 |
| 500 | 454,421,640 | 376,128,916 |
| 752 | 2,067,093,093 | 1,988,800,369 |
| 753 | 2,077,445,918 | 1,999,153,194 |

The original cumulative total at level 400 should be read from the original progression model; the proposed cumulative value is 142,361,199. The key comparison is that level 100 is reduced by about 43%, level 225 by about 45%, while the maximum-level total falls by only about 3.8%. Thus the proposal accelerates the first campaign without removing the hardcore endgame.

## 4. First-playthrough hybrid-income simulation

Use an illustrative hybrid activity mix:
- 50% combat and encounter rewards.
- 30% paid professional services (smithing, armoring, alchemy, processing contracts, etc.).
- 20% net trade profit after material acquisition, processing, fees, transport and other operating costs.

These percentages are scenario assumptions, not final design constraints. Gathering and production do not directly create souls; payment for a service or a sale is the income event. Do not count the same resource both as a production reward and as sale revenue.

### Level 1 to 100 target

The proposed cumulative level cost is 1,138,689 souls.

A net average of 19,000–28,500 souls/hour gives approximately 60–40 hours respectively. At a central planning rate of 23,000 net souls/hour:

1,138,689 / 23,000 = 49.5 active hours

This meets the requested 40–60 hour target. Net income means after operating costs and normal expedition expenses, not gross sales.

### Level 100 to 225

| Segment | Additional level-up cost | Illustrative net income rate | Estimated active time |
|---|---:|---:|---:|
| 101–150 | 2,813,033 | 35,000 souls/h | 80.4 h |
| 151–200 | 6,127,701 | 55,000 souls/h | 111.4 h |
| 201–225 | 4,874,686 | 75,000 souls/h | 65.0 h |
| **Total 101–225** | **13,815,420** | — | **256.8 h** |

Estimated active time from level 1 to level 225: about **306 hours**, including the 49.5 hours to level 100.

The increasing net income rates assume the player gains access to more dangerous encounters, higher-value professional commissions, better recipes, customers, markets and regions. These rates must be verified in actual playtests. The hybrid mix is a planning scenario; no profession or activity is guaranteed a fixed share.

## 5. Interpretation of level 225

Level 225 is the recommended readiness target for the final campaign, not a mandatory lock or an exact power guarantee. Two characters at level 225 may have very different combat effectiveness because of attribute allocation, weapon mastery, equipment, Combat Arts, resistances and player skill.

Campaign bosses and regions must therefore be balanced against build capability and equipment, not only the general level.

## 6. Equipment and other progression costs

The level-up estimates above do not include the extra time and souls required for:
- Acquiring exceptional weapons and armor.
- Gathering or purchasing high-grade materials.
- Processing materials and crafting.
- Reinforcing weapons from +0 to +10.
- Repairing and maintaining equipment.
- Buying or making recipes, tools and workshop access.
- Alchemy ingredients and healing-potion stock.

The previous provisional +10 model estimated 0.70 B(r) in materials plus 0.20 B(r) in NPC labor per weapon. It remains a test assumption, not a final price. A player crafting personally avoids the NPC labor fee but still bears materials, tools, facility, maintenance and time costs.

Healing charges still refill only at level-up/rest points by consuming previously crafted or purchased potions; resting never creates free potions.

## 7. Balance safeguards

- Do not inflate gross enemy rewards to compensate for a curve discount without measuring net progression.
- Ensure combat, professions and hybrid play can all support the expected net income.
- Prevent guaranteed trade arbitrage and repeated-service loops.
- Keep equipment investment competitive with attribute purchases; players should face meaningful choices without being trapped.
- Test early-zone farming, high-cycle farming, boss reward repetition and overkill-bonus exploits.
- Test players who reach level 225 with very different builds and equipment.
- Check whether the 40–60 hour target is achieved by normal play, not by repetitive grinding or optimal-route exploitation.

## 8. Recommendation

Adopt this as the next candidate curve for controlled playtesting. It meets the requested level-100 time target under an explicit, plausible-to-test net income range, places level 225 at approximately 306 active hours under the illustrative hybrid scenario, and preserves almost all of the original maximum-level cost.

Do not call it final balance until playtest telemetry confirms the assumed net income rates, campaign length and equipment costs.