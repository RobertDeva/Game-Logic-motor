# Modelo de crecimiento de atributos y estadísticas derivadas

**Proyecto:** Mythical Exodus  
**Estado:** especificación de diseño en consolidación; no todas las fórmulas ni los parámetros están cerrados.  
**Propósito:** registrar las decisiones y propuestas matemáticas recuperadas de las conversaciones de diseño, distinguiendo lo establecido de lo que todavía requiere aprobación o calibración.

## 1. Alcance y criterio rector

Este documento recoge el modelo de crecimiento de los atributos primarios y su influencia sobre las estadísticas derivadas del personaje.

La matriz de relaciones entre atributos y estadísticas entregada durante el diseño es la referencia funcional. Las fórmulas deben adaptarse a esa matriz: no se eliminarán relaciones principales o secundarias para simplificar los cálculos.

La nueva distribución de influencias de **Aura, Maná y Prana** sustituye la ponderación igualitaria anterior. El modelo matemático debe respetar esa distribución. Los valores concretos de la matriz deben consultarse en su fuente original; no se reproducen aquí porque no aparecen completos en los fragmentos recuperados.

## 2. Reglas y criterios establecidos

- Cada atributo tiene un intervalo de 0 a 99 y un máximo de **99**.
- El crecimiento de la contribución de un atributo se divide en **tres fases**:
  1. Primer ritmo constante, desde el valor inicial hasta un punto de transición.
  2. Segundo ritmo constante, desde ese punto de transición hasta 30. Puede tener una pendiente distinta de la primera.
  3. Rendimiento parabólico decreciente desde 30 hasta 99.
- La disminución del rendimiento comienza exactamente después de 30.
- En la tercera fase la contribución acumulada continúa aumentando, pero cada punto adicional aporta menos que el anterior.
- En 99 se alcanza el máximo de contribución potencial de esa relación atributo-estadística.
- Las influencias principales y secundarias conservan límites de contribución diferentes.
- Una estadística puede recibir contribuciones de varios atributos.
- Cada estadística debe tener un límite global que impida que la acumulación de contribuciones produzca valores descontrolados.
- Las diferencias entre los atributos iniciales de las clases deben considerarse en el cálculo.
- No se deben fijar valores absolutos de estadísticas sin contrastarlos con el balance de combate correspondiente.

## 3. Función de crecimiento vigente

La arquitectura vigente es la curva de tres fases registrada en la última revisión recuperada:

\[
F(A)=
\begin{cases}
r_1 A, & A\leq b\\[4pt]
r_1b+r_2(A-b), & b<A\leq30\\[4pt]
F(30)+r_2(A-30)-\dfrac{r_2}{138}(A-30)^2, & A>30
\end{cases}
\]

Donde:

- \(A\): valor actual del atributo.
- \(b\): punto de transición entre los dos primeros ritmos constantes.
- \(r_1\): primer ritmo de crecimiento.
- \(r_2\): segundo ritmo de crecimiento.
- \(F(30)\): valor acumulado de la función al alcanzar 30.

### Comportamiento esperado

- **Fase 1:** crecimiento lineal a ritmo constante.
- **Fase 2:** crecimiento lineal a un segundo ritmo constante, que puede diferir del primero.
- **Fase 3:** crecimiento parabólico con rendimiento marginal decreciente.
- La función conserva continuidad en los puntos de unión.
- En 30 comienza la disminución progresiva de la pendiente.
- La derivada de la tercera fase llega a cero en 99; por tanto, el último punto alcanza el máximo de contribución sin que la curva retroceda.

Los parámetros \(b\), \(r_1\) y \(r_2\) todavía deben calibrarse. No se han fijado valores numéricos definitivos para ellos.

## 4. Contribuciones de atributos a estadísticas

Cada relación entre un atributo y una estadística tendrá su propio máximo de contribución. Se utilizará una notación general:

\[
C_{ij}(A_i)=M_{ij}\cdot G_i(A_i)
\]

Donde:

- \(C_{ij}\): contribución del atributo \(i\) a la estadística \(j\).
- \(M_{ij}\): contribución máxima de esa relación.
- \(G_i\): función de crecimiento normalizada para la relación, basada en la curva de tres fases.
- \(A_i\): valor actual del atributo.

La normalización debe tener en cuenta el valor inicial de atributo de cada clase para evitar contar dos veces los beneficios de partida. En una revisión anterior se propuso:

\[
G_i(A_i)=
\frac{F(A_i)-F(A_{i,0})}
{F(99)-F(A_{i,0})}
\]

con \(A_{i,0}\) como valor inicial del atributo para la clase. Esta expresión representa el criterio de normalización por clase; debe verificarse en la implementación matemática final, especialmente cuando se definan los valores iniciales y los límites de cada relación.

La suma de contribuciones debe respetar el límite global propio de cada estadística. La fórmula concreta de combinación y la política de saturación todavía deben definirse y simularse.

## 5. Influencias principales y secundarias

Las influencias principales deben tener un límite de contribución superior al de las secundarias. Las contribuciones secundarias deben seguir siendo significativas, sin que la acumulación de muchas de ellas convierta una estadística en una estrategia dominante.

En la revisión de Vitalidad máxima se presentó como **propuesta**, no como parámetro definitivamente aprobado, una relación de peso 10:1 entre influencia principal y secundaria. Bajo esa hipótesis:

- Vitalidad, como influencia principal, aportaría el 58,82 % del crecimiento adicional distribuido.
- Cada uno de los siete atributos secundarios aportaría el 5,88 %.
- Los siete secundarios sumarían el 41,18 % restante.

