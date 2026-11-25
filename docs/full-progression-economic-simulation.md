# Simulación de progresión completa: nivel máximo y equipamiento

**Estado:** simulación de referencia, no balance definitivo.  
**Objetivo:** estimar el esfuerzo económico y el tiempo activo para alcanzar el nivel máximo y obtener equipamiento de máximo nivel mediante una fuente híbrida de ingresos.

## 1. Datos confirmados del modelo

- El personaje comienza en nivel 1.
- Cada punto de atributo comprado aumenta el nivel general exactamente en 1.
- Todas las mejoras de atributo cuestan lo mismo para un mismo nivel.
- La clase no cambia.
- Máximos derivados de los atributos iniciales:
  - Espadachín Mágico: nivel 752 (751 puntos comprados).
  - Guerrero: nivel 753 (752 puntos comprados).
  - Segador: nivel 753 (752 puntos comprados).
- Coste de la mejora cuyo nivel objetivo es x:
  - Tabla DS3 para x=2..12: 673, 689, 706, 723, 740, 757, 775, 793, 811, 829, 847.
  - Para x>=13: round(0.02*x^3 + 3.06*x^2 + 105.6*x - 895).
- La economía usa B(r)=C(r-2), pero r es la referencia económica del objeto/contenido, no necesariamente el nivel del personaje.
- Las almas son moneda y recurso de progresión.
- La simulación anterior propuso, todavía sin aprobación definitiva, costes de refuerzo +1..+10 que suman 0.70 B(r) en materiales y 0.20 B(r) de mano de obra NPC.
- El jugador puede ejercer profesiones, fabricar, procesar recursos y prestar servicios si tiene capacidad.
- Recolectar y fabricar no generan almas por sí mismos; los ingresos llegan mediante ventas o servicios pagados.
- Las pociones curativas tienen cargas limitadas y solo se reponen en puntos de subida de nivel consumiendo pociones fabricadas o compradas. Descansar no las repone gratis.

## 2. Coste exacto de alcanzar el nivel máximo

Se sumó el coste de cada mejora de atributo desde el nivel objetivo 2 hasta el nivel máximo.

| Clase | Nivel máximo | Mejoras compradas | Almas acumuladas para niveles |
|---|---:|---:|---:|
| Espadachín Mágico | 752 | 751 | 2,067,093,093 |
| Guerrero | 753 | 752 | 2,077,445,918 |
| Segador | 753 | 752 | 2,077,445,918 |

La diferencia entre Espadachín Mágico y las otras dos clases procede exclusivamente de sus atributos iniciales y del nivel máximo resultante.

## 3. Dónde se concentra el coste

| Tramo de niveles objetivo | Coste acumulado | Porcentaje del total EM |
|---|---:|---:|
| 2–100 | 1,997,916 | 0.10 % |
| 101–250 | 36,821,334 | 1.78 % |
| 251–500 | 415,602,390 | 20.11 % |
| 501–752 | 1,612,671,453 | 78.02 % |

**Hallazgo crítico:** el 78.02 % del coste total del Espadachín Mágico se concentra en los últimos 252 niveles. La curva hace que el endgame domine la duración total. No es un error aritmético: es una consecuencia directa de la curva cúbica elegida. Pero sí es el mayor riesgo de diseño que revela esta simulación.

## 4. Modelo de ingresos híbridos utilizado para estimar tiempo

No existen todavía tasas validadas de almas netas por hora para combate, profesiones y comercio. Para no presentar conjeturas como datos del juego, se construyó un escenario ilustrativo con estas condiciones:

- 50 % de ingresos netos procedentes del combate y recompensas de encuentros.
- 30 % de servicios profesionales remunerados (por ejemplo, herrería, armería, alquimia o procesamiento por encargo).
- 20 % de comercio, calculado como beneficio neto después de adquirir/procesar mercancías, tasas y transporte.
- No se cuentan dos veces los mismos recursos: recolectar/producir no genera almas; su venta o el servicio remunerado es el punto de ingreso.
- Las tasas netas por hora de cada tramo son hipótesis de simulación, no valores aprobados ni datos observados.

### Escenario híbrido de referencia

| Tramo de niveles objetivo | Coste del tramo | Ingreso neto supuesto | Horas estimadas |
|---|---:|---:|---:|
| 2–100 | 1,997,916 | 5,000 almas/h | 400 h |
| 101–250 | 36,821,334 | 50,000 almas/h | 736 h |
| 251–500 | 415,602,390 | 250,000 almas/h | 1,662 h |
| 501–752 | 1,612,671,453 | 750,000 almas/h | 2,150 h |
| **Total** | **2,067,093,093** | — | **4,949 h** |

Las tasas crecen por tramo porque se supone acceso progresivo a regiones, clientes, recetas, mercados y contratos de mayor valor. Esta tabla no demuestra que el juego ya permita obtener esos ingresos: define qué economía tendría que sostener el ritmo de progresión propuesto.

### Tiempo de calendario según dedicación

Para el escenario de 4,949 horas activas:

| Tiempo de juego semanal | Tiempo aproximado |
|---|---:|
| 8 h/semana | 11.9 años |
| 15 h/semana | 6.3 años |
| 20 h/semana | 4.8 años |
| 30 h/semana | 3.2 años |
| 40 h/semana | 2.4 años |

