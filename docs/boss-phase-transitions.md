# Fases y transiciones de jefes — Mythical Exodus

**Estado:** estado de fases y ventanas de transición implementados; comportamiento adaptativo pendiente.

## Reglas implementadas

- Un jefe se configura con 3, 4 o 5 fases, cada una respaldada por una barra de Vitalidad.
- Cuando se pierde exactamente una barra de forma permanente, se avanza a la siguiente fase.
- La Postura acumulada se conserva al 40 % de su valor anterior (reducción del 60 %); no se modifica la capacidad máxima.
- La nueva fase comienza con una ventana vulnerable de duración configurable.
- Al terminar esa ventana, el jefe pasa a una transición invulnerable de duración configurable.
- Al terminar la transición invulnerable, vuelve al estado de combate activo.
- Los ataques especiales de transición solo pueden habilitarse en fases marcadas explícitamente por el perfil de contenido y solo durante la transición invulnerable.
- La pérdida de la última barra marca al jefe como derrotado y no inicia una fase adicional.

## Integridad de barras

El estado exige que cada transición corresponda a exactamente una barra menos que la esperada para la fase actual. Si un impacto elimina varias barras de una vez, se devuelve un error tipado en vez de saltar silenciosamente fases. El coordinador de combate deberá impedir o resolver explícitamente ese caso antes de avanzar la fase.

## Contratos

- BossPhaseProfile: cantidad de fases y fases que admiten ataques especiales de transición.
- BossPhaseState: fase actual, recursos, estado de transición y marcas temporales lógicas.
- TryAdvanceAfterBarDepletion: valida la pérdida de una sola barra, conserva 40 % de Postura y comienza la nueva fase.
- AdvanceTo: transita por ventana vulnerable, transición invulnerable y combate activo.

## Pendiente

La memoria de patrones del jugador, adaptación de defensa/agresión/recolocación y conservación de esa memoria entre fases pertenecen al futuro sistema de IA de jefes. No se han inventado patrones, tiempos ni comportamiento de combate. La definición exacta de qué fases pueden usar ataques especiales se configura por jefe; no se fija una interpretación numérica de «últimas fases».
