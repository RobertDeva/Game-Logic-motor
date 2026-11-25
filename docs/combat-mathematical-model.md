# Mythical Exodus — modelo matemático del combate

**Estado:** especificación fundacional en desarrollo  
**Versión:** 0.1  
**Propósito:** definir un modelo matemático determinista, extensible y comprobable para resolver daño, defensa, postura, estados y sus interacciones en el motor lógico independiente.

Este documento desarrolla las reglas matemáticas. No sustituye las decisiones funcionales del GDD: cuando una regla confirmada del GDD y una fórmula propuesta parezcan entrar en conflicto, prevalece la regla funcional y la fórmula debe adaptarse.

## 1. Principios del modelo

1. **Una sola resolución común:** jugador, enemigos y jefes utilizan las mismas reglas fundamentales. Las diferencias se expresan mediante datos, capacidades, inmunidades y reglas explícitas, no mediante excepciones ocultas por facción.
2. **Canales separados:** Vitalidad, Postura, estados y durabilidad son magnitudes distintas. Una acción puede afectar a una o varias, pero nunca se deduce una de otra salvo que una regla lo indique.
3. **Composición por componentes:** un ataque puede contener varios componentes de daño (físico, fuego, hielo, energía, etc.). Cada componente se resuelve contra sus defensas y afinidades correspondientes.
4. **Determinismo:** con el mismo estado inicial, acción, configuración y semilla aleatoria, la simulación produce el mismo resultado. Las tiradas aleatorias, si existen, se realizan mediante un generador inyectado y reproducible.
5. **Datos antes que código:** armas, ataques, armaduras, enemigos, estados, afinidades y curvas se configuran como datos. El motor ejecuta reglas genéricas.
6. **Sin doble aplicación:** cada modificador declara a qué canal y etapa afecta. Una bonificación no puede aplicarse simultáneamente como daño base y como multiplicador salvo que se configure expresamente.
7. **Balance separado de la regla:** las ecuaciones definen el comportamiento; sus parámetros numéricos se calibran mediante simulación y pruebas. No se fijan cifras arbitrarias sin datos de juego.

## 2. Convenciones, unidades y precisión

- Estadísticas del personaje: enteros de 1 a 99, según las reglas de progresión ya definidas.
- Vitalidad y Postura: puntos internos no negativos; la interfaz puede mostrarlos como barras o porcentajes.
- Daño, defensa y presión de postura: magnitudes no negativas en unidades internas de combate.
- Probabilidades: valores normalizados entre 0 y 1.
- Multiplicadores: valores reales no negativos; 1 representa neutralidad.
- Duraciones: segundos de simulación o ticks de combate, pero el motor deberá escoger una única unidad temporal canónica antes de implementar ventanas de reacción.
- Los cálculos intermedios conservan precisión decimal. El redondeo se realiza una sola vez al aplicar el resultado al estado persistente, con una regla global y estable (recomendación: redondeo al entero más cercano, mitades alejándose de cero).
- Se prohíben NaN, infinito, divisiones por cero y valores negativos inválidos. Los datos de contenido se validan al cargarse.

## 3. Estadísticas y sus responsabilidades

Estadísticas existentes: Vitalidad, Fuerza, Agilidad, Resistencia, Voluntad, Ruptura, Destreza y Armonía.

| Estadística | Responsabilidad matemática |
|---|---|
| Vitalidad | Capacidad máxima de Vitalidad. No aumenta directamente el daño. |
| Fuerza | Escalado de armas y acciones físicas que declaren afinidad con Fuerza. |
| Agilidad | Capacidades de movimiento y, cuando una acción/equipo lo declare, parámetros de movilidad o recuperación. No se convierte automáticamente en daño. |
| Resistencia | Mitigación/resistencia general y capacidad de soportar presión, distribuida por tipos de daño mediante perfiles configurables. |
| Voluntad | Aumenta todo tipo de daño y reduce la duración de los estados negativos que afectan al personaje. |
| Ruptura | Estadística candidata para probabilidad/eficacia crítica y evasión, según la descripción manuscrita original; su papel exacto debe mantenerse como provisional hasta validar el alcance de esa lectura. |
| Destreza | Escalado del daño de Artes de Combate. También puede alimentar requisitos o parámetros técnicos si el contenido lo declara. |
| Armonía | Escalado del daño especial y de las acciones que declaren afinidad con energía. |

