# Catálogo económico provisional de los 60 jefes de la primera vuelta

**Estado:** modelo paramétrico y escenario de sensibilidad; no es una tabla de recompensas definitiva.  
**Alcance:** 49 jefes normales, 10 jefes especiales y 1 jefe final.  
**Propósito:** cuantificar el peso económico de los jefes sin inventar niveles de referencia, condiciones de bonificación ni ubicaciones que todavía no están definidas.

## 1. Distribución del catálogo

| Identificadores | Categoría | Cantidad | Multiplicador de contenido provisional |
|---|---|---:|---:|
| N01–N49 | Jefe normal | 49 | ×1 |
| E01–E10 | Jefe especial | 10 | ×2.5 |
| F01 | Jefe final | 1 | ×5 |
| **Total** | | **60** | |

Los multiplicadores ×1, ×2.5 y ×5 son los coeficientes experimentales aceptados para esta primera simulación. No son reglas finales ni sustituyen el nivel económico, el peligro real o las características de cada encuentro.

## 2. Fórmula de recompensa base

Se conserva la fórmula de recompensas del modelo de combate:

[
R_{base,i}=round(B(r_i)\cdot0.03\cdot15\cdot M_{contenido,i})
]

Equivalente a:

[
R_{base,i}=round(0.45\cdot B(r_i)\cdot M_{contenido,i})
]

Donde:
- (r_i) es el nivel económico de referencia propio del jefe, acorde con su región y dificultad.
- (B(r)=C(r-2)) es la unidad económica de referencia actualmente documentada.
- (0.03) es el coeficiente inicial de recompensa de enemigo común.
- (15) es el multiplicador de amenaza de jefe.
- (M_{contenido}) es ×1, ×2.5 o ×5 según la categoría provisional anterior.

**Importante:** la referencia económica (B(r)) sigue la función documentada en el modelo de combate. La curva acelerada de coste de nivel del documento de rebalanceo no se aplica automáticamente a (B(r)). Son dos parámetros relacionados, pero distintos. Si se decide que las recompensas de enemigos también deben seguir la curva acelerada, habrá que versionar esa decisión y recalcular todo el catálogo, no mezclar ambas curvas silenciosamente.

## 3. Recompensas de referencia si todos los jefes tuvieran el mismo r

La tabla siguiente no afirma que todos los jefes tengan el mismo nivel económico. Es una prueba de sensibilidad que permite ver el orden de magnitud de las recompensas con la fórmula vigente.

| Referencia r | B(r) | Cada jefe normal | Cada especial | Jefe final | Total de los 60 |
|---:|---:|---:|---:|---:|---:|
| 13 | 829 | 373 | 933 | 1,865 | 29,472 |
| 25 | 3,396 | 1,528 | 3,820 | 7,641 | 120,713 |
| 50 | 13,436 | 6,046 | 15,116 | 30,231 | 477,645 |
| 75 | 30,901 | 13,905 | 34,764 | 69,527 | 1,098,512 |
| 100 | 57,666 | 25,950 | 64,874 | 129,748 | 2,050,038 |
| 150 | 146,596 | 65,968 | 164,920 | 329,841 | 5,211,473 |
| 200 | 295,226 | 132,852 | 332,129 | 664,258 | 10,495,296 |
| 225 | 396,616 | 178,477 | 446,193 | 892,386 | 14,099,689 |

Los totales se calculan sumando 49 recompensas normales, 10 especiales y una final, redondeando cada recompensa individual al alma entera más cercana. Los valores pueden diferir en unas pocas almas de cálculos que redondeen después de sumar.

## 4. Fórmula real para el catálogo heterogéneo

En la campaña real, cada jefe tendrá su propio (r_i). Por tanto, el total base será:

[
R_{jefes}=
\sum_{i=1}^{49}round(0.45B(r_{N_i}))
+
\sum_{j=1}^{10}round(1.125B(r_{E_j}))
+
round(2.25B(r_{F}))
]

No se debe multiplicar una recompensa ya redondeada para obtener la categoría siguiente; cada recompensa se calcula directamente desde su fórmula y se redondea una sola vez.

La tabla de sensibilidad no sustituye este cálculo individual. Para cerrar el total de campaña necesitamos asignar a cada uno de los 60 jefes su referencia económica real.

## 5. Bonificaciones adicionales: contabilidad separada

La recompensa final de un jefe se registra en componentes separados:

[
R_{final,i}=R_{base,i}+R_{overkill,i}+R_{especial,i}+R_{unico,i}
]

