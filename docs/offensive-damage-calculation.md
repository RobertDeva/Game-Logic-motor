# Cálculo ofensivo — Mythical Exodus

**Estado:** primera implementación del cálculo ofensivo de dominio.  
**Fuente matemática:** docs/combat-mathematical-model.md, secciones 5 y 22 (escalado v1.0).

## Alcance implementado

Para cada componente ofensivo se calcula:

1. Curva normalizada de cada atributo declarado, según el perfil versionado v1.0.
2. Factor de escalado: 1 + suma de (coeficiente × curva del atributo).
3. Attack Rating: potencia base × factor de escalado.
4. Multiplicadores configurados de movimiento, Arte/habilidad y condición del equipo.
5. Multiplicador universal de Voluntad: 1 + 0.15 × curva de Voluntad.
6. Potencia final del componente y suma de los componentes.

La suma de coeficientes de escalado por componente está limitada a 0.75, como establece la sección 22.3 del modelo. Cada componente declara explícitamente qué atributos participan. Agilidad, Vitalidad, Resistencia, Ruptura y otras estadísticas no escalan daño por defecto; solo lo hacen si el contenido lo declara y la regla funcional lo permite.

## Curvas v1.0

AttributeScalingCurveSet.Version1 contiene los pesos documentados para las ocho estadísticas. Cada curva usa los tramos 1–25, 26–50, 51–75 y 76–99, normalizados para que el valor 1 produzca 0 y el 99 produzca 1. Los pesos y la fórmula se centralizan en el dominio y no se dispersan como constantes por cada arma.

## Contratos

- AttributeScalingCoefficient: estadística y coeficiente explícito no negativo.
- OffensiveDamageComponentDefinition: clave única, potencia base, coeficientes y multiplicadores de movimiento, habilidad y condición.
- OffensiveDamageCalculator: función pura sobre atributos y definiciones de componentes.
- OffensiveDamageCalculationResult: salida por componente y potencia total, antes de defensa.

## Límites deliberados

Este bloque no calcula defensa plana, resistencias, vulnerabilidades, inmunidades, afinidades, críticos ni daño final aplicado. Tampoco aplica presión de Postura ni redondea valores intermedios. Voluntad se aplica una sola vez al daño de Vitalidad y no aumenta Postura. Los coeficientes y multiplicadores concretos pertenecen a los datos de armas, movimientos y Artes; no se inventan perfiles de contenido en esta implementación.
