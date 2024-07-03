@startuml
skinparam class {
    BackgroundColor White
    ArrowColor Black
    BorderColor Black
}

class User {
    <<Actor>>
    + EditaCantidadProducto(product: Product, quantity: int)
}

class Frontend {
    + HTTP_POST(apiEndpoint: string, data: object): HttpResponse
    + HTTP_PUT(apiEndpoint: string, data: object): HttpResponse
}

class CartController {
    - mediator: IMediator
    + AddCartAsync(cart: Cart): PurchaseNumber
    + UpdateCartAsync(cart: Cart): PurchaseNumber
}

interface IMediator {
    + Send(command: ICommand): Task
}

class CreateSalesCommand {
    + CreateSalesCommand(date: DateTime, confirmation: int, paymentMethod: string, total: decimal, address: string, purchaseNumber: string)
    + Handle(): Sales
}

class UpdateSalesCommand {
    + UpdateSalesCommand(uuid: Guid, date: DateTime, confirmation: int, paymentMethod: string, total: decimal, address: string, purchaseNumber: string)
    + Handle(): Sales
}

class ProductController {
    - mediator: IMediator
    + GetProductById(productId: Guid): Product
}

class ProductRepository {
    + GetById(productId: Guid): Product
}

class Sales {
    + Uuid: Guid
    + Date: DateTime
    + Confirmation: int
    + PaymentMethod: string
    + Total: decimal
    + Address: string
    + PurchaseNumber: string
}

User --> Frontend: EditaCantidadProducto(product, quantity)
Frontend --> CartController: HTTP_PUT("/api/Cart", updatedCart)
CartController --> IMediator: new UpdateSalesCommand(...)
IMediator ..> UpdateSalesCommand: Send(command)
UpdateSalesCommand --> ProductController: GetProductById(productId)
ProductController --> ProductRepository: GetById(productId)
ProductRepository --> ProductController: Product details
ProductController --> UpdateSalesCommand: Product details
UpdateSalesCommand --> IMediator: Send(createSalesLineCommand)
IMediator --> CartController: Sales object
CartController --> Frontend: PurchaseNumber

CartController --> IMediator: new CreateSalesCommand(...)
IMediator ..> CreateSalesCommand: Send(command)
CreateSalesCommand --> IMediator: Send(updateSalesCommand)
IMediator --> UpdateSalesCommand: UpdateSalesCommand
UpdateSalesCommand --> Sales: Updates sales information
@enduml