Las estadísticas no deben tener efectos secundarios implícitos. Cada efecto debe aparecer en esta tabla, en una regla derivada o en los datos de una acción/equipo.

### 3.1 Conversión de estadísticas a contribuciones

Se propone una función común de escalado con rendimientos decrecientes y parámetros configurables:

\[
S(x; c_1,c_2,r_1,r_2)=
\begin{cases}
x & x\le c_1\\
c_1+r_1(x-c_1) & c_1<x\le c_2\\
c_1+r_1(c_2-c_1)+r_2(x-c_2) & x>c_2
\end{cases}
\]

donde (c_1,c_2) son puntos de cambio y (1\ge r_1\ge r_2\ge0). Los puntos y rendimientos se calibrarán; no se codificarán como constantes dispersas.

Para una acción con escalados declarados:

\[
A_{escala}=\sum_i w_i\,S(stat_i)
\]

Los coeficientes (w_i) pertenecen al arma/acción. Una estadística no listada tiene coeficiente cero.

## 4. Vitalidad máxima y recursos

La Vitalidad máxima se deriva de una base de personaje y de la estadística Vitalidad:

\[
HP_{max}=HP_{base}+G_{HP}(S(Vitalidad))
\]

La función (G_{HP}) se configurará como curva monótona creciente, con rendimientos decrecientes si las pruebas lo justifican. El nivel/clase puede determinar la base inicial; no se debe sumar dos veces el valor inicial de clase.

Aura, Maná y Prana son recursos energéticos diferenciados. Sus reservas máximas, regeneración y costes se resolverán en el subsistema de energías. El daño de una habilidad que los consuma utiliza el pipeline de daño de este documento, pero el coste de energía no se confunde con daño.

## 5. Modelo de ataque y potencia ofensiva

Cada acción ofensiva define uno o más componentes de daño. Cada componente incluye, como mínimo:
- tipo de daño;
- potencia base;
- coeficientes de escalado;
- multiplicador propio de movimiento/ataque;
- etiquetas de origen (arma, Arte, energía, entorno, estado);
- reglas de objetivo, aliados, inmunidades y propagación;
- canal de postura, si lo produce;
- efectos secundarios y condiciones de aplicación.

Para un componente (k):

\[
P_k=(B_k+A_{escala,k})\cdot M_{mov,k}\cdot M_{habilidad,k}\cdot M_{condición,k}
\]

- (B_k): potencia base del componente.
- (A_{escala,k}): contribución de estadísticas declaradas.
- (M_{mov,k}): coeficiente del movimiento o ataque concreto.
- (M_{habilidad,k}): coeficiente del Arte/habilidad.
- (M_{condición,k}): condición del arma/equipo y otros factores directos del origen.

El contenido deberá definir si la potencia base ya incluye el valor intrínseco del arma. La implementación elegirá una única convención para evitar sumar el ataque del arma dos veces.

### 5.1 Voluntad y bonificaciones ofensivas

Voluntad aumenta todos los tipos de daño. Se aplicará como una contribución ofensiva común y explícita:

\[
M_{vol}=1+F_{vol}(S(Voluntad))
\]

La función (F_{vol}) será monótona y de crecimiento controlado. Se aplica a componentes físicos y especiales, pero no al daño de postura salvo que una acción lo declare expresamente. Los efectos que aumenten daño se clasificarán como aditivos o multiplicativos; no se mezclan sin una etapa definida.

### 5.2 Críticos

