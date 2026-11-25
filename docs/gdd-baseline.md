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


## Calendario y paso del tiempo del mundo

Mythical Exodus contará con un sistema temporal propio para representar el paso del tiempo dentro del mundo y permitir que las actividades, condiciones y acontecimientos evolucionen de forma coherente.

- El mundo podrá representar días y noches, ciclos de calendario y estaciones. La estructura exacta del calendario —duración de días, meses, estaciones y año— queda pendiente de definición.
- El tiempo podrá influir en las rutinas y horarios de los NPC, la disponibilidad de comercios y servicios, la actividad de determinados enemigos, las condiciones ambientales y la ocurrencia de ciertos acontecimientos.
- Algunos lugares, personajes, actividades, encuentros u oportunidades podrán estar disponibles únicamente en determinados momentos o condiciones temporales, siempre que el diseño lo establezca de forma legible para el jugador.
- Las estaciones y ciclos podrán influir en el clima, la vegetación, las rutas, los recursos y otros aspectos del territorio cuando sean pertinentes para cada región.
- Los acontecimientos autónomos, las relaciones, la alerta de facciones y las rutinas de NPC podrán utilizar el tiempo del mundo para programar, actualizar o resolver cambios, respetando sus reglas de causalidad y sus límites narrativos.
- El tiempo debe avanzar de forma coherente y no depender exclusivamente de que el jugador observe una actividad. Las actividades y acontecimientos podrán progresar fuera de su presencia mediante la simulación temporal ya prevista.
- El sistema deberá permitir distintos ritmos de actualización: no todas las entidades necesitan procesarse continuamente si sus estados y resultados pueden resolverse de forma determinista al avanzar el tiempo.
- El avance temporal deberá ser controlable por las reglas del juego. Descansar, esperar, viajar, realizar actividades u otras acciones podrán consumir tiempo si así se define; no se asume todavía una duración concreta para ninguna actividad.
- El calendario y sus efectos deberán integrarse con la narrativa principal para evitar que el paso del tiempo invalide arbitrariamente misiones, personajes o hitos imprescindibles. Los límites y excepciones narrativas deberán definirse explícitamente.
- La arquitectura deberá representar el tiempo del mundo de forma independiente del reloj real, de la tasa de fotogramas y del motor gráfico. La unidad temporal, precisión, persistencia, pausas y reglas de actualización quedan pendientes de diseño técnico.

El objetivo es que el mundo tenga un ritmo propio y que el momento en que el jugador actúe o llegue a un lugar pueda tener consecuencias jugables y narrativas, sin introducir presión temporal injustificada ni comprometer la coherencia de la historia.


## Comportamiento de enemigos y criaturas según el ciclo día/noche

El ciclo día/noche podrá modificar de forma significativa la actividad y el comportamiento de enemigos y criaturas del mundo.

- Algunas especies o grupos podrán tener patrones diurnos, nocturnos o crepusculares. Su actividad, presencia y zonas de aparición dependerán de su naturaleza, hábitat y circunstancias.
- Depredadores y otras criaturas podrán salir a cazar, alimentarse, descansar, refugiarse o regresar a sus guaridas en momentos determinados.
- Algunos enemigos podrán establecer, abandonar o reorganizar campamentos según la hora, sus rutinas, objetivos y condiciones del territorio.
- Las patrullas podrán cambiar sus recorridos, horarios, composición, frecuencia y nivel de vigilancia durante el día y la noche.
- Determinados enemigos o criaturas podrán aparecer, retirarse o desplazarse a otras zonas según el ciclo temporal, sin que ello implique que desaparezcan arbitrariamente si el jugador está cerca.
- El comportamiento nocturno no será necesariamente más hostil o peligroso en todos los casos: cada especie, facción y grupo tendrá patrones configurables coherentes con su identidad y contexto.
- La iluminación y visibilidad podrán influir en la detección, vigilancia, sigilo y percepción de los combatientes cuando esas reglas estén definidas. El cálculo físico y la representación visual corresponden a las capas especializadas; el núcleo lógico resolverá sus consecuencias jugables a partir de datos y condiciones explícitas.
- Los cambios de actividad deberán respetar la continuidad del mundo, las capacidades de cada entidad y la información que posea. No se permitirá que enemigos se teletransporten o conozcan la posición del jugador sin una causa o mecanismo válido.
- Las rutinas y transiciones podrán resolverse mediante distintos niveles de simulación, manteniendo resultados coherentes tanto si el jugador presencia los cambios como si ocurren fuera de su presencia.

La finalidad es que el ciclo temporal transforme la ecología, la actividad de las facciones y las oportunidades tácticas del mundo, en lugar de limitarse a un cambio visual de iluminación.


## Clima dinámico y condiciones meteorológicas cambiantes

El clima podrá cambiar durante la partida y no estará limitado a una condición fija por región. Cada territorio podrá experimentar variaciones meteorológicas coherentes con su geografía, estación, altitud, ecosistema y demás condiciones ambientales definidas.

- Las condiciones podrán evolucionar entre estados como cielo despejado, nubosidad, lluvia, tormentas, niebla, nieve, viento y otros fenómenos apropiados para cada región.
- La frecuencia, duración, intensidad y transiciones de los fenómenos serán configurables por región y estación. No todos los fenómenos estarán disponibles en todos los territorios.
- El clima podrá afectar a la visibilidad, percepción, detección, sigilo, desplazamiento y comportamiento de criaturas y NPC cuando existan reglas explícitas que relacionen esas condiciones.
- Las condiciones meteorológicas podrán modificar el estado del entorno: superficies mojadas o congeladas, acumulación de nieve, barro, vegetación, cursos de agua y otros elementos pertinentes.
- El clima podrá interactuar con el sistema general de propiedades y reacciones ambientales. Por ejemplo, la lluvia podrá mojar superficies y combatientes; el frío podrá favorecer la congelación; el viento podrá modificar la propagación de fuego, humo u otros efectos cuando las reglas lo permitan.
- Las condiciones meteorológicas podrán influir en el combate de forma recíproca y legible. Sus consecuencias deberán ser coherentes para jugador, enemigos y jefes, respetando resistencias, inmunidades y capacidades individuales.
- Las criaturas y facciones podrán modificar sus rutinas, refugiarse, desplazarse o aprovechar determinadas condiciones meteorológicas según su naturaleza, recursos e inteligencia.
- Los fenómenos peligrosos deberán ofrecer señales y oportunidades de respuesta apropiadas. No se utilizará el clima para imponer daño inevitable o penalizaciones arbitrarias al jugador.
- El estado meteorológico podrá variar entre regiones y actualizarse aunque el jugador no esté presente. La simulación fuera de la zona activa podrá utilizar niveles de detalle reducidos sin perder coherencia temporal.
- El núcleo lógico representará estados, transiciones, propiedades y consecuencias meteorológicas de forma determinista y desacoplada de la representación visual y de la simulación física.

Los patrones climáticos concretos, sus probabilidades, intensidades, duración, alcance regional y reglas de interacción se definirán posteriormente. El objetivo es que el clima sea un sistema dinámico del mundo y una fuente coherente de cambios ambientales y tácticos, no un simple efecto visual.


## Fenómenos meteorológicos extremos y recuperación territorial

El mundo podrá experimentar fenómenos meteorológicos extremos, excepcionales y de intensidad variable, capaces de afectar temporalmente a regiones y comunidades.

- Entre los fenómenos posibles se incluyen huracanes, inundaciones, sequías, ventiscas, tormentas eléctricas intensas, olas de calor o frío y otros desastres meteorológicos apropiados para cada territorio.
- La aparición, intensidad, duración y alcance de estos fenómenos dependerán de las condiciones geográficas, climáticas y estacionales de cada región. No todos los territorios estarán expuestos a todos los fenómenos.
- Sus consecuencias podrán incluir daños en construcciones e infraestructuras, rutas temporalmente bloqueadas, pérdida o deterioro de recursos, alteraciones de ecosistemas, desplazamiento de criaturas y cambios en las actividades de las comunidades.
- Las afecciones provocadas por estos fenómenos serán temporales. No se diseñarán como transformaciones irreversibles del territorio.
- Después de cada fenómeno deberá existir un proceso de recuperación progresiva. Según el daño y el territorio, este podrá incluir drenaje de inundaciones, retirada de nieve y escombros, reparación de infraestructuras, recuperación de rutas, regeneración de vegetación y fauna, restablecimiento de actividades y recuperación de recursos.
- La recuperación no será necesariamente instantánea ni uniforme: su velocidad dependerá de la gravedad del fenómeno, las características naturales de la región, los recursos disponibles y la capacidad de sus habitantes o facciones para responder.
- Las comunidades y facciones podrán participar en la recuperación mediante trabajo, organización, recursos y decisiones. Su capacidad de respuesta podrá variar y producir ritmos de recuperación distintos entre regiones.
- Durante la recuperación podrán persistir efectos secundarios temporales, como servicios limitados, escasez, rutas parcialmente transitables o cambios en las rutinas de NPC. Estos efectos deberán disminuir conforme se cumplan las condiciones de recuperación.
- El sistema deberá garantizar que las consecuencias temporales tengan condiciones de finalización y no queden bloqueadas indefinidamente por estados imposibles o dependencias circulares.
- Los fenómenos extremos y su recuperación podrán avanzar sin la presencia del jugador, mediante simulación temporal de distintos niveles de detalle, manteniendo coherencia y determinismo.
- El núcleo lógico gestionará los estados, daños, etapas y condiciones de recuperación; la representación visual y la simulación física de los fenómenos corresponderán a las capas especializadas.

El objetivo es que los fenómenos extremos produzcan consecuencias relevantes y oportunidades de juego, pero que el territorio pueda recuperarse con el tiempo. No deberán convertirse en cambios permanentes e irreversibles del mundo.


## Desastres geológicos y recuperación territorial

El mundo podrá experimentar desastres naturales de origen geológico, con efectos temporales sobre el terreno, las infraestructuras, los ecosistemas y las comunidades.

- Entre los fenómenos posibles se incluyen terremotos, erupciones volcánicas, deslizamientos de tierra, desprendimientos, hundimientos y otros procesos geológicos apropiados para cada región.
- La posibilidad, frecuencia, intensidad y alcance de cada fenómeno dependerán de las características geológicas y geográficas del territorio. No todas las regiones estarán expuestas a los mismos riesgos.
- Sus consecuencias podrán incluir daños en edificios e infraestructuras, derrumbes, rutas bloqueadas, cambios temporales en la accesibilidad, incendios secundarios, alteraciones de cursos de agua, caída de ceniza y desplazamiento de criaturas y habitantes.
- Las afecciones territoriales y sociales causadas por estos desastres serán temporales y deberán contar con procesos de recuperación. No se utilizarán para imponer transformaciones permanentes e irreversibles del territorio.
- La recuperación podrá incluir retirada de escombros y ceniza, estabilización de laderas, reparación de construcciones, reconstrucción de rutas e infraestructuras, recuperación de servicios y restauración progresiva de actividades comunitarias y ecosistemas.
- La duración y el ritmo de recuperación dependerán de la gravedad del desastre, la naturaleza del daño, las condiciones del terreno, los recursos disponibles y la capacidad de respuesta de las comunidades y facciones.
- Los efectos secundarios podrán persistir durante distintas etapas de recuperación, pero deberán existir condiciones explícitas para que cada afección temporal avance y finalice. El sistema evitará estados bloqueados y dependencias circulares.
- Las comunidades, facciones y NPC podrán responder al desastre mediante evacuación, refugio, asistencia, reparación y reorganización de actividades, según sus capacidades y la información disponible.
- Los desastres y la recuperación podrán progresar sin la presencia del jugador mediante simulación temporal de distintos niveles de detalle, manteniendo coherencia y determinismo.
- Los desastres geológicos podrán interactuar con el clima y con el sistema de propiedades y reacciones ambientales cuando existan reglas explícitas. Por ejemplo, la lluvia podrá agravar ciertos deslizamientos o las cenizas podrán afectar temporalmente la visibilidad y las actividades.
- El núcleo lógico gestionará estados, consecuencias y etapas de recuperación; la representación visual y la simulación física de los fenómenos corresponderán a las capas especializadas.

El objetivo es que los desastres geológicos sean acontecimientos relevantes y coherentes con la naturaleza de cada región, capaces de alterar temporalmente la vida y las oportunidades del mundo, pero siempre sujetos a un proceso de recuperación.


## Ciclos ecológicos de flora y fauna

Los ecosistemas de Mythical Exodus evolucionarán a lo largo del tiempo. La flora y la fauna tendrán ciclos naturales propios y sus poblaciones podrán cambiar según las condiciones del territorio, sin permanecer estáticas ni depender exclusivamente de la presencia del jugador.

- Las especies animales podrán reproducirse, alimentarse, descansar, competir, desplazarse y migrar según su naturaleza, ciclo vital, hábitat y disponibilidad de recursos.
- Las poblaciones podrán aumentar o disminuir por reproducción, mortalidad, depredación, enfermedades, escasez de alimento, cambios ambientales y otros factores ecológicos definidos.
- La flora podrá crecer, florecer, producir frutos o semillas, dispersarse, marchitarse y regenerarse de acuerdo con la especie, la estación, el clima, el suelo y la disponibilidad de agua y nutrientes.
- Las estaciones y las condiciones meteorológicas influirán en los ciclos de reproducción, migración, alimentación, crecimiento y actividad de las especies cuando resulte coherente con su biología.
- Los fenómenos meteorológicos extremos y los desastres geológicos podrán afectar temporalmente a hábitats, recursos y poblaciones. Sus consecuencias ecológicas deberán integrarse con los procesos de recuperación territorial ya establecidos.
- Las relaciones entre especies, como depredación, competencia, simbiosis y dependencia alimentaria, podrán modelarse cuando sean relevantes para el ecosistema y estén definidas mediante reglas explícitas.
- La disponibilidad de alimento, agua, refugio y espacio habitable deberá limitar el crecimiento de las poblaciones. No se permitirán expansiones ilimitadas ni generación espontánea de recursos o criaturas.
- Las migraciones y desplazamientos deberán respetar hábitats adecuados, rutas plausibles, barreras territoriales y condiciones ambientales. Las especies no aparecerán o desaparecerán arbitrariamente para simular cambios de población.
- La actividad de la fauna y el estado de la flora podrán influir en la exploración, los recursos recolectables, la presencia de depredadores y las oportunidades del jugador, sin convertir cada cambio ecológico en una obligación de intervención.
- Las acciones del jugador, las comunidades y las facciones podrán afectar a los ecosistemas mediante caza, recolección, destrucción de hábitats, protección, restauración y otras actividades plausibles.
- Los ecosistemas podrán recuperarse después de perturbaciones temporales mediante reproducción, recolonización, regeneración vegetal y restauración de hábitats. Los ritmos dependerán de la especie, la región, la magnitud del daño y las condiciones disponibles.
- La simulación podrá utilizar distintos niveles de detalle según la relevancia y proximidad de cada ecosistema. Los resultados deberán conservar coherencia temporal y determinismo aunque se resuelvan periodos fuera de la presencia del jugador.
- El sistema deberá evitar ciclos de simulación infinitos, crecimiento descontrolado de poblaciones y cálculos innecesarios. Las reglas, límites ecológicos, escalas temporales y criterios de actualización quedan pendientes de diseño técnico y numérico.

