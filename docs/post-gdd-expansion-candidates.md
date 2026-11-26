# Expansión de conceptos y sistemas — propuestas posteriores al GDD

> **Estado del documento:** catálogo de ideas candidatas.  
> **Autoridad:** complementa el GDD baseline; no lo sustituye ni lo modifica.  
> **Regla de interpretación:** salvo donde se indique expresamente que una decisión ya está acordada, los conceptos de este documento permanecen **EN EVALUACIÓN**. No deben convertirse en requisitos de implementación, cifras definitivas ni canon del mundo sin una decisión posterior.

Este documento reúne las ideas de expansión compartidas para Mythical Exodus y las amplía con las precisiones discutidas sobre jefes colosos. Su propósito es preservar posibilidades de diseño, identificar dependencias y preparar futuras decisiones sin introducir complejidad prematura en el motor lógico.

## 1. Identidad del proyecto y arquitectura general

**Estado: EN EVALUACIÓN**

- Explorar la identidad de *Action-RPG sistémico* o *Single-Player MMO*: un juego para un solo jugador que aspire a escala, persistencia, economía, simulación y reactividad asociadas a mundos MMO, sin multijugador, servidores remotos ni problemas de latencia de red.
- La expresión “Single-Player MMO” es una etiqueta conceptual, no una definición comercial o técnica cerrada.
- El núcleo lógico en C# sería la autoridad sobre reglas, estados y simulación; el futuro motor gráfico actuaría como cliente de presentación que consulta estados y comunica intenciones/acciones.
- Mantener la independencia del núcleo respecto de Unity, Unreal u otro motor gráfico.
- Se ha propuesto un mundo de aproximadamente 37 km², con escala de referencia comparable a Skyrim. La cifra es orientativa y no constituye todavía una dimensión aprobada.
- Considerar simulación de distinta fidelidad según proximidad e importancia: detalle alto cerca del jugador y abstracción para zonas lejanas. La reducción de fidelidad no debe equivaler a borrar acontecimientos relevantes ni a detener arbitrariamente sistemas persistentes.
- Investigar streaming del mundo por regiones/celdas y persistencia de los cambios entre niveles de simulación.

### Separación de responsabilidades

**Núcleo lógico:** entidades, reglas, estados, tiempo, rutinas, combate, capacidades, misiones, economía, simulación, eventos y persistencia lógica.

**Motor gráfico/cliente:** representación visual, modelos, animaciones, cámara, interfaz, carga de recursos visuales, colisiones físicas de presentación, raycasts y comunicación de intenciones al núcleo.

Las decisiones físicas del motor gráfico pueden ayudar a detectar superficies, obstáculos o puntos de agarre, pero el resultado relevante para las reglas debe expresarse mediante contratos explícitos que el núcleo pueda validar. No introducir dependencias del dominio hacia APIs gráficas.

## 2. Exploración y orientación

**Estado: EN EVALUACIÓN**

- Explorar una interfaz sin minimapa permanente; el mapa general estaría disponible como una pantalla o pestaña independiente.
- Evitar marcadores de misión, flechas de navegación y puntos brillantes que conduzcan automáticamente al objetivo.
- Hacer que el mundo sea legible mediante hitos visuales reconocibles, siluetas, arquitectura, accidentes geográficos y referencias naturales.
- Permitir orientación mediante el sol, las estrellas, el clima y el ciclo día/noche, si esos sistemas se desarrollan.
- La información de orientación debe proceder de lo que el personaje puede conocer o percibir, no de una omnisciencia automática de la interfaz.
- Las mazmorras podrían funcionar como espacios instanciados o mapas independientes.
- Se ha propuesto usar transiciones/cinemáticas para ocultar cargas. Es una decisión de presentación, no una regla del dominio.
- Al entrar en una mazmorra, podría reducirse o suspenderse la simulación detallada del exterior mientras la mazmorra recibe el nivel de detalle principal. Antes de decidirlo hay que definir qué sucede con el tiempo global, NPC, mercados, ecosistemas, misiones temporizadas, efectos persistentes y otros procesos.

## 3. Misiones libres y objetivos adaptativos

**Estado: EN EVALUACIÓN**

- Sustituir la navegación automática por un diario de pistas, testimonios y observaciones.
- Las descripciones pueden ser espaciales y contextuales, por ejemplo: “el martillo está cerca del río, al este del molino”.
- El jugador debe interpretar la información y explorar; no se presupone una flecha ni un marcador exacto.
- Las misiones deberían modelar la intención u objetivo, las pistas conocidas, entidades y lugares relevantes, acontecimientos y posibles alternativas, en lugar de depender únicamente de una secuencia rígida de pasos.
- Los NPC pueden desplazarse, cambiar de ocupación o morir. Si un NPC relevante desaparece, la misión podría actualizar su rumbo mediante nuevas pistas, sustitutos o rutas alternativas, en vez de romperse automáticamente.
- No todas las misiones tienen que ser recuperables: consecuencias irreversibles pueden ser válidas si se diseñan intencionalmente y se comunican mediante el mundo.
- Habrá que definir qué información conoce el diario, cuándo se actualiza y cómo evitar que revele datos que el personaje todavía desconoce.

