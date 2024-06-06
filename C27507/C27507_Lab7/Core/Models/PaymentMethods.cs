namespace MyStoreAPI.Models
{
    //Estatica para poderla manejar desde cualquier lugar del proyecto
    public static class PaymentMethods{

        //Inicializamos y declaramos la lista de pagos de una vez
        public static List<PaymentMethod> paymentMethodsList {get;} = new List<PaymentMethod>{            
            new PaymentMethod(PaymentMethodNumber.CASH, false),
            new PaymentMethod(PaymentMethodNumber.CREDIT_CARD, true),
            new PaymentMethod(PaymentMethodNumber.DEBIT_CARD, true),
            new PaymentMethod(PaymentMethodNumber.SINPE, true)
            //... futuros nuevos metodos de pago
        };        
        
    }
}