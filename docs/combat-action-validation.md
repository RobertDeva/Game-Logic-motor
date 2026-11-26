# Validación de solicitudes de combate

**Estado:** flujo de solicitud y validación implementado; resolución de consecuencias pendiente.

## Modelo

- `CombatActionRequest` identifica la acción solicitada, su actor y un objetivo opcional.
- `CombatantSnapshot` ofrece una vista inmutable del identificador, participación en simulación y recursos de un combatiente.
- `CombatActionContext` proporciona actor y objetivo a las condiciones declaradas por una acción.
- `CombatActionValidator` comprueba coherencia de identificadores, presencia del objetivo y participación activa antes de evaluar precondiciones.

El identificador de entidad no se vuelve a generar ni se sustituye por una identidad de combate: el snapshot conserva el `EntityId` original. Se permite que actor y objetivo sean la misma entidad, para admitir acciones sobre uno mismo.

## Límite entre validación y resolución

La validación no consume recursos, no altera Vitalidad o Postura, no aplica daño, no crea efectos y no emite eventos. Un resultado satisfactorio solo significa que la solicitud coincide con los snapshots proporcionados y que las precondiciones declaradas se cumplen.

El snapshot es explícito y lo suministra el llamador. Este bloque no implementa todavía el registro/consulta de entidades ni garantiza que el snapshot sea el estado más reciente; esa responsabilidad deberá resolverse en Application al coordinar un caso de uso.

## Reglas no asumidas

No se deduce que un combatiente con Vitalidad agotada esté automáticamente retirado de la simulación: la derrota y la muerte son estados de gameplay separados del ciclo de vida técnico. Tampoco se fijan alcance, costes, daño, presión de Postura, ventanas de desvío, interrupciones ni prioridad de acciones.

## Pruebas

Las pruebas cubren solicitudes válidas, desajustes de identidad, actores/objetivos retirados, objetivos ausentes o inesperados, y acciones cuyo objetivo es el propio actor.
