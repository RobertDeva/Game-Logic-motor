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
