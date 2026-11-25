# Mythical Exodus — línea base funcional del GDD

Este documento registra los sistemas que guiarán el desarrollo del motor. Es una línea base de diseño, no una especificación numérica completa.

## Identidad

- Género: Dungeon Crawler / Hack & Slash.
- Perspectiva prevista: tercera persona o 2.5D, con alta verticalidad.
- Pilares: combate de precisión, movilidad acrobática, progresión basada en riesgo y magia por fusión.

## Clases y estadísticas

Estadísticas base: Vitalidad, Fuerza, Agilidad, Resistencia, Voluntad, Ruptura, Destreza y Armonía.

Las clases son elecciones permanentes durante la partida; no existe cambio de clase.

Clases iniciales y energías:
- Guerrero: Aura; armas propias: mandoble, espada y escudo. Arma secundaria compartida: arco.
- Espadachín Mágico: Maná; armas propias: estoques y espadas largas. Arma secundaria compartida: arco.
- Segador: Prana; armas propias: katanas y guadañas. Arma secundaria compartida: arco.

Las estadísticas y configuraciones de contenido deberán ser data-driven.

## Energías

Energías básicas: Aura, Maná y Prana. Un personaje comienza con su energía base y puede aprender una segunda mediante instructores. Dominar dos energías permite desbloquear una energía superior, conservando el acceso a las básicas.

Las fusiones confirmadas son: Maná + Aura = Energía Natural; Maná + Prana = Poder Astral; Prana + Aura = Energía Abisal. Al dominar las dos energías básicas correspondientes se desbloquea la energía superior; el personaje conserva el acceso a las energías básicas.

## Combate

- Estados principales: Vitalidad y Postura.
- Reducir la vitalidad enemiga ralentiza la recuperación de su postura.
- Romper la postura habilita una ejecución llamada Muertebendita (Deathblow).
- Deflect perfecto: ventana temporal precisa; ejemplo provisional del GDD: 0,2 segundos. La ventana y la efectividad dependen del arma y la clase.
- Artes de combate consumen energía y pueden dañar postura, aplicar estados o curar.
- El sistema de mejora de Artes de Combate toma como referencia el de Sekiro, pero cada Arte tendrá su propio nivel de dominio mejorable de forma individual.

Los valores numéricos de daño, recuperación, ventanas y costes quedan pendientes de especificación.

## Movilidad y exploración

Se contemplan correr por paredes, saltos de rebote, deslizamiento, gancho, escalada, ataques aéreos, deflects en carrera por paredes, ejecuciones desde altura, herramientas de exploración, mazmorras y puzles que combinan movilidad y magia.

La simulación lógica modelará capacidades, estados, restricciones y resultados; la representación física y animación serán responsabilidad del motor de presentación/física futuro.

## Progresión

- Las Almas se utilizan para subir de nivel siguiendo el modelo general de los Soulslike: el jugador las gasta en zonas de descanso.
- Cada personaje comienza en nivel 1.
- Cada aumento de nivel concede exactamente un punto para aumentar en una unidad una estadística elegida.
- Cada una de las ocho estadísticas tiene un límite máximo individual de 99.
- Descansar en una zona de descanso reaparece enemigos.
- La regla exacta para obtener/consumir Almas y convertirlas en niveles queda pendiente de especificación.
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

- Fórmulas de conversión de estadísticas a salud, daño, defensa, movilidad, críticos, evasión y demás resultados.
- Tabla de escalado de armas y clases.
- Matriz completa de afinidades elementales/energéticas, transcrita flecha por flecha desde el diagrama.
- Curva/coste de Almas necesario para cada subida de nivel.
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

El arco es el arma secundaria común a las tres clases, según confirmación del creador.

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

Confirmación del creador: estos números son las estadísticas básicas/iniciales de cada clase. Se utilizarán como valores de partida al crear un personaje de esa clase. Todavía quedan por definir el rango permitido, los límites y el coste de mejorar cada estadística.

### Diagrama de energías

La tercera página representa Aura, Maná y Prana como energías básicas y muestra tres recuadros de energías especiales/superiores: Energía Natural, Poder Astral y Energía Abisal. Hay múltiples flechas entre los nodos. Las flechas representan relaciones direccionales de fuerza/debilidad entre energías; sus puntas son significativas y deben respetarse al transcribir el diagrama. No deben tratarse como simples conexiones de fusión. Convención confirmada por el creador: la punta de la flecha señala la energía que tiene la ventaja. Por tanto, una flecha A → B significa que A tiene ventaja sobre B.

### Consecuencia para la implementación

La tabla anterior proporciona los valores iniciales confirmados para construir las configuraciones de las tres clases. Las descripciones sirven para definir responsabilidades candidatas de cada estadística. Las fórmulas, límites, escalados, efecto incompleto de Voluntad y progresión de armas siguen sin estar especificados.


### Aclaración del creador sobre Voluntad y las flechas

El creador confirmó expresamente que Voluntad:
- Reduce la duración de todos los efectos de estado que afecten al personaje.
- Aumenta el daño de todo tipo, no solamente el daño especial.

También aclaró que las puntas de las flechas del diagrama codifican las relaciones de fuerza y debilidad entre energías. Por tanto, la lectura debe conservar la dirección de cada flecha; no es correcto interpretar las líneas como relaciones sin dirección. Antes de implementar la matriz, se documentará qué extremo representa la energía con ventaja y cuál la energía vulnerable.


Confirmación adicional del creador: en el diagrama de afinidades, la punta de cada flecha indica el lado que tiene ventaja. La relación se registrará como «origen tiene ventaja sobre destino» cuando la flecha vaya del origen al destino (A → B).


### Confirmación del creador sobre las estadísticas iniciales

Los valores de las columnas EM/G/S son los atributos básicos iniciales de Espadachín Mágico, Guerrero y Segador, respectivamente. La configuración inicial de cada personaje se construirá a partir de la columna correspondiente a su clase.


### Confirmaciones del creador: armas, niveles y fusiones

- El arco es el arma secundaria compartida por todas las clases.
- Cada personaje comienza en nivel 1.
- Cada subida de nivel otorga un punto de atributo, que se asigna aumentando en una unidad una de las ocho estadísticas.
- Cada estadística tiene un máximo individual de 99.
- Las fusiones son:
  - Maná + Aura → Energía Natural.
  - Maná + Prana → Poder Astral.
  - Prana + Aura → Energía Abisal.
- El orden de aprendizaje de las dos energías no cambia la fusión indicada; las energías básicas se conservan al desbloquear la superior.

Queda pendiente definir cómo se obtienen los puntos de experiencia/Almas necesarios para subir de nivel, y si existe alguna condición adicional para aprender la segunda energía o dominar la fusión.


### Confirmaciones del creador: progresión, dominio y permanencia de clase

- La subida de nivel sigue el modelo Soulslike: se gastan Almas en zonas de descanso para subir de nivel.
- Cada nivel ganado concede un único punto de atributo, asignable a una estadística, con máximo individual de 99.
- Las clases son permanentes durante la partida; el personaje no puede cambiar de clase.
- La mejora de Artes de Combate toma como referencia el sistema de Sekiro. Cada Arte de Combate tiene su propio nivel de dominio y puede mejorarse individualmente.

No se han fijado todavía los costes exactos de Almas por nivel ni los requisitos, recursos o efectos concretos de cada nivel de dominio de un Arte. No se asumirán automáticamente los árboles, puntos de habilidad ni costes del videojuego de referencia: Sekiro es una inspiración para el enfoque, mientras que las reglas finales serán propias de Mythical Exodus.


