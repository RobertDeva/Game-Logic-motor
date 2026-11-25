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

La transcripción anterior registra únicamente las relaciones visibles. **No se completa la matriz por simetría ni por analogía**: una relación no enumerada permanece neutral. La reciprocidad se aplica exclusivamente a los tres pares representados mediante arcos bidireccionales.

### 7.2 Multiplicadores de afinidad confirmados

Los multiplicadores funcionales definidos son:

| Situación del enfrentamiento | Multiplicador de daño |
|---|---:|
| La energía atacante tiene ventaja sobre la energía defensora | ×1.50 |
| La energía atacante tiene desventaja frente a la energía defensora | ×0.75 |
| El par de energías tiene relación recíproca en el diagrama | ×1.25 en ambos sentidos |
| No existe relación dibujada entre ambas energías | ×1.00 (neutral) |

En los enfrentamientos recíprocos, el multiplicador ×1.25 sustituye a los multiplicadores ordinarios de ventaja y desventaja: se aplica en ambos sentidos, no ×1.50 en una dirección y ×0.75 en la contraria.

La afinidad se consulta como dato dirigido (Affinity(attacker, defender)). El multiplicador de afinidad se aplica una sola vez al componente de daño compatible con la energía implicada, dentro de la resolución de daño. No se aplica automáticamente a presión de Postura, duración ni probabilidad de estados alterados; esos canales necesitarán reglas explícitas si deben verse afectados.

La imagen determina la topología cualitativa de la matriz y estos valores determinan su escala inicial. Los multiplicadores quedan registrados como parámetros de balance centralizados y versionados, para que puedan calibrarse posteriormente sin alterar las relaciones definidas.

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
- calibración final de los valores base y topes de HP, daño universal, resistencia, crítico y escalados secundarios; el perfil inicial de curvas y efectos está definido en la sección 22;
- calibración final de puntos de rendimientos marginales y límites críticos, a partir de simulaciones de encuentros;
- calibración posterior de los multiplicadores de afinidad (valores iniciales confirmados: ×1.50, ×0.75 y ×1.25 para relaciones recíprocas);
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


## 20. Estudio comparativo de referencias Soulslike

**Propósito:** utilizar sistemas publicados y análisis técnicos de Dark Souls, Dark Souls III, Elden Ring, Sekiro y Bloodborne como referencias de diseño y contraste. No se importan automáticamente sus cifras, curvas, reglas ni limitaciones. Las reglas confirmadas de Mythical Exodus y sus seis afinidades energéticas siguen siendo la autoridad funcional.

### 20.1 Hallazgos relevantes

#### Dark Souls / Dark Souls III

Los análisis técnicos de la serie describen un proceso por componentes: se calcula el poder de ataque correspondiente, se aplica el multiplicador propio del movimiento antes de la defensa, se resuelve la defensa del tipo correspondiente, se aplican otros modificadores y finalmente se suman los componentes. El multiplicador de movimiento puede hacer que dos ataques de la misma arma tengan resultados muy distintos. La curva de defensa es no lineal y depende de la relación entre ataque y defensa, en lugar de limitarse siempre a una resta plana.

**Aplicación a Mythical Exodus:** conservar potencia base y escalado del arma separados del coeficiente del movimiento; evaluar una curva de defensa no lineal como alternativa o complemento a la defensa plana actualmente propuesta. La decisión final deberá compararse en simulaciones, especialmente para armas rápidas de bajo daño por impacto y armas lentas de gran impacto.

#### Elden Ring

El análisis de parámetros y el calculador técnico consultado separan el ataque de cada tipo de daño, las curvas de escalado de atributos, el multiplicador de movimiento, la defensa y la negación porcentual. La defensa se aplica antes de la negación. Los tipos de daño se resuelven por separado. El modelo también distingue el valor de ataque mostrado de la eficacia real contra un enemigo: influyen el movimiento utilizado, las defensas, las negaciones y las ventanas de ataque disponibles.

El calculador consultado simula múltiples combates y compara tiempo de derrota, no solo Attack Rating. Sus tiempos de apertura y comportamiento de jefes se declaran estimaciones de modelado, no datos internos oficiales.

**Aplicación a Mythical Exodus:** usar curvas de escalado independientes por estadística y por componente cuando corresponda; conservar la secuencia ataque → defensa → reducción porcentual; evaluar el balance por rendimiento real en encuentros simulados, no por una cifra agregada de daño o DPS aislada.

#### Sekiro: Shadows Die Twice

La guía de mecánicas publicada por Activision confirma que los ataques bloqueados también aumentan la Postura del objetivo; los ataques que conectan sin bloqueo pueden dañar tanto Vitalidad como Postura. Los desvíos contribuyen a romper la Postura del atacante y la Vitalidad del enemigo influye en su velocidad de recuperación. Al romper la Postura se abre una oportunidad temporal de Deathblow; enemigos más poderosos pueden requerir varias ejecuciones.

**Aplicación a Mythical Exodus:** respalda el tratamiento independiente pero coordinado de Vitalidad y Postura, la recuperación de Postura condicionada por Vitalidad y la importancia de ventanas de ejecución. Mythical Exodus conserva sus propias reglas de Muertebendita, barras y transiciones de jefe, que no se sustituyen por las de Sekiro.

#### Bloodborne

El análisis técnico consultado distingue tipos y subtipos de daño, y establece que el tipo efectivo depende del ataque individual, no únicamente del arma. Los ataques pueden contener varios componentes; cada componente utiliza su propia defensa y modificadores. La potencia de ataque se forma a partir del daño base y las contribuciones de atributos, y después el multiplicador del ataque transforma esa potencia antes de resolver la defensa. El análisis también diferencia el valor de ataque mostrado del daño real aplicado.

**Aplicación a Mythical Exodus:** el tipo de daño debe pertenecer al movimiento/componente y no inferirse exclusivamente del arma equipada. Los ataques híbridos deben resolver cada componente por separado antes de sumar el daño efectivo.

### 20.2 Principios de diseño adoptados para continuar el modelo

1. **Separar arma, atributo y movimiento:** el arma aporta parámetros base y coeficientes de escalado; la acción concreta aporta sus multiplicadores y componentes.
2. **Resolver cada componente por separado:** daño físico, elemental y energético consulta las defensas, resistencias y afinidades aplicables a ese componente.
3. **Separar defensa y negación porcentual:** se mantendrán como etapas distinguibles. Se comparará una defensa no lineal basada en la relación ataque/defensa con la resta plana propuesta en la sección 6 antes de fijar la fórmula definitiva.
4. **No confundir Attack Rating con daño real:** el balance debe considerar movimiento, defensas, estados, ventanas, frecuencia de impactos y oportunidades reales de atacar.
5. **Evaluar encuentros completos:** las simulaciones medirán tiempo de derrota, daño efectivo por oportunidad, presión de Postura, consumo de recursos, estados aplicados y exposición al contraataque.
6. **Mantener Vitalidad y Postura diferenciadas:** una acción podrá afectar a ambas, pero cada canal tendrá potencia, mitigación y consecuencias propias.
7. **Separar acumulación de estados del daño directo:** cada estado tendrá su propia regla de acumulación y resistencia; no se asumirá que su acumulación escala igual que el daño del movimiento.
8. **Preservar las reglas propias del proyecto:** los valores ×1.50, ×0.75, ×1.25 y ×1.00 de afinidades energéticas, las reglas de Muertebendita y las reglas de barras/fases de jefes de Mythical Exodus prevalecen sobre cualquier referencia externa.