El objetivo es que la naturaleza de Mythical Exodus se comporte como un conjunto de ecosistemas vivos e interdependientes, capaz de cambiar y recuperarse con el tiempo. La simulación deberá aportar credibilidad y consecuencias sistémicas sin exigir que cada organismo se procese continuamente ni desplazar la prioridad de la historia principal.


## Extinción local de especies y recuperación de poblaciones

Las acciones del jugador y de las facciones podrán alterar las poblaciones naturales hasta provocar la desaparición local de determinadas especies cuando la explotación o degradación del entorno supere su capacidad de recuperación.

- La caza, pesca, recolección, destrucción de hábitats y extracción de recursos podrán reducir poblaciones o afectar su reproducción y supervivencia.
- Si la presión supera durante suficiente tiempo la capacidad de regeneración de una especie, su población podrá disminuir hasta desaparecer de una región. Esta extinción será local al territorio o ecosistema afectado, no necesariamente global.
- La vulnerabilidad dependerá de características de cada especie, como tasa reproductiva, longevidad, movilidad, tamaño de población, hábitat y disponibilidad de recursos.
- La desaparición de una especie podrá producir consecuencias ecológicas indirectas, como cambios en las relaciones depredador-presa, competencia, vegetación y disponibilidad de recursos para otras especies.
- Las especies podrán recolonizar una región mediante migración natural desde poblaciones cercanas cuando existan rutas, hábitats y condiciones adecuadas.
- Las comunidades o facciones podrán emprender medidas de recuperación ecológica, como proteger zonas, limitar la caza o recolección, restaurar hábitats, reintroducir ejemplares o favorecer la regeneración de recursos, si cuentan con conocimientos y medios plausibles.
- La recuperación no será automática ni estará garantizada: dependerá de que persistan poblaciones de origen o existan medios viables de reintroducción, y de que las causas de la desaparición hayan sido corregidas.
- Las medidas de recuperación podrán requerir tiempo y recursos y competir con otras necesidades de las comunidades. Sus resultados dependerán de su continuidad y eficacia.
- El estado de las poblaciones y las causas conocidas de su declive podrán influir en las decisiones de NPC y facciones. La información disponible deberá corresponder a lo que hayan observado o aprendido.
- El sistema distinguirá entre ausencia temporal, desaparición local y extinción global. No se asumirá que una especie desaparecida de una región se ha extinguido en todo el mundo.
- La simulación deberá respetar límites ecológicos y temporales, evitar la generación arbitraria de ejemplares y mantener coherencia entre poblaciones, hábitats y recursos.
- Las condiciones cuantitativas de sobreexplotación, umbrales de desaparición, modelos de población, viabilidad de recolonización y costes de recuperación quedan pendientes de diseño.

El objetivo es que la explotación de la naturaleza tenga consecuencias reales y comprensibles, pero también que el mundo disponga de mecanismos plausibles de recuperación cuando las condiciones lo permitan. La recuperación ecológica no borrará automáticamente las consecuencias ni garantizará que toda especie pueda regresar.


## Interacciones depredador-presa y simulación ecológica según proximidad

Las interacciones ecológicas visibles y de mayor detalle, como la caza, la competencia directa entre depredadores y los cambios inmediatos de conducta ante la escasez de presas, podrán desarrollarse cuando el jugador se encuentre cerca del ecosistema y pueda observarlas.

- En la proximidad del jugador, los depredadores podrán rastrear, perseguir y cazar presas; las presas podrán huir, esconderse, agruparse o adoptar otras respuestas propias de su especie.
- Si la disponibilidad de presas disminuye, los depredadores podrán modificar su conducta: buscar otras presas compatibles, ampliar sus zonas de búsqueda, competir con otros depredadores o desplazarse hacia otro hábitat.
- La escasez de alimento podrá afectar también a la condición, reproducción y supervivencia de los animales, de acuerdo con las reglas ecológicas de cada especie.
- Estas conductas deberán responder a necesidades y señales del entorno, no activarse como escenas arbitrarias únicamente para el jugador.
- Cuando el jugador esté lejos, el ecosistema podrá actualizarse mediante una simulación abstracta de menor detalle, resolviendo cambios de población, disponibilidad de recursos, migraciones y consecuencias ecológicas sin procesar cada persecución o interacción individual.
- La transición entre simulación detallada y abstracta deberá conservar continuidad: acercarse a una región no deberá reiniciar poblaciones ni producir resultados incompatibles con los cambios ya resueltos durante la ausencia del jugador.
- La frecuencia, el detalle y los criterios de activación de cada nivel de simulación deberán equilibrar credibilidad, rendimiento y consistencia. Los umbrales de proximidad y las reglas cuantitativas quedan pendientes de diseño técnico.

El objetivo es reservar las interacciones individuales y observables para los ecosistemas próximos al jugador, mientras el resto del mundo continúa evolucionando mediante una representación simplificada y coherente.


## Domesticación y vínculos con criaturas

El jugador podrá establecer vínculos con determinadas criaturas y, cuando su naturaleza, inteligencia, comportamiento y capacidades lo permitan, domesticarlas o cooperar con ellas.

- La domesticación no estará disponible para todas las especies. Cada criatura o especie tendrá características y condiciones propias que determinarán si puede establecer un vínculo con el jugador.
- El vínculo podrá desarrollarse progresivamente mediante interacciones, confianza, cuidado, alimentación, ayuda y otras conductas coherentes con la criatura. No se asumirá que una criatura se vuelve dócil de forma instantánea.
- Las respuestas de cada criatura dependerán de su personalidad, instintos, experiencias, estado, necesidades y relación con el jugador. Algunas podrán ser cautelosas, independientes, difíciles de domesticar o incapaces de aceptar compañía.
- Las criaturas vinculadas podrán desempeñar funciones distintas según sus capacidades: acompañar al jugador, ayudar en determinadas tareas, participar en combate o servir como monturas cuando su anatomía y comportamiento lo permitan.
- Las funciones disponibles serán específicas de cada criatura. No todas podrán combatir, transportar al jugador o realizar las mismas tareas.
- El vínculo no eliminará las necesidades ni el comportamiento natural de la criatura. Alimentación, descanso, seguridad, salud, miedo y condiciones ambientales podrán influir en su disposición y rendimiento.
- Las criaturas podrán reaccionar ante peligros, enemigos, clima y entorno de acuerdo con sus instintos y entrenamiento. No deberán obedecer órdenes imposibles o contrarias a sus capacidades.
- El jugador deberá poder emitir órdenes o indicaciones compatibles con las capacidades de la criatura. El repertorio de órdenes, su interfaz y sus límites quedan pendientes de diseño.
- El vínculo podrá fortalecerse, debilitarse o romperse según las acciones del jugador, el trato recibido, las experiencias compartidas y otros acontecimientos relevantes.
- La muerte, desaparición, separación o pérdida de confianza de una criatura vinculada tendrá consecuencias coherentes con el vínculo establecido. Las reglas concretas de lesión, recuperación, muerte permanente y reencuentro quedan pendientes.
- La domesticación y el uso de criaturas deberán integrarse con los ecosistemas y las poblaciones naturales. La captura o extracción de ejemplares no podrá ignorar límites ecológicos ni generar criaturas arbitrariamente.
- La simulación deberá respetar la continuidad del vínculo y de la criatura tanto cerca como lejos del jugador, utilizando niveles de detalle adecuados sin reiniciar sus estados o experiencias.

El objetivo es que la relación con la fauna sea una posibilidad sistémica y no una mecánica universal impuesta a todas las especies. Cada criatura conservará una identidad y unas capacidades propias, y la cooperación surgirá de la compatibilidad y del vínculo desarrollado.


## Progresión de criaturas compañeras y límite de domesticación

Las criaturas que desarrollen un vínculo con el jugador podrán adquirir experiencia, mejorar capacidades y desarrollar habilidades propias. El crecimiento será individual y estará condicionado por su especie, características y experiencias.

- Cada criatura podrá disponer de capacidades y habilidades compatibles con su especie, anatomía, temperamento y potencial individual. No todas tendrán acceso al mismo repertorio.
- La experiencia obtenida mediante actividades, exploración, entrenamiento, cooperación y enfrentamientos podrá contribuir al desarrollo de la criatura cuando esas experiencias sean relevantes para sus capacidades.
- La progresión podrá mejorar habilidades existentes o permitir el desarrollo de nuevas capacidades, siempre dentro de los límites naturales y de diseño de cada criatura.
- Las experiencias compartidas con el jugador podrán influir en la confianza, cooperación, disciplina y respuesta a determinadas órdenes, sin convertir a la criatura en una unidad idéntica a las demás.
- El crecimiento no será automático ni uniforme: las características individuales, el uso de habilidades y las circunstancias podrán producir compañeros diferentes incluso dentro de una misma especie.
- La cantidad de criaturas que el jugador podrá mantener domesticadas y vinculadas activamente será muy limitada. El límite exacto queda pendiente de definición y deberá ser una regla explícita del sistema, no una capacidad ilimitada ampliable sin restricciones.
- El límite busca dar importancia a cada vínculo y evitar que el jugador acumule grandes grupos de criaturas como recursos intercambiables. La selección y el cuidado de cada compañera deberán implicar decisiones significativas.
- Las reglas para criaturas adicionales —por ejemplo, si podrán existir vínculos no activos, liberar o sustituir una criatura, o establecer vínculos nuevos tras perder una anterior— quedan pendientes de diseño y no se presuponen.
- La progresión y el límite de domesticación deberán integrarse con la continuidad ecológica y con la simulación de criaturas cerca y lejos del jugador.

El objetivo es que cada criatura compañera pueda desarrollar una identidad y capacidades propias, mientras que el número reducido de vínculos disponibles favorezca relaciones significativas en lugar de coleccionar acompañantes.


### Restricción de especies compatibles con domesticación y compañerismo

La domesticación y el compañerismo estarán restringidos a un conjunto reducido de especies. Que una criatura sea inteligente, dócil, interactúe con el jugador o pueda ser alimentada no significa que sea domesticable ni apta para convertirse en compañera.

- Solo determinadas especies, definidas explícitamente por el diseño del mundo, podrán establecer vínculos de domesticación y compañerismo con el jugador.
- La compatibilidad será una característica propia de cada especie y podrá depender de su biología, comportamiento, inteligencia, estructura social, temperamento y necesidades.
- Dentro de una especie compatible, no todos los individuos tendrán necesariamente la misma disposición o posibilidad de establecer un vínculo; podrán existir condiciones individuales adicionales.
- La lista de especies compatibles deberá ser deliberadamente pequeña en comparación con la diversidad total de criaturas del mundo.
- Las especies no compatibles conservarán sus comportamientos naturales y podrán interactuar con el jugador de otras maneras, pero no podrán convertirse en compañeras domesticadas por el simple hecho de acumular interacciones.
- Esta restricción es independiente del límite de cantidad: el sistema limitará tanto cuántas criaturas puede mantener vinculadas el jugador como qué especies pueden llegar a ser compañeras.
- La compatibilidad de una especie no garantiza que su domesticación sea sencilla ni que pueda desempeñar cualquier función. Las condiciones del vínculo y las funciones posibles seguirán dependiendo de sus capacidades y características.

La intención es que las criaturas compañeras sean excepcionales dentro de la fauna del mundo: el jugador no podrá domesticar indiscriminadamente a cualquier especie ni convertir la domesticación en un sistema universal.


### Muerte permanente de criaturas compañeras

Las criaturas compañeras podrán morir definitivamente durante la aventura.

- La muerte de una criatura compañera será permanente: no podrá resucitarse ni recuperarse mediante sistemas ordinarios o habilidades.
- La pérdida será una consecuencia real de los peligros de la aventura y de las decisiones tomadas durante el combate, la exploración y otras situaciones.
- El sistema deberá representar correctamente la muerte y retirar a la criatura de los vínculos y funciones activos del jugador, evitando que siga participando en la simulación como si estuviera viva.
- La muerte de una compañera tendrá consecuencias coherentes para el vínculo y para los personajes o sistemas que hayan conocido el acontecimiento. El alcance concreto de esas consecuencias queda pendiente de diseño.
- La muerte permanente no implica que todas las criaturas compañeras deban ser vulnerables en cualquier circunstancia ni elimina la necesidad de reglas justas, señales legibles y oportunidades razonables de protegerlas.
- Las reglas de lesión, incapacitación, retirada, recuperación de heridas y condiciones que pueden conducir a la muerte quedan pendientes de definición. No se debe confundir una criatura incapacitada o herida con una criatura muerta.

El objetivo es que cada vínculo tenga un riesgo real y que la protección de una compañera forme parte de las decisiones significativas del jugador.


### Miedo, instinto de supervivencia y respuesta al peligro de las criaturas compañeras

Las criaturas compañeras podrán experimentar miedo y reaccionar por instinto ante amenazas y situaciones que perciban como peligrosas.

