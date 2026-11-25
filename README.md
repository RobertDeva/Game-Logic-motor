# Mythical Exodus — Game Logic Motor

Motor lógico independiente para **Mythical Exodus**, un dungeon crawler / hack & slash centrado en combate de precisión, movilidad acrobática, progresión de riesgo y magia por fusión.

## Objetivo

Construir en C# un núcleo de reglas de juego independiente de cualquier motor gráfico o interfaz. El mismo núcleo deberá poder ejecutarse en pruebas automatizadas y desde un host headless, y posteriormente integrarse con un motor gráfico sin incorporar dependencias de este.

## Estado

El repositorio inicia con la fase de arquitectura y estructura de la solución. Las mecánicas del GDD se implementarán progresivamente y se documentarán mediante decisiones de arquitectura y pruebas.

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

## Documento rector

El GDD de Mythical Exodus compartido durante el diseño del proyecto es la referencia funcional inicial. Las decisiones técnicas detalladas se registrarán en `docs/`.