### Confirmación del creador: acumulación de puntos de Artes de Combate

- Los puntos obtenidos al llenar el medidor de progresión de Artes de Combate pueden guardarse y acumularse; el jugador no está obligado a gastarlos inmediatamente.
- Cada punto acumulado puede gastarse posteriormente para desbloquear un Arte de Combate nuevo o aumentar el nivel de dominio de un Arte ya desbloqueado.
- Obtener un punto reinicia el medidor de progreso parcial a cero y aumenta el umbral requerido para obtener el siguiente punto, independientemente de que el jugador haya gastado o no los puntos acumulados.
- Al morir, se pierde el progreso parcial del medidor, pero no los puntos de Artes ya obtenidos y conservados.
- El medidor de experiencia de Artes se alimenta al derrotar enemigos y es independiente de las Almas utilizadas para subir atributos en zonas de descanso.

Quedan pendientes los valores de experiencia concedidos por enemigo, el umbral inicial, la fórmula de incremento del umbral y los efectos concretos de cada nivel de dominio.

### Confirmaciones del creador: recuperación de postura y Muertebendita

- Los enemigos recuperan postura automáticamente cuando dejan de recibir presión del jugador.
- La velocidad de recuperación de postura disminuye a medida que baja la vitalidad del enemigo.
- La ruptura de la postura enemiga habilita una Muertebendita.
- Agotar por completo la vitalidad de un enemigo también lo deja vulnerable a una Muertebendita, aunque su postura no haya sido rota. Por tanto, la ruptura de postura y el agotamiento de vitalidad son condiciones independientes que pueden habilitar la ejecución.
- Los enemigos resistentes a Muertebendita conservan varias barras de vitalidad y una única barra de postura. Cada Muertebendita consume una barra completa de vitalidad y la postura se reinicia después de cada ejecución.

Los valores numéricos de recuperación, la relación matemática entre vitalidad y recuperación, y las ventanas de oportunidad de la ejecución siguen pendientes de especificación.

### Confirmaciones del creador: ejecuciones y ataques enemigos

- Una Muertebendita elimina inmediatamente a un enemigo normal, independientemente de la vitalidad que le quedara.
- Los enemigos resistentes a Muertebendita pueden tener distinta cantidad de barras de vitalidad según el enemigo, con un máximo de tres barras.
- Cada Muertebendita contra un enemigo resistente consume exactamente una barra completa de vitalidad; después de la ejecución, su única barra de postura se restablece. Al consumir la última barra, el enemigo es derrotado.
- El combate tomará como referencia la filosofía de Sekiro, adaptada a las mecánicas propias de Mythical Exodus. No se asumirá una copia literal de los sistemas del juego de referencia.
- Algunos ataques enemigos exigirán respuestas concretas en lugar de poder neutralizarse mediante bloqueo genérico. Las respuestas podrán incluir bloqueo, desvío, esquiva, salto, contraataque u otra acción definida para ese ataque.
- Cada ataque enemigo definirá sus respuestas válidas y las consecuencias de fallar o responder incorrectamente. Estas consecuencias podrán variar por ataque e incluir daño, acumulación de postura, estados alterados, derribo, aturdimiento, vulnerabilidad o ejecución especial, según corresponda.
- El daño, el impacto sobre la postura y los efectos de cada ataque/respuesta serán configurables por ataque y enemigo; no se fijan valores numéricos ni consecuencias universales en esta etapa.

Esta especificación describe el comportamiento lógico. La detección de entradas, animaciones, ventanas de reacción y presentación audiovisual se integrarán posteriormente mediante adaptadores, sin acoplar el dominio a un motor gráfico.

### Confirmación del creador: categorías de ataque por tipo de enemigo

- Los enemigos normales únicamente utilizarán ataques normales, ataques pesados y estocadas.
- Los demás tipos de enemigos podrán utilizar todas las categorías de ataque contempladas por el sistema, incluidas las categorías de barrido, agarre y ataques especiales imbloqueables.
- Que un tipo de enemigo tenga acceso a todas las categorías no significa que cada enemigo deba utilizar obligatoriamente todas ellas; su repertorio concreto se define individualmente.

### Confirmación del creador: contraataque de estocadas

- Las estocadas tendrán una respuesta de contraataque específica, inspirada funcionalmente en el Mikiri de Sekiro.
- La mecánica se adaptará a Mythical Exodus: las animaciones, ejecución y efectos podrán diferenciarse según la clase y el arma equipada.
- La respuesta será una acción contextual específica para contrarrestar estocadas, no un bloqueo o desvío genérico.
- Los requisitos temporales, ventanas de entrada, efectos sobre la postura enemiga y diferencias concretas por clase/arma quedan pendientes de diseño.

### Confirmación del creador: ventanas de desvío perfecto por arma

- Cada arma tendrá su propia ventana base para ejecutar un desvío perfecto.
- La ventana podrá mejorarse mediante estadísticas, habilidades o dominio del arma.
- Las mejoras deberán respetar un límite de diseño para evitar que la ventana de desvío perfecto se vuelva excesivamente amplia.
- Los valores base, el límite máximo, las contribuciones de cada estadística y los efectos concretos de habilidades o dominio quedan pendientes de especificación.

### Confirmación del creador: efecto del desvío perfecto sobre la postura enemiga

- Un desvío perfecto exitoso reducirá la postura del enemigo que ejecutó el ataque, siguiendo el principio de combate de Sekiro.
- Este efecto se suma a la neutralización o mitigación del ataque recibido por el jugador.
- La cantidad de postura enemiga reducida podrá configurarse según el arma del jugador y las características del ataque enemigo.
- Los valores, escalados y posibles excepciones quedan pendientes de especificación.

### Confirmación del creador: desvíos perfectos de los enemigos

- Los enemigos también podrán ejecutar desvíos perfectos contra los ataques del jugador.
- El sistema de combate tendrá una interacción bidireccional de postura: tanto el jugador como los enemigos podrán defenderse mediante desvíos perfectos y afectar la postura del oponente.
- La capacidad, frecuencia, ventana temporal y consecuencias de los desvíos enemigos podrán configurarse según el enemigo y su repertorio de combate.
- Los valores numéricos y las reglas específicas para cada tipo de enemigo quedan pendientes de diseño.

### Confirmación del creador: evolución de comportamiento de los jefes por barras de vitalidad

- Todos los jefes, no solamente los jefes especiales, cambiarán su comportamiento cada vez que pierdan una barra completa de vitalidad.
- Cada transición de barra representa un cambio de fase o estado de combate del jefe.
- El cambio podrá reflejarse en patrones y combinaciones de ataque, agresividad, ritmo de combate, repertorio y respuestas defensivas.
- El cambio de comportamiento no implica necesariamente aumentar daño, velocidad o estadísticas; la transición debe expresarse principalmente mediante comportamiento y patrones nuevos o modificados.
- La configuración concreta de cada fase será individual por jefe. No se establece que todos deban cambiar de la misma manera ni incorporar el mismo número o tipo de ataques.
- Las reglas de transición, los patrones específicos y cualquier cambio numérico asociado quedan pendientes de diseño.

### Confirmación del creador: postura de los jefes al cambiar de fase

