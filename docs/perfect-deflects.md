# Perfect deflects

## Scope

Perfect deflects are bidirectional: either the player or an enemy can perform one. The domain resolver is symmetric; the caller supplies the defender's weapon/skill/mastery profile and the attacker's move data.

The resolver handles resource consequences only. It does not decide whether an input fell inside the timing window. That decision belongs to the combat-time coordinator once the project selects a canonical time unit.

## Configurable profile

Each profile supplies:

- incoming Vitality damage multiplier (0 fully neutralizes this channel; 1 leaves it unchanged);
- incoming Posture pressure multiplier;
- weapon base Posture reduction;
- incoming attack's Posture multiplier;
- additive skill and mastery bonuses;
- maximum allowed attacker Posture reduction.

The reduction is calculated as:

`min((weaponBaseReduction × attackPostureMultiplier) + skillBonus + masteryBonus, configuredMaximum)`

The applied reduction is additionally limited by the attacker's current Posture.

## Resolution

A successful perfect deflect applies the configured residual incoming impact to the defender and reduces the attacker's Posture. Both resource states are returned immutably. The original states are not mutated.

The resolver does not invent weapon values, skill bonuses, mastery bonuses, caps, or timing windows. Content supplies the numerical profile. The same resolver supports player-to-enemy and enemy-to-player interactions by swapping actor roles.

## Deferred

- Canonical combat-time unit and window matching.
- Weapon-specific base window values and improvement curves.
- Whether timing improvements expand both sides of a window or alter a specific tolerance.
- Content calibration and integration with the full combat action coordinator.
