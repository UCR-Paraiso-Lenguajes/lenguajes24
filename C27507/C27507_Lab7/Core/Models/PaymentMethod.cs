namespace MyStoreAPI.Models
{
    public class PaymentMethod{

        //Instancia del Enum
        public PaymentMethodNumber payment { get; set; }
        public bool verify { get; set; }

        public PaymentMethod(PaymentMethodNumber payment, bool verify){
            this.payment = payment;
            this.verify = verify;
        }        
    }
    

    //Tipos de pago por default
    public enum PaymentMethodNumber{
        CASH = 1,
        CREDIT_CARD = 2,
        DEBIT_CARD = 3,
        SINPE = 4
    }
}