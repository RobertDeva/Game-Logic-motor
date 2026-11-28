# Modelo de crecimiento de atributos y estadísticas derivadas

**Proyecto:** Mythical Exodus  
**Estado:** especificación en evolución. Contiene valores base establecidos, una curva matemática definida en una etapa anterior y una arquitectura posterior de soft caps propuesta que todavía requiere fórmula y simulación.  
**Fuentes de diseño:** documentos conversacionales «Diseño del núcleo lógico» actualizados el 27 de noviembre de 2026.  
**Propósito:** consolidar las decisiones recuperadas sin convertir propuestas de balance en reglas aprobadas ni completar con suposiciones los datos que las fuentes no muestran.

## 1. Principios del modelo

1. El personaje tiene ocho atributos primarios: **Vitalidad, Fuerza, Agilidad, Resistencia, Voluntad, Ruptura, Destreza y Armonía**.
2. Cada atributo se desarrolla dentro del intervalo **0–99**.
3. Las estadísticas derivadas se calculan a partir de un punto de partida universal y de las contribuciones de los atributos que las afectan.
4. Las clases no necesitan fórmulas independientes: utilizan el mismo sistema y comienzan con distribuciones iniciales de atributos diferentes.
5. Cada atributo y cada estadística deben poder tener curvas y parámetros propios. No se presupone que una única curva idéntica represente correctamente todos los sectores.
6. La matriz de influencias definida durante el diseño es la autoridad funcional. Este documento no sustituye ni reconstruye las relaciones que no aparecen completas en las fuentes consultadas.
7. La progresión del jugador y la escala de los enemigos son sistemas separados.

## 2. Punto de partida universal y valores base

El origen matemático es un personaje hipotético con los ocho atributos en cero:

\[
A_1=A_2=\cdots=A_8=0
\]

Este personaje no tiene por qué corresponder a una clase jugable. Permite definir valores base independientes de las distribuciones iniciales de cada clase.

La forma general propuesta para una estadística aditiva es:

\[
S=S_0+\sum_{i=1}^{8}C_i(A_i)
\]

Donde:

- \(S_0\): valor base de la estadística cuando todos los atributos están en cero.
- \(C_i(A_i)\): contribución del atributo correspondiente.
- \(S\): valor final de la estadística.

Las estadísticas porcentuales y multiplicativas deben utilizar operaciones adecuadas a su naturaleza; no se sumarán indiscriminadamente como si todas fueran valores planos.

### 2.1 Valores base establecidos en la revisión más reciente

| Estadística | Valor base |
|---|---:|
| Vitalidad máxima | **500 PV** |
| Poder de ataque físico básico | **25** |
| Postura máxima | **100** |
| Recuperación de Postura | **5 % por segundo** |
| Capacidad máxima de carga | **60 unidades** |
| Aura máxima | 0 |
| Maná máximo | 0 |
| Prana máximo | 0 |
| Defensas | 0 |
| Absorciones | 0 % |
| Velocidades y multiplicadores | 1,00 |
| Potencias ofensivas restantes | 0 |
| Bonificaciones de exploración | 0 % |

La fuente indica que los demás valores base aceptados se conservan. Las estadísticas que no aparecen con una cifra en esta tabla no deben recibir valores inventados: necesitan una referencia y calibración propias.

### 2.2 Valores anteriores sustituidos

Una fuente anterior había planteado Vitalidad máxima de 100 PV como referencia inicial. La revisión posterior la sustituyó por **500 PV**.

Por tanto, 100 PV no es el valor base vigente.

## 3. Curva de crecimiento definida en la etapa anterior

Una revisión anterior concretó una función normalizada de tres tramos y fijó el punto de transición entre los dos segmentos lineales en **10**:

\[
F(A)=
\begin{cases}
\dfrac{3A}{139}, & 0\leq A\leq10\\[6pt]
\dfrac{2A+10}{139}, & 10<A\leq30\\[6pt]
1-\dfrac{(99-A)^2}{9591}, & 30<A\leq99
\end{cases}
\]

Donde \(A\) es el valor del atributo y \(F(A)\) representa la fracción normalizada de la contribución máxima alcanzada.

### 3.1 Comportamiento y puntos de referencia

- **0–10:** crecimiento lineal inicial.
- **10–30:** segundo crecimiento lineal, más moderado.
- **30–99:** crecimiento parabólico decreciente.
- La función alcanza 100 % en 99.
- El crecimiento acumulado no retrocede; lo que disminuye es la ganancia marginal.