- Cuando un jefe pierde una barra completa de vitalidad y comienza su siguiente fase, se reduce en un 60 % la postura que tenga acumulada en ese instante.
- Por tanto, el jefe conserva el 40 % de la postura acumulada antes de la transición. No se restaura la postura hasta el 60 % de su capacidad máxima.
- Ejemplo ilustrativo: si el jefe tenía acumulado un 80 % de postura, tras la transición conservará un 32 % de postura acumulada (el 40 % de 80 %).
- Esta regla se aplica a todos los jefes que cambian de fase al perder una barra de vitalidad.

### Confirmación del creador: pérdida definitiva de barras de vitalidad de los jefes

- Cuando un jefe pierde una barra completa de vitalidad, esa barra queda perdida definitivamente.
- El cambio de fase no restaura ni devuelve vitalidad de la barra consumida.
- La transición de fase solamente aplica los cambios de comportamiento establecidos y la reducción del 60 % de la postura acumulada; no concede recuperación de vitalidad.

### Confirmación del creador: vulnerabilidad e invulnerabilidad durante la transición de fase de los jefes

- Al perder una barra completa de vitalidad, el jefe inicia el proceso de cambio de fase.
- Antes de entrar en la transición, existe una breve ventana durante la cual el jefe sigue siendo vulnerable y puede recibir daño.
- Después de esa ventana, el jefe pasa a ser invulnerable mientras ejecuta la transición de fase.
- La duración exacta de la ventana de vulnerabilidad y de la transición invulnerable queda pendiente de especificación.
- Esta transición no restaura ninguna vitalidad perdida. Al completarse, se aplican el nuevo comportamiento de fase y la reducción del 60 % de la postura acumulada, conservando el 40 %.

### Confirmación del creador: cantidad de barras de vitalidad de los jefes y ataques de transición

- Todos los jefes tendrán entre 3 y 5 barras de vitalidad. La cantidad concreta se define individualmente para cada jefe dentro de ese rango.
- Esta regla específica de jefes establece un rango distinto al máximo general de tres barras indicado para otros enemigos resistentes a Muertebendita; no debe aplicarse ese límite general a los jefes.
- Durante las transiciones de fase, los jefes no realizarán ataques de cambio de fase en todas las fases.
- Únicamente en las últimas fases de un jefe podrá ejecutarse un ataque especial asociado al cambio de fase.
- La fase o fases exactas en las que cada jefe puede realizar ese ataque, así como sus características y consecuencias, se definirán individualmente.
- Se mantiene la secuencia confirmada: breve ventana inicial de vulnerabilidad y, después, invulnerabilidad durante la transición. El ataque especial, cuando corresponda en las últimas fases, forma parte de la transición invulnerable.

### Confirmación del creador: ataques especiales de cambio de fase compartidos y únicos

- El diseño de ataques especiales de cambio de fase combinará ataques compartidos entre determinados grupos de jefes con ataques exclusivos de cada jefe.
- Los ataques compartidos podrán reutilizar una mecánica o patrón base entre jefes compatibles, sin exigir que sus ejecuciones sean idénticas.
- Cada jefe podrá contar además con ataques de transición propios que reflejen su identidad y estilo de combate.
- La selección de qué jefes comparten ataques y cuáles son sus ataques exclusivos se definirá individualmente durante el diseño de cada jefe.

### Confirmación del creador: adaptación defensiva de los jefes a las estrategias del jugador

- Además de cambiar de comportamiento al perder cada barra de vitalidad, los jefes podrán adaptar sus respuestas defensivas durante el combate según las estrategias que observen utilizar al jugador.
- La adaptación se basará en acciones y patrones de combate observados durante el enfrentamiento; no se permitirá que la IA consulte entradas futuras ni información interna sobre decisiones que el jugador todavía no haya ejecutado.
- Las respuestas defensivas adaptativas podrán incluir cambios en la selección o frecuencia de bloqueos, desvíos perfectos, esquivas, contraataques y otras respuestas que estén dentro del repertorio configurado para ese jefe.
- La adaptación no otorgará inmunidad permanente frente a una estrategia ni modificará arbitrariamente las estadísticas del jefe para contrarrestar al jugador.
- Cada jefe tendrá una capacidad y un grado de adaptación definidos por su diseño individual. La intensidad, velocidad, memoria y condiciones para modificar sus respuestas quedan pendientes de especificación.

### Confirmación del creador: persistencia de la memoria adaptativa de los jefes entre fases

- Los jefes conservarán durante todo el enfrentamiento la memoria de los patrones y estrategias del jugador que hayan aprendido.
- La memoria adaptativa no se reiniciará al perder una barra de vitalidad ni al comenzar una nueva fase.
- Cada nueva fase podrá cambiar los patrones y el comportamiento base del jefe, pero sus respuestas defensivas seguirán aprovechando lo aprendido en fases anteriores.
- Esta persistencia busca mantener una dificultad elevada mediante ajustes adaptativos graduales, sin exigir cambios drásticos de comportamiento entre fases.
- La forma de almacenar, ponderar, olvidar o actualizar observaciones durante el combate queda pendiente de diseño; no se presupone que el jefe recuerde cada acción individual indefinidamente.

### Confirmación del creador: adaptación mutua y engaño táctico durante los combates contra jefes

- Los jefes podrán reconocer cambios deliberados en la estrategia del jugador y ajustar sus respuestas defensivas en consecuencia.
- El jugador podrá explotar la adaptación del jefe mediante fintas, cambios de ritmo, variaciones de ataques y otras decisiones tácticas para provocar o aprovechar respuestas defensivas previsibles.
- La IA deberá basar sus ajustes en patrones y acciones observables durante el combate; no tendrá acceso a intenciones privadas ni a entradas futuras del jugador.
- La adaptación será dinámica y bidireccional: el jefe aprende de las estrategias observadas y el jugador puede aprender a anticipar, provocar y superar las respuestas del jefe.
- El objetivo de diseño es que el jugador tenga que evolucionar su estrategia a lo largo del enfrentamiento, sin que exista una única secuencia óptima que resuelva todo el combate.
- No se exige que el jefe identifique con certeza la intención del jugador; la detección de cambios y fintas se implementará como inferencia basada en comportamiento observable.
- Las señales utilizadas, umbrales de detección, tiempos de adaptación y herramientas concretas de engaño quedan pendientes de diseño.


## Adaptación de agresividad ante el juego defensivo del jugador

Los jefes pueden aumentar su presión ofensiva cuando detectan que el jugador mantiene una estrategia excesivamente pasiva o defensiva. El objetivo es impedir que el combate se convierta en una espera unilateral en la que el jefe funcione como un objetivo pasivo.

- La IA puede reconocer conductas defensivas repetidas u otras señales observables de pasividad.
- Como respuesta, puede incrementar su agresividad y presión ofensiva, ajustar la frecuencia de sus ataques o seleccionar respuestas más adecuadas para forzar al jugador a actuar.
- Este ajuste forma parte de la adaptación táctica del jefe y debe respetar sus capacidades, repertorio y personalidad individuales.
- El jefe no puede leer inputs futuros, conocer intenciones privadas ni modificar arbitrariamente sus estadísticas para obtener ventaja.
- El aumento de agresividad no debe equivaler a ataques inevitables ni eliminar las oportunidades legítimas de respuesta del jugador.
- Los umbrales que determinan cuándo se considera pasivo al jugador, la intensidad del aumento y su duración quedan pendientes de balance y diseño.

La intención de diseño es que el jugador tenga que participar activamente en el intercambio, alternando defensa, presión, ataque y engaño, en lugar de poder neutralizar el combate limitándose a esperar.


## Reposicionamiento táctico y control de distancia de los jefes

