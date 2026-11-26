# Presión y recuperación de Postura — Mythical Exodus

**Estado:** cálculo configurable de presión y recuperación implementados.  
**Fuente:** sección 8 del modelo matemático del combate y regla funcional de recuperación de enemigos.

## Presión de Postura

El dominio aplica la fórmula documentada:

Q = (Q_base + Q_escala) × M_ataque × M_respuesta × M_afinidad

Q_recibida = Q × (1 − R_postura) × M_postura

PosturePressureInput recibe los componentes y multiplicadores ya definidos por el contenido/acción. La resistencia de Postura se valida entre 0 y 1. El factor de afinidad es explícito y por defecto neutral; no se hereda automáticamente la afinidad elemental de daño, porque el GDD exige que cada canal declare si recibe ese efecto.

No se inventan coeficientes para calcular Q_escala a partir de atributos o armas. El valor llega resuelto por el contenido o por una regla posterior de escalado específica.

## Recuperación

Los enemigos recuperan Postura cuando no reciben presión. La recuperación disminuye al bajar su proporción de Vitalidad:

v_rec = v_base × F_vitalidad(Vitalidad_actual / Vitalidad_máxima)

PostureRecoveryCurve implementa interpolación lineal entre puntos de configuración. Exige puntos en los extremos 0 y 1, ratios estrictamente crecientes y multiplicadores no decrecientes con la Vitalidad. La curva puede fijar un mínimo cero o positivo según el perfil; sus puntos reales no se inventan.

PostureRecoveryCalculator devuelve cero de recuperación cuando se recibió presión en el tick. Si no hubo presión, calcula la tasa usando la Vitalidad actual. PostureRecoveryApplier aplica la cantidad calculada sin reducir Postura por debajo de cero y sin mutar el estado original.

## Orden temporal

El cálculo distingue presión recibida durante el tick de recuperación permitida. Application debe ordenar los eventos de modo que la presión de un impacto no se reduzca mediante una recuperación procesada antes del impacto en el mismo instante. La política de ticks/eventos y la programación automática del reloj pertenecen al coordinador de simulación.

## Límites

No se fijan valores base, puntos de curva, coeficientes de escalado ni reglas de recuperación del jugador. La regla confirmada de recuperación automática aplica a enemigos; no se extrapola al jugador sin decisión funcional. Ruptura de Postura, ventana de vulnerabilidad y Muertebendita se implementarán como etapas posteriores.
