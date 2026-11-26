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