Los jefes pueden modificar su posición y distancia respecto al jugador para mantener un combate dinámico y evitar que el jugador domine indefinidamente el intercambio cuerpo a cuerpo.

- Según su estilo de combate, situación y lectura de las acciones observables del jugador, un jefe puede retirarse momentáneamente, reposicionarse o crear distancia.
- El reposicionamiento puede servir para recuperar espacio, cambiar el ángulo de ataque, preparar una acción de su repertorio o interrumpir una presión cuerpo a cuerpo demasiado dominante.
- Estas decisiones deben ser coherentes con la identidad, capacidades y contexto de cada jefe; no todos tienen que utilizar las mismas tácticas.
- El movimiento no debe convertirse en una huida arbitraria ni en una forma de evitar indefinidamente el combate.
- El jefe no puede leer inputs futuros ni conocer información privada del jugador.
- Frecuencia, distancias, condiciones de activación y oportunidades de castigo durante estas maniobras quedan pendientes de diseño y balance.

El combate debe alternar de forma significativa presión, defensa, ataque y movimiento. Tanto el jugador como el jefe deben poder disputar el espacio y modificar el ritmo del enfrentamiento.


## Uso táctico del entorno por parte de los jefes

Los jefes pueden aprovechar elementos del escenario para modificar el desarrollo del combate, crear oportunidades y controlar el espacio. Esta capacidad depende del entorno disponible y de las características de cada jefe.

- Un jefe puede lanzar escombros u otros objetos del escenario para obligar al jugador a apartarse, interrumpir su presión o crear distancia.
- También puede aprovechar obstáculos, desniveles, coberturas, estrechamientos y otras características del terreno para favorecer su estilo de combate o condicionar el posicionamiento del jugador.
- Las acciones deben estar justificadas por elementos realmente presentes e interactuables en el escenario; el jefe no puede crear objetos o modificar el terreno sin una mecánica que lo permita.
- El uso del entorno debe ser coherente con la identidad, capacidades y repertorio del jefe. No todos los jefes utilizarán las mismas interacciones.
- Estas acciones deben ofrecer señales observables y oportunidades de respuesta adecuadas al tipo de amenaza, sin convertirse en ataques arbitrarios o inevitables.
- La disponibilidad de objetos, sus propiedades físicas relevantes, la destrucción del escenario y las reglas concretas de interacción quedan pendientes de especificación.

El entorno forma parte del espacio táctico del combate: no es únicamente un decorado, sino un conjunto de oportunidades y restricciones que ambos combatientes pueden aprovechar cuando sus capacidades lo permiten.


## Uso táctico del entorno por parte del jugador

El jugador también puede aprovechar el entorno contra los jefes. Las oportunidades ambientales deben ser coherentes con las reglas del escenario y con las capacidades de los combatientes, manteniendo una relación justa entre las posibilidades de ambos.

- El jugador puede activar o provocar mecanismos y peligros del escenario cuando existan medios jugables para hacerlo, como trampas, derrumbes u objetos que puedan impactar contra el jefe.
- Puede utilizar el posicionamiento, los obstáculos y las características del terreno para obtener ventajas tácticas.
- Las interacciones deben depender de elementos realmente presentes y habilitados en el escenario; no se presupone que todos los objetos sean manipulables o destructibles.
- Los jefes pueden reaccionar a estas acciones según sus capacidades, comportamiento y adaptación táctica, sin conocer inputs futuros ni información privada del jugador.
- Las oportunidades, riesgos, señales, consecuencias y posibles límites de cada interacción deben ser legibles y consistentes. El entorno no debe favorecer arbitrariamente a una de las partes.
- Qué elementos pueden activarse, dañarse, desplazarse o destruirse, y sus efectos concretos sobre vitalidad, postura, estados y fases del jefe quedan pendientes de especificación.

La interacción ambiental es bidireccional: tanto el jugador como el jefe pueden explotar el escenario, y cada oportunidad debe respetar las reglas y posibilidades establecidas para ese encuentro.


## Alteraciones persistentes del escenario durante el combate

El escenario puede sufrir cambios que persistan durante el resto del enfrentamiento. Las alteraciones no se revierten automáticamente mientras continúa ese combate, salvo que una mecánica específica establezca lo contrario.

- Elementos como columnas, puentes, coberturas y otros componentes pueden destruirse, derrumbarse o quedar inutilizados.
- Algunas zonas pueden convertirse en áreas peligrosas o quedar modificadas de forma que cambien las rutas, el posicionamiento y las opciones tácticas.
- Tanto las acciones del jefe como las del jugador pueden provocar alteraciones, siempre que las reglas del escenario y sus capacidades lo permitan.
- Los cambios deben tener consecuencias coherentes y legibles para ambos combatientes; la simulación debe conservar el estado resultante del escenario durante el encuentro.
- No se presupone que todos los elementos sean destructibles ni que todas las alteraciones sean permanentes fuera del combate actual.
- La persistencia entre fases, la restauración al reiniciar el encuentro y la persistencia entre visitas o sesiones quedan pendientes de definición.

El escenario evoluciona a medida que avanza el enfrentamiento y sus cambios acumulados forman parte del contexto táctico de las fases posteriores.


## Ataques de jefes que alteran directamente la arena

Los jefes pueden ejecutar ataques y habilidades que modifiquen directamente las condiciones del escenario, creando peligros, restricciones u oportunidades tácticas.

- Según su identidad y repertorio, pueden incendiar superficies o zonas, congelar el suelo, abrir grietas, provocar derrumbes o modificar temporalmente partes del terreno.
- Estas alteraciones pueden ser temporales o persistentes durante el combate, según la naturaleza de la habilidad y las reglas del encuentro.
- Los efectos pueden cambiar la movilidad, las rutas disponibles, el posicionamiento y las condiciones para atacar o defenderse.
- Las alteraciones deben tener señales observables y reglas consistentes para que el jugador pueda comprender el peligro y responder.
- El jugador puede aprovechar las consecuencias de estas modificaciones cuando las reglas del escenario lo permitan; no se presupone que todos los efectos sean manipulables.
- Cada jefe solo puede producir alteraciones compatibles con sus capacidades y temática. No se trata de una habilidad universal compartida por todos.
- Duración, extensión, acumulación, interacción entre efectos y consecuencias específicas quedan pendientes de diseño y balance.

Estas habilidades forman parte del repertorio de combate del jefe y hacen que la arena pueda cambiar activamente durante el enfrentamiento, en lugar de limitarse a recibir daños colaterales.


## Combinación e interacción entre efectos ambientales

Los efectos ambientales pueden interactuar y combinarse entre sí cuando sus propiedades y las reglas del escenario lo permitan. Estas interacciones pueden producir consecuencias tácticas emergentes que no necesitan estar diseñadas como una secuencia única y predeterminada.

- El hielo puede modificar el comportamiento de una superficie y, al romperse, generar terreno peligroso o fragmentos.
- El fuego puede interactuar con materiales inflamables y provocar combustión o explosiones si el escenario define esa propiedad.
- Un derrumbe puede bloquear rutas, modificar coberturas y obligar a ambos combatientes a cambiar su posicionamiento.
- Las interacciones pueden originarse tanto en acciones del jefe como del jugador y deben respetar las mismas reglas del mundo.
- No todos los efectos tienen que combinarse: cada interacción debe estar definida explícitamente por propiedades, condiciones y consecuencias compatibles.
- El sistema debe evitar combinaciones contradictorias, resultados duplicados no intencionados y cadenas infinitas de activaciones.
- Orden de resolución, prioridades, duración, acumulación y reglas de propagación quedan pendientes de especificación técnica y balance.