## 4. Monturas y movilidad anatómica

**Estado: EN EVALUACIÓN**

La propuesta es que las monturas ofrezcan capacidades de desplazamiento distintas según su anatomía, en lugar de ser simples vehículos intercambiables.

| Arquetipo propuesto | Capacidades de movilidad |
|---|---|
| Lobo gigante | Alta velocidad horizontal, saltos largos y rebotes en paredes; no escala superficies verticales libremente. |
| Araña o reptil trepador | Escalada vertical y desplazamiento por techos, sujeto a superficies compatibles. |
| Ave | Planeo y control de caída; no implica vuelo ilimitado. |

- La carga del equipo podría afectar el rendimiento del parkour del jugador y de la montura.
- Las monturas podrían reaccionar con miedo ante amenazas abrumadoras.
- El jugador podría intentar calmarlas, recuperar su control o desmontar.
- Se ha propuesto la muerte permanente de las monturas si caen a un abismo o mueren en combate. Es una decisión de alto impacto y sigue pendiente; habría que estudiar recuperación, sustitución, vínculo, consecuencias y posibles frustraciones.
- Las capacidades deben expresarse en el dominio como acciones/restricciones verificables. La detección concreta de superficies y la animación pertenecen a la integración con el motor gráfico.

## 5. Jefes y encuentros extraordinarios

**Estado: EN EVALUACIÓN, con algunas premisas de estructura ya discutidas.**

### 5.1 Separar categorías de encuentro

No se debe tratar “jefe especial” como sinónimo exclusivo de “coloso”. La categoría especial puede albergar varios diseños extraordinarios. Los colosos son una familia posible dentro de ella.

También hay que distinguir:
- **Jefes de progresión principal:** vinculados al avance de la campaña.
- **Jefes opcionales:** descubribles en el mundo y no obligatorios para completar la campaña.

La distribución exacta, el número total y la relación entre ambas clasificaciones siguen abiertos. El catálogo de 60 jefes utilizado en simulaciones anteriores era un escenario de cálculo, no un límite definitivo del contenido.

### 5.2 Jefes humanoides de combate técnico

- Enfrentamientos centrados en lectura de patrones, precisión, desvíos, contraataques, ruptura de postura y ejecución.
- La referencia de diseño es un duelo exigente, no una obligación de que todos los jefes compartan idénticos patrones o barras.
- Pueden tener varias barras de vitalidad y cambios de fase.
- La adaptación de IA debe conservar la identidad y los patrones reconocibles del jefe; no debería convertirse en una respuesta omnisciente o injusta al jugador.

### 5.3 Colosos: criaturas gigantes, no necesariamente humanoides

Los colosos no tienen que tener forma humana. Pueden ser bestias, monstruos, criaturas cuadrúpedas, seres alados, entidades serpentinas u otras anatomías gigantes. Su diseño debe partir de la anatomía y el comportamiento de cada criatura, no de una plantilla humanoide ampliada.

- Estarían repartidos por distintas regiones del mundo y podrían descubrirse mediante exploración.
- Se conciben como encuentros opcionales, no como bloqueos obligatorios de la campaña.
- Pueden variar enormemente en locomoción, anatomía, entorno y forma de atacar.
- El combate no se resolvería simplemente golpeando sus piernas o atacando desde el suelo. El jugador debería reconocer oportunidades para alcanzar zonas vulnerables.
- La escala del encuentro puede convertir partes del cuerpo y el terreno circundante en espacios de combate.

### 5.4 Arpeo, agarre y escalada durante el combate

Se propone una mecánica inspirada en el arpeo de Sekiro, adaptada al lenguaje propio de Mythical Exodus:

1. El coloso realiza una acción, cambia de postura, expone una extremidad o deja accesible una superficie.
2. El jugador identifica un punto de anclaje válido.
3. El arpeo permite alcanzar o recuperar contacto con esa zona, si la distancia, línea de acceso y estado del objetivo lo permiten.
4. El jugador escala, se desplaza o se aferra mientras la criatura se mueve.
5. La criatura puede sacudirse, sumergirse, rodar, golpear la zona, cambiar de orientación o intentar desprender al jugador.
6. El jugador busca una ruta hacia un punto débil y aprovecha una ventana para atacarlo.
7. El daño, la interrupción o la debilitación de una región puede alterar el comportamiento del coloso, si así se define para ese encuentro.

No todos los colosos tienen que usar el mismo arpeo ni ofrecer los mismos puntos de agarre. Una criatura sin superficies apropiadas podría requerir otra estrategia. La herramienta no debe convertir cualquier parte del cuerpo en un anclaje válido por defecto.

### 5.5 Puntos débiles y anatomía funcional