La probabilidad crítica se calcula desde Ruptura y modificadores explícitos:

\[
p_{crit}=clamp(F_{crit}(S(Ruptura))+\sum b_{crit},p_{min},p_{max})
\]

Si ocurre crítico:

\[
P_{crit}=P\cdot M_{crit}
\]

El crítico modifica Vitalidad por defecto. Su interacción con postura, estados, daño ambiental y ejecuciones debe ser declarada por la acción; no se hereda automáticamente. Los límites evitan que la probabilidad alcance certeza por acumulación ilimitada.

## 6. Defensa y resolución de daño a Vitalidad

La defensa se separa en:
- **defensa plana**, asociada principalmente a armadura/material y específica por tipo;
- **resistencia porcentual**, asociada a Resistencia, equipo, estados, afinidades y efectos;
- **vulnerabilidad**, que puede aumentar el daño recibido;
- **inmunidad**, que bloquea un tipo o efecto de manera explícita.

Para cada componente (k):

\[
D_{plano,k}=\max(0,P_k-DEF_k)
\]

\[
R_k=clamp(R_{base,k}+R_{equipo,k}+R_{estado,k},R_{min},R_{max})
\]

\[
D_k=D_{plano,k}\cdot(1-R_k)\cdot M_{vulnerabilidad,k}
\]

\[
D_{total}=\sum_k D_k
\]

La defensa plana se aplica antes de la resistencia porcentual. Los límites de resistencia impiden que el daño ordinario se vuelva negativo o que una acumulación accidental produzca invulnerabilidad. La inmunidad es una regla explícita independiente, no una resistencia porcentual extrema.

Resistencia del personaje contribuye mediante una curva configurable por tipo:

\[
R_{base,k}=F_{res,k}(S(Resistencia))
\]

No se asumirá que todos los tipos de daño reciben exactamente la misma protección: cada tipo puede tener coeficiente distinto, manteniendo una contribución general de Resistencia.

El daño final aplicado no puede superar la Vitalidad actual del objetivo:

\[
HP' = \max(0,HP-D_{total})
\]

