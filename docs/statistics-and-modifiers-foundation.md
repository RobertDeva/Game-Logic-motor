# Fundamentos de estadísticas y modificadores

**Proyecto:** Mythical Exodus  
**Estado:** contratos de almacenamiento implementados; resolución matemática pendiente.

## 1. Separación del modelo

El dominio distingue tres conceptos:

- **Atributos primarios:** los ocho atributos del personaje, almacenados en `CharacterAttributes`.
- **Estadísticas:** valores identificados independientemente de los atributos primarios.
- **Modificadores:** descripciones inmutables de cambios asociados a una fuente y dirigidos a una estadística.

Una estadística no se calcula dentro de `CharacterAttributes`. Tampoco se asume que cada atributo corresponda a una sola estadística.

## 2. Contratos implementados

- `StatId`: identificador textual estable; rechaza valores vacíos y normaliza espacios exteriores.
- `StatValue`: conserva por separado el valor base y el efectivo. No calcula uno a partir del otro.
- `StatModifierOperation`: vocabulario inicial de operaciones: plana, porcentaje aditivo, multiplicador y sustitución.
- `StatModifier`: identidad propia, identidad de la fuente, estadística objetivo, operación y magnitud.

Se utiliza `decimal` para representar estos valores de dominio sin introducir NaN o infinito. La precisión final y el redondeo de fórmulas todavía deben definirse por regla de juego.

## 3. Decisiones deliberadamente no implementadas

Estos contratos no deciden:

- cómo se calcula una estadística a partir de atributos, equipo u otros datos;
- el orden de aplicación de operaciones;
- si los modificadores se acumulan, se sustituyen, se excluyen o se limitan;
- si porcentajes se suman entre sí o se aplican secuencialmente;
- cómo se resuelven varios modificadores de sustitución;
- duración, expiración, activación, pausa o eliminación;
- prioridad, procedencia compuesta o interacción entre efectos;
- redondeo intermedio o final.

Los cuatro tipos de operación son vocabulario explícito, no una política de cálculo ya aprobada. No debe escribirse un evaluador hasta que las reglas anteriores estén respaldadas por el diseño funcional.

## 4. Identidad y ciclo de vida

Cada modificador tiene un identificador propio y un identificador de fuente. La fuente permite rastrear qué entidad originó el modificador, pero no implica que eliminarla retire automáticamente el modificador. Esa relación se definirá junto con el sistema de efectos y su ciclo de vida.

No se introduce duración en este contrato. Los modificadores permanentes y temporales requieren reglas explícitas de expiración y eliminación, integradas con el reloj lógico existente.

## 5. Pruebas

Las pruebas cubren validación de identificadores, conservación independiente de valores base/efectivo, identidad y procedencia del modificador, y rechazo de operaciones desconocidas.
