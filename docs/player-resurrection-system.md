# Sistema de resurrección del jugador

**Estado:** reglas funcionales y flujo principal de dominio implementados.  
**Referencia de diseño:** sistema de resurrección de Sekiro, adaptado a Mythical Exodus.

## 1. Reglas aprobadas

- El jugador comienza con una oportunidad de resurrección.
- Dos misiones específicas pueden desbloquear una oportunidad adicional cada una. El máximo es tres.
- Cuando recibe un golpe mortal, el jugador decide voluntariamente si resucita. La resurrección nunca se dispara automáticamente.
- La oportunidad inicial se recupera al regresar a una zona de descanso.
- Las oportunidades adicionales se consumen individualmente. Las que no se han gastado permanecen utilizables.
- Cada enemigo eliminado aporta progreso según su dificultad. El dominio recibe la contribución configurada por contenido; no fija recompensas ni umbrales numéricos.
- El medidor puede continuar acumulando mientras existan cargas adicionales gastadas pendientes de recuperación, incluso si otras cargas recuperadas siguen bloqueadas para su uso.
- Al completar el umbral, una carga adicional gastada pasa a recuperada y bloqueada. Se habilita para uso al regresar a una zona de descanso.
- El progreso parcial se conserva entre enemigos y descansos. No se acumula progreso sin cargas adicionales gastadas que recuperar.
- Cada resurrección aplica un debuff temporal.
- Si no existe ninguna carga utilizable al recibir un golpe mortal, se produce Game Over. Si el jugador rechaza la resurrección, también se produce Game Over.
- Tras Game Over, el jugador regresa al último punto de descanso registrado.

## 2. Modelo de cargas

ResurrectionState mantiene de forma inmutable:
- capacidad desbloqueada (1–3);
- disponibilidad de la carga inicial;
- cargas adicionales utilizables;
- cargas adicionales recuperadas pero bloqueadas hasta descansar;
- cargas adicionales gastadas y pendientes de recuperación;
- progreso parcial y umbral configurado.

TryUnlockAdditionalOpportunity representa la recompensa de cada misión específica. TryRecordEnemyDefeat recibe la contribución ya calculada para la dificultad del enemigo. ReturnToRestPoint restaura la carga inicial y habilita únicamente las cargas adicionales ya recuperadas.

## 3. Flujo de vida del jugador

PlayerResurrectionState coordina el ciclo:

1. TryBeginLethalEvent: ante el golpe mortal, abre una decisión si hay al menos una carga utilizable; si no hay ninguna, entra directamente en Game Over.
2. TryResurrect: el jugador selecciona explícitamente Initial o Additional. El dominio consume exactamente esa carga, devuelve al jugador al estado Alive y activa el debuff configurado.
3. TryAcceptDeath: permite rechazar la resurrección y entrar en Game Over.
4. TryReturnAfterGameOver: devuelve el estado del jugador al último punto de descanso registrado y aplica las reglas de recuperación de cargas del descanso.
5. TryRestAt: registra un nuevo punto de descanso y actualiza las cargas.
6. TryRecordEnemyDefeat y TryUnlockAdditionalOpportunity: integran la recuperación por enemigos y las ampliaciones por misiones con el estado de jugador.

La selección de carga es explícita. No se impone una prioridad automática entre la carga inicial y las adicionales porque esa prioridad no ha sido definida como regla de diseño.

## 4. Debuff temporal

ResurrectionDebuffDefinition recibe una duración positiva expresada en ticks de simulación y uno o más StatModifier definidos por contenido.

Al resucitar, TemporaryResurrectionDebuff registra el instante de activación y el de expiración. ModifiersAt expone los modificadores mientras el debuff está activo; AdvanceTo retira el efecto al alcanzar o superar el instante de expiración.

No se han fijado estadísticas penalizadas, magnitudes, duración concreta ni reglas de acumulación. Los modificadores efectivos deben proceder de configuración. El motor de estadísticas todavía no aplica modificadores; Application deberá integrar los modificadores activos con ese pipeline cuando esté implementado.

## 5. Límites de integración

Este bloque implementa el ciclo de dominio, no la presentación ni la persistencia:
- Application/host debe detectar el impacto letal y presentar la decisión al jugador.
- Application debe aplicar los modificadores activos al cálculo efectivo de estadísticas.
- Application/host debe usar LastRestPointId para reubicar al jugador y reconstruir el estado del mundo al volver tras Game Over.
- La persistencia de cargas, progreso y punto de descanso queda pendiente del sistema de guardado.
- La tabla de progreso por dificultad y el umbral de recuperación siguen siendo datos de balance no definidos.

La Muertebendita de ejecución contra enemigos es una mecánica independiente y no comparte este flujo.
