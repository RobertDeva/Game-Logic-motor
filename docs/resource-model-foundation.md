# Fundamentos del modelo de recursos

**Proyecto:** Mythical Exodus  
**Estado:** contrato de estado implementado; reglas de operación pendientes.

## 1. Contrato común

- `ResourceId` identifica un recurso de forma estable.
- `ResourcePool` conserva su cantidad actual y su máximo.
- Los valores se representan con `decimal`.
- El estado es inmutable: los cambios producen una nueva instancia.
- Se valida que el máximo no sea negativo y que la cantidad actual esté entre cero y el máximo.

El modelo puede representar recursos como Vitalidad, Postura, Maná, Aura y Prana sin asumir que todos tengan las mismas reglas de uso o recuperación.

## 2. Límites de esta implementación

No se implementan todavía métodos genéricos de consumir, restaurar, regenerar, drenar o rellenar. Tampoco se decide si una operación que excede un límite se rechaza, se limita al máximo, se convierte en exceso o produce otro efecto.

Esas decisiones dependen del recurso y de la acción que lo modifica. Por ejemplo, la recuperación de Postura de los enemigos, el gasto de Maná y la pérdida de Vitalidad no deben forzarse a una misma política solo por compartir una estructura de almacenamiento.

## 3. Decisiones pendientes

- granularidad y redondeo de cada recurso;
- reglas de coste, consumo y recuperación;
- regeneración automática y condiciones que la habilitan;
- interacción con modificadores de estadísticas;
- recursos con múltiples barras o segmentos;
- persistencia, reposición y puntos de descanso;
- tratamiento de exceso, bloqueo y agotamiento.

El contrato actual modela un único depósito acotado. Las barras múltiples se modelarán cuando se implemente el estado de combate, sin presumir que son intercambiables con un único valor agregado.
