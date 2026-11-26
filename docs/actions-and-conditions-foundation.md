# Fundamentos de acciones y condiciones

**Estado:** contratos de dominio implementados; acciones concretas y resolución de combate pendientes.

## 1. Separación de responsabilidades

Una acción representa una operación que el juego podría intentar realizar. Sus condiciones determinan si el contexto actual permite intentarla. La validación no ejecuta la acción, no consume recursos y no modifica entidades.

La resolución será una etapa separada que recibirá una acción validada y producirá una transición de estado o un rechazo tipado.

## 2. Identidad de acciones

ActionId proporciona un identificador estable y textual para definiciones de acciones. Los identificadores no dependen de nombres de interfaz ni de localización.

## 3. Condiciones puras

IGameCondition<TContext> evalúa un contexto explícito y devuelve ConditionEvaluation. Las implementaciones deben ser puras: no cambian el contexto, no consumen recursos, no publican eventos y no generan efectos secundarios.

ConditionSet<TContext>:
- conserva el orden de declaración;
- evalúa todas las condiciones, sin cortocircuito;
- acumula todos los motivos de rechazo;
- devuelve una evaluación inmutable;
- no define cómo corregir una condición fallida.

Esto permite mostrar al jugador todos los motivos por los que una acción no está disponible, y mantiene el resultado determinista cuando las condiciones son puras.

## 4. Definición de acción

ActionDefinition<TContext> agrupa un identificador y un conjunto de condiciones. Validate solo evalúa precondiciones. No representa todavía un ataque, esquiva, desvío, habilidad o interacción concreta.

## 5. Límites deliberados

Todavía no se definen:
- costes de Aura, Maná, Prana u otros recursos;
- alcance, línea de visión, orientación o ventanas temporales;
- prioridad o interrupción entre acciones;
- fórmulas de daño o presión de Postura;
- consumo de recursos al iniciar, completar o interrumpir una acción;
- resolución, eventos, efectos o postacciones.

Estas reglas requieren contextos y políticas específicas del GDD. No se incorporan valores provisionales en contratos genéricos.

## 6. Pruebas

Las pruebas verifican conjuntos vacíos, evaluación ordenada, acumulación de errores, instantánea inmutable de rechazos, rechazo de condiciones nulas e integración de precondiciones en una definición de acción.