La cantidad efectiva aplicada es (HP-HP'). Esta diferencia es la que deben consumir efectos que dependan del daño realmente recibido, salvo que su definición indique otra cosa.

## 7. Afinidades entre energías y tipos especiales

Las energías básicas son Aura, Maná y Prana; las fusiones confirmadas son Energía Natural, Poder Astral y Energía Abisal. El GDD establece que la dirección de cada flecha del diagrama de afinidades es significativa y que el origen de la flecha tiene ventaja sobre el destino.

### 7.1 Transcripción del diagrama original

La imagen manuscrita establece seis nodos: Aura, Maná, Prana, Energía Natural, Energía Abisal y Poder Astral. Se conserva la convención del diagrama: **la flecha sale de la energía con ventaja y apunta hacia la energía que recibe la desventaja**.

Relaciones dirigidas identificadas:

| Origen (ventaja) | Destino (desventaja) | Representación en el dibujo |
|---|---|---|
| Energía Natural | Aura | Flecha horizontal hacia Aura |
| Energía Abisal | Aura | Flecha horizontal hacia Aura |
| Energía Natural | Maná | Flecha descendente |
| Energía Abisal | Prana | Flecha descendente |
| Energía Natural | Prana | Diagonal descendente hacia Prana |
| Energía Abisal | Maná | Diagonal descendente hacia Maná |
| Aura | Maná | Diagonal descendente hacia Maná |
| Prana | Aura | Diagonal ascendente hacia Aura |
| Maná | Prana | Flecha horizontal hacia Prana |
| Poder Astral | Aura | Flecha vertical ascendente hacia Aura |
| Poder Astral | Maná | Diagonal ascendente hacia Maná |
| Poder Astral | Prana | Diagonal ascendente hacia Prana |

El dibujo también contiene tres pares de arcos con flechas en ambos extremos. Por tanto, se registran como relaciones recíprocas explícitas:

- Energía Natural ↔ Energía Abisal.
- Energía Natural ↔ Poder Astral.
- Energía Abisal ↔ Poder Astral.

Estas tres relaciones recíprocas no se deducen de una sola flecha: el diagrama dibuja una flecha en cada sentido.

La transcripción anterior registra únicamente las relaciones visibles. **No se completa la matriz por simetría ni por analogía**: una relación no enumerada permanece neutral provisionalmente. La reciprocidad se aplica exclusivamente a los tres pares representados mediante arcos bidireccionales.

Una vez transcrita, cada relación se representa como dato dirigido (Affinity(attacker, defender)), con modificadores separados para daño, postura y aplicación de estados. No se presupone que una ventaja de daño implique automáticamente ventaja en todos los demás canales. La imagen determina la topología cualitativa de la matriz, no sus multiplicadores numéricos; estos quedan sujetos a calibración y pruebas.

## 8. Postura

La Postura es un canal independiente de Vitalidad. Cada combatiente posee capacidad máxima, postura acumulada y reglas de recuperación.

Un ataque puede generar presión de postura:

\[
Q=(Q_{base}+Q_{escala})\cdot M_{ataque}\cdot M_{respuesta}\cdot M_{afinidad}
\]

La presión efectiva recibida se modifica mediante estabilidad, defensas de postura, estado, acción defensiva y propiedades específicas:

\[
Q_{recibida}=Q\cdot(1-R_{postura})\cdot M_{postura}
\]

\[
Postura'=clamp(Postura+Q_{recibida}-Recuperación,0,Postura_{max})
\]

El orden temporal exacto entre impacto y recuperación se resolverá por ticks/eventos: un impacto no debe perder parte de su presión por una recuperación ejecutada antes de tiempo en el mismo instante.

### 8.1 Recuperación y relación con Vitalidad

Regla funcional confirmada: los enemigos recuperan Postura cuando dejan de recibir presión del jugador; la velocidad de recuperación disminuye conforme baja su Vitalidad.

Modelo propuesto:

\[
v_{rec}=v_{base}\cdot F_{vitalidad}(HP/HP_{max})
\]

donde (F_{vitalidad}) es monótona creciente respecto a la proporción de Vitalidad: cuanto más saludable está el enemigo, más rápido recupera; cuanto menos Vitalidad conserva, más lenta es la recuperación. La curva debe tener un mínimo positivo o cero según el diseño individual. El mismo perfil puede configurarse para jugador y enemigos, pero la regla confirmada para enemigos no se extrapola al jugador sin decisión expresa.

### 8.2 Ruptura de postura y Muertebendita

La ruptura se produce cuando la Postura acumulada alcanza la capacidad máxima. La condición habilita la Muertebendita, sujeta a las reglas de ejecución y vulnerabilidad del objetivo.

Regla funcional confirmada: agotar por completo la Vitalidad también habilita Muertebendita, independientemente de que se haya roto la Postura. Por tanto, el sistema evalúa dos condiciones independientes:
- (Postura\ge Postura_{max});
- (HP=0) o estado equivalente de agotamiento de barra definido por el tipo de enemigo.

Una Muertebendita contra enemigo normal lo derrota inmediatamente. Contra enemigo resistente consume una barra completa de Vitalidad y reinicia su única barra de Postura. Los enemigos resistentes generales pueden tener hasta tres barras; los jefes tienen de tres a cinco barras y reglas de fase propias.

### 8.3 Jefes y transición de fase

Cuando un jefe pierde una barra completa:
- la barra perdida no se restaura;
- se produce transición de fase y cambio de comportamiento;
- conserva el 40 % de la Postura acumulada en el instante definido para la transición (equivale a reducirla un 60 %);
- existe una breve ventana de vulnerabilidad antes de la transición invulnerable;
- solo en las últimas fases puede aparecer un ataque especial de transición, según la configuración individual.

La operación matemática es:

\[
Postura_{fase\ nueva}=0.40\cdot Postura_{anterior}
\]

No se aplica el 40 % a la capacidad máxima, sino a la cantidad acumulada.

## 9. Desvíos, bloqueos, esquivas y respuestas

Cada ataque enemigo declara respuestas válidas. La resolución de una respuesta modifica por separado:
- daño a Vitalidad;
- presión de Postura sobre defensor;
- presión de Postura sobre atacante;
- estados y consecuencias especiales;
- ventana de contraataque o vulnerabilidad.

Un desvío perfecto exitoso neutraliza o mitiga el ataque según sus datos y reduce la Postura del atacante. La cantidad depende del arma del defensor y del ataque enemigo. Los enemigos también pueden ejecutar desvíos perfectos contra el jugador.

Cada arma define una ventana base de desvío perfecto. Estadísticas, habilidades y dominio pueden mejorarla, pero un límite configurado impide que crezca sin control:

\[
T_{deflect}=clamp(T_{base}+F_{mejoras},T_{min},T_{max})
\]

La unidad temporal canónica y los valores de ventanas quedan pendientes de establecerse antes de implementar el reloj de combate. La fórmula no presupone todavía si una mejora añade tiempo por ambos lados de la ventana o solo modifica una tolerancia concreta.

## 10. Estados alterados y efectos temporales

Cada estado define:
- canal y tipo (daño periódico, control, debilitamiento, beneficio, etc.);
- condición de aplicación y umbral;
- resistencia relevante;
- duración base;
- regla de acumulación;
- periodicidad y potencia, si produce daño;
- reglas de refresco, extensión, sustitución o inmunidad;
- interacciones con otros estados.

Voluntad reduce la duración de los estados negativos que afectan al personaje:

\[
T_{final}=T_{base}\cdot F_{voluntad}(S(Voluntad))\cdot M_{resistencia}\cdot M_{efectos}
\]

La función de Voluntad debe reducir duración de forma monótona, con suelo para evitar que un estado aplicable quede trivializado accidentalmente. Los estados beneficiosos no se acortan por Voluntad salvo que una regla específica lo establezca.

Para acumulación por impactos, cada estado define su propio modelo: acumulación hasta umbral, acumulación con decaimiento, aplicación por probabilidad o aplicación directa. No se impondrá una única regla a todos los estados.

El daño periódico pasa por el mismo pipeline defensivo de Vitalidad, identificando su origen y evitando que vuelva a activar indefinidamente el mismo efecto que lo generó.

## 11. Artes de Combate y energías

Las Artes de Combate pueden consumir energía, dañar Vitalidad, presionar Postura, aplicar estados o curar. Cada Arte declara:
- coste y tipo de energía;
- potencia y escalados (Destreza, Armonía u otros expresamente definidos);
- componentes de daño;
- presión de Postura;
- estados/curación;
- tiempos, condiciones y objetivos válidos.

El nivel de dominio individual de cada Arte puede modificar parámetros configurados (potencia, coste, postura, duración, alcance lógico o efectos), pero cada mejora declara qué parámetros modifica y con qué curva. No se aplican mejoras globales ocultas.

## 12. Armas, armaduras, materiales y durabilidad

El arma define potencia base, escalados, tipo de daño, postura, ventana de desvío, peso y propiedades especiales. La armadura define defensa plana y/o resistencias por tipo, peso y propiedades.

La condición del objeto se expresa como proporción:

\[
C=clamp(Durabilidad_{actual}/Durabilidad_{max},0,1)
\]

El rendimiento se deriva de una curva por categoría de objeto:

\[
M_{condición}=F_{condición}(C)
\]

La curva debe ser monótona: menor durabilidad nunca mejora accidentalmente el rendimiento ordinario. Armas pueden perder potencia; piezas defensivas pueden perder protección. Los daños estructurales críticos pueden aplicar modificadores o desactivar propiedades concretas y se modelan como condiciones explícitas, no como una penalización universal.

Materiales y calidad modifican los parámetros de base del objeto durante su fabricación. La combinación de materiales no debe introducir efectos imposibles de rastrear: el objeto resultante almacena sus propiedades finales calculadas y su procedencia para inspección y balance.

## 13. Evasión, movilidad y Ruptura

La descripción original atribuye a Ruptura mejora de evasión y daño crítico, pero requiere validación de alcance. Si se confirma, la evasión se modelará como modificación de una probabilidad o ventana, nunca como una anulación automática del ataque.

La probabilidad de evasión se limitará y resolverá contra la precisión/seguimiento del ataque:

\[
p_{evade}=clamp(F_{evade}(S(Ruptura))+B_{evade}-A_{precisión},p_{min},p_{max})
\]

La evasión no debe confundirse con esquiva ejecutada: una esquiva es una acción con tiempo, trayectoria y estado; la fórmula anterior solo aplica a mecánicas pasivas que explícitamente utilicen probabilidad.

Agilidad afecta a movimiento, escalada y capacidades físicas. Su relación con ventanas, desplazamientos, recuperación o acciones se definirá en los subsistemas correspondientes, sin añadir efectos automáticos al daño.

## 14. Curación, daño excedente y límites

La curación define fuente, potencia, escalados, restricciones y objetivo. Por defecto:

\[
HP'=min(HP_{max},HP+Curación_{efectiva})
\]

La curación no revierte barras de jefe definitivamente perdidas ni restaura fases consumidas. Los efectos de curación sobre estados y Postura son independientes y deben declararse.

El daño excedente no se convierte automáticamente en daño a otra barra, Postura o entidad. Las barras múltiples se consumen mediante reglas de enemigo/Muertebendita específicas.

## 15. Orden canónico de resolución

Cada impacto se procesa en este orden lógico:

1. Validar acción, actor, objetivo, alcance lógico, ventana y reglas de selección.
2. Resolver respuesta defensiva contextual (bloqueo, desvío, esquiva, contraataque u otra).
3. Determinar si el impacto conecta y qué componentes alcanzan al objetivo.
4. Calcular potencia base y escalados de cada componente.
5. Aplicar modificadores ofensivos de acción, arma, dominio, Voluntad y efectos.
6. Resolver crítico si el componente/acción admite críticos.
7. Aplicar defensa plana correspondiente.
8. Aplicar resistencia, afinidad y vulnerabilidad en el orden configurado y único del tipo.
9. Aplicar inmunidad/excepciones explícitas.
10. Aplicar daño efectivo a Vitalidad.
11. Calcular y aplicar presión de Postura como canal independiente.
12. Evaluar umbrales: ruptura de Postura, agotamiento de barra, Muertebendita y transición de fase.
13. Aplicar estados y efectos secundarios que cumplan sus condiciones.
14. Propagar interacciones ambientales autorizadas, con registro de eventos ya procesados y límite de cadena.
15. Emitir un resultado de combate auditable con valores previos, modificadores, valores finales y eventos disparados.

Este orden es la propuesta base. Las acciones que requieran otra secuencia deberán declarar una regla especial explícita y testeada; no se permitirá cambiar el orden por accidente.

## 16. Contrato de datos y trazabilidad

Cada resolución debe poder explicar:
- valor original de cada componente;
- estadísticas y curvas utilizadas;
- modificadores aditivos y multiplicativos;
- defensa plana y resistencia aplicada;
- afinidad consultada;
- daño final y daño realmente aplicado;
- presión de Postura calculada y aplicada;
- estados evaluados, aplicados, rechazados o inmunes;
- transición de barra/fase y razón;
- versión de las reglas y configuración.

Los cálculos puros deben estar separados de la mutación del estado del combate. La resolución calcula un resultado; un paso de aplicación valida y actualiza el estado. Esto permite pruebas unitarias, repetición determinista, depuración y simulaciones de balance.

## 17. Pruebas matemáticas obligatorias

Como mínimo, las pruebas automatizadas deberán cubrir:

- Monotonía: aumentar Fuerza/Armonía/Destreza en una acción que las escala no reduce su potencia.
- Límites: estadísticas mínimas/máximas, resistencias extremas, inmunidades y durabilidad cero.
- Defensa: aumentar defensa o resistencia no aumenta daño recibido.
- Voluntad: aumentar Voluntad no aumenta la duración de estados negativos y sí aumenta los tipos de daño definidos.
- Vitalidad/Postura: daño a Vitalidad no altera Postura salvo regla explícita; presión de Postura no reduce Vitalidad salvo regla explícita.
- Recuperación: con menor proporción de Vitalidad, un enemigo no recupera Postura más rápido.
- Muertebendita: ruptura de Postura y agotamiento de Vitalidad habilitan la condición de forma independiente.
- Jefes: al cambiar de fase conservan exactamente el 40 % de Postura acumulada y no recuperan la barra de Vitalidad perdida.
- Deflect: éxito y fallo respetan ventanas y respuestas configuradas.
- Estados: acumulación, duración, refresco, inmunidad y daño periódico sin bucles.
- Determinismo: mismo estado, acción, datos y semilla producen el mismo resultado.
- Reacciones en cadena: sin eventos duplicados ni propagación infinita.
- Simetría de reglas: jugador y enemigos utilizan el mismo pipeline salvo diferencias de datos/reglas explícitas.

## 18. Decisiones pendientes de calibración o confirmación

No se inventarán a partir de huecos documentales:
- valores base y curvas exactas de HP, escalado, Voluntad, Resistencia, Ruptura y Destreza/Armonía;
- puntos de rendimientos decrecientes y límites críticos;
- multiplicadores numéricos de las afinidades energéticas; la topología dirigida ya está transcrita en la sección 7.1;
- defensa y resistencia por tipo, incluyendo qué parte de Resistencia es universal;
- parámetros de postura, recuperación, presión por ataque y umbrales;
- unidad temporal, prioridad de eventos y ventanas de reacción;
- lista, acumulación, resistencias y duraciones de estados;
- coeficientes de arma, materiales, calidad y deterioro;
- interacción exacta de críticos con cada tipo de daño;
- si Ruptura afecta a evasión y en qué mecánicas concretas;
- reglas del jugador respecto a recuperación de Postura, que no se extrapolan de las reglas confirmadas para enemigos.

Estas decisiones se calibrarán mediante escenarios de referencia y pruebas automatizadas, no por intuición aislada. Cada parámetro numérico tendrá nombre, unidad, rango permitido, valor por defecto versionado y pruebas que expliquen su efecto.

## 19. Método de calibración

Se crearán escenarios de prueba reproducibles para:
- personajes iniciales de las tres clases;
- personajes con estadísticas bajas, medias, altas y cercanas al límite;
- armas ligeras, pesadas, rápidas, lentas y mágicas;
- objetivos con defensas ligeras, medias, pesadas, resistencias y vulnerabilidades;
- enemigos normales, resistentes y jefes con distintas barras;
- secuencias de presión, defensa, desvío, ruptura, recuperación y ejecución.

Se medirán tiempo medio para derrotar, número de intercambios, daño por ventana de oportunidad, presión de Postura por acción, frecuencia de críticos, eficacia marginal de cada punto de estadística y diferencias entre configuraciones. El objetivo no es igualar todas las armas o clases, sino asegurar que cada opción tenga una identidad útil, contrajuego y coste de oportunidad.

Los valores de balance solo se aprobarán cuando las simulaciones y pruebas de regresión demuestren que no existen estrategias dominantes accidentales, progresiones explosivas ni combinaciones que invaliden las defensas o el sistema de Postura.