- Cada coloso podría tener puntos vulnerables con ubicación, accesibilidad, protección y función propias.
- Los puntos débiles podrían estar ocultos, protegidos por placas, cubiertos por extremidades o expuestos solo durante ciertas acciones.
- Dañar una región podría afectar capacidades concretas: locomoción, equilibrio, ataques, defensa, visión u otras funciones anatómicas.
- No debe asumirse que todos los puntos débiles son simples multiplicadores de daño; algunos pueden ser objetivos de interacción o condiciones de fase.
- Hay que diferenciar **zona anatómica**, **punto débil**, **punto de agarre** y **objetivo de fase**. Pueden coincidir, pero no son conceptos equivalentes.
- La postura podría modelarse por regiones anatómicas o subsistemas para determinados colosos. No está decidido que todos compartan una única barra global ni que todos deban tener una barra independiente por parte.
- El diseño debe impedir que el jugador pueda repetir indefinidamente una única maniobra o punto débil sin responder a los cambios del enemigo.

### 5.6 Entorno, monturas y variedad de colosos

- Algunos encuentros pueden requerir movilidad terrestre, escalada, planeo o monturas.
- Otros pueden permitir varias estrategias para evitar que una montura concreta se convierta en requisito universal.
- El terreno puede ser parte activa del combate: alturas, precipicios, agua, vegetación, estructuras y superficies de agarre.
- Los ataques del coloso pueden alterar el escenario o crear nuevas rutas y peligros.
- Las monturas podrían servir para perseguir, alcanzar, distraer o escapar, según su anatomía y las características del coloso.
- La lógica del encuentro debe funcionar sin depender de animaciones específicas; el motor gráfico representará movimientos y contactos.

### 5.7 Recompensas y restos del coloso

- Se ha propuesto que, al derrotarlo, el cuerpo permanezca como escenario o lugar de recolección.
- El despiece o recolección anatómica podría proporcionar materiales únicos vinculados a su biología.
- Deben definirse límites, riesgos, tiempo, herramientas, conservación y posibles consecuencias ecológicas antes de implementar el sistema.
- La recompensa por derrotar al jefe debe seguir siendo un único acontecimiento definitivo; fases, partes dañadas o ejecuciones intermedias no deben generar duplicados.
- Las reglas económicas existentes y las fórmulas de recompensa no se modifican por esta propuesta. Cualquier nuevo multiplicador o tabla requiere una simulación y aprobación independientes.

### 5.8 Jefe final híbrido

Se ha propuesto que el jefe final combine ambos lenguajes de combate:
- Las primeras fases serían humanoides, centradas en precisión, lectura de patrones, desvíos, postura y adaptación.
- En las últimas fases podría transformarse en una bestia gigantesca.
- La transformación debe cambiar sustancialmente el espacio, las amenazas y las decisiones del jugador; no ser solo un aumento de tamaño, vitalidad o daño.
- La fase de bestia podría incorporar puntos débiles anatómicos, movilidad, agarre, escalada o interacción con el entorno.
- Esta estructura permitiría que el enfrentamiento final ponga a prueba habilidades aprendidas durante la campaña y, opcionalmente, en encuentros con colosos.
- No se ha definido todavía la forma, anatomía, número de fases, barras, vulnerabilidades ni requisitos concretos del jefe final.

## 6. Consecuencias para el motor lógico

**Estado: orientación arquitectónica; los contratos concretos siguen pendientes.**

Las ideas anteriores sugieren futuras capacidades del dominio, pero no justifican implementar ahora sistemas especializados sin requisitos cerrados.

Áreas que probablemente necesitarán modelado:
- Regiones del mundo, localizaciones, proximidad y niveles de fidelidad de simulación.
- Capacidades de movimiento e interacción condicionadas por anatomía, superficie, carga y estado.
- Monturas como entidades con capacidades, necesidades y estados propios.
- Combate con objetivos anatómicos, puntos de agarre, puntos débiles y estados por región.
- Misiones basadas en objetivos, pistas, acontecimientos, conocimiento y alternativas.
- Rutinas de NPC y consecuencias de su muerte, desplazamiento o cambio de actividad.
- Tiempo global y políticas explícitas para simulación dentro/fuera de mazmorras.
- Persistencia de cambios de mundo, encuentros, NPC, monturas y misiones.

### Límites entre dominio y presentación

El núcleo debe decidir si una acción es válida y qué consecuencias lógicas produce. El motor gráfico puede detectar superficies candidatas mediante raycasts y física cinemática, pero debe comunicar resultados mediante datos/contratos independientes del motor.

Ideas de presentación propuestas:
- World streaming por celdas.
- Raycasts para detectar superficies de parkour, anclajes y puntos de interacción.
- Física cinemática en lugar de simular toda la locomoción con física realista.
- Niveles de detalle de IA: lógica abstracta para entidades lejanas y comportamiento detallado cerca del jugador.
- Object pooling para modelos y objetos visuales.

Estas son decisiones del futuro cliente gráfico y no deben incorporarse como dependencias del núcleo lógico.

## 7. Riesgos y preguntas que requieren diseño

Antes de implementar estos sistemas será necesario resolver, entre otras, las siguientes cuestiones:

1. ¿Qué acontecimientos deben simularse siempre aunque una región esté lejos o una mazmorra esté activa?
2. ¿Cómo se avanza el tiempo global al entrar y salir de una instancia?
3. ¿Qué datos de una misión puede conocer el diario y qué eventos provocan una actualización?
4. ¿Cómo se evita que la muerte o desaparición de un NPC rompa cadenas de misiones sin eliminar consecuencias?
5. ¿Qué superficies, distancias, estados y restricciones hacen válido un arpeo?
6. ¿Cómo se representa el agarre del jugador mientras la criatura se mueve, gira o cambia de fase?
7. ¿Qué reglas separan daño a una zona, daño a un punto débil y daño a la vitalidad global?
8. ¿Qué consecuencias tienen las heridas anatómicas sobre la IA y la locomoción del coloso?
9. ¿Qué alternativas existen si el jugador no tiene una montura o capacidad de movilidad determinada?
10. ¿Cómo se comunica el peligro y la opcionalidad de un coloso sin marcadores intrusivos?
11. ¿Qué consecuencias jugables tiene la muerte permanente de una montura y cómo se evita una pérdida injusta?
12. ¿Qué partes del cuerpo pueden recolectarse y cómo se evita la explotación o duplicación de materiales?
13. ¿Qué mecánicas del combate humanoide siguen presentes durante la transformación del jefe final?

## 8. Relación con decisiones y documentos existentes

- **GDD baseline:** permanece como referencia funcional inicial. Este documento no lo reemplaza ni reescribe.
- **Auditoría arquitectónica:** las propuestas ayudan a priorizar futuras áreas de revisión (entidades, simulación, acciones, efectos, eventos y persistencia), pero no alteran sus contratos actuales.
- **Modelo de recompensas de jefes:** no se cambian fórmulas, multiplicadores, cantidades ni reglas de acreditación.
- **New Game Plus:** estas ideas no fijan reglas de reinicio, escalado o recompensas de NG+.
- **Código del dominio:** no se añaden entidades, interfaces, enums o algoritmos de colosos, monturas, misiones adaptativas o streaming por el mero hecho de documentar estas posibilidades.

## 9. Estados de decisión

Para evitar confusiones, toda propuesta futura debe etiquetarse con uno de estos estados:

- **APROBADA:** decisión funcional aceptada y lista para convertirse en requisito, aunque todavía pueda necesitar diseño técnico.
- **EN EVALUACIÓN:** idea válida que requiere discusión, comparación o resolución de preguntas.
- **EXPERIMENTAL:** hipótesis que debe probarse mediante prototipo o simulación antes de decidir.
- **DESCARTADA:** idea que no se incorporará, conservada aquí solo si es útil registrar el motivo.

En esta versión, las propuestas nuevas de este documento se consideran **EN EVALUACIÓN**, salvo las precisiones que el usuario haya confirmado explícitamente durante la conversación. La clasificación no constituye aprobación automática de todos los detalles derivados.

## 10. Orden sugerido para futuras decisiones

1. Definir el modelo de mundo, regiones y niveles de simulación.
2. Definir capacidades de movimiento e interacción, incluyendo agarre y escalada.
3. Definir el modelo general de encuentros y distinguir jefes principales, opcionales y especiales.
4. Diseñar un encuentro piloto de coloso no humanoide para validar anatomía, agarre, puntos débiles y fases.
5. Definir el modelo de NPC, conocimiento y misiones adaptativas.
6. Definir persistencia y tiempo entre mundo exterior e instancias.
7. Solo después, convertir decisiones estables en contratos del núcleo y pruebas automatizadas.

**Principio rector:** conservar la ambición sistémica del mundo sin anticipar abstracciones, fórmulas o infraestructura antes de que las reglas de juego estén suficientemente definidas.


## 11. Reputación territorial, memoria histórica y efecto leyenda

**Estado: AMPLIACIÓN DE UN SISTEMA PREVISTO; reglas y magnitudes concretas EN EVALUACIÓN.**

Esta propuesta desarrolla el Sistema de Reputación Territorial y la Memoria Histórica de las comunidades contemplados en el diseño de Mythical Exodus. La intención es que las acciones del jugador dejen consecuencias sociales persistentes: el mundo no solo registra que un jefe fue derrotado, sino que distintas comunidades interpretan ese acontecimiento, lo recuerdan y reaccionan de acuerdo con sus intereses, valores y relaciones.

El objetivo de diseño es el **efecto leyenda**: las hazañas deben seguir teniendo significado después de terminar el combate, cuando el jugador regresa a asentamientos, visita talleres, conversa con NPC o se encuentra con miembros de facciones afectadas.

### 11.1 El mundo recuerda las hazañas

Una victoria importante puede producir consecuencias que se manifiesten de distintas formas:

- **Reconocimiento de NPC:** habitantes y personajes que conozcan la hazaña pueden reconocer al jugador, cambiar su trato, mostrar gratitud, respeto, temor, resentimiento o desconfianza.
- **Reacciones de comerciantes y profesionales:** algunos podrían ofrecer mejores condiciones, acceso a servicios, encargos o mercancías; otros podrían negarse a comerciar o exigir precios más altos. Los descuentos no deben ser automáticos ni universales: dependerán de la relación, la cultura, el interés y la política de cada comerciante.
- **Rumores, relatos y trovadores:** tabernas, viajeros y comunidades pueden transmitir versiones de acontecimientos importantes. La historia puede deformarse, exagerarse, fragmentarse o tardar en llegar a regiones lejanas.
- **Diálogos evolutivos:** un NPC que antes subestimaba al jugador podría pedirle consejo, disculparse o tratarlo con deferencia. Otro podría sentir envidia, miedo o rechazo.
- **Ceremonias y cambios visibles:** una comunidad agradecida podría decorar calles, organizar una celebración, ofrecer escolta o convocar al jugador ante sus autoridades. Son ejemplos de respuestas posibles, no recompensas garantizadas por cada jefe.
- **Memoria personal y comunitaria:** el diario podría registrar títulos, acontecimientos y consecuencias relevantes, diferenciando lo que el jugador presenció de lo que otros cuentan.

La reacción debe guardar proporción con la importancia del acontecimiento y con el conocimiento real que cada personaje o comunidad tenga de él. No todo NPC debería reconocer instantáneamente al jugador en cualquier lugar.

### 11.2 Reputación, notoriedad y alerta son dimensiones distintas

No conviene reducir todas las reacciones a una sola puntuación global. Como modelo conceptual, separar:

1. **Reputación:** valoración que una comunidad, territorio o facción tiene del jugador. Puede ser positiva, negativa o neutral.
2. **Notoriedad:** cuánto se conoce la identidad o las hazañas del jugador dentro de una red de comunicación. Una persona puede ser famosa sin ser querida.
3. **Alerta o amenaza percibida:** cuánto considera una facción que el jugador representa un peligro activo. Puede aumentar por acciones hostiles, aunque la reputación con otras comunidades sea positiva.

Estas dimensiones permitirían situaciones como:
- Una aldea considera héroe al jugador por liberar a sus habitantes.
- La facción del tirano derrotado lo considera enemigo y ofrece una recompensa por capturarlo.
- Una comunidad distante todavía no sabe quién es, aunque la reputación local sea muy alta.
- Un grupo enemigo conoce su identidad y sus tácticas, pero no necesariamente comparte información perfecta con todos sus miembros.

Los nombres, escalas, límites, umbrales y fórmulas de estas dimensiones quedan pendientes.

### 11.3 Relaciones entre territorios y facciones como grafo

Se propone representar las relaciones sociales y políticas mediante un **grafo de relaciones**, en lugar de asumir que cada facción existe de forma aislada.

- Los nodos pueden representar comunidades, territorios, organizaciones, facciones, asentamientos o instituciones, según el modelo que se defina.
- Las conexiones expresan relaciones como alianza, rivalidad, subordinación, dependencia, conflicto, comercio o afinidad cultural.
- Una acción puede afectar directamente a varios nodos y, dependiendo de las reglas de transmisión, producir consecuencias indirectas en otros.
- Las relaciones pueden ser dirigidas y asimétricas: que A considere aliado a B no obliga a que B tenga la misma valoración de A.
- El grafo no debe propagar automáticamente cada cambio a todo el mundo. La información, la influencia y las consecuencias políticas necesitan reglas explícitas de alcance, tiempo, confianza y canales de comunicación.

Ejemplo conceptual: derrotar a un jefe que oprimía una región podría mejorar la reputación con la comunidad liberada, empeorarla con la facción a la que pertenecía el jefe y modificar la posición de organizaciones aliadas o rivales. Los valores numéricos concretos no están fijados.

### 11.4 Acciones históricas y consecuencias de doble filo

Los acontecimientos importantes deben interpretarse según el contexto y los intereses de cada grupo.

- Derrotar a un tirano puede ser celebrado por sus víctimas y considerado un asesinato o una amenaza por sus seguidores.
- Destruir una criatura que aterrorizaba una región puede generar gratitud, pero también preocupación si esa criatura mantenía bajo control otra amenaza.
- Ayudar a una facción puede perjudicar las relaciones con sus rivales.
- Actuar con crueldad, saquear, atacar civiles o incumplir acuerdos podría deteriorar reputación y aumentar la alerta.
- Una misma hazaña puede generar consecuencias positivas y negativas simultáneamente.

El sistema debe registrar el acontecimiento y aplicar reglas de interpretación configurables por comunidad/facción. No debe codificar una moral universal que convierta automáticamente toda victoria en una recompensa positiva.

### 11.5 Reacción de NPC y enemigos

La reputación y la alerta pueden influir en el comportamiento, siempre dentro de lo que cada entidad sabe:

- NPC amistosos pueden ofrecer diálogos, servicios, encargos o acceso a espacios antes restringidos.
- Comerciantes pueden modificar sus condiciones comerciales según su relación, sus recursos y sus intereses.
- Guardias pueden saludar, escoltar, vigilar, interrogar o impedir el acceso.
- Enemigos de una facción hostil pueden reconocer al jugador, prepararse, pedir refuerzos, organizar emboscadas o intentar evitar el enfrentamiento.
- Algunos enemigos pueden huir por miedo o por una evaluación táctica desfavorable; otros pueden buscar gloria enfrentándose al jugador.
- El conocimiento de las tácticas del jugador puede influir en la preparación enemiga, pero no debe convertirse en omnisciencia ni en adaptación injusta. La transmisión de información y la capacidad de aprender requieren límites.