| Atributo | Contribución alcanzada |
|---:|---:|
| 0 | 0 % |
| 10 | 21,58 % |
| 20 | 35,97 % |
| 30 | 50,36 % |
| 40 | 63,71 % |
| 50 | 74,97 % |
| 60 | 84,14 % |
| 70 | 91,23 % |
| 80 | 96,24 % |
| 90 | 99,16 % |
| 99 | 100 % |

Esta fórmula es una definición matemática concreta de la etapa anterior. La revisión posterior propone una arquitectura más elaborada de soft caps (sección 5); por ello, la fórmula de esta sección debe conservarse como referencia y no declararse automáticamente la curva final vigente para todas las estadísticas.

## 4. Influencias principales y secundarias: propuestas previas

En una etapa anterior se propuso una ponderación de **10:1** entre influencia principal y secundaria:

- Influencia principal: peso 10.
- Influencia secundaria: peso 1.

Para las estadísticas tratadas entonces se propuso:

\[
G=\frac{\sum_i w_iF(A_i)}{\sum_i w_i}
\]

y:

\[
S=S_0+B\cdot G
\]

Donde \(B\) era un presupuesto máximo de crecimiento de la estadística.

La revisión posterior modificó este enfoque: las influencias principales y secundarias deben tener contribuciones independientes y **no están obligadas a compartir un presupuesto máximo común**. Por tanto:

- La ponderación 10:1 queda como propuesta previa pendiente de confirmación, no como regla definitiva.
- El presupuesto de crecimiento igual al valor base queda descartado como restricción general.
- No debe limitarse cada estadística a duplicar su valor base.
- Los máximos de contribución deben definirse por estadística y relación, con curvas y escalas adecuadas.

## 5. Arquitectura posterior propuesta: soft caps inspirados en Elden Ring

La revisión más reciente cambia el objetivo de balance: el crecimiento no debe quedar restringido a que cada estadística alcance como máximo el doble de su valor base.

Se adopta como referencia de diseño la filosofía de *soft caps* de Elden Ring: una estadística puede seguir aumentando hasta el atributo 99, pero la ganancia por punto invertido cambia según el tramo. No se copiarán literalmente los valores de Elden Ring; se adaptará el comportamiento a los ocho atributos y a la matriz de influencias del proyecto.

### 5.1 Tramos propuestos

| Tramo del atributo | Comportamiento propuesto |
|---|---|
| 0–10 | Crecimiento lineal inicial |
| 10–30 | Crecimiento lineal con un ritmo diferente |
| 30–60 | Crecimiento decreciente moderado |
| 60–80 | Crecimiento decreciente pronunciado |
| 80–99 | Crecimiento muy reducido |

Se conserva el inicio del rendimiento decreciente en 30, pero se proponen varios grados de reducción después de ese punto. Esta arquitectura todavía necesita una función matemática concreta, parámetros y simulaciones. No se deben inventar pendientes ni valores intermedios para presentarlos como aprobados.

### 5.2 Curvas especializadas

La revisión indica que cada estadística tendrá su propia curva y sus propios puntos de rendimiento decreciente. También se había establecido que cada uno de los ocho atributos tendría parámetros propios.

Características que deben orientar las curvas de los atributos:

| Atributo | Sectores que debe priorizar |
|---|---|
| Vitalidad | Supervivencia, resistencia física y Postura |
| Fuerza | Ataque físico, carga y potencia |
| Agilidad | Movilidad y velocidad de ejecución |
| Resistencia | Absorción y resistencia a estados |
| Voluntad | Poder ofensivo y efectos especiales |
| Ruptura | Críticos, esquivas y ruptura |
| Destreza | Precisión, eficacia y ejecución |
| Armonía | Maná, poderes elementales y efectos |

Estas prioridades no sustituyen la matriz completa de influencias. Las curvas deben preservar las relaciones principales y secundarias que se definieron en la encuesta original.

## 6. Vitalidad máxima: escala y estado

La relación registrada para Vitalidad máxima incluye los ocho atributos:

| Atributo | Influencia registrada |
|---|---|
| Vitalidad | Principal |
| Fuerza | Secundaria |
| Agilidad | Secundaria |
| Resistencia | Secundaria |
| Voluntad | Secundaria |
| Ruptura | Secundaria |
| Destreza | Secundaria |
| Armonía | Secundaria |

