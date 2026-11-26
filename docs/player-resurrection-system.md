# Sistema de resurrección del jugador

**Estado:** reglas funcionales aprobadas; primera lógica de dominio implementada.  
**Alcance:** cargas de resurrección, desbloqueo, recuperación por enemigos y descanso. La activación durante un golpe mortal, el debuff temporal y la transición a Game Over pertenecen a bloques de integración posteriores.

## Reglas funcionales

- El jugador comienza con una oportunidad de resurrección.
- Dos misiones específicas pueden desbloquear una oportunidad adicional cada una. La capacidad máxima es de tres.
- La oportunidad inicial se consume al resucitar y se restaura al regresar a una zona de descanso.
- Las oportunidades adicionales se consumen individualmente. Una oportunidad adicional gastada deja de estar disponible hasta que se recupere mediante progreso de enemigos y se regrese a una zona de descanso.
- Cada enemigo eliminado aporta progreso de recuperación según su dificultad. La cantidad concreta aportada por cada dificultad y el umbral de recuperación son datos de balance configurables; no se fijan aquí.
- El progreso se acumula incluso mientras las oportunidades adicionales recuperadas no pueden utilizarse.
- Al alcanzar el umbral, una oportunidad adicional gastada pasa a estar recuperada, pero bloqueada. Al regresar a una zona de descanso, las oportunidades recuperadas y bloqueadas vuelven a estar utilizables.
- El progreso parcial se conserva entre enemigos y descansos. Si ya no queda ninguna oportunidad adicional gastada que recuperar, no se acumula progreso desperdiciado.
- El descanso restaura siempre la oportunidad inicial, desbloquea las oportunidades adicionales que ya se hayan recuperado y no regala cargas adicionales que todavía no se hayan recuperado.

## Modelo de dominio implementado

`ResurrectionState` es un objeto de estado inmutable y puro. Mantiene por separado:

| Dato | Significado |
|---|---|
| `MaximumOpportunities` | Capacidad total desbloqueada, de 1 a 3 |
| `InitialChargeAvailable` | Disponibilidad de la oportunidad inicial |
| `AdditionalUsableCharges` | Oportunidades adicionales recuperadas y utilizables |
| `AdditionalLockedCharges` | Oportunidades adicionales recuperadas pero bloqueadas hasta descansar |
| `AdditionalSpentCharges` | Oportunidades adicionales consumidas y todavía no recuperadas |
| `DefeatProgress` | Progreso parcial hacia la siguiente recuperación |
| `ProgressRequiredPerAdditionalCharge` | Umbral suministrado por configuración |

Cada desbloqueo de misión aumenta la capacidad y habilita la nueva oportunidad adicional. `TryRecordEnemyDefeat` recibe la contribución de progreso ya determinada por los datos de dificultad del enemigo. Así el dominio no inventa recompensas por categoría.

## Límites explícitos

Este bloque no implementa todavía:
- el comando/acción que ofrece al jugador resucitar en el instante de un golpe mortal;
- la elección o prioridad de carga cuando ambas clases están disponibles;
- el debuff temporal por resurrección;
- el Game Over y el retorno al último punto de descanso;
- la persistencia del estado entre sesiones;
- la tabla de contribuciones por dificultad ni el umbral numérico de progreso.

Estas responsabilidades se añadirán en sus respectivos bloques sin mezclar la mecánica de resurrección con las ejecuciones de Muertebendita contra enemigos.