La reacción concreta dependerá de personalidad, rol, facción, situación, fuerza relativa, contexto y conocimiento disponible. Una reputación alta no obliga a que todos los miembros de una facción se comporten igual.

### 11.6 Arquitectura lógica orientada a acontecimientos

Se propone estudiar una arquitectura basada en acontecimientos del dominio para conectar combate, reputación, memoria, NPC, diálogos, comercio y mundo.

Flujo conceptual:

1. El sistema de combate resuelve la derrota definitiva de un jefe.
2. Se registra un acontecimiento con identidad del jefe, participantes, lugar, momento lógico y resultado.
3. Los sistemas interesados reciben el acontecimiento mediante un mecanismo explícito de publicación/consumo.
4. El sistema de reputación determina qué comunidades y facciones resultan afectadas y aplica las reglas correspondientes.
5. La memoria histórica registra el hecho y las interpretaciones conocidas.
6. Los sistemas de NPC, diálogo, comercio, guardias y encuentros consultan el estado social pertinente cuando toman decisiones.
7. La propagación de rumores puede ocurrir posteriormente, según rutas, testigos, viajeros u otros canales definidos.

Un nombre como BossDefeated puede servir para identificar el tipo de acontecimiento, pero no se debe asumir que basta con emitir un evento genérico OnBossDefeated(BossId). Para que el sistema sea fiable, el contrato probablemente necesitará contexto suficiente y una identidad única del acontecimiento.

**Separación de responsabilidades:**
- Combate determina y registra el resultado definitivo del encuentro.
- Reputación interpreta las consecuencias sociales.
- Memoria histórica conserva hechos y versiones conocidas.
- Comunicación/rumores determina quién llega a conocerlos y cuándo.
- NPC y facciones reaccionan a su propio estado, conocimiento y contexto.
- Diálogo y presentación muestran esas consecuencias sin ser dueños de las reglas sociales.

No se propone introducir event sourcing ni un bus de eventos complejo por adelantado. Primero deben definirse los contratos y garantías necesarios; una publicación sencilla de eventos de dominio puede ser suficiente.

### 11.7 Consistencia, persistencia y prevención de explotación

- La derrota definitiva de un jefe debe producir una sola consecuencia histórica y una sola aplicación de reputación por acontecimiento.
- Recargar una partida, abandonar y volver a entrar en una región o repetir una transición no debe duplicar el cambio.
- El acontecimiento y su procesamiento deben poder persistirse o reconstruirse de forma consistente.
- Si una consecuencia afecta a varios nodos del grafo, debe evitarse que una interrupción deje el mundo en un estado parcialmente actualizado sin posibilidad de recuperación.
- Los rumores no deben otorgar conocimiento instantáneo: se debe registrar quién sabe qué, desde cuándo y por qué canal, cuando ese nivel de detalle sea necesario.
- Las consecuencias económicas derivadas de reputación deben integrarse con la economía ya diseñada y evitar descuentos infinitos, precios explotables o generación indirecta de riqueza sin control.
- Los cambios de reputación no deben conceder automáticamente equipo legendario, objetos gratuitos o títulos; esas recompensas requieren condiciones y contenido explícitos.

### 11.8 Ejemplo ilustrativo de una secuencia

El siguiente ejemplo es narrativo y no define valores oficiales:

- El jugador derrota a un coloso que amenazaba un bosque y a una aldea cercana.
- La comunidad afectada registra el acontecimiento y mejora su valoración del jugador.
- La facción que veneraba o controlaba al coloso interpreta la derrota como una agresión y aumenta su hostilidad.
- Los habitantes que presenciaron el combate comienzan a contar lo ocurrido.
- Con el paso del tiempo, un viajero lleva la noticia a otro asentamiento.
- Al regresar, el jugador puede encontrar reconocimiento, nuevas opciones de diálogo y una celebración local.
- En territorio de la facción enemiga, sus patrullas podrían reconocerlo, vigilarlo o preparar una emboscada si han recibido información suficiente.

La consola headless podría mostrar una traza de diagnóstico semejante a esta:

~~~text
> resolve boss_encounter colossus_forest
[COMBAT] Encounter resolved: victory
[HISTORY] Event recorded: BossDefeated / colossus_forest
[REPUTATION] Forest village: improved
[REPUTATION] Colossus cult: worsened
[ALERT] Colossus cult: threat assessment increased
> enter settlement forest_village
[NPC] Blacksmith recognizes the player
[DIALOGUE] Contextual dialogue set selected
~~~

La traza representa resultados hipotéticos; no son comandos implementados ni valores acordados.

### 11.9 Preguntas pendientes