### 20.3 Calidad y límites de las referencias

- La guía de Sekiro de Activision es una fuente editorial del distribuidor para las relaciones funcionales descritas, pero no publica todas las fórmulas internas.
- Las fórmulas detalladas de Dark Souls y Bloodborne proceden de documentación técnica comunitaria y pruebas/reconstrucciones; son útiles como referencia, no como especificación oficial del código fuente.
- El calculador de Elden Ring es una implementación comunitaria basada en datos del juego y simulaciones. Distingue explícitamente datos extraídos, reglas reconstruidas y estimaciones de comportamiento de jefes.
- Las cifras concretas de esos juegos no se trasladarán a Mythical Exodus sin análisis dimensional, pruebas de progresión y simulaciones propias.

### 20.4 Fuentes consultadas

- Dark Souls Wiki, “Damage Calculation”: https://darksouls.wikidot.com/damage-calculation
- Bloodborne Wiki, “Damage Calculation” y “A Hunter's Guide to Damage”: https://www.bloodborne-wiki.com/2015/10/damage-calculation.html
- Activision Support, “Sekiro: Shadows Die Twice Game Mechanics”: https://support.activision.com/fi/sekiro/articles/sekiro-shadows-die-twice-game-mechanics
- Elden Ring Infusion Calculator, documentación del modelo y fórmulas: https://github.com/kideuler/Elden-Ring-Calculator
- Elden Ring Wiki, “Motion Value”: https://eldenring.wiki.gg/wiki/Motion_Value


## 21. Comparación matemática de escalado y defensa

Esta sección transforma las referencias externas en candidatos comprobables para Mythical Exodus. Los datos de otros juegos son referencias de comportamiento, no valores de balance aprobados para este proyecto.

### 21.1 Escalado de atributos: tres capas separadas

Se recomienda separar:

1. **Curva del atributo:** cuánto aporta cada punto de una estadística.
2. **Afinidad de escalado del equipo/acción:** cuánto aprovecha esa estadística concreta.
3. **Potencia base y coeficiente del movimiento:** cuánto daño tiene el arma y cuánto modifica el ataque ejecutado.

Para el componente k:

\[
AR_k = B_k\left(1+\sum_i C_{k,i}\,G_{k,i}(s_i)\right)
\]

- (B_k): ataque base del componente.
- (C_{k,i}): coeficiente de escalado del componente con la estadística i.
- (G_{k,i}): curva normalizada de la estadística para ese componente.
- (s_i): valor de la estadística, entre 1 y 99.

El coeficiente y la curva no son intercambiables: dos armas pueden compartir estadística y curva, pero aprovecharla en distinta proporción. También pueden tener perfiles distintos si una es física y otra energética.

El movimiento se aplica después al componente:

\[
P_k=AR_k\cdot MV_k\cdot M_{arte,k}\cdot M_{condición,k}
\]

Así, la potencia base, el escalado y el multiplicador de movimiento no se mezclan ni se contabilizan dos veces.

### 21.2 Curvas de referencia observadas

La documentación comunitaria de Elden Ring registra, para determinadas afinidades físicas, esta progresión de valor de escalado interno:

| Estadística | Valor de curva de referencia |
|---:|---:|
| 10 | 15 |
| 20 | 35 |
| 30 | 46.5 |
| 40 | 57.5 |
| 50 | 67.5 |
| 60 | 75 |
| 70 | 82.5 |
| 80 | 90 |
| 90 | 92.5 |
| 99 | 96 |

Para determinadas afinidades no físicas, la misma fuente presenta otro perfil: 19, 40, 53, 66.5, 80, 85, 90, 95, 97.5 y 100 en esos mismos puntos. La diferencia demuestra que el juego no utiliza una única curva universal para todos los componentes.

Estos números **no se adoptan como valores de Mythical Exodus**. Se usarán como dos perfiles comparativos de referencia para probar:
- crecimiento fuerte temprano y rendimientos decrecientes posteriores;
- crecimiento más sostenido hasta la zona media;
- utilidad marginal de invertir en una segunda estadística frente a concentrar puntos en una primera.

La curva del proyecto debe ser una función de datos interpolada o definida por tramos, con dominio [1,99], monotonicidad no decreciente y límites validados. Los puntos de cambio se podrán definir por familia de estadística, arma y componente. No se impondrán automáticamente los mismos puntos de corte a Vitalidad, Resistencia, Fuerza, Agilidad, Voluntad, Ruptura, Destreza y Armonía.

### 21.3 Perfiles de escalado recomendados para simulación

Sin fijar todavía números definitivos, deben probarse al menos estos perfiles:

- **Especializado:** una estadística principal aporta la mayor parte del crecimiento; las secundarias aportan poco.
- **Híbrido:** dos estadísticas comparten el crecimiento de forma significativa.
- **Distribuido:** tres o más estadísticas aportan incrementos menores.
- **Sin escalado:** el componente depende principalmente de su potencia base y del movimiento.

Los perfiles son datos de armas/Artes, no clases rígidas. Una clase puede usar distintos perfiles mediante equipo y progresión, respetando las reglas funcionales del GDD.

### 21.4 Defensa no lineal basada en la relación ataque/defensa

Dark Souls documenta una función por tramos según la razón (r = Ataque / Defensa). El porcentaje de ataque que atraviesa la defensa crece de forma no lineal: es bajo cuando el ataque es muy inferior a la defensa, aumenta alrededor de la igualdad y se aproxima a un techo cuando el ataque supera ampliamente la defensa.

Como candidato comparativo se registra esta función:

\[
f(r)=
\begin{cases}
0.10 & r\le 0.125\\
0.1+\frac{19.2}{49}(r-0.125)^2 & 0.125<r\le1\\
0.7-\frac{0.4}{3}(r-2.5)^2 & 1<r\le2.5\\
0.9-\frac{0.8}{121}(r-8)^2 & 2.5<r\le8\\
0.90 & r>8
\end{cases}
\]

\[
D_{postdef}=P\cdot f(P/DEF)
\]

La fórmula es una reconstrucción documentada de Dark Souls y se debe validar en sus puntos de unión antes de reutilizarla. Para implementación, se protegerá el caso (DEF=0) con una rama explícita: sin defensa plana, el componente conserva toda su potencia antes de negaciones posteriores.

Elden Ring documenta una estructura equivalente por tramos de la razón ataque/defensa: el daño previo a negación se mueve entre el 10 %, 40 %, 70 % y 90 % del ataque según los umbrales 1/8, 1, 2.5 y 8 veces la defensa. Después aplica la negación porcentual correspondiente al tipo de daño.