No se incluyen pausas, tiempo fuera de actividad económica, aprendizaje del jugador ni contenido narrativo no remunerado.

## 5. Sensibilidad: cuánto cambia el resultado con la tasa neta media

Si se simplifica todo el recorrido a una tasa neta media constante, el coste de niveles del Espadachín Mágico requeriría:

| Ingreso neto medio | Horas activas |
|---:|---:|
| 100,000 almas/h | 20,671 h |
| 150,000 almas/h | 13,781 h |
| 250,000 almas/h | 8,268 h |
| 300,000 almas/h | 6,890 h |
| 500,000 almas/h | 4,134 h |
| 1,000,000 almas/h | 2,067 h |

Esta tabla es una sensibilidad matemática, no una predicción. El escenario por tramos es más representativo de una economía que se desbloquea progresivamente.

## 6. Equipamiento de máximo nivel: qué se puede calcular y qué no

El objetivo de equipamiento máximo incluye, como mínimo:

1. Arma de calidad superior fabricada con materiales superiores o conseguida mediante contenido especial.
2. Refuerzo del arma desde +0 hasta +10.
3. Armadura de alta calidad, con sus propias piezas, materiales y posibles mejoras.
4. Accesorios y equipo complementario de alto nivel.
5. Reparación, mantenimiento y consumibles para sostener su uso.
6. Recetas, planos, técnicas, herramientas e instalaciones profesionales necesarias.

La propuesta preliminar de refuerzo equivale a 0.70 B(r) en materiales y 0.20 B(r) en mano de obra NPC por arma, totalizando 0.90 B(r). Si el jugador realiza el trabajo, evita pagar la mano de obra NPC, pero no elimina el coste de oportunidad de su tiempo, herramientas, instalaciones ni materiales.

**No se asigna un r=752 automáticamente al mejor equipo.** La referencia económica del arma, armadura o receta debe corresponder a su región, rareza, calidad y complejidad. Todavía no existen tablas de materiales por nivel de refuerzo, recetas de equipo final, costes de armadura/accesorios ni tiempos de fabricación. Por tanto, sumar un precio exacto del mejor equipo al coste de niveles sería inventar datos.

En consecuencia, las 4,949 horas son una estimación de alcanzar el nivel máximo en el escenario híbrido, no el tiempo completo certificado para alcanzar también el mejor equipamiento. El tiempo total real será mayor y dependerá de los requisitos de obtención, fabricación y mejora del equipo.

## 7. Diagnóstico de balance

### Resultado

La curva de niveles es matemáticamente consistente, pero su coste final es extraordinariamente concentrado: casi cuatro quintas partes de las almas se necesitan entre los niveles 501 y 752. Para que el nivel máximo sea alcanzable en un tiempo razonable, la economía de endgame tendría que sostener ingresos netos muy altos.

### Riesgos detectados

- Si los ingresos de profesiones y comercio no escalan con el endgame, el progreso se vuelve excesivamente lento.
- Si el comercio permite beneficio garantizado por arbitraje, la profesión comercial eclipsará el combate.
- Si los enemigos escalan con el jugador y entregan recompensas desproporcionadas, el farmeo puede convertirse en la única ruta óptima.
- Si el coste de equipo final compite directamente con las mejoras de atributos, el jugador puede quedar atrapado entre progresión y equipamiento.
- El jugador artesano ahorra tarifas NPC, pero debe pagar materiales, herramientas, instalaciones, mantenimiento y tiempo.

### Ajustes recomendados para probar, no aplicar todavía

1. Simular la curva con datos de ingresos netos reales por hora en cada tramo.
2. Definir una proporción objetivo entre progreso de nivel, equipamiento y gastos de expedición.
3. Probar una curva de endgame menos concentrada únicamente si los playtests muestran que la duración es excesiva.
4. Mantener las tres rutas —combate, profesión/comercio e híbrida— viables, sin garantizar que todas generen la misma cantidad de almas por hora.
5. No reducir costes ni aumentar recompensas hasta contar con telemetría de una economía funcional.

## 8. Datos que faltan para cerrar la simulación total

- Ingreso neto por hora de combate por tramo.
- Ingreso neto por hora de cada profesión.
- Margen neto de comercio tras transporte, tasas y materiales.
- Cantidad y calidad de materiales requeridos para cada refuerzo +1 a +10.
- Recetas, requisitos y costes del mejor equipo de cada clase.
- Costes de reparación, instalaciones y herramientas.
- Consumo medio de pociones por expedición y coste de reabastecimiento.
- Tiempo necesario para desbloquear recetas, maestros y zonas de recursos.

**Conclusión:** con la curva actual, el coste de niveles hasta el máximo está entre 2.067 y 2.077 mil millones de almas, según la clase. Un escenario híbrido de referencia arroja unas 4,949 horas activas para el Espadachín Mágico, pero esa duración depende de tasas de ingreso hipotéticas y no incluye todavía el tiempo adicional para conseguir el mejor equipo. No se modifican las reglas ni los multiplicadores en esta fase: la prueba identifica que el endgame económico y la curva de niveles necesitan validación antes de declararse equilibrados.