- Las respuestas podrán activarse ante enemigos claramente superiores, amenazas desconocidas, heridas graves, dolor, aislamiento, acorralamiento, peligros ambientales u otras circunstancias relevantes para la especie y el individuo.
- Según su naturaleza, temperamento, experiencias, vínculo con el jugador y estado actual, una criatura podrá huir, buscar refugio, retroceder, esconderse, adoptar una postura defensiva, quedarse paralizada o intentar protegerse.
- En determinadas circunstancias, el instinto de supervivencia podrá prevalecer temporalmente sobre una orden del jugador. La criatura no tendrá que obedecer órdenes que contradigan de forma extrema su percepción de peligro o sus capacidades.
- La intensidad y duración de estas reacciones dependerán de la especie, características individuales, experiencias, heridas, confianza y contexto. No todas las criaturas reaccionarán igual.
- El miedo no debe convertirse en desobediencia arbitraria o constante: las reacciones deberán tener causas coherentes y ser comprensibles a partir de la situación y del comportamiento observable.
- El vínculo con el jugador y las experiencias compartidas podrán influir en la disposición a afrontar riesgos, pero no eliminarán necesariamente los instintos naturales ni garantizarán obediencia absoluta.
- Las condiciones para recuperar la calma, volver a cooperar y aceptar órdenes después de una reacción de miedo quedan pendientes de definición.

El objetivo es que las compañeras se comporten como criaturas con instintos y límites propios, no como herramientas que ejecutan órdenes de manera incondicional.


### Subsistema de comunicación entre el jugador y sus criaturas compañeras

El motor incluirá un subsistema específico para la comunicación entre el jugador y las criaturas compañeras. Permitirá transmitir órdenes e intenciones mediante distintos canales, y modelará cómo cada criatura percibe e interpreta esas señales.

- Los canales de comunicación podrán incluir órdenes, señales, gestos, llamadas y otras formas de comunicación apropiadas para el mundo y para las capacidades de cada especie.
- Cada criatura tendrá un repertorio de señales que puede percibir y comprender, condicionado por su especie, sentidos, inteligencia, experiencias y características individuales.
- La comprensión no será universal: una señal podrá ser reconocida por una criatura y resultar desconocida o ininteligible para otra.
- La confianza, el vínculo y las experiencias compartidas podrán mejorar progresivamente la interpretación de señales y la cooperación.
- La comunicación no garantizará obediencia. La criatura interpretará la señal según su comprensión, estado emocional, instintos, capacidades, necesidades y contexto.
- Las órdenes deberán respetar las capacidades físicas y cognitivas de la criatura. No se podrán exigir acciones que no pueda realizar o comprender.
- El subsistema deberá contemplar señales malinterpretadas, ignoradas o imposibles de percibir cuando las circunstancias lo justifiquen, por ejemplo debido a distancia, ruido, visibilidad, distracción o miedo.
- La comunicación deberá funcionar de forma coherente con el comportamiento autónomo y el instinto de supervivencia de las criaturas, sin convertirlas en unidades controladas de manera absoluta.
- El diseño deberá permitir ampliar el repertorio de señales y respuestas por especie y por criatura sin acoplar el núcleo lógico a animaciones, audio, dispositivos de entrada o un motor gráfico.
- El catálogo de órdenes, señales, condiciones de percepción, progresión de comprensión y reglas de interpretación queda pendiente de especificación.

Este subsistema busca que la cooperación se desarrolle como una relación aprendida entre el jugador y cada criatura, en lugar de asumir que todas comprenden automáticamente un conjunto universal de órdenes.


### Especialización, mejora y equipamiento personalizado de criaturas compañeras

Las criaturas compañeras podrán desempeñar funciones especializadas y mejorar sus capacidades. También podrán equiparse con elementos personalizados compatibles con su especie y características individuales.

- Las funciones posibles podrán incluir combate, exploración, rastreo, detección de amenazas, transporte y apoyo, entre otras que el diseño establezca.
- Las funciones disponibles dependerán de la especie, anatomía, sentidos, capacidades naturales, temperamento y características individuales. No todas las criaturas podrán desempeñar todas las funciones.
- Una criatura podrá destacar en una o varias funciones y desarrollar sus capacidades mediante experiencia y actividades relevantes, de acuerdo con su potencial.
- Las mejoras podrán reforzar capacidades existentes o desarrollar habilidades compatibles con la criatura. No deberán eliminar sus limitaciones naturales ni hacer que todas las especies terminen siendo intercambiables.
- El jugador podrá equipar a sus compañeras con elementos personalizados. Cada elemento deberá ser compatible con la anatomía, tamaño, especie y capacidades de la criatura.
- El equipamiento podrá modificar capacidades, ofrecer protección o habilitar funciones concretas cuando el tipo de elemento lo permita. Sus efectos, requisitos, restricciones y posibles inconvenientes deberán estar definidos explícitamente.
- El equipamiento no deberá conceder capacidades físicamente imposibles para la criatura ni permitir que una especie incompatible desempeñe cualquier función por medio de objetos.
- La disponibilidad de espacios, categorías, materiales, fabricación, adquisición, mejora, mantenimiento y pérdida del equipamiento queda pendiente de diseño.
- El sistema deberá distinguir las capacidades naturales de la criatura de las aportadas o modificadas por mejoras y equipamiento, para que sus efectos puedan calcularse y retirarse correctamente al cambiar o quitar elementos.
- La especialización y el equipamiento deberán integrarse con el subsistema de comunicación: las órdenes disponibles y las respuestas posibles dependerán de lo que la criatura pueda realmente hacer.
- El núcleo lógico gestionará reglas, compatibilidad, atributos y efectos del equipamiento sin depender de modelos visuales, animaciones o sistemas específicos de un motor gráfico.

El objetivo es que cada compañera pueda desarrollar un papel propio y una configuración personalizada, sin perder su identidad biológica ni las diferencias entre especies.


### Inventario propio y capacidad de carga de criaturas compañeras

Cada criatura compañera podrá disponer de un inventario propio y limitado para transportar determinados objetos y recursos durante la aventura.

- La capacidad de carga será limitada y dependerá de las características de la criatura, como especie, anatomía, tamaño y capacidades individuales.
- Los objetos transportables deberán ser compatibles con la criatura y con la forma en que puede sujetarlos, almacenarlos o llevarlos.
- Algunas criaturas podrán recoger o transportar determinados recursos cuando sus sentidos, anatomía y comportamiento lo permitan; esta capacidad no será universal.
- El inventario podrá tener restricciones por tipo, tamaño, peso, volumen o cantidad de objetos, según las reglas que se definan para cada criatura.
- El peso y la carga transportada podrán afectar al desplazamiento, la resistencia, la movilidad u otras capacidades cuando corresponda.
- El inventario de la criatura será independiente del inventario del jugador. El intercambio de objetos requerirá una interacción o acción válida y compatible con las capacidades de ambos.
- La criatura no recogerá, utilizará ni entregará objetos de manera arbitraria: sus acciones dependerán de las órdenes que comprenda, su autonomía, sus necesidades y las circunstancias.
- Los objetos transportados deberán conservar su identidad, cantidad, estado y propiedades durante la simulación, incluso cuando la criatura se encuentre lejos del jugador.
- La capacidad exacta, los límites de carga, las categorías de objetos, las reglas de recogida e intercambio y las posibles consecuencias de perder o separar a la criatura quedan pendientes de especificación.

El objetivo es permitir que las compañeras contribuyan a la exploración y gestión de recursos sin convertirse en almacenes ilimitados ni eliminar las decisiones de inventario del jugador.


### Uso autónomo de objetos por criaturas compañeras

Las criaturas compañeras podrán utilizar por iniciativa propia determinados objetos de su inventario cuando la situación, sus capacidades y su comprensión lo justifiquen.

- El uso autónomo estará limitado a objetos que la criatura pueda manipular y utilizar de acuerdo con su anatomía, inteligencia, aprendizaje y experiencia.
- Podrán utilizar objetos para atender necesidades o responder a circunstancias, por ejemplo tratar heridas, recuperarse o afrontar determinados peligros, siempre que el objeto sea apropiado y sepan utilizarlo.
- La decisión dependerá de su estado, necesidades, percepción del peligro, conocimientos, personalidad y contexto. No se activará de forma arbitraria ni garantizará siempre la respuesta óptima.
- La criatura deberá disponer del objeto y cumplir sus requisitos de uso; el sistema consumirá o modificará sus cantidades y estado según el efecto definido.
- Algunos objetos podrán requerir una orden del jugador, asistencia externa o condiciones que impidan su uso autónomo.
- Las reglas de selección de objetos, prioridades, aprendizaje, restricciones y efectos concretos quedan pendientes de especificación.

El objetivo es que las compañeras puedan actuar con cierta autonomía y aprovechar los recursos que transportan, sin asumir que comprenden o pueden utilizar cualquier objeto.

### Límite de carga y penalizaciones de peso del jugador

El jugador tendrá un límite de carga relacionado con el peso de los objetos que lleva y del equipamiento que utiliza. La carga afectará a su rendimiento físico y a su movilidad, siguiendo el principio de gestión de peso habitual en los Soulslike.

- El sistema deberá contabilizar el peso de los objetos transportados y del equipamiento del jugador mediante datos explícitos de cada elemento.
- Existirá un límite de carga; sus valores y la fórmula para calcularlo quedan pendientes de definición.
- La carga podrá afectar a la velocidad de movimiento y a la agilidad efectiva del jugador. La naturaleza y magnitud exactas de las penalizaciones dependerán de los umbrales y fórmulas que se diseñen.
- El peso del equipamiento deberá considerarse en el cálculo, además del peso de los objetos transportados.
- El sistema deberá distinguir claramente el peso total transportado de la carga equipada para permitir reglas y efectos diferenciados si se definen posteriormente.
- Las reglas deberán ser deterministas y configurables, y el núcleo lógico calculará sus efectos sin depender de animaciones o de un motor gráfico.
- Quedan pendientes el límite base, posibles modificadores, umbrales de carga, penalizaciones adicionales y la relación exacta con la estadística Agilidad.

El objetivo es que las decisiones de equipo e inventario tengan consecuencias tácticas reales: llevar más peso o equipamiento pesado podrá ofrecer ventajas defensivas u otras utilidades, pero tendrá un coste en movilidad.


### Categorías de carga y efectos progresivos sobre el movimiento

La carga del jugador se organizará en categorías diferenciadas: ligera, media, pesada y sobrecargada. Cada categoría aplicará efectos progresivos sobre el rendimiento físico.

- La categoría se determinará a partir de la relación entre la carga transportada y la capacidad de carga efectiva del personaje.
- La carga ligera tendrá las menores restricciones de movilidad.
- La carga media aplicará penalizaciones moderadas.
- La carga pesada impondrá restricciones más significativas sobre el movimiento y las acciones físicas.
- La sobrecarga representará superar el límite de carga permitido y podrá imponer restricciones severas, definidas por el diseño.
- Las categorías podrán afectar a la velocidad de movimiento, esquiva, salto, agilidad efectiva y recuperación de postura. Cada efecto deberá especificarse explícitamente; no se asumirá que todas las categorías afectan por igual a todas las acciones.
- La relación entre carga, estadística Agilidad y capacidad de carga deberá definirse mediante reglas y fórmulas deterministas.
- El sistema deberá permitir configurar umbrales y penalizaciones sin acoplarlos al motor de presentación.
- Los umbrales cuantitativos, las fórmulas, los modificadores por clase/equipamiento y las restricciones exactas de la sobrecarga quedan pendientes de diseño.

El objetivo es que el peso tenga consecuencias tácticas graduales y legibles, de modo que el jugador pueda elegir entre movilidad y capacidad de protección o transporte.


### Capacidad de carga natural según la clase

La capacidad de carga del jugador tendrá una base diferenciada según su clase, reflejando sus características y estilo de combate.

- Cada clase tendrá un valor o modificador propio de capacidad de carga natural.
- El Guerrero estará orientado a soportar cargas mayores que las otras clases.
- El Espadachín Mágico y el Segador tendrán capacidades de carga naturales diferenciadas y coherentes con sus respectivos perfiles, sin asumir que sean idénticas entre sí.
- La estadística Agilidad y otros modificadores que se definan podrán influir en la capacidad de carga efectiva.
- El equipamiento, las habilidades y los efectos temporales podrán modificar la capacidad de carga cuando sus reglas lo permitan.
- La clase elegida seguirá siendo permanente durante la partida; estas diferencias no implican que el jugador pueda cambiar de clase.
- Los valores base, las diferencias exactas entre clases y la fórmula de combinación con estadísticas y modificadores quedan pendientes de balance y especificación.

La intención es que la gestión del peso refuerce la identidad de cada clase y sus decisiones de equipamiento, sin impedir que cualquier clase pueda adaptar su configuración dentro de sus límites.


### Restricciones de movilidad acrobática por sobrecarga

Cuando el jugador supere su capacidad máxima de carga y entre en la categoría de sobrecarga, determinadas acciones de movilidad acrobática podrán quedar restringidas o resultar imposibles.

- La sobrecarga podrá impedir acciones que exijan una relación elevada entre fuerza, impulso, equilibrio y movilidad, como correr por paredes, realizar saltos de gran distancia o utilizar el gancho.
- Las restricciones se definirán por acción y por sus requisitos; no se asumirá que todas las acciones acrobáticas se bloquean automáticamente.
- El sistema podrá considerar la carga efectiva, la Agilidad, las capacidades del personaje, el equipamiento y las condiciones del entorno para determinar si una acción es viable.
- Las acciones que sigan siendo posibles podrán recibir penalizaciones o restricciones adicionales según las reglas correspondientes.
- El jugador deberá recibir información clara sobre qué acciones están limitadas por la sobrecarga y por qué.
- Las restricciones deberán aplicarse de forma coherente y determinista tanto al validar acciones como al resolver sus resultados.
- Los requisitos de carga y las restricciones exactas de cada acción acrobática quedan pendientes de especificación y balance.

El objetivo es que superar la capacidad de carga tenga consecuencias relevantes también para la exploración y la movilidad, y no únicamente para la velocidad de desplazamiento durante el combate.


### Recuperación dinámica de movilidad al reducir la carga

Las penalizaciones y restricciones derivadas del peso se actualizarán dinámicamente cuando cambie la carga del jugador.

- Al retirar, guardar, transferir o consumir objetos, el sistema recalculará la carga efectiva del jugador.
- Cuando la carga descienda a una categoría inferior, se actualizarán las penalizaciones correspondientes y podrán recuperarse las acciones que vuelvan a cumplir sus requisitos.
- Al dejar de estar sobrecargado, las restricciones específicas de esa categoría desaparecerán sin exigir descanso, uso de una zona de descanso ni activación de un mecanismo especial.
- La recuperación de movilidad será consecuencia directa del nuevo estado de carga y deberá reflejarse de forma coherente en la validación de acciones.
- Los efectos independientes de la carga —por ejemplo, heridas, estados alterados o condiciones ambientales— seguirán aplicándose aunque el jugador reduzca su peso.
- El recálculo deberá ser determinista y evitar estados obsoletos cuando se intercambien, equipen, desequipen o consuman objetos.

