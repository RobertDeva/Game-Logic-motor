# Fundamentos del dominio implementados

**Proyecto:** Mythical Exodus  
**Estado:** primera implementación funcional, validada por CI  
**Última validación:** commit `0c27d91554912b665504a3f0ce050954c5ae7373`

Este documento registra los contratos que ya existen en código y separa explícitamente lo implementado de las decisiones que siguen pendientes.

## 1. Identidad

Archivo: `src/MythicalExodus.Domain/Primitives/EntityId.cs`

- Identificador basado en GUID.
- Creación mediante `EntityId.New()`.
- Se rechaza explícitamente `Guid.Empty`.
- El valor por defecto de un `struct` sigue siendo técnicamente posible en C#; `IsEmpty` permite detectarlo y las entidades lo rechazan al construirse.
- La identidad no codifica tipo, facción, ubicación ni estado de juego.

## 2. Entidades y ciclo de vida técnico

Archivos:
- `src/MythicalExodus.Domain/Entities/DomainEntity.cs`
- `src/MythicalExodus.Domain/Entities/EntityLifecycleState.cs`

Toda entidad concreta hereda de `DomainEntity` y recibe su identidad desde fuera. La clase base proporciona:

- Identidad estable e igualdad por identidad.
- Estado técnico inicial `Active`.
- Transición terminal `Removed` mediante `Remove()`.
- Eliminación idempotente: la primera llamada devuelve `true`; las posteriores, `false`.

**Importante:** `Removed` significa que la entidad deja de participar en la simulación. No significa muerto, derrotado, inconsciente, destruido narrativamente ni eliminado del inventario. Esos estados pertenecen a modelos especializados. Una entidad puede morir y permanecer en el mundo como cadáver; el dominio concreto decidirá cuándo corresponde retirarla de la simulación.

## 3. Atributos primarios del personaje

Archivos:
- `src/MythicalExodus.Domain/Characters/CharacterAttribute.cs`
- `src/MythicalExodus.Domain/Characters/CharacterAttributes.cs`

Se modelan los ocho atributos confirmados:

1. Vitality
2. Strength
3. Agility
4. Endurance
5. Willpower
6. Rupture
7. Dexterity
8. Harmony

Reglas implementadas:
- Rango inclusivo de 1 a 99.
- Construcción validada.
- Lectura mediante propiedades o indexador tipado.
- Suma total de atributos.
- Incremento inmutable de un atributo.
- Rechazo de atributos desconocidos, incrementos no positivos y valores que superen 99.

Los nombres de los miembros están en inglés para mantener consistencia con el código; corresponden exactamente a las ocho estadísticas de la especificación funcional.

Los valores iniciales de las clases todavía no están encapsulados aquí. Se añadirán como configuraciones de creación de personaje, evitando que este objeto de valor conozca las clases o el sistema de progresión.

## 4. Separación que debe mantenerse

Los atributos primarios no son las estadísticas derivadas. Por ejemplo, Vitality es un atributo; la Vitalidad máxima es una estadística calculada. No se deben guardar ambas como si fueran el mismo dato ni calcular su fórmula dentro de `CharacterAttributes`.

El sistema de estadísticas derivadas y modificadores aún no se ha implementado. Antes de hacerlo se deben establecer contratos para:

- Identidad estable de cada estadística.
- Valor base frente a valor efectivo.
- Tipos de modificador y operaciones permitidas.
- Orden determinista de aplicación.
- Fuente, objetivo y ciclo de vida del modificador.
- Acumulación, sustitución, exclusión y eliminación.
- Precisión numérica y política única de redondeo.

No se ha fijado todavía una política universal de suma/multiplicación ni un orden arbitrario de modificadores. Debe derivarse de las reglas de juego y poder configurarse donde corresponda.

## 5. Pruebas automatizadas

Pruebas añadidas:
- `tests/MythicalExodus.Domain.Tests/Entities/DomainEntityTests.cs`
- `tests/MythicalExodus.Domain.Tests/Characters/CharacterAttributesTests.cs`

Se verifican identidad, estado inicial, transición idempotente, igualdad, validación de identidad, límites de atributos, inmutabilidad, suma e indexador.

## 6. Validación

La solución se compiló en configuración Release y las pruebas se ejecutaron correctamente mediante GitHub Actions:

- Build: correcto.
- Test: correcto.
- Workflow: [ejecución CI](https://github.com/RobertDeva/Game-Logic-motor/actions/runs/45768313112).

## 7. Próxima etapa

Continuar con el modelo de estadísticas derivadas y modificadores, seguido del modelo de recursos. Las reglas matemáticas concretas deben seguir las especificaciones funcionales existentes; las fórmulas aún provisionales no deben convertirse en constantes definitivas por conveniencia de implementación.