La forma general aditiva se había expresado así:

\[
PV_{\max}=500+\sum_{i=1}^{8}C_i(A_i)
\]

### 6.1 Objetivo provisional de escala

La revisión más reciente propone como objetivo inicial:

- Todos los atributos en 0: **500 PV**.
- Vitalidad en 99 y el resto de atributos en 0: **2.500 PV**.

Los **2.500 PV no son un máximo aprobado**. Son un objetivo provisional de diseño para comenzar el cálculo. Las contribuciones secundarias de los demás atributos se añadirán respetando la matriz y los límites que se definan. No se debe concluir que 2.500 PV sea el máximo total del personaje.

La propuesta anterior de 1.000 PV como máximo matemático queda expresamente descartada por ser demasiado restrictiva para la escala buscada.

## 7. Otras estadísticas con cálculos preliminares

La revisión previa presentó cálculos iniciales que sirven como antecedentes, no como límites definitivos:

### 7.1 Capacidad máxima de carga

- Valor base establecido: **60 unidades**.
- Influencias principales registradas en el cálculo preliminar: Fuerza y Voluntad.
- La propuesta anterior calculaba 120 unidades como máximo al duplicar la base. Ese límite no debe conservarse como regla: el modelo posterior rechaza la duplicación obligatoria de los valores base.

El peso real de armas, armaduras y accesorios no cambia por aumentar la capacidad; cambia la carga que el personaje puede soportar.

### 7.2 Poder de ataque físico básico

- Valor base establecido: **25**.
- Influencias principales registradas en el cálculo preliminar: Fuerza, Voluntad y Destreza.
- El valor 50 propuesto anteriormente como máximo por duplicación de la base no es un límite definitivo.
- Este poder representa el poder ofensivo básico antes de considerar arma, mejoras y movimiento utilizado.

### 7.3 Recuperación de Postura

- Valor base establecido: **5 % por segundo**.
- Influencia principal registrada: Vitalidad.
- Influencia secundaria registrada: Agilidad.
- La propuesta anterior de 10 % por segundo como máximo no es un límite definitivo.
- La recuperación sigue dependiendo de que el personaje no esté recibiendo presión.

Los valores de contribución finales para estas estadísticas deben definirse con el nuevo modelo de curvas y comprobarse mediante simulación.

## 8. Escala independiente de enemigos

La progresión del jugador y las estadísticas de los enemigos deben mantenerse separadas.

- **Jugador:** estadísticas calculadas mediante atributos, equipamiento, efectos y progresión.
- **Enemigos:** estadísticas determinadas por su diseño, categoría, región, dificultad y características particulares.
- **Jefes:** pueden tener valores de Vitalidad, ataque, defensa y Postura muy superiores a los del jugador.
- **Enemigos especiales:** pueden superar incluso los parámetros habituales de los jefes normales.

El límite de 99 de los atributos del jugador no debe convertirse en un techo para las estadísticas de enemigos. Un enemigo podrá tener 10.000, 50.000 o más PV cuando su función de diseño lo justifique. Esos números son ejemplos de escala posible, no valores asignados a enemigos concretos.

## 9. Clases y aplicación de las curvas

Las clases comparten el mismo sistema matemático. Cada clase comienza con una distribución inicial diferente de los ocho atributos; sus estadísticas se calculan aplicando las curvas a esos valores iniciales.

Una expresión general de la etapa inicial era:

\[
S_{\text{clase}}=S_0+\sum_{i=1}^{8}C_i(A_{i,\text{inicial}})
\]

La fórmula final deberá incorporar la arquitectura especializada y los soft caps definidos para cada estadística. No se deben crear fórmulas particulares por clase para compensar curvas generales mal calibradas.

Los valores base no se modificarán para compensar una curva mal diseñada: primero se construye el sistema de crecimiento y después se evalúan sus resultados.

## 10. Auditoría y simulaciones obligatorias

Antes de cerrar el balance, comprobar:

