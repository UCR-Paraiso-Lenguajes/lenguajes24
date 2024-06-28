# ProductSearch utilizando árbol invertido

El código proporciona una implementación de búsqueda de productos utilizando un árbol invertido para indexar palabras clave en los nombres y descripciones de los productos.

## Clase InvertedTreeNode

La clase `InvertedTreeNode` representa un nodo en el árbol invertido. Contiene dos campos privados:
- `Words`: Un diccionario que mapea palabras clave a nodos hijos en el árbol.
- `Products`: Un conjunto que almacena los productos asociados con las palabras clave en este nodo.

Se proporcionan métodos públicos para:
- Verificar si una palabra clave está presente en el nodo (`ContainsWord`).
- Obtener el nodo hijo correspondiente a una palabra clave (`GetNode`).
- Agregar una palabra clave y su nodo hijo al diccionario (`AddWord`).
- Verificar si un producto está asociado con este nodo (`ContainsProduct`).
- Agregar un producto al conjunto de productos (`AddProduct`).

## Clase ProductSearch

La clase `ProductSearch` se utiliza para realizar búsquedas de productos utilizando el árbol invertido. Al construir un objeto `ProductSearch`, se indexan los productos proporcionados y se construye el árbol invertido.

Métodos importantes:
- `BuildInvertedTree`: Construye el árbol invertido indexando palabras clave de los nombres y descripciones de los productos.
- `Search`: Realiza una búsqueda de productos dado un término de búsqueda. Divide el término de búsqueda en palabras clave, busca cada palabra clave en el árbol invertido y devuelve los productos asociados con todas las palabras clave.

Se utilizan expresiones regulares para dividir el texto en palabras clave y se aplican transformaciones para normalizar el texto a minúsculas durante el proceso de indexación y búsqueda.

El árbol invertido y la búsqueda de productos están diseñados para ser eficientes en términos de tiempo y memoria, utilizando estructuras de datos como diccionarios y conjuntos para un acceso rápido y sin duplicados.

![invertedTreeNode](https://www.planttext.com/api/plantuml/png/ZPHDJiCm48NtESMeAoAW1-W2bHyLiQ12j19hAvw2XMD7umcbjyTsapPsMzf8aPE9lpVFRwHC7GlYjjP5urTLgKOOXj62BU6ZDKnMQwGH20TPBGNUoCgsuWKAghxYzgIIRdunFqYCSac8SoinxWyFHP4rWTkxL6W21jTfwQhPlhcoIEf7s8TMQJdiFc2rjGsiB04AptRr0lnrJlKP-SEIT3EozH9_kHSTpqMDSDrhZP_GLQGQLjQKKJXjkVOQiNoqPuXd0lTIXbPBcnmYa3TzctAd4P04ZIs0BKIx85KLl0ZQ_8Dqge7gF3bmk0q6ZIp2VS7Kkb6vk8IGpW_h8DOXd4RtZPjTUYUIcoaZmOJZ1Me82CtzXzR8WoWoL-_8BPa-5WShxoxYfqUNARmYVU9UG1aVBQ5hjT_HvQeTRYqPovcj0_Ki-vr3h1lUvCBTm2uMsUF5r8F-F_47)
[PlantUml](plantuml/InvertedTreeNode.md)

## Diagrama de actividad WebSocket
![webSocketActivityDiagram]([https://www.planttext.com/api/plantuml/png/ZPHDJiCm48NtESMeAoAW1-W2bHyLiQ12j19hAvw2XMD7umcbjyTsapPsMzf8aPE9lpVFRwHC7GlYjjP5urTLgKOOXj62BU6ZDKnMQwGH20TPBGNUoCgsuWKAghxYzgIIRdunFqYCSac8SoinxWyFHP4rWTkxL6W21jTfwQhPlhcoIEf7s8TMQJdiFc2rjGsiB04AptRr0lnrJlKP-SEIT3EozH9_kHSTpqMDSDrhZP_GLQGQLjQKKJXjkVOQiNoqPuXd0lTIXbPBcnmYa3TzctAd4P04ZIs0BKIx85KLl0ZQ_8Dqge7gF3bmk0q6ZIp2VS7Kkb6vk8IGpW_h8DOXd4RtZPjTUYUIcoaZmOJZ1Me82CtzXzR8WoWoL-_8BPa-5WShxoxYfqUNARmYVU9UG1aVBQ5hjT_HvQeTRYqPovcj0_Ki-vr3h1lUvCBTm2uMsUF5r8F-F_47](https://planttext.com/api/plantuml/png/PLAxJWCn4EplAwQyrA7A5GGG3Mr0r1lV1gmutiYFeA3y31_0Do2H7sDznoKvQ6nvTZmxi_Oy9WefRrnrm5FC5ApWK7LWDHi0zYaG28wGBFkM6cNVj1lhbMjNgB-_xYEe0DSJf5VsgEFnSr81ivuM6PIJwbX3CEAH8mViiQKOtoIqJQ7UabchEdAophuJ7fuVErntjR2pr0h8OHN4fyBRuqNzjGNMZaPaRRavzpI12OAbX6BUPs-inAPYfsPgByNT0NTE0oNM77flE90ntF8uiAvsLPtJcfxxB--WiMCuYPYUEcDp_8W8RBfwB4PxiGNtLhE7pxoJuSPVwR7urLcuZXQiJxQaCeUJvqbfSPbsB32zc8dokdY_8STVGRRLN0lbZ_m0))
[PlantUml](plantuml/WebSocketActivityDiagram.md)