1. ¿La reputación se calcula por asentamiento, territorio, comunidad, facción o una combinación jerárquica?
2. ¿Qué relaciones forman el grafo y cuáles son sus reglas de dirección, intensidad y actualización?
3. ¿Cómo se distinguen reputación, notoriedad, alerta y conocimiento individual?
4. ¿Qué acontecimientos generan cambios y cuáles requieren testigos, pruebas o confirmación?
5. ¿Cómo viajan los rumores y qué alcance tienen según distancia, rutas y relaciones?
6. ¿Qué umbrales habilitan diálogos, servicios, acceso, recompensas o respuestas hostiles?
7. ¿Cómo se equilibran precios, descuentos, impuestos, recompensas y sanciones?
8. ¿Qué información táctica puede aprender una facción sobre el jugador y cuánto tarda en transmitirla?
9. ¿Cómo se comportan facciones fragmentadas o NPC que discrepan de la postura oficial?
10. ¿Cómo se persisten y procesan acontecimientos para evitar duplicados y estados inconsistentes?

### 11.10 Relación con sistemas existentes

- Esta sección amplía las ideas de reputación territorial, memoria histórica, NPC dinámicos, economía y simulación social.
- No establece todavía valores como +50 o -30; cualquier cifra del ejemplo original debe tratarse como ilustrativa.
- No convierte todos los acontecimientos de combate en cambios de reputación. Cada tipo de acontecimiento necesita reglas de interpretación.
- No cambia las fórmulas de almas, recompensas de jefes, precios base ni progresión. Los efectos económicos de la reputación deberán diseñarse junto con el sistema económico.
- No exige un grafo global complejo, event sourcing ni una infraestructura de mensajería antes de demostrar que son necesarios.
- Las ideas permanecen separadas del GDD baseline y del código hasta que se aprueben sus reglas funcionales.

## 12. Eventos del dominio, facciones, entidades reactivas y misiones

**Estado: PROPUESTA ARQUITECTÓNICA EN EVALUACIÓN.** Esta sección recoge una posible organización de los sistemas descritos para conectar combate, reputación, NPC y misiones. No declara implementado un bus de eventos, un grafo de facciones, máquinas de estados ni un sistema de misiones.

### 12.1 Acontecimientos y comunicación entre subsistemas

Se propone que los sistemas publiquen acontecimientos de dominio que puedan interesar a otros subsistemas, evitando que el código de combate conozca directamente las implementaciones de reputación, misiones, diálogo o IA.

Ejemplo conceptual: al resolverse definitivamente la derrota de un jefe, el dominio registra un acontecimiento con identidad del encuentro, participantes, lugar, instante lógico y resultado. Los consumidores que correspondan podrían reaccionar:

- Misiones: comprobar si el acontecimiento satisface algún objetivo activo.
- Reputación e historia: interpretar el hecho según las comunidades y facciones afectadas.
- NPC y diálogo: actualizar reconocimiento, conocimiento o contexto conversacional cuando la información haya llegado hasta ellos.
- Mundo y encuentros: actualizar consecuencias persistentes relacionadas con el jefe derrotado.

El emisor no debe necesitar conocer quién consume el acontecimiento. Sin embargo, esto no obliga a introducir desde el primer momento un Event Bus global o una infraestructura compleja. La primera implementación podría usar contratos y publicación explícita dentro de un límite bien definido. La necesidad de colas, prioridades, reintentos o mensajería más elaborada debe demostrarse con casos reales.

**Invariantes por diseñar:**
- Distinguir comandos/intenciones de acontecimientos ya ocurridos.
- Definir identidad, orden y momento de publicación de cada acontecimiento.
- Evitar procesar dos veces consecuencias irreversibles por recarga, reingreso o repetición de una operación.
- Controlar reentrancia, ciclos de reacciones y cadenas de acontecimientos.
- Determinar qué sucede si un consumidor falla después de que el acontecimiento ya fue registrado.
- Mantener el dominio independiente de infraestructura externa y del motor gráfico.

No se adopta todavía un patrón Event Sourcing ni se prescribe una librería de Event Bus.

### 12.2 Grafo de facciones, comunidades y hostilidad

La propuesta amplía la sección 11: las relaciones entre facciones, comunidades, territorios y asentamientos podrían representarse mediante un grafo de relaciones explícitas, posiblemente dirigidas y asimétricas.

El grafo no debe reducir toda relación social a una única cifra. Conviene mantener separados, cuando el diseño lo requiera:

- Reputación o valoración del jugador por una comunidad/facción.
- Hostilidad o disposición a actuar contra el jugador.
- Notoriedad: cuánto se conoce su identidad o sus acciones.
- Conocimiento: qué hechos concretos conoce cada actor y por qué medios.
- Relaciones entre las propias organizaciones: alianza, rivalidad, subordinación, dependencia o comercio.

Una facción podría considerar enemigo al jugador sin que todas las comunidades del mundo compartan esa valoración. Las reacciones deben depender de relaciones, información disponible, autoridad local y contexto.

La entrada en una región podría provocar una evaluación de las relaciones pertinentes y desencadenar respuestas como vigilancia, advertencias, rechazo de servicios, cierre de comercios, diálogo contextual o combate. Estas respuestas son posibilidades, no consecuencias universales ni automáticas.

**No están aprobados** un umbral fijo como reputación inferior a -50, un estado global binario de “zona hostil”, ni la propagación inmediata de hostilidad a todos los guardias y comerciantes. Los umbrales, alcance, excepciones, tiempos de reacción y condiciones de conocimiento requieren diseño y pruebas.