### 21.5 Modelo de defensa que se recomienda probar en Mythical Exodus

Se compararán dos modelos con idénticos encuentros y distribuciones de atributos:

**Modelo A — resta plana actual**

\[
D_A=\max(0,P-DEF)\cdot(1-R)
\]

Ventaja: sencillo, transparente y fácil de calibrar. Riesgo: una defensa elevada puede anular completamente ataques débiles, mientras que el comportamiento alrededor de la igualdad ataque/defensa puede ser abrupto.

**Modelo B — defensa relativa no lineal**

\[
D_B=P\cdot f(P/DEF)\cdot(1-R)
\]

Ventaja: mantiene daño residual frente a objetivos muy resistentes y permite que la eficacia defensiva dependa de la fuerza relativa del golpe. Riesgo: puede sentirse poco intuitivo y exige calibrar cuidadosamente la curva y los límites.

**Candidato preferido para prototipo:** Modelo B como hipótesis de trabajo, no como decisión definitiva. La defensa plana puede seguir existiendo como estadística de entrada, pero su efecto se interpreta mediante la curva relativa. La negación porcentual (R) permanece como etapa separada y específica por componente. Inmunidades y reglas especiales se evalúan aparte.

Para (DEF=0), definir (f=1). Para valores inválidos o negativos, la validación de datos debe fallar al cargar contenido, no corregirse silenciosamente durante el combate.

### 21.6 Orden de resolución recomendado

Por componente:

1. Potencia base y contribuciones de estadísticas.
2. Multiplicador del movimiento/ataque.
3. Modificadores ofensivos válidos, incluidos los de Voluntad y Arte.
4. Crítico, si el componente lo admite.
5. Defensa plana convertida a mitigación mediante el modelo elegido.
6. Negación porcentual por tipo.
7. Afinidad energética aplicable (×1.50, ×0.75, ×1.25 o ×1.00), una sola vez.
8. Vulnerabilidades adicionales e inmunidades explícitas, con precedencia configurada y sin duplicar afinidades.
9. Suma de componentes y aplicación de daño efectivo.

El orden de afinidad y negación debe quedar centralizado por regla y probarse; si el diseño decide que la afinidad multiplica el poder previo a defensa en vez del daño posterior, se cambiará en un único lugar y se comparará mediante pruebas. No se permitirá que cada arma decida arbitrariamente el orden.

### 21.7 Matriz de simulación para atributos 1–99

La calibración debe recorrer valores de atributos de 1 a 99, no solo niveles representativos. Para cada estadística y perfil de escalado se calcularán:

- contribución absoluta al ataque;
- incremento marginal de un punto al siguiente;
- incremento porcentual respecto al valor anterior;
- puntos donde cambian los rendimientos marginales;
- comparación de inversión especializada e híbrida con presupuesto total equivalente;
- impacto del movimiento ligero, pesado, cargado, salto, habilidad y crítico;
- daño final contra defensas bajas, medias y altas.

Los presupuestos de comparación deben mantener constante la suma de puntos invertidos. Ejemplos de perfiles a simular: 40/10, 25/25, 55/10, 35/30, 70/10 y 50/30. Son distribuciones de prueba, no límites ni recomendaciones de progresión.

### 21.8 Criterios para elegir el modelo

No se seleccionará una curva por parecerse más a un Souls. Se elegirá por resultados observables:

- cada punto invertido produce una mejora predecible y no negativa;
- especialización y construcción híbrida tienen ventajas y costes distintos;
- ningún punto de corte vuelve inútil una estadística de forma abrupta;
- armas rápidas no dominan por número de impactos sin considerar daño por ventana y exposición;
- armas lentas pueden compensar su cadencia con impacto, presión de Postura y oportunidad;
- la defensa no vuelve invulnerable al objetivo salvo inmunidad definida;
- la negación porcentual conserva un papel útil sin producir reducciones ilimitadas;
- los resultados se mantienen razonables a lo largo del rango 1–99 y frente a enemigos de distintos perfiles.

### 21.9 Referencias específicas

- Dark Souls Wiki, Damage Calculation: curva de defensa y multiplicadores de movimiento: https://darksouls.wikidot.com/damage-calculation
- Elden Ring Wiki, Damage Negation: relación ataque/defensa y aplicación posterior de negación: https://eldenring.wiki.gg/wiki/Damage_Negation
- Elden Ring Wiki, Weapon Scaling: curvas y valores de escalado por estadística: https://eldenring.wiki.gg/wiki/Scaling
- Elden Ring Infusion Calculator: reconstrucción de AR a partir de daño base, escalado, curvas y mejoras: https://github.com/kideuler/Elden-Ring-Calculator
- Dark Souls III Wiki, Scaling: correcciones de escalado y puntos de rendimientos decrecientes: https://darksouls3.wikidot.com/scaling

## 22. Sistema de escalado de atributos — especificación v1.0

Esta sección establece el primer perfil matemático completo y coherente para las ocho estadísticas. Es la referencia canónica para los prototipos de escalado. Los valores aquí indicados son parámetros iniciales de Mythical Exodus, no cifras importadas de Soulslike. Se versionan para poder calibrarlos con simulaciones sin cambiar la estructura matemática.

### 22.1 Dominio y normalización

Todas las estadísticas tienen dominio entero [1,99]. El valor 1 representa el mínimo funcional y 99 el máximo previsto. Cada curva devuelve una contribución normalizada entre 0 y 1:

- 0 en estadística 1;
- 1 en estadística 99;
- crecimiento monótono;
- rendimientos marginales definidos por perfil;
- sin saltos en los puntos de cambio.

Se divide el dominio en cuatro tramos: 1–25, 26–50, 51–75 y 76–99. Para un valor s:

\[
n_1=clamp(s-1,0,24),\quad
n_2=clamp(s-25,0,25)
\]
\[
n_3=clamp(s-50,0,25),\quad
n_4=clamp(s-75,0,24)
\]

Cada estadística tiene cuatro pesos marginales (w1,w2,w3,w4). Su curva es:

\[
G(s)=\frac{w_1n_1+w_2n_2+w_3n_3+w_4n_4}
{24w_1+25w_2+25w_3+24w_4}
\]

El peso de un tramo expresa cuánto vale un punto adicional dentro de ese tramo, no una bonificación instantánea al alcanzar el umbral. Por ello, cruzar 25, 50 o 75 no produce saltos.

### 22.2 Perfiles iniciales por estadística