El objetivo es que la carga sea una condición dinámica: el jugador podrá recuperar movilidad reorganizando su inventario y equipamiento en cualquier momento en que las reglas de interacción lo permitan.


### Almacenamiento compartido en cofres de zonas de descanso

El jugador podrá depositar objetos en cofres vinculados a las zonas de descanso y acceder al mismo almacenamiento desde otras zonas de descanso habilitadas.

- Los cofres de las zonas de descanso estarán conectados a un almacenamiento compartido, de modo que los objetos depositados en uno puedan recuperarse desde otro.
- Depositar objetos los retirará de la carga transportada por el jugador; recuperarlos los añadirá y actualizará su peso y categoría de carga.
- El almacenamiento conservará la identidad, cantidad, estado y propiedades de los objetos depositados.
- El acceso al almacenamiento estará sujeto a las reglas de interacción de las zonas de descanso; no se asumirá acceso remoto desde cualquier lugar.
- Las operaciones de depósito y retirada deberán validar espacio disponible, compatibilidad y cualquier restricción que se defina para los objetos.
- El límite de capacidad del almacenamiento, si existe, y las reglas de organización, filtros y acceso quedan pendientes de especificación.
- El sistema deberá evitar duplicaciones o pérdidas de objetos durante transferencias y mantener consistencia entre inventario del jugador y almacenamiento.

El objetivo es permitir que el jugador prepare su equipamiento y gestione sus recursos entre expediciones sin cargar permanentemente con todos los objetos que ha obtenido.


### Independencia del inventario de las criaturas compañeras

El inventario de cada criatura compañera será independiente tanto del inventario del jugador como del almacenamiento compartido de las zonas de descanso.

- Depositar o retirar objetos del almacenamiento compartido no modificará automáticamente el inventario de ninguna criatura compañera.
- Los objetos que transporte una criatura permanecerán bajo su inventario mientras no ocurra una transferencia válida.
- El intercambio entre el jugador y una criatura requerirá una interacción permitida por las capacidades, el estado y las circunstancias de ambos.
- La transferencia deberá actualizar de forma consistente el inventario de origen y el de destino, respetando sus límites de capacidad, compatibilidad y restricciones.
- Guardar objetos en el almacenamiento compartido no permitirá retirar indirectamente objetos del inventario de una criatura; primero deberá realizarse una transferencia válida al jugador u otro medio expresamente permitido.
- La separación física entre jugador y criatura no fusionará sus inventarios ni hará que los objetos aparezcan automáticamente en el almacenamiento.
- La identidad, cantidad, estado y propiedades de cada objeto deberán conservarse durante las transferencias, evitando duplicaciones o pérdidas.

El objetivo es mantener una gestión de recursos coherente: el jugador, cada criatura compañera y el almacenamiento de las zonas de descanso constituyen contenedores distintos, conectados únicamente mediante operaciones explícitas y válidas.


### Conservación y recuperación del inventario tras la muerte de una criatura compañera

Cuando una criatura compañera muera definitivamente, los objetos que llevaba en su inventario permanecerán asociados a su cadáver para que el jugador pueda recuperarlos mediante una interacción válida.

- La muerte no transferirá automáticamente los objetos al inventario del jugador ni al almacenamiento compartido.
- El cadáver conservará la identidad, cantidad, estado y propiedades de los objetos que transportaba en el momento de la muerte.
- El jugador podrá recuperar esos objetos si alcanza el cadáver y las circunstancias permiten interactuar con él.
- La recuperación respetará la capacidad, compatibilidad y restricciones del inventario de destino; no se permitirá recoger más de lo que pueda transportarse si las reglas de carga lo impiden.
- Los objetos recuperados dejarán de estar asociados al cadáver y pasarán al inventario de destino sin duplicarse.
- La criatura seguirá muerta de forma permanente: recuperar sus pertenencias no permitirá resucitarla ni restaurar el vínculo.
- La persistencia del cadáver, el deterioro o desaparición de objetos, los peligros que puedan impedir la recuperación y las condiciones especiales de pérdida quedan pendientes de especificación.

El objetivo es que la muerte permanente tenga consecuencias y que las pertenencias de la compañera no desaparezcan arbitrariamente, ofreciendo al jugador la posibilidad de recuperarlas si logra llegar hasta ellas.


### Persistencia de cadáveres y ritos de sepultura y luto para criaturas compañeras

El cadáver de una criatura compañera que haya muerto definitivamente permanecerá en el lugar de su muerte incluso cuando el jugador abandone la zona y regrese más adelante. Además de recuperar sus pertenencias, el jugador podrá realizar interacciones de sepultura y luto.

- El cadáver y su estado deberán persistir al abandonar la zona, cambiar de región o regresar posteriormente, de acuerdo con las reglas generales de persistencia del mundo.
- Los objetos que la criatura llevaba seguirán asociados al cadáver y podrán recuperarse mediante una interacción válida, respetando los límites y restricciones del inventario de destino.
- El jugador podrá interactuar con el cadáver para realizar una sepultura como acto deliberado de despedida y reconocimiento de la criatura.
- También existirá una interacción de luto que permita al jugador expresar duelo por la pérdida de su compañera.
- La sepultura y el luto serán interacciones diferenciadas: una se refiere al tratamiento del cuerpo y la otra al acto de duelo. Podrán formar parte de un mismo proceso de despedida, sin asumir que una sustituye automáticamente a la otra.
- La muerte seguirá siendo permanente. Ninguna de estas interacciones permitirá resucitar a la criatura ni restaurar el vínculo.
- La relación entre recuperación de pertenencias y sepultura deberá respetar la continuidad de los objetos: no se perderán ni duplicarán por realizar el rito. El orden de las interacciones y el acceso posterior a los objetos deberán definirse explícitamente.
- Las formas de sepultura, requisitos, duración, posibles recursos necesarios, respuestas de otros personajes, consecuencias narrativas y mecánicas del luto, así como la persistencia de tumbas o memoriales, quedan pendientes de diseño.

El objetivo es que la pérdida de una compañera tenga presencia duradera en el mundo y pueda recibir un tratamiento significativo, sin reducir su muerte a la desaparición de una entidad o a una simple transferencia de inventario.


### Memoria y reacciones de los NPC ante la muerte de una criatura compañera

Los NPC que hayan conocido a una criatura compañera podrán recordar su muerte y reaccionar ante ella según la relación que mantenían con la criatura y la información de la que dispongan.

- La reacción de cada NPC dependerá de su vínculo previo con la criatura, sus experiencias compartidas, personalidad, valores y circunstancias.
- Las respuestas podrán incluir tristeza, duelo, respeto, compasión, indiferencia, alivio, hostilidad u otras reacciones coherentes con el personaje y la relación.
- Los NPC no conocerán automáticamente la muerte: deberán haberla presenciado, descubrirla o recibir información por un medio plausible.
- La información recibida podrá ser incompleta o imprecisa; las reacciones deberán basarse en lo que cada NPC realmente conoce o cree conocer.
- La memoria de la muerte podrá influir en diálogos, comportamiento, relaciones con el jugador y participación en actos de despedida o memoriales, cuando corresponda.
- No todos los NPC reaccionarán de la misma manera ni estarán obligados a mostrar una reacción visible.
- La memoria y sus consecuencias deberán respetar la continuidad del mundo y no convertir a los NPC en observadores omniscientes.
- Los detalles de persistencia, duración, transmisión de la noticia y consecuencias concretas quedan pendientes de diseño.

El objetivo es que la muerte de una compañera pueda tener repercusiones sociales y emocionales coherentes, determinadas por los vínculos y conocimientos reales de los personajes.


### Memoriales permanentes para criaturas compañeras fallecidas

El jugador podrá crear memoriales permanentes para honrar a sus criaturas compañeras fallecidas, además de disponer de la interacción de sepultura.

- El jugador podrá elegir entre ubicaciones válidas del mundo para establecer un memorial, sujeto a las reglas del lugar y a los recursos o requisitos que se definan.
- El memorial será una entidad persistente del mundo y permanecerá en la ubicación elegida, incluso cuando el jugador abandone la región y regrese posteriormente.
- Los memoriales serán distintos de las sepulturas: podrán complementar una tumba o servir como homenaje en otro lugar permitido, sin sustituir automáticamente el tratamiento del cadáver.
- El memorial podrá estar asociado a la identidad y la historia de la criatura fallecida, conservando su vínculo con el jugador y los acontecimientos compartidos que se decida registrar.
- Los NPC que conozcan la historia de la criatura podrán reconocer el memorial y reaccionar ante él de acuerdo con sus relaciones, conocimientos y circunstancias.
- La creación, personalización, materiales, restricciones de ubicación, interacciones posteriores y posibles efectos narrativos o mecánicos quedan pendientes de diseño.

El objetivo es permitir que el jugador elija cómo y dónde recordar a sus compañeras, dando continuidad física y narrativa a esas pérdidas en el mundo.


### Registro de experiencias compartidas con criaturas compañeras

El mundo registrará acontecimientos significativos vividos por el jugador junto a cada criatura compañera, de modo que su historia compartida pueda conservarse y reflejarse en recuerdos, epitafios y memoriales.

- El registro estará asociado a la identidad individual de cada criatura, no únicamente a su especie o tipo.
- Podrá incluir acontecimientos significativos como combates, exploraciones, rescates, supervivencia ante peligros, descubrimientos y otros momentos relevantes definidos por el juego.
- Los acontecimientos registrados deberán derivarse de hechos que realmente hayan ocurrido durante la partida; no se inventarán recuerdos ni se atribuirán experiencias que la criatura no haya vivido o presenciado.
- El sistema podrá conservar contexto relevante del acontecimiento, como participantes, lugar, circunstancias y resultado, según los datos disponibles.
- Los recuerdos podrán utilizarse para generar o seleccionar epitafios, inscripciones, contenidos de memoriales y otras referencias narrativas a la historia compartida.
- La selección y presentación de recuerdos deberá respetar la identidad de la criatura y la continuidad de los acontecimientos; no todos los sucesos tendrán que registrarse ni mostrarse.
- Los criterios de relevancia, capacidad y organización del registro, personalización de epitafios, selección de recuerdos y persistencia entre partidas quedan pendientes de diseño.

El objetivo es que cada compañera tenga una historia individual reconocible y que los memoriales puedan recordar experiencias auténticas de la aventura compartida.


### Relaciones autónomas entre criaturas compañeras

Las criaturas compañeras podrán desarrollar relaciones entre sí a partir de sus encuentros, convivencia y experiencias compartidas.

- Las relaciones podrán incluir amistad, confianza, afecto, rivalidad, temor, rechazo, hostilidad u otros vínculos coherentes con la naturaleza y personalidad de cada criatura.
- Los vínculos podrán ser asimétricos: una criatura podrá sentir confianza o afecto por otra sin que la otra corresponda de la misma manera.
- La evolución de las relaciones dependerá de experiencias observables, interacciones, cooperación, competencia, conflictos, ayuda recibida y circunstancias de convivencia.
- La especie, temperamento, personalidad, inteligencia, experiencias previas y estado de cada criatura influirán en cómo establece y expresa vínculos.
- Las relaciones podrán afectar a la cooperación, proximidad, comunicación, apoyo, competencia por recursos, tolerancia, conflictos y respuestas ante peligros.
- Las criaturas no estarán obligadas a llevarse bien por pertenecer al grupo del jugador; podrán surgir tensiones o incompatibilidades persistentes.
- La separación, ausencia, lesión o muerte de una criatura podrá afectar a las relaciones de las demás cuando exista un vínculo y conocimiento del acontecimiento.
- Cada criatura solo reaccionará a hechos que haya percibido o conocido mediante un medio plausible; no tendrá conocimiento omnisciente de lo ocurrido a otras.
- Las reglas de interacción, intensidad y evolución de vínculos, memoria, resolución de conflictos y efectos concretos quedan pendientes de diseño.

El objetivo es que el grupo de compañeras desarrolle una dinámica social propia, con vínculos y tensiones que emerjan de sus características y experiencias, en lugar de comportarse como unidades indiferenciadas que obedecen únicamente al jugador.


### Vínculos grupales y coordinación autónoma entre criaturas compañeras

Las criaturas compañeras podrán desarrollar vínculos grupales y coordinar espontáneamente determinadas conductas sin necesitar una orden directa del jugador para cada acción.

- La cohesión y las dinámicas grupales surgirán de las relaciones individuales, experiencias compartidas, tiempo de convivencia, confianza, compatibilidad y circunstancias.
- Las criaturas podrán cooperar, acompañarse, protegerse, apoyarse, mantenerse próximas, repartirse tareas compatibles o responder conjuntamente ante amenazas cuando sus capacidades y relaciones lo permitan.
- También podrán aparecer tensiones, exclusiones, competencia o desacuerdos dentro del grupo; la pertenencia al mismo no garantiza armonía ni cooperación universal.
- La coordinación dependerá de la inteligencia, especie, temperamento, capacidades, experiencia, estado físico y emocional, vínculos y percepción de cada individuo.
- Las conductas autónomas deberán respetar las capacidades reales de cada criatura, sus necesidades, el entorno y los riesgos que perciba.
- La coordinación no implicará obediencia automática a otras criaturas ni conocimiento compartido de información que no hayan percibido o recibido.
- Las órdenes del jugador podrán influir en la conducta grupal cuando las criaturas las comprendan, pero no eliminarán sus vínculos, criterios individuales ni reacciones propias.
- Las reglas de cohesión, liderazgo emergente, reparto de funciones, coordinación en combate y exploración, resolución de desacuerdos y límites de autonomía quedan pendientes de diseño.

El objetivo es que el grupo pueda desarrollar una dinámica propia y actuar de forma coordinada y creíble, sin convertirse en una formación rígida controlada por órdenes constantes ni perder la individualidad de sus integrantes.


### Mediación del jugador en conflictos entre criaturas compañeras

Los conflictos y las relaciones negativas entre criaturas compañeras no provocarán por sí solos que una criatura abandone voluntariamente el grupo. En su lugar, el jugador dispondrá de mecanismos para mediar en los desacuerdos y favorecer la mejora de las relaciones.

