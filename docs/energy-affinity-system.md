# Matriz de afinidades energéticas — Mythical Exodus

**Estado:** topología y multiplicadores funcionales implementados.  
**Fuente:** diagrama original transcrito en la sección 7 de docs/combat-mathematical-model.md.

## Tipos de energía

Aura, Maná (Mana), Prana, Energía Natural (NaturalEnergy), Energía Abisal (AbyssalEnergy) y Poder Astral (AstralPower).

## Relaciones dirigidas

La flecha sale de la energía atacante con ventaja y apunta a la energía defensora:

| Atacante con ventaja | Energía defensora |
|---|---|
| Energía Natural | Aura |
| Energía Abisal | Aura |
| Energía Natural | Maná |
| Energía Abisal | Prana |
| Energía Natural | Prana |
| Energía Abisal | Maná |
| Aura | Maná |
| Prana | Aura |
| Maná | Prana |
| Poder Astral | Aura |
| Poder Astral | Maná |
| Poder Astral | Prana |

La dirección inversa produce desventaja. No se infieren relaciones nuevas por simetría.

## Pares recíprocos

- Energía Natural ↔ Energía Abisal.
- Energía Natural ↔ Poder Astral.
- Energía Abisal ↔ Poder Astral.

Estos pares utilizan ×1.25 en ambas direcciones y sustituyen los multiplicadores ordinarios de ventaja/desventaja.

## Multiplicadores

| Relación | Multiplicador |
|---|---:|
| Ventaja | ×1.50 |
| Desventaja | ×0.75 |
| Recíproca | ×1.25 en ambos sentidos |
| Neutral | ×1.00 |

La misma energía contra sí misma es neutral. Las relaciones no enumeradas también son neutrales.

## Implementación

EnergyAffinityMatrix contiene la topología versionada. EnergyAffinityResolver aplica el multiplicador una sola vez a la potencia del componente energético y devuelve un resultado auditable. La composición con los cálculos ofensivo y defensivo debe invocar esta etapa una sola vez por componente compatible.

La afinidad no modifica automáticamente presión de Postura, estados, duración ni probabilidad de efectos. Esos canales requieren reglas separadas si el contenido debe afectarlos.