| Estadística | w1 (1–25) | w2 (26–50) | w3 (51–75) | w4 (76–99) | Intención |
|---|---:|---:|---:|---:|---|
| Vitalidad | 1.25 | 1.00 | 0.55 | 0.25 | Beneficio temprano alto; la supervivencia sigue creciendo, pero cada punto tardío aporta menos. |
| Fuerza | 0.70 | 1.10 | 0.80 | 0.35 | Escalado ofensivo más valioso en la zona media, con rendimientos decrecientes al especializarse mucho. |
| Agilidad | 1.10 | 0.85 | 0.45 | 0.20 | La movilidad básica mejora pronto; la inversión extrema no debe romper tiempos ni animaciones. |
| Resistencia | 1.20 | 0.90 | 0.45 | 0.20 | Protección y tolerancia útiles desde temprano, con límites para evitar invulnerabilidad. |
| Voluntad | 0.65 | 0.95 | 0.85 | 0.45 | Progresión sostenida en zona media; su efecto universal sobre daño exige moderación. |
| Ruptura | 0.55 | 0.95 | 0.75 | 0.35 | Crecimiento crítico gradual, evitando que la probabilidad se dispare al principio o al final. |
| Destreza | 0.75 | 1.10 | 0.75 | 0.30 | Especialización en Artes de Combate, con zona media fuerte. |
| Armonía | 0.65 | 0.95 | 0.90 | 0.40 | Escalado especial sostenido, con rendimientos decrecientes en valores extremos. |

Valores normalizados resultantes en puntos de control:

| Estadística | G(1) | G(25) | G(50) | G(75) | G(99) |
|---|---:|---:|---:|---:|---:|
| Vitalidad | 0.000 | 0.401 | 0.736 | 0.920 | 1.000 |
| Fuerza | 0.000 | 0.231 | 0.609 | 0.884 | 1.000 |
| Agilidad | 0.000 | 0.414 | 0.748 | 0.925 | 1.000 |
| Resistencia | 0.000 | 0.428 | 0.762 | 0.929 | 1.000 |
| Voluntad | 0.000 | 0.218 | 0.551 | 0.849 | 1.000 |
| Ruptura | 0.000 | 0.206 | 0.576 | 0.869 | 1.000 |
| Destreza | 0.000 | 0.252 | 0.637 | 0.899 | 1.000 |
| Armonía | 0.000 | 0.218 | 0.551 | 0.866 | 1.000 |

Los pesos son parámetros versionados. Cambiar un peso modifica la curva completa, pero no la semántica de la estadística ni el contenido que la utiliza.

### 22.3 Conversión de curva en efecto jugable

La curva normalizada no es por sí misma daño, defensa ni puntos de Vitalidad. Cada subsistema transforma G(s) mediante parámetros con unidades explícitas.

**Vitalidad — capacidad máxima**

\[
HP_{max}=HP_{clase}+HP_{bonus,99}\cdot G_{vit}(Vitalidad)
\]

Perfil inicial recomendado: HP_bonus,99 = 0.50 × HP_clase. A 99, la estadística puede aportar hasta un 50 % adicional sobre la base de clase; el valor concreto se conserva como parámetro de balance. Vitalidad no añade daño ni Postura.

**Fuerza — daño físico de armas y acciones compatibles**

\[
AR_k=B_k\left(1+\sum_i C_{k,i}G_i(s_i)\right)
\]

Fuerza solo participa cuando el arma, movimiento o componente declara su coeficiente. El contenido puede combinar Fuerza con otra estadística. La suma de coeficientes de escalado de un componente no podrá superar inicialmente 0.75; así, el escalado de atributos no puede añadir más de un 75 % de la potencia base antes de los modificadores globales. Se permiten valores inferiores o cero.

**Agilidad — movilidad, no daño automático**

\[
M_{movilidad}=1+0.12\cdot G_{agi}(Agilidad)
\]

\[
T_{recuperación}=T_{base}\left(1-0.10\cdot G_{agi}(Agilidad)\right)
\]

Estos modificadores solo se aplican a parámetros de desplazamiento y recuperación que declaren explícitamente escalado con Agilidad. No aceleran automáticamente ataques, animaciones, ventanas de desvío perfecto, invulnerabilidad de esquiva ni cadencia de armas. Cada acción conserva sus límites temporales y de animación.

**Resistencia — mitigación por tipo**

\[
R_{estadística,k}=R_{max,k}\cdot G_{res}(Resistencia)
\]

Perfil inicial: R_max,k no supera 0.20 (20 puntos porcentuales) por tipo de daño. Un coeficiente de afinidad defensiva por tipo puede reducir ese máximo, nunca aumentarlo sin una regla de contenido explícita. La resistencia total sigue limitada por el máximo global de mitigación del sistema; la inmunidad continúa siendo una regla separada.

**Voluntad — daño universal y duración de estados negativos**

\[
M_{vol}=1+0.15\cdot G_{vol}(Voluntad)
\]

\[
T_{neg}=T_{base}\cdot\max(0.75,1-0.25\cdot G_{vol}(Voluntad))
\]

Voluntad aumenta todos los tipos de daño hasta un 15 % en el perfil inicial y reduce hasta un 25 % la duración de estados negativos recibidos. El multiplicador de daño se aplica una sola vez por componente y no aumenta automáticamente la presión de Postura. La reducción de duración no afecta estados beneficiosos.

**Ruptura — probabilidad crítica**

\[
p_{crit}=clamp(p_{base}+0.12\cdot G_{rup}(Ruptura)+p_{otros},p_{min},p_{max})
\]

La contribución propia de Ruptura tiene un máximo inicial de 12 puntos porcentuales. El límite absoluto recomendado de probabilidad crítica es 35 %, salvo acciones cuyo diseño declare una regla excepcional. Ruptura no aumenta automáticamente el multiplicador de daño crítico. Su posible influencia sobre evasión queda fuera de esta versión hasta validar su alcance funcional; no se debe implementar por inferencia.

**Destreza — Artes de Combate**

Destreza participa en el escalado de daño de Artes de Combate mediante el mismo modelo por componente:

\[
AR_{arte,k}=B_k\left(1+C_{dex,k}G_{dex}(Destreza)+\sum_{j\ne dex}C_{k,j}G_j(s_j)\right)
\]

El coeficiente pertenece al Arte y puede ser cero. Destreza no aumenta automáticamente el daño de ataques ordinarios ni el coste/recuperación de energía.

**Armonía — daño especial y energético**

\[
AR_{especial,k}=B_k\left(1+C_{arm,k}G_{arm}(Armonía)+\sum_{j\ne arm}C_{k,j}G_j(s_j)\right)
\]

Armonía participa en componentes especiales o energéticos que la declaren. Las afinidades energéticas confirmadas se aplican en su etapa propia y una sola vez; no se incorporan dentro de la curva de Armonía.

### 22.4 Reglas de composición y prevención de doble escalado

1. Las curvas solo convierten estadísticas en contribuciones normalizadas.
2. Los coeficientes de arma/Arte determinan qué estadísticas participan y en qué proporción.
3. Voluntad se aplica como modificador global de daño separado; no debe incluirse simultáneamente en la suma de coeficientes del componente.
4. El multiplicador del movimiento se aplica una sola vez.
5. Afinidad energética se consulta una sola vez por componente compatible.
6. La presión de Postura tiene su propio valor base, coeficientes y mitigación; no hereda el daño final salvo que una acción lo configure explícitamente.
7. La defensa no modifica el valor de ataque mostrado; solo la resolución contra el objetivo.
8. Los valores intermedios conservan precisión decimal y se redondean al aplicar el resultado al estado persistente.