### 12.3 Entidades y máquinas de estados

Se propone modelar las entidades del mundo mediante contratos comunes solo cuando compartan invariantes reales, y especializar sus capacidades y comportamiento según el tipo de entidad.

Posibles familias de comportamiento:

- NPC: rutinas, necesidades, desplazamiento, interacción, conocimiento, diálogo y reacción social.
- Enemigos comunes: patrulla, percepción, investigación, combate, retirada u otros comportamientos definidos por su diseño.
- Jefes: patrones de combate, fases, transiciones, vulnerabilidades, adaptación y reglas particulares del encuentro.
- Colosos: estados y capacidades específicos de su anatomía, regiones funcionales, agarre, desplazamiento y objetivos de fase, según el encuentro.

Las máquinas de estados son una herramienta candidata, no una obligación de que toda entidad use una única State Machine. Algunos comportamientos podrían requerir estados jerárquicos, capacidades compuestas, decisiones basadas en utilidad o simples reglas explícitas. Se debe elegir la solución más sencilla que represente el comportamiento sin crear estados combinatorios inmanejables.

Tampoco se decide que NPC, mobs y jefes deban heredar todos de una clase concreta como `Character`. La composición y los contratos pequeños deben evaluarse frente a la herencia, evitando una clase base que acumule responsabilidades incompatibles.

Los estados concretos mencionados en la propuesta original —por ejemplo, patrulla, investigación, huida, transición de fase o vulnerabilidad anatómica— son ejemplos ilustrativos. Su vocabulario definitivo dependerá de los comportamientos aprobados.

### 12.4 Misiones, objetivos y diario

Se propone representar una misión mediante datos y reglas que permitan evaluar:

- Requisitos de activación y progreso.
- Objetivos y condiciones de cumplimiento.
- Acontecimientos relevantes.
- Recompensas y consecuencias.
- Pistas, testimonios y conocimiento disponible para el jugador.
- Rutas alternativas y condiciones de resolución, fracaso o cierre.

El diario no debería ser una simple lista de textos estáticos ni conocer acontecimientos que el personaje todavía no ha descubierto. Debe presentar información derivada del estado de la misión y del conocimiento que el jugador posee.

Los acontecimientos de NPC —muerte, desaparición, traslado o cambio de ocupación— podrían modificar una misión si existe una regla que relacione ese acontecimiento con sus objetivos. La misión podría continuar por una ruta alternativa, quedar bloqueada, resolverse de otra manera o fracasar. Ninguna de estas consecuencias debe aplicarse automáticamente a todas las misiones.

La muerte persistente de un NPC no implica por sí sola que el jugador pueda encontrar siempre su cadáver ni que el cadáver revele información suficiente; eso depende de la persistencia, localización y reglas de conocimiento que se diseñen.

### 12.5 Ejemplo de integración, no implementación

Secuencia hipotética:

1. El combate confirma la derrota definitiva de un jefe.
2. Se registra un acontecimiento único.
3. Las misiones interesadas evalúan si alguno de sus objetivos se ha cumplido.
4. El sistema histórico conserva el hecho y su contexto.
5. Reputación interpreta las consecuencias para las comunidades relacionadas.
6. Los rumores transmiten la noticia por canales y tiempos plausibles.
7. NPC y facciones reaccionan solo cuando disponen de información suficiente.
8. El diario muestra las pistas y cambios que el personaje puede conocer.

Esta secuencia ilustra la cooperación entre sistemas; no exige que todos se ejecuten de forma síncrona ni que exista un bus global.

### 12.6 Decisiones pendientes y límites de implementación

Antes de convertir esta propuesta en código deben definirse, como mínimo:

1. Catálogo de acontecimientos de dominio y sus datos obligatorios.
2. Límites de publicación y consumo, orden, reentrancia e idempotencia.
3. Si basta una publicación local explícita o existe una necesidad real de Event Bus.
4. Tipos de nodos y relaciones del grafo, direccionalidad y reglas de propagación.
5. Diferencia funcional entre reputación, hostilidad, notoriedad y conocimiento.
6. Umbrales y respuestas concretas de guardias, comerciantes, NPC y facciones.
7. Capacidades compartidas por entidades y límites de especialización.
8. Comportamientos que realmente necesitan máquinas de estados y su granularidad.
9. Modelo de misión, objetivos, requisitos, recompensas, fracaso y alternativas.
10. Reglas de conocimiento que determinan qué puede mostrar el diario.
11. Persistencia y recuperación ante fallos para acontecimientos y consecuencias.
12. Pruebas de integración que demuestren que una consecuencia no se duplica ni se propaga a actores desinformados.

**Relación con el roadmap técnico:** estos conceptos no alteran el orden inmediato de construir estado de personaje y combate sobre los contratos existentes. Los acontecimientos pueden surgir primero como resultados explícitos de dominio; la arquitectura de publicación se refinará cuando haya más de un consumidor real. Reputación, IA y misiones se implementarán después de definir sus contratos y reglas. No se añade código por registrar esta propuesta.