1. Que todas las relaciones de la matriz de influencias estén representadas.
2. Que las influencias principales y secundarias mantengan la jerarquía prevista.
3. Que la suma de muchas influencias secundarias no produzca una estadística desproporcionada.
4. Que invertir en un atributo no se convierta automáticamente en la mejor estrategia para obtener recursos.
5. Que las diferencias entre atributos iniciales de las clases se reflejen correctamente.
6. Que las especializaciones tengan identidad y que las distribuciones equilibradas sigan siendo viables.
7. Que alcanzar 99 en un atributo no vuelva irrelevantes a los demás.
8. Que las curvas tengan continuidad y un comportamiento marginal coherente en sus puntos de transición.
9. Que las estadísticas del jugador y las de los enemigos puedan balancearse independientemente.
10. Que los valores de Vitalidad, daño, Postura y carga sean viables frente a armas, enemigos, jefes y ataques especiales.

La revisión señaló especialmente el riesgo de que **Descubrimiento, obtención de materiales y recolección** reciban tantas influencias secundarias que cualquier inversión de atributos se convierta en una estrategia dominante para conseguir recursos.

## 11. Orden de trabajo

El orden de desarrollo establecido en las fuentes es:

1. Completar y validar los valores base de todas las estadísticas.
2. Definir las curvas especializadas de los ocho atributos.
3. Establecer los límites de influencia principal y secundaria.
4. Construir las fórmulas de cada estadística.
5. Aplicar las distribuciones iniciales de las clases.
6. Simular personajes especializados y equilibrados.
7. Revisar especialmente Vitalidad máxima y su relación con el daño recibido.

La revisión más reciente añade que las simulaciones deben considerar también escalas independientes para enemigos y que no se fijarán máximos arbitrarios antes de validar las curvas.

## 12. Reglas del sistema que este modelo debe respetar

- No hay regeneración automática de Vitalidad.
- No hay regeneración automática de energías.
- No existe una barra de resistencia física.
- Postura es un sistema independiente.

## 13. Historial de decisiones y estado de vigencia

| Tema | Estado actual |
|---|---|
| Ocho atributos primarios y rango 0–99 | Establecido en las fuentes |
| Origen matemático con atributos en 0 | Establecido como método de cálculo |
| Vitalidad base 500 PV | Establecido; sustituye la propuesta anterior de 100 |
| Ataque físico básico base 25 | Establecido |
| Postura máxima base 100 | Establecido |
| Recuperación de Postura base 5 %/s | Establecido |
| Capacidad de carga base 60 unidades | Establecido |
| Aura, Maná y Prana base 0 | Establecido |
| Curva exacta con tramos 0–10, 10–30 y 30–99 | Definida en la etapa anterior |
| Soft caps adicionales 30–60, 60–80 y 80–99 | Propuesta posterior; requiere función matemática |
| Curvas propias por atributo y estadística | Dirección de diseño establecida; parámetros pendientes |
| Ponderación principal/secundaria 10:1 | Propuesta anterior; no definitiva |
| Presupuesto máximo igual al valor base | Enfoque anterior descartado como restricción general |
| Vitalidad en 99 y demás atributos en 0: objetivo 2.500 PV | Objetivo provisional, no máximo aprobado |
| Máximo anterior de Vitalidad 1.000 PV | Descartado |
| Capacidad máxima de carga 120 | Resultado preliminar descartado como límite |
| Ataque físico máximo 50 | Resultado preliminar descartado como límite |
| Recuperación de Postura máxima 10 %/s | Resultado preliminar descartado como límite |
| Escala de estadísticas de enemigos independiente | Dirección de diseño establecida |

## 14. Pendientes explícitos

No implementar como reglas definitivas hasta resolver:

- La fórmula matemática concreta de las nuevas curvas con soft caps.
- Los parámetros y puntos de transición de cada atributo y estadística.
- Los máximos de contribución de cada relación principal y secundaria.
- La confirmación o sustitución de la ponderación 10:1.
- Los valores base aún no cuantificados, incluidas las estadísticas de carga por categoría.
- Los límites y escalas de Aura, Maná y Prana.
- Los límites de estadísticas ofensivas, defensivas, Postura, estados alterados, movilidad, Artes y exploración.
- La fórmula de combinación de contribuciones, incluyendo operaciones aditivas, porcentuales y multiplicativas.
- La normalización y el tratamiento de atributos iniciales de clase.
- Los parámetros y escalas propios de enemigos y jefes.
- Las simulaciones de balance para distribuciones especializadas, equilibradas y extremas.

**Criterio de cierre:** las decisiones expresamente establecidas se conservan; las fórmulas y cifras identificadas como propuestas permanecen provisionales. Ningún ejemplo de escala ni máximo preliminar debe convertirse en regla de juego sin validación y aprobación.
