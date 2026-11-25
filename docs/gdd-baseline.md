# Mythical Exodus — línea base funcional del GDD

Este documento registra los sistemas que guiarán el desarrollo del motor. Es una línea base de diseño, no una especificación numérica completa.

## Identidad

- Género: Dungeon Crawler / Hack & Slash.
- Perspectiva prevista: tercera persona o 2.5D, con alta verticalidad.
- Pilares: combate de precisión, movilidad acrobática, progresión basada en riesgo y magia por fusión.

## Clases y estadísticas

Estadísticas base: Vitalidad, Fuerza, Agilidad, Resistencia, Voluntad, Ruptura, Destreza y Armonía.

Clases iniciales y energías:
- Guerrero: Aura; mandoble, espada y escudo, arco.
- Espadachín Mágico: Maná; estoques y espadas largas.
- Segador: Prana; katanas y guadañas.

Las estadísticas y configuraciones de contenido deberán ser data-driven.

## Energías

Energías básicas: Aura, Maná y Prana. Un personaje comienza con su energía base y puede aprender una segunda mediante instructores. Dominar dos energías permite desbloquear una energía superior, conservando el acceso a las básicas.

Energías superiores nombradas en el GDD: Energía Natural, Poder Astral y Energía Abisal. Las combinaciones exactas y la matriz de ventajas/debilidades deben transcribirse y validarse a partir del diagrama original antes de implementarse.

## Combate

- Estados principales: Vitalidad y Postura.
- Reducir la vitalidad enemiga ralentiza la recuperación de su postura.
- Romper la postura habilita una ejecución llamada Muertebendita (Deathblow).
- Deflect perfecto: ventana temporal precisa; ejemplo provisional del GDD: 0,2 segundos. La ventana y la efectividad dependen del arma y la clase.
- Artes de combate consumen energía y pueden dañar postura, aplicar estados o curar.

Los valores numéricos de daño, recuperación, ventanas y costes quedan pendientes de especificación.

## Movilidad y exploración

Se contemplan correr por paredes, saltos de rebote, deslizamiento, gancho, escalada, ataques aéreos, deflects en carrera por paredes, ejecuciones desde altura, herramientas de exploración, mazmorras y puzles que combinan movilidad y magia.

La simulación lógica modelará capacidades, estados, restricciones y resultados; la representación física y animación serán responsabilidad del motor de presentación/física futuro.

## Progresión

- Almas funcionan como moneda y experiencia.
- Se gastan en zonas de descanso para mejorar estadísticas o energías.
- Descansar reaparece enemigos.
- Al morir se pierden las almas no gastadas; pueden recuperarse en el lugar de la muerte, pero una segunda muerte antes de recuperarlas las elimina definitivamente.

## Economía y mundo

Crafteo de pociones, armas y mejoras a partir de materiales. Reputación territorial afecta precios, diálogos y acceso a zonas. NPC relevantes: instructores, herreros y comerciantes especiales.

## Narrativa y finales

La derrota del Señor de las Tinieblas conduce a finales condicionados por variables ocultas:
1. El Ciclo se Repite: derrotarlo sin levantar la maldición de corrupción; el protagonista se convierte en el nuevo Señor de las Tinieblas.
2. El Sellado Temporal: levantar la maldición, buena reputación y no usar el receptáculo divino; la fuente queda sellada temporalmente.
3. La Purga: levantar la maldición, mala reputación y no usar el receptáculo; el protagonista usa ese poder para sí mismo y se vuelve enemigo de los reinos.
4. El Sacrificio: levantar la maldición y usar el receptáculo divino; el protagonista se sacrifica, purifica la fuente y pierde sus poderes.

## Preguntas abiertas antes de implementar reglas

- Fórmulas y límites de las ocho estadísticas.
- Tabla de escalado de armas y clases.
- Combinaciones exactas de energías y matriz de afinidades.
- Unidades de tiempo y reglas de prioridad para Deflect.
- Fórmulas de daño, postura, recuperación, stagger y ejecución.
- Estados alterados, duración, acumulación y resistencias.
- Reglas exactas de movimiento, anclajes y costes.
- Condiciones completas y precedencia de variables para los cuatro finales.


## Referencia de los cuadernos originales (fotografías aportadas)

Esta sección registra información legible en las tres páginas manuscritas aportadas por el creador. Se conserva separada de las decisiones posteriores del GDD para no borrar el origen ni convertir lecturas dudosas en reglas definitivas.

### Lista de características anotadas

En la primera página aparecen, además de la identificación del proyecto y el género manuscrito como «Soulslike - Hack/Slash», las siguientes características:

