@startuml

namespace Core {
    class PaymentMethods {
        - Type PaymentType
        - string Name
        - bool IsEnabled

        + Find(Type type): Type
    }
}

namespace StoreApi.Controllers {
    class StoreController {
        - static List<PaymentMethods> paymentMethods

        + UpdatePaymentEnabledAsync(paymentMethod: int): IActionResult
        + GetPaymentMethods(): Task<List<PaymentMethods>>
    }
}

namespace StoreApi.Hubs {
    class PaymentMethodsHub {
        + PaymentMethodsChange(paymentMethods: PaymentMethods): Task
    }
}

namespace ReactApp {
    namespace api {
        class initialStore {
            + useFetchInitialStore({ category: string[], search: string }): List<Product>
            + useFetchGetPaymentMethods(): List<PaymentMethod>
            + useFetchUpdatePaymentIsEnabled(paymentMethod: number | null): void
            + useSignalRGetPaymentMethods(): { paymentMethods: List<PaymentMethod> }
        }
    }

    namespace Admin {
        class PaymentMethodsPage {
            + products: List<Product>
            + paymentMethods: List<PaymentMethod>

            + handlePaymentMethodChange(methodId: number): void
        }
    }

    namespace Abacaxi {
        class Checkout {
            + render(): JSX.Element
        }
    }
}

PaymentMethodsHub --> StoreApi.Controllers.StoreController : Uses
StoreApi.Controllers.StoreController --> Core.PaymentMethods : Uses

ReactApp.api.initialStore --> StoreApi.Controllers.StoreController : Calls
ReactApp.api.initialStore --> StoreApi.Hubs.PaymentMethodsHub : Calls

ReactApp.Admin.PaymentMethodsPage --> ReactApp.api.initialStore : Uses
ReactApp.Abacaxi.Checkout --> ReactApp.api.initialStore : Uses

@enduml