El objetivo es que el escenario genere oportunidades y riesgos emergentes mediante reglas consistentes, en vez de depender exclusivamente de eventos guionizados.


## Sistema general de propiedades y reglas ambientales combinables

Las interacciones ambientales se resolverán mediante un sistema general de propiedades, condiciones y reglas combinables, en lugar de implementar cada combinación como un caso especial aislado.

- Los elementos del escenario y los efectos ambientales podrán declarar propiedades relevantes para sus interacciones, como inflamabilidad, temperatura, fragilidad, conductividad u otras que se definan para el juego.
- Las reglas determinarán qué condiciones permiten una interacción, qué efectos produce y cómo cambia el estado de los elementos implicados.
- El sistema debe permitir incorporar nuevas propiedades, elementos y efectos mediante datos y reglas extensibles, minimizando la necesidad de modificar la lógica central.
- Las interacciones deben resolverse de forma determinista y consistente para un mismo estado y conjunto de acciones.
- Deben existir mecanismos para controlar prioridades, orden de resolución, duración, acumulación, propagación y prevención de ciclos o activaciones infinitas.
- No se presupone que cualquier propiedad pueda combinarse con cualquier otra: las interacciones válidas deben estar definidas explícitamente.
- La arquitectura técnica concreta, el formato de definición de reglas y el conjunto inicial de propiedades quedan pendientes de diseño e implementación.

Este sistema debe ser independiente del motor gráfico y de la simulación física concreta. El núcleo lógico define estados, condiciones y consecuencias; las capas externas se encargan de representar visualmente los efectos y, cuando corresponda, simular su física.


## Aplicación del sistema de propiedades e interacciones a los combatientes

El sistema general de propiedades e interacciones también podrá aplicarse al jugador y a los enemigos, incluidos los jefes. Así, las reglas ambientales y las interacciones entre combatientes compartirán un modelo coherente y extensible.

- Los combatientes podrán tener propiedades y condiciones relevantes para las interacciones, como estar mojados, congelados, ardiendo, electrificados o tener vulnerabilidades específicas.
- Los ataques, habilidades, estados y efectos del entorno podrán consultar esas propiedades y aplicar consecuencias definidas por reglas compatibles.
- Por ejemplo, un combatiente mojado puede conducir electricidad si las reglas del efecto lo establecen; una criatura inflamable puede reaccionar al fuego; y el hielo puede modificar su vulnerabilidad ante determinados ataques.
- Las interacciones deben poder afectar, según corresponda, a vitalidad, postura, movilidad, acciones disponibles y otros estados de combate.
- Las reglas se aplicarán de forma coherente al jugador y a los enemigos, salvo que una diferencia esté justificada explícitamente por una característica o inmunidad propia.
- Las propiedades, resistencias, vulnerabilidades e inmunidades concretas serán configurables por entidad y no se asumirán universales.
- El sistema debe mantener resolución determinista, control de prioridades y duración, y protección contra ciclos o activaciones infinitas.
- La representación visual y la simulación física seguirán separadas del núcleo lógico, que determina las condiciones y consecuencias del juego.

El objetivo es unificar las interacciones de escenario, objetos y combatientes en un mismo marco de reglas, evitando sistemas aislados y comportamientos contradictorios.


## Reacciones en cadena entre combatientes y elementos del escenario

Las interacciones podrán propagarse en cadena entre varios combatientes, objetos y elementos del escenario cuando las propiedades, condiciones y reglas definidas lo permitan.

- Un efecto aplicado a un combatiente u objeto puede desencadenar nuevas interacciones con entidades cercanas o conectadas, si existe una regla explícita que permita esa propagación.
- Por ejemplo, una descarga eléctrica puede propagarse desde un enemigo mojado hacia otro combatiente cercano y activar un mecanismo eléctrico del escenario, siempre que las condiciones de cada interacción se cumplan.
- La propagación puede atravesar distintas clases de entidades: jugador, enemigos, jefes, objetos, superficies y mecanismos del escenario.
- Cada paso de la cadena debe comprobar alcance, conectividad, propiedades, resistencias, inmunidades y demás condiciones relevantes.
- Las reacciones deben resolverse de forma determinista, con orden y prioridades definidos, para que un mismo estado inicial y las mismas acciones produzcan el mismo resultado.
- El sistema debe registrar qué interacciones ya se han procesado dentro de una misma cadena y establecer límites o condiciones de terminación para impedir ciclos, propagaciones infinitas y activaciones duplicadas no intencionadas.
- La propagación no implica que todos los efectos se transmitan automáticamente ni que atraviesen cualquier entidad o material; cada ruta debe estar autorizada por reglas explícitas.
- El alcance, la intensidad, la atenuación, el número máximo de saltos y las reglas concretas de propagación quedan pendientes de especificación y balance.

Estas reacciones en cadena deben surgir de la combinación de reglas generales del mundo, permitiendo situaciones tácticas emergentes sin depender de secuencias guionizadas para cada caso.


## Daño e interacciones entre enemigos

Los enemigos pueden dañarse y afectarse entre sí mediante ataques, habilidades, estados y efectos ambientales, incluso cuando el jugador no interviene directamente.

- Los ataques de un enemigo pueden alcanzar a otros enemigos si estos se encuentran dentro del área o trayectoria afectada y las reglas del ataque lo permiten.
- Las explosiones, descargas, peligros ambientales y reacciones en cadena pueden afectar a aliados y adversarios por igual, según sus propiedades, resistencias, inmunidades y reglas específicas.
- Los jefes también pueden afectar accidentalmente a sus subordinados u otros enemigos presentes en el encuentro.
- No se presupone que todos los ataques dañen a cualquier entidad: cada acción puede definir reglas de selección de objetivos, facciones, excepciones o inmunidades.
- Las consecuencias deben resolverse mediante el mismo sistema general de propiedades e interacciones, con resultados deterministas y sin privilegios arbitrarios por tratarse de aliados.
- La posibilidad de que ciertos enemigos eviten deliberadamente el fuego amigo, coordinen posiciones o aprovechen tácticamente el daño entre combatientes dependerá de su inteligencia y comportamiento, y queda pendiente de diseño.

El combate debe permitir consecuencias emergentes entre todos los participantes, no limitar las interacciones dañinas a las acciones del jugador contra los enemigos.


## Coordinación táctica entre enemigos

Determinados enemigos podrán coordinar sus acciones durante el combate para crear presión conjunta y aprovechar las oportunidades que generen sus aliados.

- Un enemigo puede presionar al jugador mientras otro intenta rodearlo, flanquearlo o cerrar sus rutas de escape.
- Un arquero u otro atacante a distancia puede aprovechar la distracción o la posición creada por un aliado para atacar.
- Algunos grupos podrán ejecutar ataques combinados, relevos de presión, emboscadas u otras maniobras compatibles con sus capacidades.
- La coordinación dependerá de la inteligencia, entrenamiento, naturaleza, vínculos y capacidades de cada enemigo o grupo. No todos los enemigos actuarán coordinadamente.
- Las maniobras deben respetar el espacio, la posición, los tiempos de acción y las posibilidades reales de cada participante; no deben producir ataques inevitables ni teletransportaciones o sincronizaciones arbitrarias.
- Los enemigos podrán verse afectados por las mismas reglas de colisión, daño entre aliados e interacciones ambientales ya establecidas.
- Las formas de comunicación, el alcance de coordinación, la selección de roles, la adaptación cuando un aliado cae y las maniobras concretas quedan pendientes de diseño.

