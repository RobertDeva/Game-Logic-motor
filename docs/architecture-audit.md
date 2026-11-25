# Auditoría arquitectónica del núcleo lógico

**Proyecto:** Mythical Exodus  
**Repositorio:** RobertDeva/Game-Logic-motor  
**Alcance:** auditoría de la estructura y de la infraestructura arquitectónica existente. No constituye una auditoría de mecánicas implementadas, porque el código de dominio aún no existe.

## 1. Resultado ejecutivo

La solución tiene una separación inicial correcta en cuatro proyectos: Domain, Application, Infrastructure y Headless, además de un proyecto de pruebas. La dirección de dependencias documentada mantiene el dominio aislado de infraestructura y presentación.

Sin embargo, la estructura actual es principalmente esqueleto. Los proyectos Domain, Application e Infrastructure no contienen todavía implementaciones de sistemas lógicos. Headless solo imprime mensajes y Domain.Tests no contiene pruebas.

**Conclusión:** la arquitectura por capas está iniciada, pero todavía no existe un núcleo lógico funcional. No debe confundirse la existencia de los proyectos con la implementación de los sistemas.

## 2. Estado comprobado

| Área | Estado observado |
|---|---|
| Solución .NET 8 / C# 12 | Configurada |
| Domain | Proyecto vacío |
| Application | Proyecto vacío |
| Infrastructure | Proyecto vacío |
| Headless | Host mínimo, sin composición real |
| Domain.Tests | Proyecto xUnit sin pruebas |
| Separación de dependencias | Documentada y coherente |
| Tiempo determinista | Principio documentado; no implementado |
| Carga de contenido JSON | Decisión documentada; no implementada |
| Persistencia | Responsabilidad prevista; no implementada |
| CI | No comprobada como operativa en esta auditoría |
| Mecánicas de juego | No implementadas en código |

## 3. Sistemas internos que el motor necesita

Estos son sistemas de arquitectura y ejecución, no sistemas de contenido del mundo.

### 3.1 Modelo de dominio y ciclo de vida de entidades
Definir identidad estable, estado, ciclo de vida y límites de las entidades. Evitar introducir un ECS completo sin necesidad demostrada. Empezar con composición y objetos de dominio explícitos.

### 3.2 Modelo de atributos y estadísticas
Separar atributos base, valores derivados, modificadores y valores efectivos. Definir orden determinista de aplicación, origen, duración, acumulación y eliminación de modificadores. Evitar que cada mecánica calcule estadísticas por su cuenta.

### 3.3 Recursos y invariantes
Representar recursos como vitalidad, postura, maná, aura y prana con reglas explícitas: valor actual, máximo, consumo, recuperación, límites y eventos de cambio. Las reglas específicas de cada recurso pueden variar sin duplicar infraestructura.

### 3.4 Acciones y resolución
Diseñar un flujo común para solicitar, validar, ejecutar y resolver acciones. Debe admitir resultados rechazados con razones explícitas y efectos de resolución ordenados. Ataques, defensas, habilidades e interacciones podrán usar el mismo mecanismo sin compartir reglas que no les correspondan.

### 3.5 Condiciones y requisitos
Proporcionar evaluación explícita y comprobable de condiciones: requisitos de recursos, estados, objetivos, alcance lógico y otras restricciones. Las condiciones no deben esconder mutaciones.

### 3.6 Efectos y modificadores
Gestionar efectos instantáneos y persistentes, duración, acumulación, fuente, objetivo, activación, expiración, eliminación y efectos encadenados. Debe soportar múltiples postacciones anidadas con límites y protección contra ciclos infinitos.

### 3.7 Eventos y reacciones
Definir eventos de dominio para comunicar hechos que ya ocurrieron. Separar eventos de dominio de comandos/solicitudes. Establecer orden de procesamiento, política ante errores, reentrancia y prevención de duplicados.

### 3.8 Tiempo y simulación
Implementar reloj de simulación inyectado y ticks explícitos. Las reglas no consultan DateTime.Now ni temporizadores del sistema. Determinar cómo se procesan duraciones, enfriamientos, regeneración y eventos programados.

### 3.9 Aleatoriedad determinista
Centralizar generación aleatoria detrás de una abstracción con semilla reproducible. Las pruebas deben poder repetir exactamente una secuencia de resultados.