### 22.5 Reglas de validación de contenido

El cargador de datos rechazará:
- estadísticas fuera del intervalo [1,99];
- pesos de curva negativos o no finitos;
- curvas que no sean monótonas;
- coeficientes de escalado negativos o no finitos;
- suma de coeficientes superior al presupuesto configurado;
- resistencias fuera de sus límites;
- probabilidades fuera de [0,1] o límites críticos incoherentes;
- modificadores que declaren el mismo efecto en más de una etapa;
- acciones que pretendan escalar con una estadística sin declarar su canal y coeficiente.

No se corregirán silenciosamente los datos inválidos durante el combate.

### 22.6 Pruebas y criterios de aceptación

Las pruebas automáticas deben recorrer las 99 entradas posibles de cada curva y comprobar:
- G(1)=0 y G(99)=1;
- 0 ≤ G(s) ≤ 1;
- G(s+1) ≥ G(s);
- continuidad en los puntos 25, 50 y 75;
- la contribución marginal corresponde al peso del tramo;
- ningún punto de estadística reduce el efecto correspondiente;
- los topes de HP, movilidad, recuperación, resistencia, daño universal y crítico se respetan;
- Voluntad se aplica una sola vez a daño y nunca aumenta presión de Postura por defecto;
- Agilidad no altera ataques ni ventanas de desvío sin declaración explícita;
- las construcciones especializadas e híbridas se comparan con el mismo presupuesto total de puntos;
- armas y Artes sin coeficiente para una estadística no reciben escalado accidental de ella.

### 22.7 Estado de aprobación

Esta versión fija una arquitectura y un primer perfil numérico coherente para iniciar prototipos. Los pesos de las curvas y los topes de efecto son parámetros iniciales de balance, no verdades inmutables. La estructura, los dominios, la separación de responsabilidades, la ausencia de doble aplicación y los criterios de validación sí se consideran reglas del sistema. La calibración final deberá basarse en simulaciones de encuentros completos y pruebas de regresión.

## 23. Primera auditoría matemática del sistema de escalado

Se realizó un barrido determinista de los 99 valores posibles para cada una de las ocho curvas definidas en la sección 22. También se compararon construcciones especializadas e híbridas con el mismo presupuesto de puntos. Estos resultados validan propiedades matemáticas de la curva; todavía no equivalen a pruebas ejecutadas sobre el motor C# ni a balance de encuentros reales.

### 23.1 Barrido de las ocho curvas

Para las ocho estadísticas se verificó:
- G(1)=0 y G(99)=1;
- todas las salidas permanecen dentro de [0,1];
- los 98 incrementos marginales son positivos;
- no hay retrocesos entre valores consecutivos;
- cada tramo mantiene su pendiente definida y los cambios de pendiente no producen saltos en el valor acumulado.

Resultado: las ocho curvas cumplen estas propiedades en todo el dominio 1–99.

### 23.2 Comparación especializada e híbrida

Se utilizó un componente de prueba con coeficientes iguales de Fuerza y Destreza: C_fuerza = 0.375 y C_destreza = 0.375. La suma de coeficientes alcanza el presupuesto máximo inicial de 0.75. Se compararon parejas con presupuesto total de puntos equivalente:

| Construcción | Puntos | G_Fuerza + G_Destreza | Factor de ataque |
|---|---:|---:|---:|
| Especializada 40/10 | 50 | 0.5525 | 1.2072 |
| Híbrida 25/25 | 50 | 0.4830 | 1.1811 |
| Especializada 55/10 | 65 | 0.7588 | 1.2846 |
| Híbrida 35/30 | 65 | 0.7113 | 1.2667 |
| Especializada 70/10 | 80 | 0.9239 | 1.3465 |
| Híbrida 50/30 | 80 | 0.9383 | 1.3518 |

El resultado no fuerza una superioridad universal de una construcción: la especialización gana en los dos primeros presupuestos, mientras que la híbrida supera ligeramente a la especializada en el presupuesto de 80 puntos. Esto produce una decisión interesante sin penalizar automáticamente la diversificación. La conclusión depende de las curvas y coeficientes del componente; no se debe generalizar a todas las armas o Artes.

### 23.3 Interpretación y límites

- El perfil normalizado es matemáticamente válido y monotónico en 1–99.
- La distribución de rendimientos marginales es diferenciada por estadística.
- Los coeficientes de contenido permiten que un arma favorezca especialización y otra recompense híbridos.
- El límite de suma 0.75 impide que el escalado de atributos por sí solo multiplique sin control la potencia base.
- Los topes de Vitalidad, movilidad, recuperación, resistencia, Voluntad y crítico son hipótesis iniciales de balance y deben probarse contra enemigos y tiempos de combate reales.
- La auditoría no prueba todavía el tiempo para derrotar, el equilibrio entre clases, el impacto de armaduras, la interacción con defensa no lineal, afinidades ni presión de Postura.

La siguiente fase técnica consiste en convertir estas fórmulas en funciones puras del dominio C#, acompañadas por pruebas unitarias que reproduzcan el barrido 1–99. Después se conectarán a escenarios de combate y se calibrarán mediante resultados de encuentros completos.

## 24. Progresión del personaje, economía de almas y escalado dinámico de enemigos

Esta sección integra las reglas funcionales de progresión y economía con el modelo de estadísticas y dificultad. No sustituye las reglas de combate anteriores: establece cómo la progresión del mundo alimenta sus parámetros.

### 24.1 Nivel del personaje y atributos

El personaje posee ocho atributos: Vitalidad, Fuerza, Agilidad, Resistencia, Voluntad, Ruptura, Destreza y Armonía. Cada atributo tiene un valor inicial propio y un máximo de 99.

El personaje comienza en nivel 1. El nivel no está limitado a 99 y se define como 1 más la suma de los incrementos permanentes obtenidos sobre los valores iniciales:

\[
Nivel_{personaje}=1+\sum_{i=1}^{8}(A_i-A_{i,inicial})
\]

El nivel máximo teórico, contando el nivel inicial 1, es:

\[
Nivel_{máximo}=1+\sum_{i=1}^{8}(99-A_{i,inicial})
\]

Esta definición presupone que cada incremento de un atributo cuesta un punto de nivel y que no existen puntos negativos respecto al estado inicial. Los valores iniciales concretos de los ocho atributos determinan el máximo numérico definitivo. El nivel representa el nivel inicial 1 más los puntos invertidos, no la suma bruta de los valores de atributo.

Dos personajes con el mismo nivel pueden tener distribuciones distintas; por ello, el nivel total nunca será una medida suficiente del poder de combate. El análisis de poder deberá considerar también los atributos individuales, el equipo, las Artes, los recursos, las resistencias y la configuración de combate.

### 24.2 Almas: recurso económico y coste de progresión