- **Base:** se concede una sola vez al confirmar la derrota definitiva.
- **Overkill:** usa la fórmula ya definida en el modelo de combate; máximo +50 % de la recompensa base. Se calcula usando el daño efectivo del evento final y la Vitalidad máxima total del jefe, incluyendo todas sus barras.
- **Especial:** recompensa adicional por cumplir una condición propia del jefe especial. La condición, cuantía y si puede obtenerse en la primera o en sucesivas derrotas deben definirse en su ficha. No se inventa un porcentaje común.
- **Único:** recompensa independiente por primera derrota, objetivo opcional u otro logro de encuentro, solo si el diseño lo establece.

No se debe pagar la recompensa base por cada fase, barra de Vitalidad o Muertebendita intermedia. Las bonificaciones que se conceden una sola vez deben tener una marca persistente de reclamación para impedir duplicados tras recargar, abandonar y reingresar o repetir una fase.

Las recompensas de jefe no deben duplicarse por invocaciones, daño de aliados, daño ambiental o reaparición de actores. El evento que concede almas debe ser la resolución autoritativa de derrota definitiva del encuentro.

## 6. Efecto normalizado de los 60 jefes

Con los multiplicadores provisionales, si todos tuvieran la misma referencia económica, el conjunto equivale a:

- 49 unidades de recompensa de jefe normal.
- 25 unidades por los 10 jefes especiales.
- 5 unidades por el jefe final.
- **79 unidades equivalentes de jefe normal.**

Sin multiplicadores especiales, los 60 encuentros equivaldrían a 60 unidades. La hipótesis actual incrementa en 31.67 % la recompensa base conjunta respecto a pagar a todos como jefes normales de la misma referencia.

Este 31.67 % solo describe la ponderación por categoría. No es el incremento definitivo de almas de campaña: los niveles económicos individuales y las bonificaciones aún no están incluidos.

## 7. Qué falta para calcular cuánto reducen el tiempo de progresión

Para cada jefe necesitamos:
1. Identificador y nombre interno.
2. Categoría (normal, especial o final).
3. Región, orden aproximado y ciclo.
4. Nivel económico (r_i).
5. Número de barras/fases y Vitalidad máxima total.
6. Recompensa base calculada.
7. Condiciones y cuantía de bonificaciones especiales.
8. Si existe recompensa de primera derrota u objetivo opcional.
9. Tiempo esperado de encuentro y probabilidad de que un jugador normal lo derrote durante la ruta principal.

Con esos datos se calculará el total de almas de jefes realmente obtenible en una primera vuelta. Después se añadirá a la simulación de ingresos de enemigos normales y especiales no jefes, servicios profesionales y comercio neto. Se publicarán dos tiempos comparables para nivel 100 y 225: sin jefes y con el catálogo completo de jefes.

No se debe convertir el total teórico de los 60 jefes en ingreso por hora dividiéndolo por una duración arbitraria. Hay que modelar cuándo se encuentran, cuánto tarda cada combate y qué parte de la recompensa se espera que obtenga un jugador que no completa contenido opcional.

## 8. Conclusión de esta iteración

La estructura de 60 jefes y la ponderación experimental ya permiten calcular recompensas de forma consistente cuando se conozcan sus referencias económicas. La simulación muestra la sensibilidad y el peso relativo de los 10 especiales y el jefe final, pero no autoriza todavía a afirmar cuántas horas se ahorrarán para alcanzar los niveles 100 o 225.

**Decisión pendiente:** validar los multiplicadores ×2.5 y ×5 y completar las fichas de los 60 encuentros. Hasta entonces, todos los valores de este documento permanecen como escenario provisional de balance.


## 9. Duración estimada de los encuentros y tiempo total de combate contra jefes

Se establece como supuesto de diseño para esta simulación que cada pelea contra un jefe dura entre **3 y 7 minutos** de combate efectivo.

Para los 60 encuentros únicos de la primera vuelta:

| Escenario | Duración por jefe | Tiempo acumulado de los 60 |
|---|---:|---:|
| Mínimo | 3 min | 180 min = 3 h |
| Referencia media | 5 min | 300 min = 5 h |
| Máximo | 7 min | 420 min = 7 h |

La media de 5 minutos es el punto central del intervalo, no una duración medida en pruebas de juego.

Este tiempo representa únicamente el combate efectivo contra los jefes. No incluye desplazamiento hasta los encuentros, exploración, preparación, gestión de inventario, diálogos, intentos fallidos, reintentos ni descansos. Tampoco presupone que todos los jugadores derroten a los 60 jefes en su primera ruta.

Para el cálculo de progresión, las recompensas de los jefes deben asignarse al momento en que se derrota cada encuentro. No se debe convertir todo el total de almas de jefes en una tasa horaria uniforme ni sumarlo a los ingresos de enemigos normales sin respetar el tiempo y el orden de la ruta. La simulación de campaña debe añadir estos 3–7 h de combate efectivo a los tiempos de los demás encuentros y actividades, y acreditar cada recompensa una sola vez tras la derrota correspondiente.