- El jugador podrá intervenir cuando detecte tensiones, rivalidades, rechazo u otros conflictos entre compañeras.
- La mediación podrá incluir interacciones o acciones destinadas a reducir tensiones, fomentar confianza, facilitar cooperación y mejorar la convivencia.
- Las opciones disponibles y su eficacia dependerán de la naturaleza del conflicto, las personalidades, temperamentos, vínculos previos, experiencias y circunstancias de las criaturas implicadas.
- La intervención no garantizará una reconciliación inmediata ni obligará a las criaturas a desarrollar amistad o afecto; los cambios deberán ser graduales y coherentes con sus características y experiencias.
- Algunas diferencias o incompatibilidades podrán ser persistentes, por lo que el jugador quizá deba mantener esfuerzos de mediación o aceptar una convivencia cordial pero distante.
- La mediación podrá influir en la cohesión y coordinación grupal, pero no eliminará la individualidad ni las relaciones asimétricas de las criaturas.
- Los conflictos y sus posibles consecuencias deberán ser legibles para el jugador, que necesitará oportunidades razonables para comprender la situación e intervenir.
- Los métodos concretos de mediación, requisitos, costes, frecuencia, límites y efectos sobre cada tipo de relación quedan pendientes de diseño.

El objetivo es dar al jugador un papel activo en el bienestar y la cohesión del grupo, permitiéndole ayudar a resolver conflictos sin convertir las relaciones en valores que puedan modificarse instantáneamente o a voluntad.


### Liderazgo emergente entre criaturas compañeras

Dentro del grupo podrá surgir de forma natural una criatura que asuma un papel de liderazgo o coordinación, sin que el jugador tenga que designarla obligatoriamente.

- El liderazgo emergerá de una combinación de personalidad, temperamento, experiencia, capacidades, iniciativa, confianza y relaciones con las demás criaturas.
- El liderazgo podrá ser reconocido por algunas compañeras y no por otras; no será necesariamente universal ni estará basado en una jerarquía rígida.
- Una criatura con liderazgo podrá influir en la coordinación, cohesión, respuesta ante amenazas, desplazamientos y reparto espontáneo de tareas compatibles.
- El liderazgo no concederá control absoluto sobre las demás ni garantizará obediencia. Cada criatura conservará su individualidad, necesidades, capacidades, vínculos y criterio de respuesta.
- Podrán existir liderazgos situacionales o compartidos: distintas criaturas podrán tomar la iniciativa en contextos donde sus capacidades o experiencias resulten relevantes.
- Los conflictos, cambios de relación, ausencias, lesiones, experiencias y acontecimientos podrán modificar quién ejerce influencia o coordinación dentro del grupo.
- El jugador conservará la posibilidad de intervenir mediante órdenes y mediación, respetando los límites de comprensión y autonomía de cada criatura.
- Los criterios de reconocimiento, influencia, sucesión, liderazgo situacional y efectos concretos sobre el comportamiento grupal quedan pendientes de diseño.

El objetivo es permitir jerarquías sociales flexibles y emergentes entre las compañeras, sin imponer un líder permanente ni convertir al resto en subordinadas sin voluntad propia.


### Reconocimiento del jugador como líder del grupo y autonomía de las compañeras

Las criaturas compañeras podrán reconocer al jugador como figura principal de liderazgo del grupo y, al mismo tiempo, conservar iniciativa y autonomía para actuar cuando él no intervenga.

- El reconocimiento del jugador como líder dependerá del vínculo, confianza, experiencias compartidas, trato recibido y características individuales de cada criatura.
- Ese reconocimiento no será idéntico ni necesariamente tendrá la misma intensidad para todas las compañeras.
- Las criaturas podrán seguir tomando iniciativas, coordinándose entre sí y respondiendo a circunstancias inmediatas sin esperar una orden para cada acción.
- La autonomía estará limitada por sus capacidades, comprensión, personalidad, necesidades, estado, relaciones y percepción de la situación.
- Las órdenes del jugador tendrán influencia según el vínculo y la capacidad de comprensión de cada criatura, pero no anularán automáticamente sus instintos, límites o criterio individual.
- El liderazgo emergente entre compañeras podrá coexistir con el reconocimiento del jugador como líder principal; una criatura podrá asumir coordinación situacional cuando resulte apropiado.
- La confianza y el reconocimiento podrán fortalecerse o deteriorarse según las experiencias y decisiones del jugador, sin convertirse en obediencia absoluta.
- Los criterios concretos de reconocimiento, niveles de confianza, iniciativa, prioridades entre órdenes y autonomía quedan pendientes de diseño.

El objetivo es que el jugador sea el referente principal del grupo sin convertir a las compañeras en unidades pasivas: podrán confiar en él y seguir su liderazgo, pero también pensar, reaccionar y actuar por iniciativa propia.


### Separación temporal autónoma y reencuentro de criaturas compañeras

Las criaturas compañeras podrán separarse temporalmente del jugador por iniciativa propia cuando sus necesidades, comportamiento, capacidades o circunstancias lo justifiquen, y podrán reunirse con él posteriormente mediante desplazamientos y medios plausibles.

- Los motivos de separación podrán incluir explorar, cazar, buscar alimento o agua, descansar, refugiarse, atender necesidades propias, investigar estímulos o responder a circunstancias del entorno.
- La decisión dependerá de la especie, personalidad, temperamento, instintos, necesidades, estado físico o emocional, experiencias, vínculo con el jugador y situación del grupo.
- La separación no equivaldrá automáticamente a abandonar el grupo ni a romper el vínculo.
- Las criaturas deberán actuar dentro de sus capacidades y conocimientos; no podrán localizar al jugador mediante conocimiento omnisciente ni teletransportarse.
- El reencuentro requerirá medios plausibles, como seguir rastros, orientarse mediante referencias conocidas, responder a señales comprendidas o desplazarse hacia un lugar conocido, según las capacidades de cada criatura.
- Distancia, terreno, clima, peligros, obstáculos, estado de la criatura y cambios del mundo podrán dificultar o impedir temporalmente el reencuentro.
- Durante la separación, las criaturas seguirán expuestas a necesidades, amenazas, relaciones y acontecimientos del mundo, con un nivel de simulación adecuado a la distancia.
- El jugador deberá poder comprender, mediante señales o información disponible, cuándo una compañera se separa y cuáles son las circunstancias observables, sin recibir información que no pueda conocer.
- Las reglas para duración, distancia, frecuencia, seguimiento, señales, puntos de reunión, riesgos, recuperación del contacto y casos en que una criatura no pueda regresar quedan pendientes de diseño.

La separación debe reforzar la autonomía y naturalidad de las compañeras, sin convertirse en desapariciones arbitrarias ni garantizar un reencuentro inmediato.


### Experiencias autónomas durante la separación de criaturas compañeras

Mientras estén temporalmente separadas del jugador, las criaturas compañeras podrán vivir acontecimientos propios que influyan en su estado, comportamiento, relaciones y desarrollo posterior.

- Según sus capacidades, necesidades, personalidad, especie y circunstancias, podrán encontrar recursos, descubrir lugares, rastrear presas, explorar rutas o identificar amenazas.
- Podrán sufrir heridas, agotamiento, hambre, miedo u otros efectos, así como enfrentarse a peligros y encuentros con criaturas o personajes.
- Podrán establecer, fortalecer, deteriorar o modificar relaciones con otras criaturas y NPC cuando existan interacciones y oportunidades plausibles.
- Las experiencias podrán aportar aprendizaje y experiencia, modificar confianza, cautela, preferencias, temores, habilidades o conductas, siempre dentro de los sistemas de progresión y personalidad que correspondan.
- Los resultados dependerán de lo que la criatura pueda percibir, comprender y hacer; no se garantizará el éxito ni se le atribuirán conocimientos o capacidades que no posea.
- Los acontecimientos relevantes quedarán asociados a la identidad de la criatura y podrán incorporarse a su historial de experiencias compartidas o propias cuando corresponda.
- Al reencontrarse, sus condiciones y experiencias podrán reflejarse en su comportamiento y en la información que pueda comunicar mediante señales o canales que el jugador comprenda.
- La simulación durante la ausencia del jugador utilizará niveles de detalle apropiados, preservando continuidad y resultados relevantes sin exigir simular cada acción con máximo detalle.
- La separación no deberá producir recompensas, lesiones, pérdidas o cambios arbitrarios: los resultados deben derivarse de riesgos, capacidades, recursos, tiempo y circunstancias.
- Los métodos de comunicación de experiencias, selección de acontecimientos relevantes, progresión obtenida, resolución de encuentros y límites de simulación quedan pendientes de diseño.

Estas experiencias permitirán que las compañeras tengan una vida y evolución propias, en lugar de permanecer como entidades suspendidas mientras el jugador está lejos.


### Comunicación a distancia entre el jugador y criaturas compañeras

Cuando estén separadas, las criaturas compañeras podrán intentar comunicarse con el jugador mediante señales o canales compatibles con su especie y las circunstancias.

- Los medios podrán incluir llamadas, sonidos, gestos, señales visuales, rastros u otras formas de comunicación que la criatura pueda producir y el jugador pueda percibir e interpretar.
- La disponibilidad y eficacia de cada medio dependerán de la especie, anatomía, sentidos, inteligencia, aprendizaje, experiencia y vínculo de la criatura.
- La distancia, el terreno, los obstáculos, el clima, el ruido, la visibilidad y otras condiciones podrán limitar o impedir la comunicación.
- La comunicación podrá servir para indicar ubicación aproximada, advertir de peligros, solicitar ayuda, señalar descubrimientos o transmitir otras necesidades y acontecimientos que la criatura pueda expresar.
- Las señales no transmitirán información ilimitada ni garantizarán que el jugador las comprenda correctamente; podrán ser ambiguas, incompletas o no llegar a su destino.
- El jugador también podrá intentar comunicarse con una compañera separada mediante canales que ella pueda percibir y comprender.
- Ninguna de las partes tendrá conocimiento automático de la ubicación, estado o experiencias de la otra: la información deberá transmitirse por un medio plausible.
- Los repertorios de señales, alcance, interpretación, fiabilidad, aprendizaje y respuestas concretas quedan pendientes de diseño.

El objetivo es mantener una posibilidad de contacto significativa durante las separaciones sin eliminar las limitaciones naturales de percepción, distancia y comunicación.


### Supervivencia autónoma de compañeras cuando pierden el contacto con el jugador

Si una criatura compañera permanece separada y no logra reunirse con el jugador durante un periodo significativo, podrá adoptar conductas de supervivencia autónoma hasta que encuentre una oportunidad plausible de regresar.

- Según su especie, capacidades, personalidad, necesidades y circunstancias, podrá buscar refugio, alimento y agua, evitar amenazas, descansar o desplazarse hacia lugares que considere seguros.
- Algunas criaturas podrán permanecer temporalmente en un área segura o establecer allí un refugio o territorio provisional, siempre que su naturaleza y capacidades lo permitan.
- La elección del lugar y las conductas de supervivencia dependerán de lo que la criatura conozca y perciba; no dispondrá de información omnisciente sobre el mundo ni sobre la ubicación del jugador.
- Podrá modificar sus decisiones si cambian las condiciones, aparecen amenazas, escasean los recursos o recibe señales fiables del jugador.
- Mantendrá sus necesidades, estados, relaciones y vulnerabilidades; sobrevivir por su cuenta no garantizará que permanezca ilesa ni que conserve todos sus recursos.
- Cuando disponga de medios plausibles para retomar el contacto, podrá intentar regresar o responder a señales del jugador, teniendo en cuenta distancia, rutas, obstáculos y riesgos.
- Estas conductas no equivaldrán por sí solas a abandonar voluntariamente el grupo ni a romper el vínculo con el jugador.
- Los umbrales temporales, criterios para elegir refugio o territorio provisional, duración, riesgos y condiciones concretas de retorno quedan pendientes de diseño.

El objetivo es que una separación prolongada no deje a la criatura suspendida ni la obligue a desaparecer arbitrariamente: deberá intentar sobrevivir de acuerdo con su naturaleza y las circunstancias del mundo.


### Evolución de las criaturas compañeras durante separaciones prolongadas

Una compañera podrá experimentar cambios significativos durante una separación prolongada del jugador, como consecuencia de los acontecimientos que realmente viva y de las condiciones a las que esté expuesta.

- Podrá desarrollar o mejorar habilidades, adquirir experiencia y modificar estrategias de supervivencia dentro de sus capacidades y del sistema de progresión correspondiente.
- Podrá sufrir heridas y conservar cicatrices u otras consecuencias persistentes cuando las reglas del mundo lo permitan.
- Su temperamento, confianza, cautela, miedos, preferencias y otros aspectos de su comportamiento podrán cambiar gradualmente por sus experiencias.
- Podrá establecer nuevos vínculos o modificar relaciones existentes con criaturas y NPC mediante interacciones plausibles.
- También podrá sufrir pérdidas, deterioro o retrocesos; la evolución no será necesariamente positiva ni implicará mejoras garantizadas.
- Los cambios dependerán de la especie, personalidad, capacidades, estado, duración de la separación, acontecimientos, recursos y entorno.
- La criatura solo podrá adquirir habilidades, conocimientos y recuerdos mediante experiencias que haya vivido, observado o aprendido de forma plausible.
- Los cambios conservarán continuidad con su identidad e historial individual y podrán reflejarse en su comportamiento cuando el jugador vuelva a encontrarla.
- La separación no será una concesión automática de progresión: los resultados deberán derivarse de la simulación y sus riesgos, sin recompensas o transformaciones arbitrarias.
- Los criterios de progresión, persistencia de lesiones y cicatrices, evolución de personalidad, adquisición de vínculos y comunicación de cambios quedan pendientes de diseño.

El objetivo es que las compañeras tengan una trayectoria propia durante la ausencia del jugador y que un reencuentro tardío pueda revelar una evolución auténtica, con consecuencias tanto positivas como negativas.


### Reacciones emocionales de las compañeras ante reencuentros tras separaciones prolongadas

Cuando una criatura compañera vuelva a encontrarse con el jugador después de una separación prolongada, podrá reaccionar emocionalmente al reencuentro de acuerdo con su identidad, vínculo y experiencias durante la ausencia.