- Desarrollo de personaje.
- Historia con múltiples desenlaces.
- Árboles de habilidades.
- Mecánicas de combate simples y dinámicas.
- Evolución de relaciones con los NPCs.
- Mazmorras ocultas.
- Puzles.
- Sistema de equipamientos y armaduras.
- Artes de combate secretas y con niveles de experiencia.
- Sistema de nivelación de estadísticas en zonas de descanso con penalizaciones por muerte.
- Equipamiento especial para la exploración del mapa.
- Sistema de crafteo y recetario.
- Maestro herrero y comerciantes especiales.
- Instructores de Artes de Combate.
- Sistema de reputación territorial.

Estos puntos amplían el inventario de sistemas que debe contemplar el diseño. No implican que ya estén implementados.

### Clases, armas y progresión de armas

La segunda página muestra estas clases y armas:

- Espadachín mágico: espada larga y estoque como armas de primer grupo.
- Guerrero: mandoble, espada y escudo como armas de primer grupo.
- Segador: katana y guadaña como armas de primer grupo.

El esquema dibuja conexiones desde los grupos de armas de las clases hacia una anotación común «Armas 2º: Arco». La fotografía no permite determinar con seguridad si el arco es una segunda categoría desbloqueable para las tres clases, una progresión común o una relación distinta. Se registra como pregunta abierta; no se codificará hasta confirmarlo.

### Funciones descriptivas de las estadísticas

La segunda página define las estadísticas de esta manera (transcripción del texto legible):

- Vitalidad: aumenta la salud máxima.
- Fuerza: aumenta el daño físico.
- Agilidad: mejora la velocidad de escalada y movimiento.
- Resistencia: aumenta la resistencia a todos los tipos de daño (el final de la línea queda fuera de la zona legible de la fotografía).
- Voluntad: disminuye la duración de los efectos de estado y aumenta el daño de todo tipo.
- Ruptura: aumenta el índice de daño crítico y mejora la evasión (lectura de la línea manuscrita; conviene confirmarla con el autor).
- Destreza: mejora el daño de Artes de Combate.
- Armonía: aumenta el daño especial.

Estas descripciones son intenciones de diseño, no fórmulas matemáticas. La función de Voluntad ya está aclarada por el creador: reduce la duración de los efectos de estado y aumenta todo tipo de daño.

### Tabla de valores iniciales por clase

La tercera página contiene una tabla con columnas «EM», «G» y «S». Por el orden y los nombres de la página de clases, se interpretan provisionalmente como Espadachín Mágico, Guerrero y Segador, respectivamente.

| Estadística | EM | G | S |
|---|---:|---:|---:|
| Vitalidad | 5 | 6 | 4 |
| Fuerza | 6 | 6 | 4 |
| Agilidad | 5 | 4 | 6 |
| Resistencia | 4 | 6 | 4 |
| Voluntad | 4 | 5 | 6 |
| Ruptura | 5 | 5 | 6 |
| Destreza | 6 | 4 | 5 |
| Armonía | 6 | 4 | 5 |

Los valores se transcriben como aparecen en la tabla. No se asume todavía el rango permitido, el coste de subir cada estadística ni si estos números son atributos iniciales, ponderaciones o puntos de distribución. Antes de implementarlos como balance definitivo se confirmarán su significado y las abreviaturas.

### Diagrama de energías

La tercera página representa Aura, Maná y Prana como energías básicas y muestra tres recuadros de energías especiales/superiores: Energía Natural, Poder Astral y Energía Abisal. Hay múltiples flechas entre los nodos. Las flechas representan relaciones direccionales de fuerza/debilidad entre energías; sus puntas son significativas y deben respetarse al transcribir el diagrama. No deben tratarse como simples conexiones de fusión. Convención confirmada por el creador: la punta de la flecha señala la energía que tiene la ventaja. Por tanto, una flecha A → B significa que A tiene ventaja sobre B.

### Consecuencia para la implementación

La tabla anterior sí proporciona una primera referencia concreta para construir configuraciones iniciales de las tres clases, siempre que el creador confirme que los números son los valores iniciales previstos. Las descripciones sirven para definir responsabilidades candidatas de cada estadística. Las fórmulas, límites, escalados, efecto incompleto de Voluntad y progresión de armas siguen sin estar especificados.


### Aclaración del creador sobre Voluntad y las flechas

El creador confirmó expresamente que Voluntad:
- Reduce la duración de todos los efectos de estado que afecten al personaje.
- Aumenta el daño de todo tipo, no solamente el daño especial.

También aclaró que las puntas de las flechas del diagrama codifican las relaciones de fuerza y debilidad entre energías. Por tanto, la lectura debe conservar la dirección de cada flecha; no es correcto interpretar las líneas como relaciones sin dirección. Antes de implementar la matriz, se documentará qué extremo representa la energía con ventaja y cuál la energía vulnerable.


Confirmación adicional del creador: en el diagrama de afinidades, la punta de cada flecha indica el lado que tiene ventaja. La relación se registrará como «origen tiene ventaja sobre destino» cuando la flecha vaya del origen al destino (A → B).