Las almas constituyen la moneda general y, simultáneamente, el recurso que se entrega para incrementar atributos. El jugador decide cuándo reservarlas para subir atributos y cuándo gastarlas en bienes, equipamiento, servicios u otras oportunidades económicas.

El coste de la siguiente mejora depende exclusivamente del nivel general actual del personaje. No depende del atributo elegido ni de su valor individual:

\[
Coste_{siguiente}=C(Nivel_{actual}+1)
\]

Todos los atributos cuestan exactamente lo mismo cuando se compran desde un mismo nivel general. Tras comprar un punto, el nivel general aumenta en uno y el siguiente punto cuesta más según la curva. La elección del atributo determina la construcción del personaje, pero nunca modifica el precio.

Las almas acumuladas no conceden niveles automáticamente. Solo se produce un incremento cuando el jugador decide invertirlas y la operación se completa.

### 24.2.1 Curva de coste de referencia

Se adopta como **curva matemática de referencia inicial** el comportamiento de progresión de Dark Souls III, adaptado al nivel de Mythical Exodus. La referencia resulta apropiada porque el coste depende del nivel global y aumenta de forma no lineal, alcanzando millones de almas en niveles elevados. No se importan clases iniciales, atributos, límites ni reglas de otros juegos.

Sea \(n\) el número de puntos de progresión comprados desde el estado inicial. El coste del siguiente punto utiliza el nivel objetivo equivalente \(x=n+2\), de modo que el primer punto corresponde al coste de alcanzar el nivel 2 de la tabla de referencia:

\[
C(n)=
\begin{cases}
T_{DS3}(x), & 2\le x\le12\\
\operatorname{round}(0.02x^3+3.06x^2+105.6x-895), & x\ge13
\end{cases}
\qquad x=n+2
\]

La función devuelve almas enteras. \(T_{DS3}\) es la tabla exacta de costes iniciales de Dark Souls III para los niveles objetivo 2–12: 673, 689, 706, 723, 740, 757, 775, 793, 811, 829 y 847. Se utiliza la tabla en ese tramo porque la aproximación polinómica publicada no reproduce todos esos enteros exactamente. Desde el nivel objetivo 13 se utiliza la expresión cúbica redondeada al entero más cercano. El coste se calcula usando el nivel objetivo, no el nivel que se abandona. Para evitar ambigüedad, \(n=0\) representa el personaje en su estado inicial; comprar el primer punto cuesta \(C(0)=673\), equivalente al tramo de nivel 1 a 2 de Dark Souls III.

**Valores ilustrativos de la curva de referencia** (coste de comprar el punto correspondiente, redondeado al alma más cercana):

| Nivel objetivo equivalente | Coste |
|---:|---:|
| 2 | 673 |
| 3 | 689 |
| 12 | 847 |
| 13 | 1.039 |
| 25 | 3.970 |
| 50 | 14.535 |
| 75 | 32.675 |
| 100 | 60.265 |
| 150 | 151.295 |
| 200 | 302.625 |
| 300 | 846.185 |
| 400 | 1.810.945 |
| 500 | 3.316.905 |
| 600 | 5.484.065 |
| 700 | 8.432.425 |
| 750 | 10.237.055 |

La tabla expresa niveles objetivo equivalentes de la curva, no niveles iniciales de clase de Mythical Exodus. La correspondencia definitiva se aplicará al contador de puntos adquiridos desde el estado inicial; no se sumará un nivel base ficticio al nivel funcional del personaje.

Esta adopción es una **línea base de balance**, no una afirmación de que la economía completa ya esté calibrada. Antes de cerrar sus parámetros se deben simular ingresos acumulados y costes de vida, equipo, reparación, materiales, servicios y comercio. La curva puede recibir un multiplicador global o una transformación versionada si las pruebas demuestran que el ritmo económico del mundo requiere otra escala, pero no se permitirán multiplicadores distintos por atributo.

### 24.2.2 Invariantes de progresión

1. Dos personajes con el mismo nivel general y la misma cantidad de almas pagan el mismo coste por el siguiente punto, sin importar su distribución de atributos.
2. Comprar cualquier atributo aumenta el nivel general exactamente en un punto.
3. El precio siguiente se calcula desde el nuevo nivel general.
4. Ninguna profesión, clase, atributo o especialización modifica directamente el coste de subir de nivel.
5. Las fuentes de ingreso pueden diferir por actividad, pero todas alimentan la misma moneda y el mismo coste de progresión.
6. Los costes de bienes, servicios, mantenimiento y comercio compiten por la misma reserva de almas; la progresión no debe asumir que el jugador dedica todos sus ingresos a subir atributos.

### 24.3 Fuentes válidas de obtención de almas

Las almas se obtienen mediante:
- combate: recompensas por derrotar enemigos y jefes, y otras recompensas de combate definidas por el contenido;
- exploración: tesoros, hallazgos, descubrimientos y recompensas de exploración;
- logros: recompensas asociadas a objetivos y logros;
- prestación de servicios: remuneración por trabajos o servicios profesionales realizados;
- comercio: ingresos derivados de operaciones comerciales y ventas.

La recolección de materias primas y la creación, transformación o fabricación de productos **no generan almas directamente**. Estas actividades producen recursos, materiales y bienes. Esos bienes pueden convertirse en ingresos posteriormente mediante comercio o pueden utilizarse para prestar servicios remunerados cuando la actividad lo permita.

El sistema debe impedir que una misma operación genere almas simultáneamente como recompensa de producción y como pago comercial. Las almas se reconocen en el punto económico válido —por ejemplo, al completar una venta o cobrar un servicio— y no al recolectar o fabricar.

El balance económico deberá contemplar costes de materiales, herramientas, transporte, mantenimiento, tarifas, oferta, demanda, calidad y otros gastos pertinentes. Las operaciones de compra y venta no deben crear riqueza neta artificial mediante ciclos repetibles sin riesgo, coste o límite económico.

### 24.4 Relación entre riqueza y progresión

Como las almas sirven para comerciar y subir atributos, la riqueza sí puede acelerar la progresión de combate. Un personaje que obtiene ingresos elevados mediante servicios o comercio podrá financiar mejoras de atributos con mayor rapidez que otro que no lo haga.

Esto es una consecuencia deliberada de la economía compartida, no un comportamiento que deba neutralizarse mediante penalizaciones ocultas. Las profesiones de recolección y fabricación contribuyen indirectamente a esa progresión al proporcionar los recursos y productos que hacen posible el comercio y los servicios, pero no entregan almas por sí mismas.

La riqueza no aumenta directamente las estadísticas ni el nivel: proporciona capacidad de pago. La subida solo ocurre al invertir almas en una mejora de atributo.

### 24.5 Modelo de precios de bienes, enseñanza y consumibles

La economía comercial utiliza la misma curva de almas de la progresión como referencia de escala, pero **no utiliza el nivel actual del comprador para inflar sus precios**. Cada bien o servicio tiene un nivel económico de referencia \(r\), determinado por la etapa del mundo, la complejidad de fabricación, la calidad y la progresión para la que fue diseñado.

Se define el valor económico base:

\[
B(r)=C(r-2)
\]