- Las reacciones podrán incluir alegría, alivio, afecto, cautela, miedo, desconfianza, distanciamiento, confusión u otras respuestas coherentes con la criatura.
- La reacción dependerá de la confianza y relación previas con el jugador, la duración y circunstancias de la separación, las experiencias vividas, los cambios personales y el estado físico y emocional actual.
- Una separación difícil o experiencias negativas podrán modificar la forma en que la criatura recibe al jugador; el reencuentro no restaurará automáticamente la confianza ni eliminará conflictos.
- Las criaturas que hayan mantenido un vínculo fuerte podrán mostrar reconocimiento y apego, pero la intensidad y expresión variarán según especie, personalidad, temperamento y capacidades.
- La criatura podrá reconocer al jugador mediante sentidos y memoria compatibles con su naturaleza; no se supondrá reconocimiento infalible en cualquier circunstancia.
- Las respuestas podrán influir en la proximidad, comunicación, cooperación, disposición a seguir órdenes y evolución posterior del vínculo.
- Las reacciones deberán expresarse mediante comportamientos y señales que el jugador pueda percibir e interpretar, sin exigir una manifestación explícita en todos los casos.
- Los criterios de reconocimiento, repertorio de reacciones, duración, intensidad y consecuencias sobre el vínculo quedan pendientes de diseño.

El objetivo es que los reencuentros reflejen la historia vivida durante la separación y el estado actual de la relación, en lugar de tratar a la compañera como si nada hubiera ocurrido.


### Sistema de equipamiento del jugador: piezas individuales y conjuntos completos

El sistema de equipamiento permitirá tanto equipar piezas de armadura de forma independiente como utilizar conjuntos completos.

- El equipamiento podrá organizarse en ranuras para piezas individuales, como casco, torso, guantes, piernas y accesorios, según las categorías que se definan.
- El jugador podrá combinar piezas de distintos conjuntos para construir configuraciones personalizadas.
- También existirán conjuntos completos concebidos como una combinación coherente de piezas, que podrán equiparse como tal sin eliminar la posibilidad de sustituir componentes individualmente.
- Las piezas conservarán sus propias características, requisitos, peso, protección, estadísticas y propiedades especiales, según corresponda.
- Podrán existir bonificaciones o efectos asociados a llevar varias piezas de un mismo conjunto o el conjunto completo; sus condiciones y magnitudes quedan pendientes de diseño.
- Las reglas de ranuras, compatibilidad, límites de accesorios, bonificaciones por conjunto y restricciones por clase quedan pendientes de especificación.

### Creación, adquisición y aprendizaje de recetas de artesanía

El sistema de artesanía permitirá crear recetas y obtenerlas o aprenderlas mediante maestros artesanos.

- El jugador podrá crear recetas de fabricación o modificación de equipamiento y otros objetos contemplados por el sistema de artesanía, utilizando las herramientas y mecanismos que se definan.
- Las recetas también podrán obtenerse o aprenderse de maestros artesanos, que actuarán como fuentes especializadas de conocimiento y técnicas.
- Los maestros artesanos podrán enseñar recetas según sus conocimientos y especialidades; no se asumirá que todos conocen o enseñan las mismas recetas.
- Una receta podrá definir los materiales, cantidades, herramientas, instalaciones, requisitos y procedimiento necesarios para fabricar o modificar un objeto.
- El aprendizaje de una receta y la disponibilidad de los recursos necesarios para ejecutarla serán condiciones diferenciadas: conocer una receta no garantiza disponer de materiales, herramientas o instalaciones.
- La creación de recetas deberá respetar las reglas de materiales, técnicas y capacidades del sistema, evitando fabricar objetos sin requisitos coherentes.
- Los métodos para crear, descubrir, comprar, recibir o aprender recetas, así como los requisitos, costes, especializaciones de maestros y validación de recetas personalizadas quedan pendientes de diseño.

El objetivo es combinar libertad de personalización con un sistema de artesanía basado en conocimiento, especialización y acceso progresivo a técnicas.


### Bonificaciones progresivas por piezas de conjuntos de equipamiento

Los conjuntos de equipamiento podrán conceder bonificaciones progresivas según la cantidad de piezas compatibles que lleve equipadas el jugador.

- Un conjunto podrá conceder una o más bonificaciones al alcanzar determinados umbrales de piezas equipadas.
- Completar el conjunto podrá desbloquear una bonificación adicional o especial.
- El jugador podrá combinar piezas de distintos conjuntos; cada conjunto evaluará por separado cuántas de sus piezas válidas están equipadas.
- Las bonificaciones, umbrales, compatibilidades y reglas para piezas sustituidas o mejoradas serán configurables y quedan pendientes de especificación.

### Artesanía interactiva mediante un sistema de ritmo y calidad de ejecución

La fabricación de objetos utilizará una interacción basada en ritmo para que el jugador participe activamente en el proceso y la creación no sea una operación completamente automática.

- Durante la fabricación, el jugador deberá ejecutar acciones o entradas sincronizadas con señales rítmicas del proceso.
- La secuencia y el tipo de interacción podrán variar según la receta, el objeto, el material, la técnica y la etapa de fabricación.
- La precisión con la que el jugador siga el ritmo influirá en la calidad del objeto resultante.
- Una ejecución especialmente precisa y consistente podrá producir objetos de mayor calidad y mejorar sus estadísticas respecto al resultado básico de esa receta.
- La calidad obtenida deberá depender del desempeño real durante el minijuego y de los límites establecidos por la receta, los materiales, las herramientas y las técnicas disponibles; no permitirá superar arbitrariamente las posibilidades físicas o técnicas del proceso.
- El resultado de la fabricación será determinista a partir de la receta, los recursos, las condiciones de elaboración y el desempeño registrado, de modo que pueda probarse en el motor lógico.
- El núcleo lógico definirá estados, ventanas de ritmo, entradas válidas, evaluación de precisión y cálculo de calidad. La presentación visual, sonora y la lectura física de las entradas corresponderán a capas externas.
- El sistema deberá permitir distintos niveles de complejidad y exigencia según la receta y la técnica, sin asumir que todos los objetos requieren la misma secuencia.
- Las categorías de calidad, las ventanas de precisión, la tolerancia al error, el número de etapas, las estadísticas afectadas y los límites de mejora quedan pendientes de diseño.

La intención es que la habilidad del jugador durante la fabricación tenga consecuencias tangibles en el equipo creado y que el proceso transmita la sensación de estar trabajando realmente el objeto.


### Consecuencias graduadas de los errores durante la artesanía rítmica

Los errores durante el minijuego de fabricación tendrán consecuencias de distinta gravedad según su frecuencia, precisión, etapa y severidad.

- Los errores ordinarios reducirán la puntuación de ejecución y, por tanto, la calidad final o las mejoras estadísticas que podría recibir el objeto.
- Los errores ordinarios no impedirán necesariamente completar la fabricación: el jugador podrá terminar el proceso y obtener el objeto, aunque con una calidad inferior.
- Determinados errores graves o fallos críticos podrán arruinar la pieza durante el proceso, impidiendo obtener el objeto previsto.
- Un fallo crítico podrá provocar la pérdida de algunos de los materiales utilizados. La pérdida no se asumirá automáticamente como total: la cantidad y los materiales afectados dependerán de la receta, la etapa y la naturaleza del fallo.
- La evaluación deberá distinguir entre errores recuperables y fallos críticos mediante reglas explícitas, legibles y configurables.
- La dificultad de la receta, los materiales, las herramientas, las técnicas y las etapas del proceso podrán influir en la tolerancia y las consecuencias de los errores.
- Los umbrales de error, criterios de fallo crítico, posibilidad de recuperación, materiales que pueden perderse y proporciones de pérdida quedan pendientes de diseño.

El objetivo es que la ejecución tenga riesgo y consecuencias reales, pero que un error menor no convierta automáticamente toda fabricación en un fracaso.


## Progresión de disciplinas artesanales del jugador

- El jugador podrá desarrollar habilidades artesanales propias, en lugar de depender exclusivamente de entregar materiales y recetas a artesanos NPC.
- Las disciplinas artesanales se desbloquearán individualmente después de completar una misión de capacitación con un maestro especializado.
- La capacitación será un requisito previo para acceder al desarrollo de la disciplina correspondiente; no bastará con encontrar materiales, comprar una receta o intentar fabricar.
- Una vez desbloqueada, la disciplina podrá desarrollarse mediante la práctica y la participación en procesos de fabricación.
- Las habilidades artesanales podrán relacionarse con el conocimiento de recetas, el dominio de técnicas y la precisión durante los minijuegos rítmicos de fabricación, influyendo en la calidad alcanzable del equipo.
- Los maestros artesanos conservarán un papel relevante como instructores y como fuente de conocimientos o recetas especializadas.
- La lista de disciplinas, requisitos y estructura de las misiones de capacitación, progresión, especializaciones, límites de calidad y relación exacta entre habilidad y minijuego quedan pendientes de definición.


## Progresión independiente de las disciplinas artesanales

- Cada disciplina artesanal tendrá una progresión y un dominio independientes.
- La experiencia, conocimientos, técnicas y mejoras obtenidos en una disciplina no desbloquearán ni aumentarán automáticamente las capacidades de otra.
- El jugador podrá especializarse de manera distinta en cada disciplina, según las capacitaciones completadas y la práctica realizada.
- El nivel de dominio de cada disciplina podrá influir en las técnicas disponibles, la ejecución del proceso de fabricación y la calidad alcanzable de sus productos.
- Las disciplinas concretas, sus niveles, criterios de progreso, técnicas y efectos cuantitativos quedan pendientes de definición.


## Jerarquía de conocimiento de maestros artesanos y métodos propios

- Los maestros artesanos tendrán distintos grados de conocimiento y especialización.
- No todos los maestros conocerán las mismas técnicas, recetas o procedimientos; cada uno podrá enseñar conocimientos que otros no poseen.
- El acceso a técnicas avanzadas dependerá del conocimiento del maestro y de los requisitos de aprendizaje que se definan para cada técnica.
- Al alcanzar un nivel determinado de maestría en una disciplina, el jugador podrá desarrollar un método artesanal propio.
- La creación del método propio permitirá combinar técnicas y conocimientos que el jugador haya aprendido de distintos maestros y fuentes válidas.
- La combinación estará limitada a conocimientos realmente adquiridos por el jugador; no permitirá incorporar técnicas desconocidas.
- El método propio podrá reflejar la experimentación y especialización del jugador, y su funcionamiento deberá respetar las reglas de fabricación, materiales, herramientas y disciplina correspondiente.
- El umbral de maestría, las reglas de compatibilidad entre técnicas, el proceso de experimentación, los riesgos, la validación y los efectos concretos de los métodos propios quedan pendientes de definición.


## Recetas originales y objetos únicos mediante métodos artesanales propios

- Los métodos artesanales originales del jugador podrán dar lugar a objetos únicos que no se obtienen mediante las recetas tradicionales conocidas.
- Estos objetos podrán presentar propiedades, estadísticas o combinaciones de características originales, siempre dentro de las reglas y límites del sistema de fabricación.
- Cuando el jugador descubra y valide una receta mediante experimentación, podrá registrarla para reproducirla posteriormente.
- El registro conservará los conocimientos necesarios para repetir el procedimiento, incluidos los materiales, técnicas y condiciones que formen parte de la receta descubierta.
- La posibilidad de reproducir una receta no elimina los requisitos de disciplina, dominio, herramientas, instalaciones o recursos que correspondan.
- Los criterios para considerar válida una receta, los límites de sus propiedades, la reproducibilidad exacta y las condiciones para compartir o transmitir recetas quedan pendientes de definición.


## Experimentación virtual de recetas sin consumo de materiales

- El subsistema de recetas incluirá un entorno de experimentación que permita al jugador simular procesos de fabricación antes de ejecutarlos realmente.
- La experimentación virtual no consumirá materiales, componentes ni herramientas físicas, y no producirá objetos reales.
- El jugador podrá probar combinaciones de materiales, técnicas, herramientas, parámetros y secuencias de fabricación dentro de las posibilidades conocidas o investigables de su disciplina.
- La simulación permitirá evaluar resultados potenciales y explorar métodos originales sin el coste material de cada intento.
- Los resultados de la simulación deberán distinguirse de los resultados garantizados de una fabricación real: las condiciones, recursos y ejecución efectiva podrán influir en el producto final.
- Las recetas o métodos descubiertos mediante experimentación deberán cumplir los criterios de validación que se definan antes de registrarse como reproducibles.
- La fabricación real seguirá consumiendo los recursos correspondientes y utilizando el sistema de ejecución rítmica y evaluación de calidad.
- El alcance de la simulación, la información que revela, sus límites de conocimiento, los criterios de validación y la relación entre resultados simulados y fabricación real quedan pendientes de definición.


## Descubrimiento asistido y creación de recetas inéditas

- Durante la experimentación virtual, el sistema podrá identificar combinaciones prometedoras de materiales, técnicas, herramientas y parámetros, y ofrecer pistas graduales al jugador.
- Las pistas orientarán la investigación sin revelar automáticamente la receta completa ni eliminar la necesidad de experimentar y comprender el proceso.
- El subsistema permitirá crear recetas genuinamente nuevas, que no formen parte del catálogo tradicional existente.
- Las recetas inéditas podrán surgir de la experimentación y combinación válida de conocimientos, técnicas y componentes, respetando las reglas de la disciplina artesanal.
- El sistema deberá distinguir entre recetas ya conocidas, variantes de recetas existentes y descubrimientos realmente nuevos.
- Una receta inédita deberá validarse antes de registrarse como reproducible y utilizarse en fabricación real.
- Las pistas, criterios de novedad, validación, límites de combinaciones y protección frente a resultados incoherentes quedan pendientes de definición.


## Descubrimiento progresivo y perfeccionamiento de recetas

- Las recetas originales podrán atravesar distintos grados de descubrimiento y perfeccionamiento.
- Una primera versión validada de una receta podrá ser funcional sin representar todavía su forma óptima.
- El jugador podrá volver a experimentar sobre recetas registradas para descubrir mejoras en materiales, proporciones, técnicas, herramientas, secuencias o parámetros de fabricación.
- Las mejoras descubiertas podrán perfeccionar el resultado del objeto, optimizar el proceso o modificar propiedades, siempre dentro de límites coherentes del sistema.
- Las variantes y revisiones deberán conservar su relación con la receta de origen, permitiendo reconocer su evolución y recuperar versiones anteriores cuando corresponda.
- La experimentación virtual permitirá investigar mejoras sin consumir materiales; la fabricación real seguirá requiriendo los recursos y condiciones correspondientes.
- Los grados de descubrimiento, criterios de perfeccionamiento, control de versiones, límites de mejora y efectos concretos quedan pendientes de definición.


## Identidad y denominación de recetas originales

