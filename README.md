# Mythical Exodus — Game Logic Motor

Motor lógico independiente para **Mythical Exodus**, un dungeon crawler / hack & slash centrado en combate de precisión, movilidad acrobática, progresión de riesgo y magia por fusión.

## Objetivo

Construir en C# un núcleo de reglas de juego independiente de cualquier motor gráfico o interfaz. El mismo núcleo deberá poder ejecutarse en pruebas automatizadas y desde un host headless, y posteriormente integrarse con un motor gráfico sin incorporar dependencias de este.

## Estado

El núcleo ya contiene contratos fundamentales para identidad, ciclo de vida de entidades, atributos primarios, estadísticas, modificadores, recursos, reloj lógico y aleatoriedad determinista. La primera entidad de jugador implementa clase permanente, atributos iniciales y cálculo de nivel. Los sistemas de combate y demás mecánicas del GDD se incorporarán progresivamente, sin convertir hipótesis de balance en reglas definitivas.

## Requisitos

- .NET 8 SDK
- Git

## Estructura prevista

- `src/MythicalExodus.Domain`: modelo de dominio y reglas puras.
- `src/MythicalExodus.Application`: casos de uso y coordinación del juego.
- `src/MythicalExodus.Infrastructure`: carga y adaptación de datos externos.
- `src/MythicalExodus.Headless`: punto de entrada de consola para pruebas manuales.
- `tests/`: pruebas automatizadas.
- `docs/`: arquitectura, decisiones y especificación funcional.

## Compilar y probar

Cuando la solución esté inicializada:

```bash
dotnet restore
dotnet build --no-restore
dotnet test --no-build
```

## Principios

- El dominio no depende de consola, motor gráfico, archivos ni infraestructura.
- Las dependencias apuntan hacia el dominio.
- Los datos de contenido se mantienen fuera del código.
- Las reglas importantes se cubren con pruebas automatizadas.
- Se priorizan cohesión alta, acoplamiento bajo, contratos explícitos y cambios pequeños.

## Documentos de diseño

- [GDD baseline](docs/gdd-baseline.md): referencia funcional inicial.
- [Fundamentos del personaje jugador](docs/player-character-foundation.md): clases permanentes, atributos iniciales, nivel derivado y límites de la primera implementación.
- [Fundamentos del estado de recursos de combate](docs/combat-resource-state-foundation.md): Vitalidad segmentada y Postura separada, con transiciones inmutables validadas.
- [Auditoría arquitectónica](docs/architecture-audit.md): estado comprobado del núcleo y secuencia de construcción pendiente.
- [Expansión de conceptos y sistemas](docs/post-gdd-expansion-candidates.md): propuestas posteriores al GDD, incluidas exploración, misiones adaptativas, monturas, jefes colosos, reputación territorial, memoria histórica, eventos, facciones, entidades reactivas, misiones dinámicas y poblaciones simuladas de asentamientos, fauna y enemigos. Las ideas permanecen separadas del baseline hasta su aprobación.

## Documento rector

El GDD de Mythical Exodus compartido durante el diseño del proyecto es la referencia funcional inicial. Las decisiones técnicas detalladas se registrarán en `docs/`.