### 3.10 Orquestación de casos de uso
Application coordina operaciones y transacciones lógicas sin absorber las reglas del dominio. Debe definir límites de cada operación y garantizar que una acción no deje el estado parcialmente actualizado.

### 3.11 Contenido y configuración
Infrastructure carga datos externos, valida esquema, referencias, rangos y compatibilidad de versiones. Los datos configurables no deben poder invalidar invariantes esenciales del dominio.

### 3.12 Persistencia y reconstrucción
Definir snapshots, versión de esquema, migraciones, identidad y reconstrucción consistente del estado. No serializar directamente objetos internos sin un contrato estable.

### 3.13 Diagnóstico y validación
Proporcionar errores tipados, trazas de resolución y herramientas para inspeccionar por qué una acción fue aceptada, rechazada o produjo determinado resultado. No depender de Console dentro del dominio.

## 4. Arquitectura objetivo propuesta

Mantener las cuatro capas existentes:

- **Domain:** entidades, objetos de valor, invariantes, cálculos y reglas puras.
- **Application:** casos de uso, coordinación, puertos y límites de operación.
- **Infrastructure:** adaptadores de persistencia, contenido, reloj externo y otras integraciones.
- **Headless:** composición, ejecución de simulaciones y diagnóstico.

Dirección de dependencias:

```
Headless ─────► Application ─────► Domain
    │                ▲
    └────► Infrastructure ───────► Domain
```

Domain no debe depender de ninguna otra capa. Infrastructure implementa interfaces definidas hacia el interior. La interfaz gráfica futura será otro adaptador externo, no una dependencia del núcleo.

No introducir por adelantado ECS, event sourcing, plugins, un framework de DI complejo ni un motor de scripting. Incorporarlos únicamente si los requisitos y pruebas justifican su coste.

## 5. Orden recomendado de construcción

1. **Contratos fundamentales:** identificadores, resultados, errores, invariantes, reloj y aleatoriedad.
2. **Estado de dominio:** entidades, atributos, estadísticas, modificadores y recursos.
3. **Resolución:** condiciones, acciones y flujo de resolución.
4. **Reactividad:** eventos, efectos, duraciones, postacciones y orden de ejecución.
5. **Simulación:** tick determinista y procesamiento temporal.
6. **Aplicación:** casos de uso y coordinación transaccional.
7. **Infraestructura:** carga validada de contenido y persistencia versionada.
8. **Headless y pruebas de integración:** composición real, escenarios reproducibles y diagnósticos.

Cada etapa debe incluir pruebas unitarias y de integración antes de depender de ella en la siguiente.

## 6. Riesgos que deben evitarse

- Una clase central que conozca y ejecute todas las mecánicas.
- Efectos que muten estado sin dejar claro su origen y duración.
- Eventos que desencadenen ciclos recursivos ilimitados.
- Orden de resolución dependiente del orden accidental de colecciones.
- Lecturas directas del reloj o aleatoriedad global.
- Datos JSON capaces de saltarse invariantes.
- Guardar referencias internas o tipos de implementación como formato de persistencia.
- Duplicar cálculos de estadísticas entre combate, habilidades y equipo.
- Confundir eventos (hechos ocurridos) con comandos (intenciones).
- Declarar una mecánica implementada porque exista su documento o proyecto.

## 7. Criterio de finalización del núcleo

El núcleo podrá considerarse arquitectónicamente funcional cuando:

- Se pueda crear y modificar un estado de juego sin motor gráfico.
- Las acciones se validen y resuelvan de forma determinista.
- Los efectos y eventos tengan orden y ciclo de vida definidos.
- El tiempo y la aleatoriedad sean controlables en pruebas.
- Los datos externos se validen antes de entrar al dominio.
- El estado pueda guardarse y reconstruirse con versión explícita.
- Los casos de uso se ejecuten desde Headless.
- Las reglas críticas estén cubiertas por pruebas automatizadas.
- Ningún proyecto de dominio dependa de consola, archivos o motor gráfico.

## 8. Límite de esta auditoría

Esta auditoría describe lo que se pudo comprobar en el árbol y los archivos actuales del repositorio. No afirma que todas las decisiones propuestas estén aprobadas como reglas definitivas del juego. Tampoco modifica ni redefine las mecánicas funcionales del GDD. Su propósito es establecer qué infraestructura lógica falta para implementarlas de forma coherente.