La coordinación debe surgir del comportamiento y las capacidades de los grupos, haciendo que algunos encuentros requieran gestionar amenazas simultáneas y no simplemente derrotar enemigos aislados.


## Cambio de comportamiento al perder compañeros

Los enemigos pueden modificar su comportamiento cuando sus aliados son derrotados, se retiran o dejan de estar disponibles para coordinarse. La reacción dependerá de su personalidad, inteligencia, naturaleza, vínculos y situación de combate.

- Algunos enemigos pueden huir, retroceder o intentar evitar el enfrentamiento cuando se quedan solos o pierden a un aliado importante.
- Otros pueden volverse más agresivos, intentar vengar a sus compañeros o asumir mayores riesgos.
- Enemigos tácticos pueden cambiar de formación, abandonar maniobras coordinadas, adoptar nuevos roles o priorizar objetivos distintos cuando pierden apoyo.
- Algunos pueden mantener su comportamiento original si su entrenamiento, disciplina, naturaleza o motivaciones lo justifican.
- Las reacciones deben basarse en hechos observables y en el contexto disponible para el enemigo; no deben conocer información privada del jugador ni anticipar acciones futuras.
- La pérdida de aliados no debe activar una reacción universal: cada arquetipo o individuo puede definir sus propias respuestas y condiciones.
- Los umbrales, tiempos de reacción y transiciones entre comportamientos quedan pendientes de diseño y balance.

La composición del grupo debe importar durante todo el encuentro: derrotar, separar o hacer huir a determinados enemigos puede cambiar la dinámica y las opciones tácticas de los supervivientes.


## Instinto de supervivencia, rendición y solicitud de refuerzos

Determinados enemigos pueden mostrar instinto de supervivencia y reaccionar cuando consideran que la situación de combate se ha vuelto desfavorable o desesperada.

- Según su personalidad, inteligencia, disciplina, lealtad y contexto, un enemigo puede intentar huir, rendirse o solicitar refuerzos.
- La rendición y la huida son comportamientos distintos y sus consecuencias concretas deberán definirse para cada tipo de encuentro.
- Solicitar refuerzos solo será posible cuando el contexto del encuentro lo permita: deben existir aliados disponibles y un medio plausible para alertarlos. No se crearán refuerzos arbitrariamente.
- Las decisiones deben basarse en información observable y disponible para el enemigo, sin conocimiento de información privada del jugador ni de acciones futuras.
- No todos los enemigos mostrarán estas conductas; algunos lucharán hasta morir por naturaleza, entrenamiento, fanatismo, órdenes u otras características justificadas.
- La negociación está expresamente fuera del alcance del sistema y no forma parte de las mecánicas contempladas.
- Las condiciones de activación, señales, tiempos, interrupciones y consecuencias de la huida, rendición y solicitud de refuerzos quedan pendientes de diseño.

Estas conductas amplían las posibles respuestas de los enemigos sin convertir la negociación en una mecánica del juego.


## Memoria de encuentros anteriores por parte de enemigos supervivientes

Determinados enemigos que sobrevivan a un encuentro y huyan podrán conservar recuerdos relevantes de su enfrentamiento con el jugador. Si vuelven a encontrarse, podrán reconocerlo y modificar su comportamiento en función de esa experiencia.

- El reconocimiento y la memoria dependerán de las capacidades, inteligencia, naturaleza y características individuales del enemigo.
- Un enemigo puede reaccionar con temor, cautela, hostilidad, preparación táctica o confianza excesiva, según lo que haya vivido y cómo lo interprete.
- Puede recordar información relevante que haya observado, como tácticas recurrentes del jugador, el uso de determinadas armas o habilidades y el resultado de su encuentro anterior.
- La memoria no implica conocimiento omnisciente: el enemigo solo puede recordar hechos que pudo percibir y conservar.
- La información recordada podrá influir en decisiones y estrategias posteriores, pero no garantiza que el enemigo conozca el estado actual del jugador ni sus capacidades presentes.
- No todos los enemigos tendrán memoria persistente entre encuentros; su capacidad y fidelidad dependerán de su diseño.
- El alcance temporal, almacenamiento, selección de recuerdos, actualización, olvido y persistencia entre sesiones quedan pendientes de diseño técnico.

La memoria entre encuentros complementa la adaptación táctica durante un combate: permite que las acciones del jugador tengan consecuencias en futuros enfrentamientos con supervivientes, sin conceder a los enemigos información que no podrían conocer.


## Transmisión de información sobre el jugador entre enemigos de una misma facción

Los enemigos supervivientes podrán transmitir a otros miembros de su facción información relevante que hayan observado sobre el jugador. Esta información puede influir en la preparación y el comportamiento de futuros encuentros.

- Un enemigo que escape o disponga de un medio plausible de comunicación puede advertir a aliados o guarniciones de su facción.
- La información transmitida puede incluir armas, habilidades, tácticas recurrentes y otros comportamientos que el emisor haya observado directamente.
- La transmisión requiere un medio y una oportunidad plausibles; no se comparte información automáticamente entre enemigos aislados o sin comunicación.
- Los receptores solo conocerán lo que efectivamente se les haya comunicado. La información puede ser incompleta, imprecisa, estar desactualizada o ser interpretada de manera distinta.
- La facción puede utilizar esos informes para prepararse, modificar formaciones, seleccionar tácticas o ajustar la vigilancia, según sus recursos, inteligencia y organización.
- La información no concede conocimiento omnisciente sobre la ubicación, estado, equipo actual ni futuras acciones del jugador.
- La capacidad de transmitir, recibir, conservar y utilizar informes dependerá de la estructura, tecnología, inteligencia y medios de comunicación de cada facción.
- Las condiciones de comunicación, alcance, fiabilidad, caducidad y consecuencias concretas quedan pendientes de diseño.

La memoria individual de los supervivientes y la transmisión de información entre miembros de una facción son sistemas relacionados pero distintos: un enemigo puede recordar una experiencia sin haber podido comunicarla, y una facción solo puede prepararse con la información que realmente haya recibido. El objetivo es que las acciones del jugador puedan tener consecuencias en encuentros posteriores y mantengan una tensión constante, sin recurrir a conocimiento injustificado por parte de los enemigos.


## Nivel de alerta de las facciones ante las acciones del jugador

Las facciones podrán mantener un nivel de alerta que evolucione según las acciones del jugador y la información que hayan recibido sobre él. Este nivel puede influir en la seguridad, preparación y comportamiento de sus miembros y territorios.

- Acciones como eliminar patrullas, atacar puestos estratégicos, sabotear instalaciones o convertirse en una amenaza recurrente pueden elevar la alerta de una facción.
- La alerta puede provocar medidas como reforzar fortalezas, aumentar o modificar patrullas, mejorar la vigilancia, proteger objetivos importantes y preparar emboscadas.
- Las respuestas dependerán de los recursos, organización, inteligencia, territorio y personalidad de cada facción; no todas reaccionarán de la misma manera.
- La alerta debe basarse en acontecimientos conocidos por la facción, mediante observación directa, supervivientes, informes u otros medios de comunicación plausibles.
- La facción no puede reaccionar a acciones que desconoce ni obtener información omnisciente sobre la ubicación, estado o planes del jugador.
- La alerta puede variar con el tiempo y según nuevos acontecimientos. Sus condiciones de aumento, reducción, persistencia, alcance territorial y efectos concretos quedan pendientes de diseño.
- El nivel de alerta de una facción debe distinguirse de la memoria individual de los enemigos y de los informes compartidos: estos sistemas pueden alimentarlo, pero no son equivalentes.

