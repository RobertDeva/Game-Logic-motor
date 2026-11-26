# Muertebendita — elegibilidad y ejecución de enemigos

**Estado:** primera resolución de ejecución implementada para enemigos normales y resistentes.  
**Regla esencial:** la vulnerabilidad habilita la acción, pero nunca ejecuta automáticamente.

## Elegibilidad

Muertebendita puede solicitarse manualmente cuando se cumple al menos una condición:
- la Postura del objetivo ha alcanzado su máximo (ruptura);
- la Vitalidad total del objetivo está agotada.

Si no se cumple ninguna, la solicitud se rechaza con un error tipado.

## Enemigos normales

Una ejecución válida derrota inmediatamente al enemigo. La resolución informa la derrota al coordinador, que será responsable de finalizar el encuentro y actualizar el ciclo de vida de la entidad.

## Enemigos resistentes

Una ejecución válida:
- consume una barra completa de Vitalidad, incluso si esa barra estaba parcialmente dañada;
- reinicia la única barra de Postura del objetivo a cero;
- derrota al enemigo cuando ya no quedan barras de Vitalidad.

Si la Vitalidad total ya estaba agotada antes de la ejecución, la ejecución confirma la derrota y reinicia Postura sin consumir otra barra.

## Contratos implementados

- VitalityTrack.TryConsumeCurrentBar: elimina la barra activa completa.
- CombatantResourceState.TryConsumeCurrentVitalityBar y TryResetPosture: transiciones inmutables.
- MuertebenditaExecutionResolver: valida la vulnerabilidad y resuelve la ejecución según el perfil normal/resistente.
- MuertebenditaExecutionResult: estado actualizado, derrota, barras consumidas y reinicio de Postura.

La resolución solo ocurre cuando el sistema de acciones invoca explícitamente Execute. No existe una llamada automática al romper Postura o agotar Vitalidad.

## Límites pendientes

- Los perfiles de jefes, sus 3–5 barras y transición de fase se implementarán en el bloque de jefes.
- Las animaciones, alcance contextual, orientación y ventana de interacción pertenecen a la capa de presentación/host.
- La validación de distancia y posición se añadirá cuando exista el modelo espacial de combate.
- No se han añadido reglas de invulnerabilidad ni tiempos de ejecución no definidos.