- El jugador podrá asignar nombres propios a las recetas originales que descubra y valide.
- El nombre quedará asociado a la identidad de la receta y a sus versiones derivadas, conservando la relación con la receta de origen.
- Los objetos fabricados mediante una receta original podrán mostrar su nombre o denominación artesanal como parte de su identidad.
- El registro del objeto podrá conservar la receta utilizada y su versión, permitiendo reconocer su procedencia artesanal.
- Los criterios de denominación, cambios de nombre, visibilidad en interfaces y tratamiento de copias o fabricaciones por otros artesanos quedan pendientes de definición.


## Reconocimiento, aprendizaje e influencia de las creaciones artesanales del jugador

- Las creaciones originales del jugador podrán llegar a ser conocidas por otros artesanos y maestros cuando exista un medio plausible para descubrirlas: observar los objetos, examinar una pieza, recibir una receta o información, o conocer su reputación.
- La reacción de cada artesano dependerá de sus conocimientos, personalidad, intereses, relaciones con el jugador, reputación de la creación y circunstancias.
- Otros artesanos podrán admirar o reconocer la calidad y originalidad de una creación, interesarse por su método, intentar aprenderlo cuando dispongan de información y medios suficientes, o desarrollar variantes propias.
- Conocer o examinar un objeto terminado no concederá automáticamente el conocimiento completo de su receta o procedimiento de fabricación.
- La imitación y el aprendizaje requerirán información suficiente, capacidades y medios compatibles; los resultados podrán ser parciales o imperfectos.
- Las variantes desarrolladas por otros artesanos deberán conservar su propia identidad y procedencia, sin confundirse con la receta original del jugador.
- La reputación y difusión de una creación podrán influir en su reconocimiento, pero no garantizarán que todos los artesanos la conozcan.
- Los medios de difusión, grados de información, condiciones de aprendizaje, reconocimiento, imitación y consecuencias económicas o sociales quedan pendientes de definición.


## Reputación artesanal regional y actividades no centradas en el combate

- El jugador podrá desarrollar una reputación artesanal que se difunda entre regiones y facciones mediante creaciones conocidas, demostraciones, intercambios, encargos, recomendaciones y otros medios plausibles.
- La reputación no será necesariamente uniforme: el reconocimiento podrá variar entre regiones, comunidades, gremios, facciones y círculos de artesanos según la información que haya llegado a cada uno.
- Maestros y artesanos podrán buscar al jugador para aprender de sus métodos, solicitar enseñanzas, proponer colaboraciones o intercambiar conocimientos cuando sus intereses, capacidades y relaciones lo justifiquen.
- Podrán surgir desafíos y competiciones de fabricación que permitan demostrar habilidad, comparar técnicas y obtener reconocimiento.
- Estas oportunidades podrán ampliar las actividades del jugador más allá del combate y la exploración, incorporando aprendizaje, creación, enseñanza, colaboración, competencia e intercambio artesanal.
- Las invitaciones, propuestas y desafíos dependerán de que los personajes conozcan la reputación del jugador por medios plausibles; no serán universales ni aparecerán automáticamente en todas partes.
- Los efectos concretos de la reputación, su difusión, las condiciones de acceso a colaboraciones, enseñanza y competiciones, así como sus recompensas y consecuencias, quedan pendientes de definición.


## Fundación y legado de talleres y gremios artesanales

- El jugador podrá fundar y desarrollar su propio taller o gremio artesanal.
- Podrá incorporar aprendices y contratar artesanos, según los requisitos y posibilidades del mundo.
- El taller o gremio podrá convertirse en un centro de producción, aprendizaje, experimentación, colaboración e intercambio de conocimientos.
- Los métodos, recetas y técnicas desarrollados por el jugador podrán transmitirse dentro de la organización cuando existan condiciones de enseñanza y aprendizaje.
- La organización podrá desarrollar una identidad y una tradición artesanal propias, cuya continuidad no dependerá exclusivamente de la presencia constante del jugador.
- La evolución, autonomía, administración, costes, contratación, sucesión, transmisión de conocimientos y persistencia de la organización quedan pendientes de definición.

## Subsistemas de actividades no centradas en el combate

- Además de la artesanía, el juego contemplará subsistemas profundos para el comercio, la cocina, la pesca y el tratamiento de heridas.
- Estos subsistemas deberán ofrecer actividades, conocimientos, progresión y decisiones propias, en lugar de reducirse a interacciones automáticas de compra, recolección, preparación o uso de objetos.
- Cada subsistema podrá incluir aprendizaje, especialización, técnicas, recetas o procedimientos, experimentación cuando resulte apropiado, calidad de ejecución y resultados dependientes de las capacidades del jugador y de las condiciones.
- Los subsistemas deberán integrarse con el mundo, sus personajes, comunidades, recursos, economía, calendario y circunstancias, manteniendo coherencia con sus reglas.
- El comercio podrá contemplar actividades y relaciones económicas más amplias que la simple compra y venta; la cocina podrá contemplar preparación y desarrollo de recetas; la pesca podrá contemplar técnicas y condiciones de captura; y el tratamiento de heridas podrá contemplar conocimientos y procedimientos de atención y recuperación.
- Cada subsistema tendrá reglas y progresión propias, sin asumir que dominar uno concede automáticamente dominio sobre los demás.
- El alcance, las mecánicas concretas, los requisitos, los riesgos, las recompensas, la progresión y las relaciones entre estos subsistemas quedan pendientes de diseño.


## Arquitectura común para profesiones y actividades no centradas en el combate

- Los subsistemas de artesanía, comercio, cocina, pesca y tratamiento de heridas compartirán una arquitectura conceptual común de aprendizaje, progresión, especialización, ejecución, calidad, reconocimiento y desarrollo de conocimientos.
- Esta arquitectura común no impondrá una mecánica de interacción idéntica a todas las actividades. Cada disciplina tendrá procedimientos, decisiones, riesgos, recursos, condiciones y formas de ejecución adaptados a su naturaleza.
- **Aprendizaje y desbloqueo:** cada disciplina podrá requerir capacitación con maestros, instructores, profesionales, misiones, descubrimientos u otros medios coherentes con la actividad. No se asumirá que todas deben desbloquearse exactamente del mismo modo.
- **Progresión independiente:** cada disciplina y sus especializaciones tendrán desarrollo propio. El dominio de una no concederá automáticamente dominio de otra.
- **Conocimientos y técnicas:** profesionales y maestros podrán poseer conocimientos diferenciados; el jugador podrá aprender técnicas específicas y combinarlas cuando sean compatibles.
- **Experimentación y simulación:** cuando la naturaleza de la actividad lo permita, el jugador podrá probar hipótesis, procedimientos o combinaciones en un entorno simulado sin consumir recursos físicos. La simulación no concederá bienes, capturas, curaciones ni resultados reales.
- **Ejecución interactiva:** las actividades no se resolverán necesariamente mediante una única orden automática. El jugador podrá participar en procesos y tomar decisiones significativas mediante mecánicas apropiadas a cada disciplina; no se impondrá el minijuego rítmico de fabricación a actividades donde no resulte adecuado.
- **Calidad y consecuencias:** los resultados dependerán de conocimientos, dominio, preparación, herramientas, recursos, condiciones del entorno, decisiones y ejecución. Los errores y fallos tendrán consecuencias proporcionadas y específicas de cada actividad, sin copiar automáticamente las reglas de fallo de la fabricación.
- **Innovación:** cuando sea coherente, el jugador podrá crear recetas, métodos, técnicas, procedimientos, estrategias comerciales u otras soluciones nuevas, respetando los límites y conocimientos necesarios de cada disciplina.
- **Registro y perfeccionamiento:** los descubrimientos válidos podrán documentarse, revisarse y perfeccionarse. Se conservará la relación entre versiones y su procedencia cuando corresponda.
- **Maestros y transmisión:** maestros, instructores y profesionales podrán enseñar conocimientos distintos. El jugador podrá compartir conocimientos y, si se establecen medios plausibles, otros personajes podrán aprenderlos, adaptarlos o desarrollar variantes.
- **Reputación y difusión:** las actividades y creaciones podrán generar reconocimiento local o regional según resultados, calidad, difusión e información realmente conocida por cada comunidad.
- **Organización y legado:** cuando la disciplina lo permita, el jugador podrá fundar y desarrollar negocios, talleres, instituciones o redes, incorporar personal y transmitir conocimientos para favorecer su continuidad durante su ausencia.
- **Integración con el mundo:** cada subsistema deberá considerar recursos, economía, calendario, clima, geografía, personajes, comunidades, facciones y acontecimientos pertinentes, sin otorgar conocimiento omnisciente a sus participantes.
- **Reglas propias y límites:** cada disciplina conservará sus requisitos, recursos, riesgos, resultados y restricciones específicos. La arquitectura común no debe borrar sus diferencias ni forzar funciones que no tengan sentido para ella.
- Los detalles de implementación, interfaces, parámetros, fórmulas, simulación, persistencia, equilibrio y mecánicas específicas de cada disciplina quedan pendientes de diseño.


## Economía dinámica y negociación basada en persuasión e investigación local

- El comercio tendrá una economía dinámica en la que precios, disponibilidad, demanda y oportunidades comerciales podrán variar según estaciones, conflictos, rutas comerciales, cosechas, catástrofes, recursos, decisiones de las comunidades y otros acontecimientos pertinentes.
- Las condiciones económicas podrán diferir entre asentamientos y regiones; no existirá necesariamente un precio o nivel de demanda universal para cada mercancía.
- La negociación con NPC utilizará un sistema de persuasión, no una simple selección automática de precio ni una prueba abstracta desconectada del contexto.
- Para negociar eficazmente, el jugador deberá investigar las necesidades, prioridades, escasez, excedentes, problemas y circunstancias de la zona mediante conversaciones e interacciones con sus habitantes y otros NPC.
- La información obtenida dependerá de quién la conoce, de lo que haya percibido o recibido y de su disposición a compartirla. Los NPC no tendrán conocimiento omnisciente de la situación económica.
- Las conversaciones podrán aportar información parcial, indirecta, imprecisa o desactualizada; el jugador deberá contrastar indicios y comprender las circunstancias antes de utilizarlos en una negociación.
- La persuasión deberá apoyarse en argumentos pertinentes a las necesidades e intereses conocidos del interlocutor y de la comunidad, así como en la credibilidad, reputación, relaciones y circunstancias de la negociación.
- Conocer las necesidades locales podrá permitir al jugador presentar ofertas, intercambios o acuerdos más atractivos y justificar sus condiciones; no garantizará automáticamente que el NPC acepte.
- La respuesta del interlocutor dependerá de sus intereses, personalidad, objetivos, recursos, confianza, relaciones, información y margen real de decisión.
- La negociación, la investigación de necesidades y la economía dinámica deberán estar conectadas: cambios en la región podrán modificar lo que necesita la comunidad y, por tanto, las oportunidades y argumentos comerciales disponibles.
- Los métodos concretos de persuasión, tipos de información, fiabilidad, factores de aceptación, límites de negociación, regateo, contratos y resolución de desacuerdos quedan pendientes de definición.


## Rutas comerciales autónomas y circulación de mercancías

- Comerciantes, asentamientos y comunidades podrán establecer y mantener rutas comerciales autónomas entre lugares conectados por medios de transporte plausibles.
- Las rutas permitirán el transporte de mercancías, recursos y suministros según la producción, excedentes, necesidades, demanda, acuerdos y capacidad logística de los participantes.
- El funcionamiento de las rutas dependerá de factores como distancia, terreno, infraestructura, medios de transporte, disponibilidad de personal, seguridad, clima, estaciones y relaciones entre comunidades.
- Las rutas podrán ser terrestres, fluviales, marítimas u otras que el mundo y sus medios de transporte permitan.
- Los comerciantes y comunidades podrán modificar, ampliar, reducir, suspender o abandonar rutas según sus circunstancias, recursos, riesgos y oportunidades.
- Guerras, emboscadas, bloqueos, catástrofes, cambios políticos, escasez y otros acontecimientos podrán retrasar, encarecer, desviar o interrumpir el transporte.
- El jugador podrá observar indicios de la circulación de mercancías y descubrir oportunidades mediante conversaciones, mercados, viajeros, comerciantes y otros medios plausibles.
- El jugador podrá participar en redes existentes o desarrollar sus propias rutas y acuerdos comerciales, sujeto a los requisitos y capacidades correspondientes.
- La simulación de rutas deberá continuar durante la ausencia del jugador con niveles de detalle adecuados y conservar continuidad de mercancías, viajes, pérdidas, retrasos y cambios de destino.
- Los NPC solo conocerán el estado de rutas y cargamentos mediante información que hayan percibido o recibido; no tendrán conocimiento omnisciente.
- Los modelos de transporte, capacidad, costes, seguridad, propiedad de mercancías, pérdidas, seguros, tiempos, escalas de simulación y gestión de rutas quedan pendientes de definición.


## Gestión empresarial y especialización múltiple del jugador

- Cuando el jugador funde o dirija una compañía, taller, gremio u otra organización vinculada a sus actividades, deberá poder gestionarla de forma activa.
- La gestión incluirá, según la naturaleza de cada organización, contratación y asignación de personal, adquisición de recursos, producción o prestación de servicios, almacenamiento, logística, finanzas, acuerdos, mantenimiento, expansión y respuesta a riesgos.
- En el comercio, el jugador podrá contratar caravanas, barcos, transportistas y escoltas para mover sus mercancías. Los transportes estarán expuestos a riesgos coherentes con la ruta y las circunstancias, incluidos robos, ataques, pérdidas, retrasos, daños, bloqueos y cambios de recorrido.
- La contratación de transporte y escoltas no garantizará el éxito del viaje; capacidades, costes, preparación, seguridad, información y acontecimientos influirán en el resultado.
- La gestión y autonomía de las organizaciones deberán continuar durante la ausencia del jugador, dentro de las instrucciones, recursos, personal, capacidades y condiciones disponibles. No se presumirá éxito automático ni funcionamiento ilimitado.
- Este principio de gestión se aplicará también a las demás actividades que puedan convertirse en negocios u organizaciones, adaptando las responsabilidades y operaciones a cada ámbito: talleres y gremios artesanales, comercio, cocina, pesca y servicios relacionados con el tratamiento de heridas.
- El jugador podrá especializarse y desarrollar dominio en todas las disciplinas disponibles. No habrá una elección excluyente que le impida aprender o progresar en otras profesiones por haber avanzado en una.
- Cada disciplina conservará su progresión independiente y sus requisitos de aprendizaje, práctica y maestría. Poder desarrollar todas no implica dominarlas simultáneamente ni eliminar los límites de tiempo, recursos, conocimientos y esfuerzo.
- La capacidad de dirigir varias organizaciones a la vez dependerá de los sistemas de personal, delegación, recursos, administración y circunstancias que se definan; no se asumirá gestión perfecta o ilimitada.
- Los modelos de administración, delegación, costes, contratación, autonomía, capacidad operativa, informes, riesgos y escalabilidad quedan pendientes de definición.


