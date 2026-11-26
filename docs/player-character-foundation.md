# Fundamentos del personaje jugador

**Estado:** primera implementación del modelo de dominio; reglas de coste y progresión económica todavía pertenecen a Application y a la política de progresión.

## 1. Clases permanentes

El dominio implementa las tres clases del GDD:

| Clase | Vitalidad | Fuerza | Agilidad | Resistencia | Voluntad | Ruptura | Destreza | Armonía | Total |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Espadachín Mágico | 5 | 6 | 5 | 4 | 4 | 5 | 6 | 6 | 41 |
| Guerrero | 6 | 6 | 4 | 6 | 5 | 5 | 4 | 4 | 40 |
| Segador | 4 | 4 | 6 | 4 | 6 | 6 | 5 | 5 | 40 |

Los identificadores de código son MagicSwordsman, Warrior y Reaper. La clase se asigna al construir PlayerCharacter y no existe una operación para cambiarla.

## 2. Estado y nivel

PlayerCharacter conserva identidad, clase, atributos iniciales y atributos permanentes actuales. Los atributos continúan sujetos al rango 1–99 de CharacterAttributes y no pueden quedar por debajo de la base de su clase.

El nivel se deriva, no se almacena duplicado:

Level = 1 + (suma de atributos actuales - suma de atributos iniciales de la clase)

Por tanto, cada incremento permanente de un atributo aumenta el nivel exactamente en uno. Los modificadores temporales y las estadísticas derivadas no intervienen en este cálculo.

## 3. Límite de la operación de incremento

TryIncreaseAttribute aplica un único punto permanente y devuelve el nuevo nivel o un error tipado. Rechaza atributos desconocidos y atributos que ya alcanzaron 99.

Esta operación **no cobra almas**. El caso de uso de Application deberá calcular el coste de progresión, comprobar el saldo y coordinar el gasto con el incremento como una única operación lógica. No debe permitirse que el personaje gane un punto si el gasto no puede confirmarse, ni que se pierdan almas si el incremento falla.

## 4. Pruebas

Las pruebas verifican los valores iniciales de las tres clases, nivel inicial 1, incremento de nivel, límite 99, rechazo de atributos desconocidos y protección de la base de clase.