Estos porcentajes no deben tratarse como valores definitivos hasta que se confirme la proporción y se pruebe con simulaciones.

## 6. Vitalidad máxima: relaciones recuperadas

La matriz descrita en la revisión disponible asigna las siguientes influencias a Vitalidad máxima:

| Atributo | Tipo de influencia |
|---|---|
| Vitalidad | Principal |
| Fuerza | Secundaria |
| Agilidad | Secundaria |
| Resistencia | Secundaria |
| Voluntad | Secundaria |
| Ruptura | Secundaria |
| Destreza | Secundaria |
| Armonía | Secundaria |

La forma general propuesta para combinar las contribuciones es:

\[
PV_{\max}=PV_{\text{base}}+\sum_{i=1}^{8}C_i(A_i)
\]

El valor absoluto de Vitalidad máxima, sus límites globales y los máximos individuales de cada contribución siguen pendientes. Deben equilibrarse frente al daño de armas, enemigos, jefes y ataques especiales. No se debe escoger un máximo arbitrario sin esa comprobación.

## 7. Auditoría y riesgos de balance identificados

La auditoría debe comprobar, como mínimo:

1. Que todas las relaciones de la matriz estén representadas y que no existan contradicciones.
2. Que las influencias principales sean perceptiblemente superiores a las secundarias.
3. Que la acumulación de influencias no haga que una estadística con muchas relaciones crezca de forma desproporcionada.
4. Que invertir en cualquier atributo no se convierta automáticamente en la mejor estrategia para obtener recursos.
5. Que las diferencias de atributos iniciales entre clases se reflejen correctamente.
6. Que especializarse tenga ventajas reales y que las distribuciones equilibradas también sean viables.
7. Que alcanzar 99 en un atributo no vuelva irrelevantes a los demás.
8. Que los límites globales se respeten incluso con distribuciones extremas.

Se identificó expresamente que **Vitalidad máxima** recibe influencias de los ocho atributos y, por tanto, necesita una combinación y un límite global cuidadosamente calibrados. También se señaló que **Descubrimiento, obtención de materiales y recolección** reciben influencias secundarias de todos los atributos; se debe evitar que esa acumulación convierta cualquier inversión de puntos en una estrategia dominante para conseguir recursos.

## 8. Orden de trabajo matemático

El trabajo de definición y balance seguirá este orden:

1. Auditar la matriz completa de relaciones entre atributos y estadísticas.
2. Revisar y establecer los límites de las estadísticas, comenzando por Vitalidad máxima, cuyos valores absolutos quedaron pendientes.
3. Definir los máximos de contribución de cada relación, distinguiendo influencias principales y secundarias.
4. Calibrar los parámetros de las curvas de crecimiento.
5. Construir las fórmulas de combinación y sus restricciones globales.
6. Simular distribuciones especializadas, equilibradas y extremas, teniendo en cuenta las clases y sus atributos iniciales.
7. Ajustar el modelo y documentar las fórmulas definitivas una vez superadas las pruebas de balance.

Las familias de estadísticas que se deben cubrir incluyen Aura, Maná y Prana; capacidades ofensivas y críticas; defensas y absorciones; Postura y equilibrio; estados alterados; movilidad y capacidad de carga; rendimiento de combate y Artes; y descubrimiento y exploración.

## 9. Reglas del sistema que deben respetarse

Este modelo de estadísticas no debe contradecir las reglas de recursos ya establecidas:

- No hay regeneración automática de Vitalidad.
- No hay regeneración automática de energías.
- No existe una barra de resistencia física.
- Postura es un sistema independiente.

## 10. Historial de evolución de la curva

Las fuentes recuperadas contienen varias formulaciones sucesivas. Para evitar que una versión anterior se confunda con la vigente:

1. Se planteó inicialmente una curva parabólica cóncava de dos tramos.
2. Después se estableció que el crecimiento debía ser lineal hasta 30 y decreciente a partir de ese punto.
3. Se corrigió una discontinuidad de pendiente de aquella formulación mediante una parábola que se unía suavemente al tramo lineal.
4. Finalmente, se precisó la estructura de **tres fases**: dos ritmos constantes antes de 30 y una fase parabólica decreciente desde 30 hasta 99.

Por tanto, las formulaciones anteriores son antecedentes del diseño. La función de la sección 3 es la arquitectura más reciente encontrada en las fuentes consultadas y es la que debe utilizarse como referencia de trabajo, sin considerar cerrados sus parámetros.

## 11. Pendiente de definición o confirmación

Los siguientes elementos no deben darse por aprobados ni inventarse al implementar:

- La matriz completa y sus pesos concretos para todas las estadísticas, si no se dispone de la fuente íntegra.
- El punto de transición \(b\) entre los dos primeros ritmos.
- Los valores de \(r_1\) y \(r_2\), por atributo o familia de estadísticas.
- Los máximos absolutos de contribución de cada relación.
- La confirmación de la proporción 10:1 entre influencia principal y secundaria.
- Los límites globales de cada estadística.
- Los valores absolutos de Vitalidad máxima y su valor base.
- La fórmula definitiva de combinación, saturación y redondeo.
- Los resultados de simulaciones que validen las distribuciones de atributos y clases.

**Criterio de cierre:** una fórmula solo podrá considerarse definitiva después de comprobar su coherencia con la matriz, sus límites y las simulaciones de balance. Las propuestas numéricas anteriores no se convertirán en reglas por el mero hecho de aparecer en una formulación preliminar.
