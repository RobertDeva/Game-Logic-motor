# New Game+ y escalado global del mundo

**Estado:** propuesta de diseño derivada de la visión del proyecto. Los principios descritos aquí quedan como dirección de diseño; las fórmulas, multiplicadores, límites y recompensas concretas siguen pendientes de simulación y aprobación.

## 1. Propósito

New Game+ (NG+) permite continuar la aventura después de completar la campaña principal, conservando la progresión del personaje y afrontando una versión más exigente del mundo. Está inspirado en los ciclos de Dark Souls, adaptado a Mythical Exodus.

No es un modo separado ni un simple aumento de estadísticas: es una nueva vuelta del mundo, con mayor peligro, mejores oportunidades y una economía que debe seguir siendo coherente.

El largo recorrido hacia el nivel máximo y el equipamiento superior puede convivir con NG+: quienes disfrutan de la progresión extensa pueden seguir perfeccionando su personaje y afrontar ciclos posteriores.

## 2. Principios rectores

1. **Conservar al personaje:** atributos, nivel, clase, Artes de combate, conocimientos, recetas, dominio profesional, inventario, equipo y progreso permanente se conservan, salvo excepciones narrativas que se definan expresamente.
2. **Reiniciar el mundo jugable:** enemigos derrotados, jefes, encuentros, estados de misiones, tesoros y ciertos recursos recuperables vuelven a su estado de ciclo, según reglas específicas.
3. **Escalado global:** todas las regiones y familias de enemigos participan en el escalado del ciclo; ninguna zona queda completamente congelada en la dificultad del primer recorrido.
4. **Escalado contextual:** el aumento no debe ser idéntico para todos. Enemigos comunes, fuertes, élites y jefes pueden tener curvas diferentes, conservando sus patrones, resistencias, fases y personalidad.
5. **Mantener identidad y legibilidad:** los enemigos antiguos siguen siendo reconocibles y peligrosos por sus mecánicas, no solo por acumular Vitalidad y daño.
6. **Recompensas proporcionales:** almas, botín y materiales deben reflejar el peligro y los costes del ciclo, sin crear inflación o explotación económica.
7. **Todas las rutas siguen siendo viables:** combate, profesiones, comercio y juego híbrido deben poder sostenerse en ciclos avanzados.

## 3. Entrada a New Game+

La entrada a NG+ se habilita al completar el hito final de la campaña principal. El jugador decide cuándo iniciar el siguiente ciclo desde un punto seguro o mediante una opción de mundo claramente identificada.

Antes de confirmar, el juego debe explicar qué estados del mundo se reiniciarán y qué progreso se conservará. No se debe iniciar automáticamente ni borrar progreso sin consentimiento.

La posibilidad de seguir explorando el ciclo actual después del final, antes de iniciar NG+, debe mantenerse si la estructura narrativa lo permite.

## 4. Qué se conserva y qué se reinicia

### Se conserva (propuesta)

- Clase permanente y atributos.
- Nivel general y almas no gastadas.
- Artes de combate desbloqueadas, mejoradas y sus puntos acumulados.
- Equipo, armas, refuerzos +0 a +10, armaduras y accesorios.
- Inventario, materiales y objetos fabricados.
- Recetas, planos, conocimientos, descubrimientos y entradas de enciclopedia.
- Dominio y técnicas profesionales aprendidas.
- Mejoras permanentes del personaje y capacidades desbloqueadas.

### Se reinicia o vuelve a estar disponible (propuesta)

- Enemigos y jefes derrotados.
- Encuentros y recompensas de ciclo.
- Estados de misiones y sucesos que dependan de la progresión de campaña.
- Tesoros y botines definidos como recuperables por ciclo.
- Estados de control territorial y eventos del mundo ligados al ciclo.

### Debe definirse por sistema

- Poblaciones ecológicas, migraciones, territorios y cambios causados por el jugador.
- Inventarios de comerciantes, contratos y precios dinámicos.
- Relaciones y reputación con NPC.
- Recursos naturales agotables y regeneración.
- Acceso a zonas, atajos y puntos de descanso.
- Mejoras permanentes de infraestructura y talleres.

El reinicio no debe borrar arbitrariamente el conocimiento o las capacidades profesionales del personaje. Las consecuencias narrativas y ecológicas requieren reglas propias.

## 5. Escalado global de dificultad

Cada ciclo tiene un índice NG (primer recorrido = ciclo 0 o ciclo 1, convención por fijar). La dificultad de cada contenido se calcula a partir de:

- Referencia de nivel de la región.
- Categoría y perfil del enemigo.
- Índice de ciclo.
- Rol del enemigo y mecánicas particulares.
- Fases, barras de Vitalidad, Postura, resistencias y capacidades.
- Contexto de encuentro y composición del grupo.

Forma conceptual:

NivelAmenaza = Escalar(ReferenciaZona, CategoriaEnemigo, CicloNG, PerfilContenido)

El escalado puede ajustar independientemente:

- Vitalidad y número/composición de barras cuando el diseño del jefe lo justifique.
- Daño, Postura, defensas y resistencias.
- Recuperación de Postura y presión ofensiva.
- Comportamientos, combinaciones, ventanas y variantes de encuentros.
- Composición y coordinación de grupos.

No se debe multiplicar todo por un único factor ciego. Las mecánicas esenciales de jefes y enemigos deben seguir funcionando, y los aumentos no deben convertir ataques inevitables en muertes instantáneas.

## 6. El mundo entero participa

“Escalado global” significa que el ciclo afecta a las amenazas del mundo completo, no que cada sistema reciba automáticamente el mismo multiplicador.