donde \(C(n)\) es la función de coste de la sección 24.2, basada en puntos comprados desde el estado inicial. Como \(r\) representa el nivel objetivo equivalente del artículo, se evalúa \(C(r-2)\): el primer nivel objetivo posible es 2 y corresponde a \(C(0)\). El precio de referencia de una categoría es:

\[
P_{base}=round(B(r)\cdot m_{categoría})
\]

El precio final puede incorporar modificadores explícitos de calidad, escasez, oferta/demanda, reputación comercial, impuestos, ubicación y estado del objeto:

\[
P_{final}=round(P_{base}\cdot M_{calidad}\cdot M_{mercado}\cdot M_{reputación}\cdot M_{ubicación})
\]

Los modificadores deberán tener límites configurables y registrarse por separado. Ningún modificador puede depender de la cantidad de almas que lleva el jugador ni de su nivel actual por sí solo. Un artículo conserva su referencia económica aunque lo compre un personaje de otra etapa; las diferencias regionales o de mercado deben proceder del mundo simulado, no de una penalización oculta al comprador.

#### 24.5.1 Enseñanzas e instrucciones de maestros

Multiplicadores iniciales de referencia para el precio de una enseñanza individual:

| Categoría de enseñanza | Multiplicador |
|---|---:|
| Básica | ×1,5 |
| Especializada | ×3 |
| Avanzada | ×6 |
| Maestría | ×12 |
| Técnica excepcional o secreta | ×20–30, además de requisitos no monetarios |

El precio corresponde a una enseñanza o desbloqueo concreto, no a todo el catálogo de un maestro. Si una técnica requiere varias sesiones, materiales, herramientas o pruebas prácticas, esos costes se detallan por separado y no se duplican dentro del precio de enseñanza.

Pagar no garantiza automáticamente el aprendizaje. Cada enseñanza puede exigir requisitos como:
- nivel de conocimiento o dominio previo;
- práctica demostrada o evaluación;
- atributo, capacidad o equipo mínimo cuando sea funcionalmente necesario;
- reputación, confianza o acceso al maestro;
- descubrimiento, misión, investigación o material especial.

Los maestros no venden necesariamente todo su conocimiento. Su rango de maestría, especialización, ubicación y relación con el jugador determinan qué pueden enseñar. El aprendizaje autónomo mediante observación, experimentación y descubrimiento continúa siendo una vía válida; el precio de la enseñanza remunera la transmisión guiada y no debe bloquear artificialmente los conocimientos que el diseño permita descubrir por cuenta propia.

#### 24.5.2 Armas y equipamiento

Multiplicadores iniciales para el precio de compra de armas:

| Categoría | Multiplicador |
|---|---:|
| Común y funcional | ×0,75 |
| Buena calidad | ×2 |
| Especializada | ×5 |
| Excepcional | ×10 |
| Única, legendaria o de importancia narrativa | ×20–40 o no comerciable |

La categoría no se asigna únicamente por daño. Debe considerar materiales, calidad de fabricación, durabilidad, propiedades especiales, rareza, complejidad técnica y prestaciones. Una pieza excepcional no debe ser solo una versión numéricamente superior: su precio debe corresponder a propiedades y costes de obtención justificables.

Las armas únicas o legendarias pueden requerir una misión, descubrimiento, fabricación especializada o derrota de un enemigo. Un precio alto no debe convertir automáticamente todo objeto único en una mercancía ordinaria. Las armaduras, escudos, accesorios y herramientas utilizarán el mismo principio, con perfiles de categoría propios y multiplicadores calibrados según su utilidad y coste de fabricación.

#### 24.5.3 Consumibles

Los consumibles se valoran por unidad y se clasifican por potencia, rareza, duración, facilidad de fabricación y efecto sobre el combate o la exploración:

| Categoría | Multiplicador por unidad |
|---|---:|
| Común | ×0,015 |
| Refinado | ×0,04 |
| Avanzado | ×0,08 |
| Raro | ×0,15 |

Los consumibles de uso frecuente deben representar un gasto pequeño pero perceptible; los consumibles potentes o raros deben exigir una decisión económica real. Los precios de lotes podrán ofrecer una reducción limitada por costes logísticos menores, pero no descuentos que hagan rentable comprar grandes cantidades para revenderlas o que anulen el coste unitario.

El balance deberá medir el gasto esperado por encuentro, expedición y hora de juego. La eficacia del consumible, la disponibilidad de ingredientes, el tiempo de fabricación y la posibilidad de obtenerlo mediante exploración también forman parte de su valor. No se debe equilibrar un consumible únicamente elevando su precio si su efecto rompe el combate.

#### 24.5.4 Materias primas y componentes de fabricación

Como referencia inicial, los materiales comprables pueden utilizar estos multiplicadores por unidad:

| Categoría de material | Multiplicador |
|---|---:|
| Común | ×0,02 |
| Poco común | ×0,05 |
| Raro | ×0,12 |
| Excepcional o específico de criatura | ×0,20–0,40, según disponibilidad |

El precio real también depende de calidad, procedencia, cantidad disponible, estacionalidad, riesgo de transporte y demanda de la región. Los componentes de origen biológico, minerales y materiales especiales deben conservar diferencias de propiedades: dos materiales con el mismo nombre pero distinta calidad no tienen por qué compartir precio.

Recolectar o fabricar no concede almas. Comprar materiales es un gasto; recolectarlos representa una inversión de tiempo, herramientas y riesgo. Fabricar un objeto no crea riqueza por sí mismo: las almas se obtienen cuando el producto se vende o cuando se cobra un servicio válido. La valoración de compra a comerciantes debe considerar costes y mercado para evitar bucles de fabricación y reventa con beneficio garantizado.

#### 24.5.5 Ejemplos numéricos de referencia

La tabla siguiente muestra precios antes de modificadores de mercado, reputación, ubicación o calidad adicional. \(r\) es el nivel económico objetivo del artículo, no el nivel actual del comprador.

| Nivel económico \(r\) | \(B(r)\) | Enseñanza básica ×1,5 | Maestría ×12 | Arma común ×0,75 | Arma excepcional ×10 | Consumible común ×0,015 | Consumible raro ×0,15 |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 13 | 1.039 | 1.558 | 12.468 | 779 | 10.390 | 16 | 156 |
| 50 | 14.535 | 21.802 | 174.420 | 10.901 | 145.350 | 218 | 2.180 |
| 100 | 60.265 | 90.398 | 723.180 | 45.199 | 602.650 | 904 | 9.040 |
| 200 | 302.625 | 453.938 | 3.631.500 | 226.969 | 3.026.250 | 4.539 | 45.394 |

Los ejemplos demuestran cómo crece la escala económica. No implican que un jugador de nivel 50 deba comprar artículos de referencia 50, ni que todo artículo de una región tenga el mismo nivel económico. La ubicación y la progresión prevista del contenido determinan \(r\).

#### 24.5.6 Reglas de equilibrio y protección contra la progresión trivial

