# Cálculo defensivo — Mythical Exodus

**Estado:** primera implementación del cálculo defensivo de dominio.  
**Fuente matemática:** docs/combat-mathematical-model.md, secciones 6 y 22.

## Fórmula aplicada por componente

Para cada componente entrante y su perfil de defensa por tipo de daño:

- Defensa plana: potencia posterior = max(0, potencia ofensiva − defensa plana).
- Resistencia estadística: contribución = límite configurado por tipo × curva v1.0 de Resistencia (atributo Endurance en el código).
- Resistencia total: clamp(contribución estadística + resistencia de equipo + resistencia de estado, mínimo configurado, máximo configurado).
- Daño final: potencia posterior × (1 − resistencia total) × multiplicador de vulnerabilidad.
- Inmunidad explícita: bloquea el componente y produce cero daño, independientemente de los demás parámetros.

El límite de contribución de Resistencia estadística no puede superar 0,20 (20 %) por tipo, como establece el modelo v1.0. Los límites mínimo y máximo de resistencia total se suministran por contenido; el máximo global definitivo no se inventa aquí. Las resistencias de equipo y estado pueden ser negativas para representar reducciones, pero el resultado queda dentro de los límites configurados. Vulnerabilidad es un multiplicador separado.

## Contratos

- DamageTypeId: identificador estable del tipo de daño.
- IncomingDamageComponent: clave del componente, tipo y potencia ofensiva ya calculada.
- DamageResistanceProfile: defensa plana, contribución máxima de Resistencia, resistencias de equipo/estado, límites totales, vulnerabilidad e inmunidad.
- DefensiveDamageCalculator: calcula cada componente independientemente y suma el daño final previo a la aplicación a Vitalidad.
- DefensiveDamageCalculationResult: detalle auditable por componente y total.

## Límites

Este bloque no modifica la potencia ofensiva de origen, no calcula afinidades energéticas ni críticos, no aplica redondeo y no cambia recursos. El resultado todavía debe pasar por CombatResourceImpactApplier para aplicarse a Vitalidad. Los perfiles por armadura, enemigo y tipo de daño son datos de contenido y no se inventan como presets de balance.