El supuesto de duración debe validarse posteriormente con pruebas de combate representativas (jefe normal, especial y final), incluyendo diferentes clases, configuraciones de equipo y habilidad del jugador. Si la duración media real difiere, se recalculará el total de horas sin alterar automáticamente las recompensas.


## 10. Rentabilidad temporal y contraste con la curva acelerada

Para comparar recompensas y duración se utiliza el supuesto de 3–7 minutos por encuentro, con 5 minutos como referencia central. Si los 60 encuentros duran lo mismo dentro de cada escenario, el tiempo total de combate efectivo es 3, 5 o 7 horas.

La tasa horaria siguiente es una **normalización matemática del total de recompensas de los 60 jefes**, no una promesa de que el jugador recibirá todas esas almas uniformemente durante una hora. En la ruta real las recompensas llegan en momentos discretos y cada jefe tiene su propio nivel económico.

| Referencia económica común | Total de los 60 | Almas/h a 3 h | Almas/h a 5 h | Almas/h a 7 h |
|---:|---:|---:|---:|---:|
| 25 | 120.713 | 40.238 | 24.143 | 17.245 |
| 50 | 477.645 | 159.215 | 95.529 | 68.235 |
| 75 | 1.098.512 | 366.171 | 219.702 | 156.930 |
| 100 | 2.050.038 | 683.346 | 410.008 | 292.863 |
| 150 | 5.211.473 | 1.737.158 | 1.042.295 | 744.496 |
| 200 | 10.495.296 | 3.498.432 | 2.099.059 | 1.499.328 |
| 225 | 14.099.689 | 4.699.896 | 2.819.938 | 2.014.241 |

### 10.1 Comparación con la progresión del personaje

La curva acelerada candidata estima un coste acumulado de **1.138.689 almas** para llegar al nivel 100. Como comprobación de tensión económica:

- Si los 60 jefes tuvieran todos referencia económica 50, sus recompensas base sumarían unas 477.645 almas, aproximadamente el 42 % del coste acumulado hasta nivel 100.
- Si todos tuvieran referencia 75, sumarían unas 1.098.512 almas, aproximadamente el 96 % de ese coste.
- Si todos tuvieran referencia 100, sumarían unas 2.050.038 almas, aproximadamente el 180 % de ese coste.

Estos escenarios **no describen la distribución real de la campaña**. Sirven para detectar que asignar referencias altas a todos los jefes podría hacer que los jefes, por sí solos, financiaran casi toda o más de toda la progresión hasta nivel 100, antes de contar enemigos normales, enemigos especiales no jefes, servicios, comercio, exploración y logros.

Por tanto, no se debe asignar (r=75) o (r=100) indiscriminadamente a los 60 jefes. Cada (r_i) debe derivarse de su región, orden de aparición, amenaza efectiva y curva de dificultad. También hay que comprobar que la fórmula de recompensas (B(r)) y la curva acelerada de coste de nivel estén calibradas entre sí; no son la misma función y el cambio de una no actualiza automáticamente la otra.

### 10.2 Rentabilidad por categoría

La recompensa/minuto de cada jefe se calcula como su recompensa individual dividida por la duración real del encuentro. Bajo la hipótesis de una misma referencia económica para todos, el jefe especial produce 2,5 veces la recompensa base del jefe normal y el final 5 veces. Si la duración también fuera igual, sus almas por minuto guardarían esa misma proporción. Pero aún no se ha establecido que las tres categorías duren lo mismo: los 3–7 minutos son por ahora un intervalo común provisional, no un calendario diferenciado por categoría.

La prueba de balance deberá comparar:
- recompensa base por minuto;
- recompensa total por minuto, incluyendo bonificaciones realmente obtenibles;
- duración efectiva y número de intentos;
- coste de consumibles, reparaciones y preparación;
- porcentaje de jugadores que derrotan cada jefe en la ruta principal;
- oportunidad de obtener esas almas frente a actividades alternativas.

### 10.3 Conclusión provisional

El tiempo de combate total de los 60 jefes se sitúa entre 3 y 7 horas, con 5 horas como referencia central. El modelo actual de recompensas puede representar una fracción pequeña o una cantidad excesiva de la progresión hasta nivel 100, dependiendo de las referencias económicas asignadas.

No se reduce todavía la estimación de 40–60 horas al nivel 100 ni la estimación aproximada de 306 horas al nivel 225. Para recalcularlas correctamente falta completar la asignación de (r_i) de los 60 jefes y sus bonificaciones, y después integrar el calendario de encuentros con las recompensas de enemigos no jefes, ingresos profesionales y comercio neto. La tasa horaria no debe confundirse con la duración total de campaña.
