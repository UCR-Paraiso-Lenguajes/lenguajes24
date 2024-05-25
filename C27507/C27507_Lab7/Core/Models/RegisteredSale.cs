using System.Collections.Generic;

namespace MyStoreAPI.Models{
    public sealed class RegisteredSale{
        public int IdSale { get; set; }
        public string PurchaseNum { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Direction { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime DateTimeSale{ get; set; }
        
        
        public RegisteredSale(int idSale, string purchaseNum, decimal subtotal, decimal tax, decimal total, string direction, PaymentMethod paymentMethod,DateTime dateTimeSale){                                                
            IdSale = idSale;
            PurchaseNum = purchaseNum;
            SubTotal = subtotal;
            Tax = tax;
            Total = total;
            Direction = direction;
            PaymentMethod = paymentMethod;
            DateTimeSale = dateTimeSale;
            validateRegisteredSale();
        }

        //Usado en DB_Sale
        public RegisteredSale(){}

        public void validateRegisteredSale(){            
            if (IdSale <= 0)
            throw new ArgumentException($"{nameof(IdSale)} no puede ser negativo ni igual a cero.");
        
            if (string.IsNullOrWhiteSpace(PurchaseNum))
                throw new ArgumentException($"{nameof(PurchaseNum)} no puede estar vacío.");

            if (SubTotal <= 0 || Total <= 0)
                throw new ArgumentException($"{nameof(SubTotal)} y {nameof(Total)} no pueden ser negativos ni iguales a cero.");
            if (Tax < 0)
            throw new ArgumentException($"{nameof(Tax)} no puede ser negativo.");

            if (string.IsNullOrWhiteSpace(Direction))
                throw new ArgumentException($"{nameof(Direction)} no puede estar vacía.");

            if (PaymentMethod == null)
                throw new ArgumentNullException($"{nameof(PaymentMethod)} no puede ser null.");
        }
    }
}