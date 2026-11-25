# Arquitectura de Mythical Exodus

## Objetivo arquitectónico

El motor lógico debe poder ejecutarse sin Unity, Unreal, Godot, consola, interfaz gráfica ni acceso directo al sistema de archivos. Un motor gráfico futuro será un adaptador externo que traduzca entradas y presente el estado producido por el núcleo.

## Capas y responsabilidades

### Domain — núcleo de reglas

Contiene entidades, objetos de valor, invariantes y reglas puras del juego. No referencia ningún otro proyecto de la solución ni bibliotecas de presentación o infraestructura. No conoce JSON, archivos, reloj del sistema, consola ni motor gráfico.

### Application — casos de uso

Coordina operaciones del juego y define puertos/abstracciones que necesita. Depende de Domain. No implementa acceso concreto a archivos ni presentación.

### Infrastructure — adaptadores externos

Implementa carga y validación de contenido externo (inicialmente JSON), persistencia y adaptadores técnicos. Puede depender de Application y Domain, nunca al revés.

### Headless — host de composición

Es el punto de entrada ejecutable para pruebas manuales y simulaciones sin gráficos. Compone dependencias y presenta información de diagnóstico. No contiene reglas del juego.

### Tests

Prueban las reglas y casos de uso sin requerir un motor gráfico. Las pruebas de dominio deben ser deterministas y no depender del reloj real.

## Dirección de dependencias

```text
Headless ───────► Application ───────► Domain
    │                  ▲
    └────► Infrastructure ───────────► Domain
                       │
                       └──────────────┘
```

Las referencias entre proyectos son deliberadamente unidireccionales. Domain permanece aislado.

## Tiempo y simulación

El diseño contempla simulación mediante pasos fijos (objetivo inicial: 60 ticks por segundo). La política de acumulación de tiempo, el reloj y la invocación del tick pertenecerán al host/capa de aplicación. Las reglas recibirán tiempo de simulación explícito; el dominio no consultará directamente el reloj del sistema. No se mezclará tiempo real de pared con tiempo determinista de simulación.

## Datos externos

Las definiciones de clases, estadísticas iniciales, armas, energías, habilidades, enemigos, objetos y recetas se cargarán desde archivos externos. Infrastructure validará el formato y transformará los datos a modelos/contratos consumibles por Application y Domain. Las reglas e invariantes críticas no se delegarán a JSON.

## Decisiones iniciales

- Runtime objetivo: .NET 8.
- Lenguaje: C# 12.
- Formato de contenido inicial: JSON con System.Text.Json.
- Host de validación: consola headless.
- Pruebas: xUnit.
- No se introduce todavía un motor de física, ECS, framework de DI ni sistema de plugins; se evaluarán cuando exista una necesidad concreta.
- No se codifican todavía las fórmulas de daño, postura, deflect o fusión: requieren especificación cuantitativa suficiente y pruebas de diseño.

## Límites de la Fase 1

Esta fase crea la solución, separa responsabilidades, establece la dirección de dependencias, prepara pruebas y CI, y documenta decisiones. No declara implementadas mecánicas de combate, parkour, progresión, magia o narrativa.
