# Auditoría arquitectónica del núcleo lógico

**Proyecto:** Mythical Exodus  
**Repositorio:** RobertDeva/Game-Logic-motor  
**Alcance:** estado real de la solución y próximos sistemas internos requeridos. Esta auditoría distingue contratos implementados de reglas de juego todavía pendientes.

## 1. Resultado ejecutivo

La solución mantiene cuatro proyectos: Domain, Application, Infrastructure y Headless, además del proyecto de pruebas. El dominio ya contiene contratos fundamentales y una primera parte del estado de personaje y recursos de combate. Todavía no existe un flujo completo de acciones, condiciones, resolución de combate, efectos, simulación, economía o mundo.

La existencia de tipos de dominio no debe confundirse con la implementación de un sistema jugable completo.

## 2. Estado comprobado

| Área | Estado |
|---|---|
| Solución .NET 8 / C# 12 y CI | Configurados; CI ejecutada en commits recientes |
| Identidad, errores y resultados tipados | Implementados |
| Ciclo de vida de entidades | Implementado |
| Atributos primarios 1–99 | Implementados |
| Clases permanentes y nivel derivado del jugador | Implementados |
| Contratos de estadísticas y modificadores | Implementados; cálculo efectivo pendiente |
| Recursos acotados | Implementados |
| Vitalidad segmentada y Postura independiente | Implementadas; transiciones inmutables y aplicación de impactos precomputados disponibles |
| Reloj lógico y aleatoriedad determinista | Implementados en sus respectivos proyectos |
| Acciones y condiciones | Contratos genéricos, solicitudes y validación de combate implementados; resolución de consecuencias pendiente |
| Cálculo ofensivo | Implementado: curvas normalizadas v1.0, coeficientes explícitos por componente, multiplicadores de movimiento/habilidad/condición y Voluntad; afinidades y críticos pendientes |
| Cálculo defensivo | Implementado: defensa plana, Resistencia derivada de atributo, resistencias de equipo/estado, límites configurables, vulnerabilidad e inmunidad; críticos pendientes |
| Afinidades energéticas | Implementadas: matriz dirigida de seis energías, pares recíprocos y multiplicadores ×1.50/×0.75/×1.25/×1.00; integración automática al pipeline de componentes pendiente |
| Presión y recuperación de Postura | Implementadas fórmulas configurables, curva de recuperación monótona según proporción de Vitalidad y aplicación inmutable; valores de contenido y coordinación temporal pendientes |
| Muertebendita contra enemigos | Implementadas elegibilidad por ruptura o Vitalidad agotada y ejecución manual para enemigos normales/resistentes; validación espacial y presentación pendientes |
| Fases y transiciones de jefes | Implementadas 3–5 fases, pérdida permanente de barras, retención del 40 % de Postura y ventanas temporales configurables; IA adaptativa y gestión de impactos multibarra pendientes |
| Aplicación de daño a Vitalidad y presión a Postura | Aplicación de magnitudes precomputadas implementada; fórmulas defensivas y cálculo de presión de Postura pendientes |
| Sistema de resurrección del jugador | Flujo de dominio implementado: decisión voluntaria tras golpe mortal, consumo explícito de carga, debuff temporal configurable y expiración por reloj lógico, Game Over y retorno al último punto de descanso; aplicación de modificadores al cálculo efectivo y reubicación/carga del mundo corresponden a Application/host |
| Resolución de desvíos, recuperación, ruptura y ejecución | Pendiente |
| Efectos y postacciones | Pendientes |
| Eventos y reacciones | Pendientes |
| Economía y progresión por almas | Pendientes en código |
| Ecosistemas, NPC, facciones y misiones | Pendientes en código |
| Persistencia y carga de contenido | Pendientes |
| Host Headless integrado | Pendiente |

## 3. Sistemas internos requeridos

### 3.1 Estado de dominio
Mantener identidad estable, ciclo de vida y objetos de valor inmutables. Las entidades de gameplay deben distinguir retirada de simulación, derrota, muerte narrativa y destrucción.

### 3.2 Estadísticas y recursos
Separar atributos base, estadísticas derivadas, modificadores y recursos actuales. Las políticas de cálculo, orden de modificadores, redondeo y límites deben ser explícitas y probadas.

### 3.3 Acciones y resolución
Diseñar un flujo común para solicitar, validar y resolver acciones. Debe producir rechazos tipados y resultados ordenados. Ataques, defensas, habilidades e interacciones pueden compartir infraestructura sin compartir reglas específicas.

### 3.4 Condiciones y requisitos
Las condiciones deben ser evaluadores explícitos y puros. No deben mutar el estado ni esconder costes. Una acción puede requerir varias condiciones y devolver todos los motivos de rechazo relevantes en orden determinista.

### 3.5 Efectos y postacciones
Gestionar efectos instantáneos y persistentes, duración, acumulación, fuente, objetivo, activación, expiración y eliminación. Las cadenas y múltiples postacciones anidadas requieren límites explícitos y protección contra ciclos.

### 3.6 Eventos y reacciones
Separar comandos (intenciones) de eventos (hechos ocurridos). Establecer orden, identidad, idempotencia, reentrancia y política de errores antes de adoptar un bus global.

### 3.7 Tiempo y simulación
Usar el reloj lógico inyectado y ticks explícitos. Ninguna regla de dominio debe depender de DateTime.Now ni de temporizadores de máquina.

### 3.8 Aplicación e infraestructura
Application coordina casos de uso y límites transaccionales. Infrastructure implementa puertos de persistencia y carga validada. Headless compone y ejecuta escenarios reproducibles.

## 4. Arquitectura objetivo

- **Domain:** entidades, objetos de valor, invariantes y reglas.
- **Application:** casos de uso, coordinación y puertos.
- **Infrastructure:** adaptadores de persistencia, contenido y servicios externos.
- **Headless:** composición, simulación y diagnóstico.

Dirección de dependencias:

```
Headless ─────► Application ─────► Domain
    │                ▲
    └────► Infrastructure ───────► Domain
```

No introducir ECS, event sourcing, un Event Bus global, framework DI complejo ni scripting antes de que las pruebas y necesidades reales lo justifiquen.

## 5. Orden de construcción

1. Contratos fundamentales.
2. Estado de dominio.
3. Completar el pipeline de resolución: cálculo ofensivo/defensivo, defensas, presión, recuperación, respuestas y ejecuciones sobre la aplicación de impactos ya implementada.
4. Efectos, eventos y postacciones.
5. Simulación temporal.
6. Casos de uso de Application.
7. Persistencia y contenido validado.
8. Host Headless y pruebas de integración.

Cada bloque debe aportar pruebas unitarias y, cuando corresponda, pruebas de integración antes de que el siguiente dependa de él.

## 6. Criterio de finalización

El núcleo será funcional cuando permita crear estado de juego sin motor gráfico, validar y resolver acciones determinísticamente, procesar efectos y eventos con orden definido, controlar tiempo y aleatoriedad, cargar datos validados, guardar y reconstruir estado versionado, y ejecutar escenarios completos desde Headless.