- **Regiones tempranas:** continúan siendo relativamente menos peligrosas que las regiones avanzadas dentro del mismo ciclo, pero sus enemigos aumentan respecto al ciclo anterior.
- **Regiones avanzadas:** conservan su mayor exigencia y también escalan.
- **Jefes:** escalan según su identidad, fases y mecánicas; no se reducen a sacos de Vitalidad.
- **Encuentros y grupos:** pueden adquirir composiciones o variantes más exigentes.
- **Criaturas y ecología:** deben conservar relaciones, territorios y comportamientos; el ciclo puede habilitar variantes más peligrosas sin invalidar la simulación ecológica.
- **Recursos y materiales:** ciclos superiores pueden abrir variantes, calidades o fuentes adicionales, pero no deben hacer obsoletas todas las zonas anteriores.
- **NPC y servicios:** los maestros y artesanos mantienen su función; pueden ofrecer desafíos, encargos o productos de ciclo superior cuando sea coherente.

El escalado del mundo no significa que todos los animales, civiles o criaturas pacíficas se conviertan en enemigos de alto nivel. Se escala su amenaza y resistencia cuando corresponda a su función.

## 7. Recompensas y economía por ciclo

La recompensa de enemigos debe seguir la fórmula general existente, basada en referencia económica, amenaza y contenido. NG+ introduce el ciclo como factor explícito, todavía sin coeficiente aprobado:

Recompensa = RecompensaBase(referencia, amenaza, contenido) × ModificadorCiclo

Reglas:

- Las almas obtenidas deben crecer de forma coherente con los costes y riesgos del ciclo.
- El bono por exceso de daño no debe duplicarse ni volverse trivial por el escalado.
- Botín y materiales pueden tener variantes o calidades de ciclos superiores.
- No se debe garantizar rentabilidad por repetir una ruta segura o comerciar consigo mismo.
- Los precios no aumentan simplemente por el nivel del personaje; cualquier ajuste de ciclo debe derivarse de oferta, demanda, escasez, región y costes reales.
- El comercio y los servicios profesionales deben conservar márgenes razonables después de gastos.
- Los recursos producidos no generan almas hasta que se venden o se presta un servicio remunerado, conforme a la regla económica existente.

El equilibrio de NG+ debe evaluar conjuntamente coste de niveles, equipamiento, refuerzo +10, reparación, consumibles y recompensas. No basta con aumentar almas sin aumentar los costes relevantes, ni aumentar los costes sin ofrecer oportunidades proporcionales.

## 8. Equipamiento y progresión en NG+

- El equipo +10 sigue siendo útil; NG+ no debe invalidarlo inmediatamente.
- Materiales superiores y recetas de ciclo pueden ofrecer especialización, propiedades o alternativas, no solo números mayores.
- La fabricación propia sigue siendo una ruta válida.
- Los NPC pueden ofrecer servicios de alto nivel, pero no deben monopolizar mejoras que el jugador pueda aprender y realizar.
- El escalado de enemigos debe considerar la combinación de atributos, equipo, Artes de combate y capacidades, no solo el nivel general.
- Debe evitarse una espiral en la que el jugador necesita equipo de NG+ para vencer enemigos de NG+, pero solo puede conseguir ese equipo después de vencerlos.

## 9. Curación y preparación

Se mantienen las reglas de curación ya establecidas:

- Capacidad limitada de cargas.
- Las pociones se fabrican o adquieren mediante el sistema de alquimia.
- Solo se reponen cargas en puntos de subida de nivel, consumiendo pociones disponibles en el inventario.
- Descansar no concede curación gratuita ni crea pociones.

En ciclos superiores, la presión de combate puede exigir mejor preparación, pero no se debe resolver el desafío aumentando arbitrariamente el consumo obligatorio de pociones.

## 10. Pruebas de equilibrio obligatorias

Antes de fijar multiplicadores de ciclo, simular al menos:

1. Personaje que inicia NG+ con equipo de campaña básico.
2. Personaje con armas +10 y equipo excepcional.
3. Especialista en combate.
4. Especialista en profesiones y comercio.
5. Personaje híbrido.
6. Jugador que regresa a zonas tempranas.
7. Jugador que intenta explotar enemigos o rutas repetibles.
8. Jefes con varias barras y enemigos resistentes a Muertebendita.
9. Economía de materiales, reparación y pociones en ciclos consecutivos.
10. Ciclos sucesivos para comprobar que la dificultad no crece sin límite práctico ni se vuelve trivial.

Métricas: tiempo para derrotar y ser derrotado, gasto de curación por expedición, almas netas por hora, coste de mejora, disponibilidad de materiales, tasa de éxito de fabricación y diversidad de rutas viables.

## 11. Parámetros deliberadamente pendientes

No se fijan aún:

- Multiplicadores de Vitalidad, daño, Postura y defensas por ciclo.
- Fórmula de crecimiento y posible límite de ciclos.
- Modificador de recompensas y botín.
- Cambios concretos en composición y variantes de enemigos.
- Qué misiones, reputaciones, inventarios y estados ecológicos se reinician.
- Si hay recompensas exclusivas cosméticas, narrativas o funcionales por ciclo.
- Requisitos exactos para iniciar el siguiente ciclo.

Estos parámetros requieren una simulación conjunta con el modelo de progresión y economía ya documentado. No deben inventarse ni incorporarse como cifras definitivas sin pruebas.

## 12. Criterio de éxito

Un NG+ satisfactorio debe permitir que un personaje muy desarrollado vuelva a recorrer el mundo y encuentre un desafío real en todas sus regiones, mientras conserva la sensación de progreso, la identidad de los enemigos y la utilidad de las profesiones.

El jugador debe poder continuar perfeccionando su personaje, equipo y dominio del mundo durante ciclos posteriores. La dificultad crece, pero la economía, las recompensas y las oportunidades también deben acompañar ese crecimiento.
