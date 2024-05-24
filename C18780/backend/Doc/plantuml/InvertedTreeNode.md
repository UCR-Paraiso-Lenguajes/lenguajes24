@startuml
participant "StoreController" as StoreController
participant "ProductSearch" as ProductSearch
participant "InvertedTreeNode" as Node

StoreController -> ProductSearch: new ProductSearch(products)
ProductSearch -> ProductSearch: BuildInvertedTree(products)
loop for each product
    ProductSearch -> ProductSearch: GetWordsFromText(product.Name + product.Description)
    loop for each word
        ProductSearch -> Node: AddWordToInvertedTree(word, product)
        Node -> Node: Add word to Words dictionary
        Node -> Node: Add product to Products set
    end
end

StoreController -> ProductSearch: Search(searchText)
ProductSearch -> ProductSearch: GetWordsFromText(searchText)
loop for each keyword
    ProductSearch -> Node: Find node for keyword
    alt node found
        Node -> ProductSearch: Return products from node
        ProductSearch -> ProductSearch: Merge products into resultSet
    else node not found
        ProductSearch -> StoreController: Return empty resultSet
    end
end
ProductSearch -> StoreController: Return resultSet

@enduml