El objetivo es que las acciones del jugador tengan consecuencias estratégicas a escala de facción y que el mundo responda de manera coherente, sin convertir cada territorio en un estado de hostilidad universal e invariable.


## Reacción de la vida cotidiana y los territorios ante los acontecimientos

El mundo de Mythical Exodus debe reaccionar a los acontecimientos de forma persistente y coherente. Las consecuencias de las acciones del jugador no se limitarán al combate ni a las fuerzas armadas de una facción: también podrán modificar la vida cotidiana de los territorios y de sus habitantes.

- La situación de seguridad de una región puede afectar a sus actividades cotidianas. Ante amenazas conocidas, algunos comerciantes pueden cerrar temporalmente, reducir sus horarios o trasladarse; los habitantes pueden evitar ciertas zonas, permanecer en sus hogares o abandonar lugares peligrosos.
- Las autoridades y facciones pueden establecer controles, inspeccionar viajeros, restringir accesos, reforzar puestos de vigilancia, imponer medidas de protección o cerrar determinadas rutas cuando dispongan de motivos y recursos para hacerlo.
- Las rutas comerciales, el abastecimiento, los servicios y la actividad económica pueden verse afectados por patrullas, emboscadas, conflictos, bloqueos, destrucción de infraestructuras y otros acontecimientos del mundo.
- Los habitantes y NPC podrán reaccionar de manera diferente según su personalidad, ocupación, lealtades, recursos, información disponible y exposición directa a los acontecimientos. No habrá una reacción uniforme de toda la población.
- La información se propagará de manera plausible. Los habitantes, comerciantes y autoridades solo podrán reaccionar a sucesos que hayan presenciado o conocido mediante rumores, testigos, mensajeros u otros medios establecidos en el mundo. Los rumores pueden ser incompletos, incorrectos o tardíos.
- Las consecuencias podrán variar entre regiones de una misma facción. Un territorio cercano al conflicto puede vivir bajo vigilancia o peligro, mientras que otro que no haya recibido noticias ni sufrido sus efectos puede mantener una vida cotidiana normal.
- Los cambios no serán necesariamente permanentes. Cuando las causas desaparezcan y las condiciones lo permitan, una región podrá recuperar gradualmente sus actividades, aunque daños, pérdidas, desplazamientos o desconfianza puedan prolongar sus consecuencias.
- Las decisiones y acciones del jugador podrán mejorar o empeorar las condiciones de una comunidad, afectar su seguridad y alterar sus oportunidades económicas y sociales. No se presupone que toda consecuencia sea negativa ni que el jugador controle directamente cada reacción.
- La respuesta del mundo debe surgir de acontecimientos, información, recursos y reglas coherentes, no de cambios arbitrarios destinados a castigar al jugador.

Este sistema complementa la alerta de facciones: la alerta representa la percepción y preparación de una organización, mientras que la reacción territorial representa cómo los acontecimientos y las respuestas de sus habitantes, autoridades y servicios transforman la vida cotidiana. Ambos sistemas podrán influirse mutuamente, pero no son equivalentes.

El objetivo es construir un mundo reactivo y persistente, en el que las acciones del jugador y los acontecimientos entre facciones tengan consecuencias visibles y funcionales más allá del encuentro inmediato. El nivel de detalle de la simulación social, los estados regionales, los canales de información, las escalas temporales y las reglas de recuperación quedan pendientes de diseño.


## Rutinas autónomas y continuidad de vida de los NPC

Los NPC, especialmente los personajes importantes, podrán tener rutinas y actividades propias que continúen aunque el jugador no esté presente. El mundo no debe limitar la existencia y actividad de sus habitantes a los momentos en que el jugador los observa.

- Los NPC podrán desarrollar actividades cotidianas según su ocupación, responsabilidades, personalidad, necesidades y circunstancias: trabajar, descansar, desplazarse, visitar lugares, comerciar, entrenar, cumplir obligaciones o relacionarse con otros personajes.
- Las rutinas serán configurables y podrán variar según horarios, días, acontecimientos, condiciones del territorio, relaciones y otros factores relevantes.
- Los NPC podrán interrumpir, modificar o abandonar sus actividades cuando surjan acontecimientos que conozcan y que afecten a su seguridad, responsabilidades o intereses.
- Los cambios de seguridad y las alteraciones territoriales ya definidas podrán repercutir en sus rutinas: un comerciante puede cerrar, un guardia cambiar de puesto, un habitante buscar refugio o un trabajador dejar de acudir a una instalación inaccesible.
- Los personajes importantes podrán tener rutinas, objetivos, relaciones y reacciones individuales más elaboradas que los NPC secundarios. El nivel de detalle dependerá de su relevancia y de las necesidades de diseño.
- Los NPC solo reaccionarán a sucesos que hayan percibido o conocido mediante canales plausibles. No tendrán conocimiento omnisciente de acontecimientos ocurridos lejos de ellos.
- Las actividades y desplazamientos deberán respetar las condiciones del mundo: lugares accesibles, rutas disponibles, horarios, recursos, peligros y capacidades del personaje.
- La ausencia del jugador no debe detener conceptualmente la vida del mundo. Los NPC podrán avanzar en sus actividades y los acontecimientos podrán modificar sus circunstancias dentro de las reglas de simulación establecidas.
- La simulación deberá permitir distintos niveles de detalle para gestionar el coste computacional sin cambiar arbitrariamente los resultados relevantes. La estrategia técnica para actualizar NPC fuera del área activa, resolver actividades no observadas y mantener continuidad entre sesiones queda pendiente de diseño.
- No se presupone que cada NPC necesite una simulación completa y continua en todo momento. El núcleo lógico deberá representar estados, rutinas, decisiones y consecuencias de forma determinista y comprobable; la presentación y animación de sus actividades corresponderán a capas externas.

El objetivo es que los NPC sean habitantes de un mundo que continúa evolucionando, y no simples elementos que aparecen para atender al jugador. Sus vidas y decisiones podrán generar acontecimientos y consecuencias que el jugador descubrirá al regresar, incluso si no estuvo presente cuando ocurrieron.


## Relaciones autónomas y cambiantes entre NPC

Los NPC podrán desarrollar y mantener relaciones entre sí independientemente de la presencia o intervención del jugador. Las relaciones forman parte de la evolución social del mundo y podrán influir en decisiones, rutinas, cooperación, conflictos y acontecimientos.