## Personalidad, desarrollo y autonomía del personal de las organizaciones

- Los empleados de talleres, gremios, compañías y demás organizaciones podrán tener personalidad, habilidades, necesidades, relaciones y ambiciones propias.
- Sus capacidades y experiencias influirán en el desempeño de las tareas que se les asignen y en su desarrollo profesional.
- Los empleados podrán aprender, mejorar habilidades, adquirir experiencia y asumir responsabilidades mayores mediante práctica, formación y oportunidades reales.
- Según su personalidad, conocimientos, iniciativa, lealtad, relaciones y circunstancias, podrán tomar decisiones dentro de sus responsabilidades, proponer mejoras, resolver problemas o cometer errores.
- Algunos empleados podrán llegar a convertirse en administradores, encargados o responsables de confianza si cuentan con capacidades y experiencia adecuadas y el jugador les delega autoridad.
- La autonomía estará limitada por el cargo, las instrucciones, los conocimientos, los recursos y la información disponibles; los empleados no tendrán conocimiento omnisciente ni autoridad que no se les haya concedido.
- Las necesidades, conflictos, ambiciones, satisfacción, confianza y relaciones del personal podrán afectar contratación, retención, cooperación, productividad y estabilidad de la organización.
- Los criterios de selección, salarios, formación, evaluación, ascensos, delegación, conflictos laborales, renuncias y posibles deslealtades quedan pendientes de definición.

## Integración y fusión de compañías de distintos sectores

- Si el jugador posee o controla organizaciones de distintos sectores, podrá integrarlas o fusionarlas para formar una estructura empresarial común cuando se cumplan los requisitos correspondientes.
- La integración podrá conectar actividades complementarias, compartir recursos e infraestructura, coordinar personal, logística, almacenamiento, producción, distribución y administración.
- La fusión no hará que las disciplinas pierdan sus reglas, necesidades ni operaciones específicas: cada sector conservará sus procesos especializados dentro de la organización integrada.
- La estructura resultante podrá aprovechar sinergias entre sectores, por ejemplo, producción artesanal, abastecimiento comercial, preparación de alimentos, pesca y servicios de tratamiento de heridas, cuando existan relaciones operativas plausibles.
- La fusión podrá requerir acuerdos, recursos, instalaciones, capacidad administrativa, compatibilidad organizativa y aceptación de las partes involucradas, según la situación.
- La integración también podrá generar costes, dependencias, conflictos de intereses, dificultades de coordinación y riesgos compartidos; no será automáticamente beneficiosa.
- Se podrán definir estructuras con divisiones, sucursales, responsables y administradores delegados para gestionar operaciones de distintos sectores.
- Las condiciones legales, económicas, políticas y sociales para fusionar organizaciones, así como sus estructuras, gobernanza, contabilidad, sinergias y riesgos, quedan pendientes de definición.


## Organizaciones familiares y continuidad dinástica

Las compañías, talleres, gremios y demás organizaciones fundadas o adquiridas por el jugador podrán evolucionar hasta convertirse en organizaciones familiares o dinásticas. Sus propiedades, negocios, conocimientos, técnicas, reputación y responsabilidades podrán transmitirse a herederos y sucesores, incluso a través de varias generaciones.

La continuidad no será automática ni garantizará que la organización conserve su prosperidad o influencia. La sucesión dependerá de las personas disponibles, sus capacidades, preparación, relaciones, intereses, legitimidad y aceptación por parte de los miembros de la organización y del entorno social o político. Las decisiones de sucesión, los conflictos entre herederos, la conservación o transformación de la identidad de la organización y sus consecuencias concretas quedan sujetos a un diseño posterior.

Esta decisión cierra por ahora la profundización del sistema de profesiones, oficios y organizaciones empresariales. Se retomará únicamente si resulta necesario para definir otros sistemas o si el usuario decide volver sobre el tema.


## Durabilidad, deterioro y reparación del equipamiento

- Las piezas de equipamiento estarán sujetas a durabilidad y podrán deteriorarse durante el uso.
- El desgaste podrá producirse por impactos, ataques recibidos, uso continuado y exposición a condiciones ambientales, según la naturaleza y los materiales de cada pieza.
- El ritmo y la forma del deterioro podrán variar entre armas, armaduras, accesorios y otros equipos; no se asumirá que todos los objetos se desgastan de manera idéntica.
- El equipamiento deteriorado podrá requerir reparación para recuperar su condición funcional.
- Las reglas concretas de durabilidad, umbrales de deterioro, efectos sobre estadísticas y rendimiento, materiales y costes de reparación, herramientas necesarias, reparadores disponibles y posibles límites de reparación quedan pendientes de definición.


### Aclaración: efectos progresivos del desgaste y reparación especializada

- El deterioro del equipamiento afectará progresivamente a su rendimiento a medida que disminuya su durabilidad; no se limitará a una penalización que aparezca únicamente al cruzar un umbral único.
- La forma concreta de esa pérdida de rendimiento dependerá del tipo de equipo y de sus propiedades. Por ejemplo, el desgaste de un arma podrá afectar su desempeño ofensivo, mientras que el de una pieza defensiva podrá reducir su protección.
- La reparación se integrará con las disciplinas de herrería y armería ya contempladas en el sistema de profesiones, en lugar de constituir un sistema profesional independiente.
- La capacidad de reparar, la calidad del resultado y los recursos o herramientas necesarios dependerán de los conocimientos y capacidades pertinentes, así como del tipo de pieza y de su estado.
- Los umbrales, fórmulas, efectos específicos por categoría de equipo, costes, materiales y límites de reparación quedan pendientes de definición.


### Aclaración: daños estructurales en estados críticos de durabilidad

- Las armas y piezas de armadura podrán sufrir daños estructurales o funcionales especiales —como fracturas, deformaciones o pérdida de componentes— cuando su durabilidad se aproxime a su límite mínimo.
- La aparición y gravedad de estos daños estarán vinculadas al estado de deterioro y a las circunstancias que los provoquen; no serán consecuencias habituales del desgaste normal mientras el equipo conserve una durabilidad adecuada.
- Estos daños especiales podrán agravar la pérdida de rendimiento y requerir reparaciones apropiadas a la naturaleza del daño.
- Los umbrales, probabilidades, tipos de daño por categoría de equipo y procedimientos específicos para restaurar cada pieza quedan pendientes de definición.


### Materiales del equipamiento y componentes de origen biológico

- Las armas, armaduras y demás piezas de equipamiento podrán fabricarse con distintos materiales, cada uno con propiedades propias que influyen en características como peso, resistencia, durabilidad y afinidad mágica.
- Los materiales no se limitarán a metales: también podrán incluir subproductos obtenidos de monstruos y animales, utilizables como materiales o componentes de fabricación cuando sus propiedades lo permitan.
- Los subproductos podrán aportar características particulares derivadas de su origen y naturaleza, diferenciando los equipos fabricados con ellos de los elaborados con materiales convencionales.
- La combinación de materiales y componentes podrá influir en las propiedades finales del equipo.
- El catálogo de materiales, sus propiedades cuantitativas, compatibilidades, métodos de obtención y combinaciones concretas queda pendiente de definición.


### Calidad y obtención interactiva de materiales

- Los materiales tendrán grados de calidad; materiales del mismo tipo y procedencia podrán presentar diferencias de calidad y propiedades.
- La obtención de materiales incluirá interacciones jugables, en lugar de limitarse siempre a una recolección automática.
- En la extracción de minerales, el jugador podrá seguir patrones y detectar o aprovechar debilidades presentes en los depósitos para extraerlos de manera más eficaz.
- La precisión y perfección de la ejecución influirán en la calidad del material obtenido: una ejecución mejor realizada podrá producir materiales de mayor calidad.
- Los materiales también podrán adquirirse mediante compra; la obtención directa y la compra serán vías complementarias.
- Los patrones concretos, las herramientas, las acciones disponibles, la evaluación de la ejecución, los grados de calidad y sus efectos cuantitativos quedan pendientes de definición.


### Obtención interactiva de subproductos animales y monstruosos

- La obtención de subproductos de animales y monstruos contará con interacciones propias, no se resolverá necesariamente mediante una recolección automática.
- El jugador deberá identificar cómo despiezar al ejemplar o extraer cada componente para conservar la mayor calidad posible.
- La técnica y precisión de la extracción influirán en la calidad y el estado de los materiales obtenidos; una ejecución más perfecta podrá conservar mejor sus propiedades.
- Los procedimientos podrán variar según la criatura y el componente que se quiera obtener.
- Las técnicas concretas, herramientas, patrones, riesgos de pérdida o daño y reglas cuantitativas quedan pendientes de definición.


### Anatomía de criaturas y progresión de extracción

- Las criaturas tendrán puntos anatómicos y componentes identificables que podrán estudiarse y aprovecharse para obtener materiales específicos.
- El conocimiento de la anatomía de cada criatura será útil tanto durante el combate, para reconocer posibles puntos vulnerables, como durante el despiece y la extracción de componentes.
- El sistema de obtención y extracción de materiales tendrá su propio nivel de progresión, siguiendo el modelo de progresión establecido para la fabricación.
- Esta progresión representará el desarrollo de la capacidad del jugador para reconocer anatomías, seleccionar técnicas y extraer componentes con mayor eficacia y calidad.
- Los detalles de niveles, experiencia, desbloqueos, especializaciones y fórmulas de progresión deberán mantener coherencia con el sistema de fabricación y quedan pendientes de especificación.


### Registro de conocimientos sobre criaturas y minerales

- El conocimiento sobre la anatomía y los componentes de cada especie se descubrirá mediante observación y experimentación, y quedará registrado en una enciclopedia de criaturas.
- Ese conocimiento servirá para reconocer puntos anatómicos, vulnerabilidades y métodos adecuados de extracción de subproductos.
- El mismo principio de descubrimiento y registro se aplicará a los minerales y depósitos: el jugador podrá observarlos y experimentar para aprender a reconocer sus características, patrones y debilidades explotables durante la extracción.
- La información descubierta sobre criaturas y minerales se conservará como conocimiento consultable y podrá mejorar la eficacia y calidad de las actividades relacionadas.
- La estructura de la enciclopedia, las categorías de conocimiento, los métodos de investigación y sus efectos concretos quedan pendientes de definición.


### Instructores para actividades y profesiones

- Todas las actividades y profesiones del juego podrán contar con instructores que enseñen y transmitan conocimientos y técnicas al jugador.
- Los instructores serán una vía de aprendizaje complementaria a la observación, la experimentación y el descubrimiento autónomo.
- Su enseñanza podrá ayudar al jugador a aprender procedimientos, técnicas y conocimientos propios de la actividad o profesión correspondiente.
- La disponibilidad de instructores, sus requisitos, especialidades, métodos de enseñanza y el alcance de los conocimientos que podrán transmitir quedan pendientes de definición.


## Instructores y conocimiento de recursos naturales

- Todas las actividades y profesiones podrán contar con instructores.
- Los instructores tendrán distintos niveles de maestría y especialización. Los maestros de mayor nivel podrán enseñar técnicas y conocimientos avanzados que no estarán disponibles con instructores menos especializados.
- El aprendizaje guiado por instructores complementará el aprendizaje autónomo mediante observación, experimentación, práctica y descubrimiento.
- El sistema de conocimiento y obtención de recursos naturales no se limitará a minerales y componentes de criaturas: incluirá también plantas, hongos y otros recursos naturales.
- Estos recursos podrán investigarse mediante observación y experimentación, registrando los descubrimientos en la enciclopedia y aplicando ese conocimiento a su identificación y recolección interactiva.
- Los detalles de rangos de instructores, requisitos de enseñanza, progresión y procedimientos particulares de recolección quedan pendientes de definición.


## Ecosistemas dinámicos y disponibilidad de recursos

- Los ecosistemas del mundo serán dinámicos, no simples ubicaciones con poblaciones y recursos estáticos.
- Las poblaciones de animales y monstruos podrán crecer o disminuir, y las especies podrán migrar o desaparecer localmente según las condiciones de su entorno.
- Las plantas, hongos y otros recursos naturales podrán expandirse, escasear o desaparecer localmente.
- Las estaciones, el clima, el terreno y las condiciones del ecosistema influirán en la distribución, abundancia y disponibilidad de especies y recursos.
- Las intervenciones del jugador, como la caza y la recolección, podrán alterar el equilibrio ecológico y tener consecuencias sobre la disponibilidad futura de recursos.
- Los modelos ecológicos concretos, sus escalas temporales, reglas de reproducción, migración, regeneración y recuperación quedan pendientes de diseño.


## Territorios, guaridas y relaciones ecológicas de las criaturas

- Los animales y monstruos podrán disponer de territorios y áreas de actividad propias.
- Las especies podrán establecer guaridas, con funciones que dependerán de su naturaleza y comportamiento.
- Las criaturas participarán en cadenas alimenticias y mantendrán relaciones de depredación y alimentación con otras especies y recursos del entorno.
- Distintas especies podrán competir por alimento, territorio y otros recursos.
- La presencia, expansión, migración o desaparición de una especie podrá afectar a las poblaciones y comportamientos de otras, contribuyendo a la dinámica de cada ecosistema.
- Las reglas particulares de territorialidad, guaridas, alimentación, reproducción y competencia quedan pendientes de especificación.


## Amenazas de criaturas y respuesta de las poblaciones

- Los monstruos podrán atacar asentamientos, caravanas y viajeros, convirtiéndose en amenazas activas para la población y las actividades del mundo.
- Las poblaciones locales podrán organizar cacerías para reducir amenazas de criaturas.
- También podrán organizar expediciones para enfrentarse a monstruos especialmente peligrosos.
- La actividad de las criaturas y las respuestas organizadas de los habitantes podrán afectar a la seguridad de las regiones.
- Los criterios de respuesta, la frecuencia, la composición y los resultados de estas expediciones y cacerías quedan pendientes de diseño.
