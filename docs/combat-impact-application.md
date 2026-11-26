# Aplicación de impactos a recursos de combate

**Estado:** aplicación de impactos resuelta y probada; cálculo ofensivo/defensivo y reglas de ruptura/ejecución siguen pendientes.

## 1. Responsabilidad

`CombatResourceImpact` transporta dos magnitudes ya calculadas:
- daño destinado a Vitalidad;
- presión destinada a Postura.

No contiene tipo de daño, defensa, afinidad, escalado, crítico ni fórmula. Esos datos pertenecen a la futura resolución ofensiva/defensiva.

## 2. Transición inmutable

`CombatResourceImpactApplier` aplica ambos canales a un `CombatantResourceState` y devuelve un `CombatImpactResult`. El estado original nunca se modifica.

La aplicación:
- resta daño de la barra activa;
- cuando una barra llega a cero, la retira y continúa con la siguiente si queda daño;
- conserva el orden de las barras;
- limita el daño aplicado a la Vitalidad restante;
- informa el exceso de daño no absorbido como `VitalityDamageOverflow`;
- suma presión a Postura sin superar su máximo;
- informa cuánto daño y presión se aplicaron realmente;
- informa cuántas barras se agotaron durante la transición.

La transición de varias barras es atómica desde el punto de vista del llamador: se recibe el estado nuevo completo o un error, nunca un estado parcialmente mutado.

## 3. Interpretación de resultados

`VitalityDamageApplied` representa la cantidad efectivamente retirada de las barras disponibles. `VitalityDamageOverflow` es el exceso que queda después de agotar todas las barras; no se aplica a una barra inexistente.

`PosturePressureApplied` representa el incremento efectivo de Postura. Si la Postura ya está en su máximo, el incremento aplicado será cero.

`VitalityBarsDepleted` cuenta las barras retiradas durante esta aplicación, no las fases de jefe ni recompensas. La transición de fase, la conservación del 40 % de Postura y la vulnerabilidad posterior son responsabilidades posteriores.

## 4. Límites funcionales

Este bloque no decide:
- cómo se calcula el daño de un ataque;
- defensa plana, resistencia, vulnerabilidad, inmunidad o afinidades;
- si un ataque genera presión y cuánto;
- recuperación de Postura;
- ruptura y Muertebendita;
- ejecución de enemigos resistentes;
- fases, transiciones o invulnerabilidad de jefes;
- efectos secundarios, estados o durabilidad.

En particular, agotar una barra no equivale aquí a derrotar definitivamente al combatiente. Solo actualiza el recurso y comunica cuántas barras se agotaron.

## 5. Pruebas

Las pruebas cubren aplicación simultánea de canales independientes, overflow entre barras, agotamiento completo, presión de Postura limitada, impacto cero, inmutabilidad y validación de magnitudes.