- Las relaciones podrán incluir amistad, confianza, rivalidad, enemistad, lealtad, alianzas, vínculos familiares, relaciones románticas y otras categorías que el diseño narrativo establezca.
- Cada relación podrá tener características propias y no tendrá que ser necesariamente simétrica: dos NPC pueden interpretar o valorar su vínculo de manera diferente.
- Las relaciones podrán evolucionar, fortalecerse, deteriorarse, romperse o transformarse a partir de experiencias compartidas, acciones, promesas, traiciones, conflictos, ayuda recibida, intereses y acontecimientos relevantes.
- Los NPC podrán iniciar, mantener o terminar vínculos por iniciativa propia, según su personalidad, objetivos, valores, circunstancias y oportunidades.
- Los vínculos podrán influir en el comportamiento: ayudar o proteger a alguien, cooperar, compartir información, evitarse, competir, enfrentarse, reconciliarse o asumir riesgos por otra persona.
- Las relaciones podrán afectar las rutinas y actividades autónomas de los NPC, así como su pertenencia a grupos, alianzas y redes sociales.
- Los acontecimientos que afecten a un NPC podrán repercutir en sus relaciones. La muerte, desaparición, huida, traición o cambio de bando de un personaje puede alterar las decisiones y vínculos de quienes lo conocían.
- La información sobre acciones y acontecimientos se basará en lo que cada NPC haya presenciado o conocido mediante medios plausibles. No todos conocerán automáticamente las acciones de los demás, y los rumores pueden ser incompletos o erróneos.
- Las relaciones no deben reducirse a una única puntuación universal cuando existan dimensiones distintas relevantes para el comportamiento. La representación interna, los atributos, umbrales y reglas concretas quedan pendientes de diseño.
- La evolución de relaciones debe ser coherente con la personalidad y memoria de cada NPC; no se producirán cambios arbitrarios únicamente para dirigir al jugador hacia un resultado narrativo.
- Las relaciones románticas podrán existir como una categoría de vínculo, pero sus condiciones, alcance narrativo y posibles consecuencias deberán definirse por separado antes de implementar comportamientos concretos.

El objetivo es que las comunidades tengan una estructura social dinámica: sus habitantes pueden formar vínculos, entrar en conflicto, cooperar y cambiar con el tiempo incluso cuando el jugador no interviene. Estas relaciones podrán producir acontecimientos y consecuencias que el jugador descubra posteriormente, sin que el mundo social gire exclusivamente alrededor de él.


## Consecuencias intergeneracionales y memoria histórica de las comunidades

Los acontecimientos importantes podrán dejar consecuencias que se transmitan entre generaciones. La historia de una comunidad no se limitará a quienes vivieron directamente un suceso: podrá influir en la identidad, las relaciones, las instituciones y las decisiones de quienes nazcan o crezcan después.

- Las guerras, persecuciones, masacres, desplazamientos, traiciones, pactos, sacrificios y actos de ayuda podrán dejar recuerdos colectivos y consecuencias duraderas en las comunidades afectadas.
- Las generaciones posteriores podrán conocer esos acontecimientos mediante relatos familiares, educación, tradiciones, monumentos, documentos, instituciones, rumores y otras formas plausibles de transmisión.
- La memoria histórica podrá influir en la confianza, el temor, el resentimiento, la gratitud, los prejuicios, las alianzas y la disposición a cooperar con otras comunidades o facciones.
- Los descendientes de personas que ayudaron, protegieron o perjudicaron a una comunidad podrán recibir vínculos, reputaciones, privilegios, obligaciones o desconfianza heredados, siempre que exista una vía social o institucional que justifique esa continuidad.
- Las consecuencias no serán necesariamente idénticas para todos los miembros de una generación. La educación, las experiencias personales, la cultura, la posición social y la información disponible podrán producir interpretaciones y respuestas diferentes.
- La memoria colectiva podrá cambiar con el tiempo. Los relatos pueden conservarse, olvidarse, reinterpretarse, disputarse o manipularse; no se presupone que todas las versiones históricas sean completas o exactas.
- Las instituciones y facciones podrán mantener políticas, celebraciones, prohibiciones, reclamaciones o conflictos originados en acontecimientos pasados, si cuentan con continuidad organizativa y conocimiento de esos hechos.
- La transmisión intergeneracional no concede recuerdos personales a quienes no vivieron los acontecimientos: los descendientes conocerán versiones transmitidas y podrán interpretarlas, cuestionarlas o rechazarlas.
- Las consecuencias heredadas podrán atenuarse, reforzarse o transformarse mediante nuevos acontecimientos, decisiones individuales, reconciliaciones, cambios políticos y relaciones entre comunidades.
- La persistencia histórica debe respetar la cronología y la causalidad. No se atribuirán consecuencias a generaciones, familias o territorios sin una cadena plausible de transmisión.
- La profundidad temporal, el modelo de generaciones, la persistencia entre etapas de la partida y las reglas de transmisión, olvido y transformación quedan pendientes de diseño.

El objetivo es que Mythical Exodus tenga una historia viva: las acciones del jugador y los acontecimientos del mundo pueden convertirse en parte de la memoria de las comunidades y afectar a personas que nunca estuvieron presentes cuando ocurrieron. El pasado debe influir en el presente sin determinarlo por completo.


## Acontecimientos históricos autónomos con impacto narrativo controlado

El mundo podrá generar acontecimientos relevantes sin intervención directa del jugador, de modo que las facciones, comunidades y territorios no permanezcan estáticos. Sin embargo, estos acontecimientos deberán producirse con moderación y bajo límites narrativos claros para no desviar, romper o volver incoherente el desarrollo principal de la historia.

- Podrán ocurrir acontecimientos como conflictos entre facciones, rebeliones, disputas sucesorias, cambios de poder, epidemias, crisis económicas, descubrimientos o desastres, siempre que sean plausibles dentro del contexto del mundo.
- La frecuencia, escala, duración y alcance de estos acontecimientos estarán controlados. No se generarán crisis importantes de manera constante ni se acumularán múltiples sucesos de gran impacto sin justificación.
- La mayoría de los acontecimientos autónomos deberán tener alcance local o regional. Los sucesos capaces de alterar profundamente el equilibrio general del mundo serán excepcionales y requerirán condiciones narrativas y sistémicas suficientes.
- Los acontecimientos deberán respetar la cronología, las capacidades de las facciones, sus recursos, relaciones, conflictos previos y la información que realmente poseen.
- La historia principal y sus hitos imprescindibles tendrán prioridad. La simulación autónoma no podrá eliminar arbitrariamente personajes, lugares, objetos, encuentros o condiciones necesarios para que la trama principal siga siendo coherente y completable.
- Los acontecimientos autónomos podrán enriquecer el contexto, modificar circunstancias secundarias y abrir o cerrar oportunidades, pero no deberán sustituir por accidente decisiones narrativas centrales ni imponer desenlaces principales que el diseño reserva al jugador.
- Cuando un acontecimiento pueda afectar a una misión, personaje o lugar importante, deberá respetar las dependencias narrativas definidas. Podrá modificar su contexto o disponibilidad si el diseño lo permite, pero no invalidar requisitos esenciales sin una alternativa intencionalmente diseñada.
- La intervención del jugador podrá prevenir, acelerar, modificar o aprovechar ciertos acontecimientos si dispone de medios plausibles. No todos los sucesos tendrán que ser evitables ni todos requerirán participación del jugador.
- Los acontecimientos podrán producir consecuencias persistentes en territorios, facciones, relaciones, economía y memoria histórica, de acuerdo con los sistemas ya definidos, pero su impacto deberá ser proporcional a su escala.
- El sistema distinguirá entre sucesos cotidianos, incidentes locales, acontecimientos regionales y eventos excepcionales de alcance amplio. Los criterios de clasificación, frecuencia, probabilidad, límites simultáneos y reglas de resolución quedan pendientes de diseño.
- La autonomía del mundo no debe convertirse en aleatoriedad sin sentido: cada acontecimiento debe tener causas identificables, condiciones de posibilidad y consecuencias coherentes. Se priorizarán la consistencia narrativa y la agencia del jugador frente a la generación indiscriminada de eventos.

El objetivo es que el mundo continúe evolucionando por sí mismo y pueda sorprender al jugador, pero sin competir descontroladamente con la historia principal. La simulación autónoma debe dar profundidad y credibilidad al relato, no apropiarse de su dirección.
