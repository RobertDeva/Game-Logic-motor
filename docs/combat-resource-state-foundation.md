# Fundamentos del estado de recursos de combate

**Estado:** estructura de dominio implementada; resolución de daño, presión, recuperación y transiciones de fase todavía pendientes.

## 1. Vitalidad y Postura son recursos distintos

El GDD establece dos canales de combate independientes:

- Vitalidad representa la capacidad de supervivencia.
- Postura representa el equilibrio/guardia y participa en las reglas de ruptura y ejecución.

CombatantResourceState conserva ambos por separado. No se calcula daño ni se decide cómo una acción modifica estos valores.

## 2. Vitalidad segmentada

VitalityTrack representa una secuencia ordenada de barras. Esto permite modelar combatientes con una sola barra y enemigos o jefes con varias, sin convertirlas en un único depósito agregado.

Invariantes implementados:

- Cada barra tiene un identificador propio y un máximo positivo.
- Los identificadores de las barras son únicos.
- Solo la barra activa (la primera) puede estar parcialmente consumida.
- Las barras posteriores permanecen completas hasta que pasan a ser la barra activa.
- La barra activa puede actualizarse mediante una transición inmutable que valida su rango.
- Una barra solo puede retirarse de la secuencia cuando su valor actual es cero.
- Retirar la última barra produce una pista vacía, que representa Vitalidad agotada.

No se fija un máximo universal de barras. Las cantidades concretas dependen del tipo de enemigo y del diseño del encuentro.

## 3. Postura

Postura se representa mediante un ResourcePool independiente y debe tener un identificador distinto al de cualquier barra de Vitalidad. Su valor puede actualizarse mediante una transición inmutable validada.

Todavía no se implementan:
- daño y presión de Postura;
- recuperación automática ni su relación con la Vitalidad;
- reglas de desvío y ruptura;
- reinicio de Postura tras ejecuciones;
- conservación del 40 % de Postura acumulada al perder una barra de jefe;
- ventanas de vulnerabilidad y transición invulnerable.

Estas reglas se implementarán en la resolución de combate, no dentro del contenedor de recursos.

## 4. Decisiones preservadas

El contrato no presupone fórmulas de daño, defensa, mitigación, presión ni recuperación. Tampoco convierte la cantidad de barras descrita para categorías de enemigos en una restricción global del dominio.

La eliminación de una barra es una transición explícita y comprobable; no se confunde con retirar al combatiente de la simulación ni con registrar su derrota definitiva.

## 5. Pruebas

Las pruebas cubren secuencia de barras, unicidad de identificadores, máximos positivos, activación ordenada, actualización inmutable de Vitalidad y Postura, retirada de barras agotadas, agotamiento total y separación entre ambos recursos.