1. El precio se determina por el artículo, su categoría y su contexto económico; nunca por el saldo de almas del comprador.
2. La curva de progresión sirve como unidad de escala común, pero los multiplicadores de cada categoría son distintos y versionados.
3. Comprar una enseñanza, arma o consumible compite directamente con reservar almas para subir atributos.
4. Los artículos de alto nivel económico no deben aparecer en mercados tempranos en cantidades ordinarias; su acceso depende de región, oferta, reputación, conocimiento y requisitos.
5. Los bienes de mayor potencia deben implicar costes de adquisición, fabricación, mantenimiento, materiales o acceso acordes con su utilidad.
6. Los consumibles deben ser asequibles para usos deliberados, no tan baratos como para eliminar la gestión de recursos.
7. La fabricación y el comercio no deben crear ciclos de beneficio neto garantizado. Deben computarse materiales, herramientas, tiempo, transporte, tarifas, desgaste, riesgo y precio real de reventa.
8. Los precios y recompensas deben probarse juntos. No basta con aumentar precios si las actividades económicas generan ingresos desproporcionados.
9. El sistema no impondrá un límite artificial a la riqueza: la progresión rápida será posible si el jugador logra ingresos elevados, pero deberá elegir entre invertirlos en atributos, conocimientos, equipo, materiales, servicios y consumibles.
10. Los multiplicadores aquí propuestos constituyen una línea base de simulación, no un balance final. Deben calibrarse con presupuestos de expedición, ritmos de ingreso, gasto de mantenimiento y curvas de progresión de cada región.

### 24.6 Rangos de progresión por zona

Cada zona define un perfil propio de dificultad y progresión, que incluye como mínimo:
- rango de nivel de personaje previsto;
- nivel de referencia de sus enemigos;
- ritmo de escalado dinámico;
- límites inferior y superior de escalado;
- modificadores o perfiles propios de sus categorías de enemigos;
- diferencias de dificultad respecto a otras zonas.

Los rangos de zonas pueden solaparse para permitir rutas alternativas y exploración no lineal. No se establece un único rango universal ni se asignan cifras definitivas en esta sección: se derivarán del mapa, los encuentros y las pruebas de balance.

Las zonas no escalan todas de la misma manera. Una región inicial puede acompañar parcialmente el crecimiento del jugador, pero tendrá un ritmo menor y un techo inferior al de una región avanzada. Una región avanzada tendrá referencias y límites superiores. Esta diferenciación debe permanecer perceptible incluso cuando el jugador regrese a regiones anteriores.

### 24.7 Escalado dinámico de niveles de enemigos

Los enemigos progresan con el jugador, incluso cuando el jugador ha financiado sus atributos mediante actividades económicas en lugar de combate. El origen de las almas no afecta al cálculo: el sistema evalúa el nivel y las capacidades actuales del personaje, no el historial de cómo consiguió su riqueza.

El nivel dinámico de un enemigo se deriva de:
- nivel actual del personaje;
- perfil de escalado de la zona;
- nivel de referencia y límites del enemigo;
- categoría del enemigo (común, fuerte, élite, jefe u otra categoría configurada);
- cuando sea necesario, contexto de encuentro y progresión del mundo.

Modelo conceptual:

\[
Nivel_{enemigo}=clamp\left(
Nivel_{referencia}+F_{zona}(Nivel_{personaje}-Nivel_{referenciaJugador}),
Nivel_{mínimo},Nivel_{máximo}
\right)
\]

La función de zona determina cuánto y cómo acompaña el enemigo al jugador. Su pendiente, forma y límites son datos de configuración; no se presupone una proporción idéntica para todas las regiones. El nivel del enemigo es una referencia de progresión, no una instrucción para multiplicar todas sus estadísticas por un único factor.

### 24.8 Escalado de capacidades y preservación de identidad

Las estadísticas efectivas de un enemigo se derivan de su arquetipo, atributos propios, nivel, equipo/capacidades, fase y perfil de zona. El nivel dinámico no debe borrar las diferencias de diseño entre enemigos.

- Los enemigos comunes progresan dentro de límites más contenidos.
- Los enemigos fuertes y élites tienen perfiles y límites propios.
- Los jefes conservan sus atributos, mecánicas, barras de Vitalidad, fases y patrones distintivos; su ajuste dinámico no puede invalidar esas reglas.
- Enemigos del mismo nivel pueden tener capacidades muy diferentes por sus arquetipos y configuraciones.
- Vitalidad, daño, Postura, defensas, resistencias y comportamiento no tienen por qué escalar todos con el mismo factor.

El cálculo de dificultad debe considerar la distribución de los ocho atributos del jugador, su equipamiento, Artes y capacidades relevantes. Dos personajes del mismo nivel pueden presentar perfiles de amenaza distintos. El sistema puede usar perfiles de combate para seleccionar parámetros de encuentro, pero no debe convertir el escalado en una respuesta perfecta que anule cualquier especialización o debilidad del enemigo.

### 24.9 Reglas para que el escalado no elimine la sensación de progreso

1. Los enemigos acompañan el crecimiento del jugador, pero no igualan automáticamente su poder.
2. Cada zona tiene una curva y límites independientes.
3. Los límites de zonas anteriores son inferiores a los de zonas posteriores, salvo excepciones de contenido expresamente diseñadas.
4. Al regresar a una zona anterior, el jugador debe conservar una ventaja perceptible cuando su progresión supere la referencia de esa zona.
5. Entrar prematuramente en una zona avanzada debe seguir siendo peligroso.
6. El nivel dinámico no modifica retroactivamente recompensas ya obtenidas ni concede almas por el mero hecho de escalar.
7. La riqueza no es una variable directa del escalado enemigo; solo influye indirectamente cuando se convierte en atributos mediante una mejora comprada.
8. Los parámetros se validarán mediante pruebas de personajes con igual nivel y distribuciones distintas, personajes enriquecidos por comercio, progresión centrada en combate y regresos a zonas anteriores.

### 24.10 Datos y pruebas requeridos

Cada zona y arquetipo enemigo deberá exponer datos configurables, no constantes dispersas en el código. Como mínimo se probarán:
- monotonicidad del escalado dentro del rango configurado;
- respeto de límites mínimos y máximos;
- diferencias de crecimiento entre zonas;
- conservación de ventaja al regresar a zonas anteriores;
- dificultad de zonas avanzadas para personajes por debajo de su rango previsto;
- personajes de igual nivel con distribuciones de atributos diferentes;
- personajes que progresan mediante comercio/servicios frente a personajes que progresan mediante combate;
- ausencia de generación directa de almas por recolección o fabricación;
- ausencia de creación neta artificial de almas mediante ciclos comerciales;
- preservación de mecánicas particulares de élites y jefes.

Los rangos, pendientes, límites, costes de mejora y precios quedan pendientes de calibración con datos del mundo y simulaciones. Las reglas de obtención de almas, cálculo de nivel, separación entre riqueza y atributos y escalado diferenciado por zona constituyen las reglas funcionales que deben respetar las futuras implementaciones.
