using System.Collections.Generic;
using MyStoreAPI.Business;

namespace MyStoreAPI.Models
{
    public sealed class Sale    
    {
        public int idSale {get;}
        public string purchaseNum {get;}
        public DateTime dateTimeSale {get;}
        public Cart cart {get;}
        public Sale(int idSale, string purchaseNum,DateTime dateTimeSale,Cart cart){

            if (idSale <= 0)
                throw new ArgumentException($"{nameof(idSale)} no puede ser negativo ni igual a cero.");
    
    
            if (string.IsNullOrWhiteSpace(purchaseNum))
                throw new ArgumentException($"{nameof(purchaseNum)} no puede estar vacío.");

            if (dateTimeSale == DateTime.MinValue) 
                throw new ArgumentException($"{nameof(dateTimeSale)} es fecha no valida");                

            if(cart == null) throw new ArgumentException($"{nameof(cart)} no puede ser nulo.");
            
            //Validacion extra para los items del carrito
            CartLogic cartLogic = new CartLogic(cart);
            cartLogic.validateCart();

            this.idSale = idSale;
            this.purchaseNum = purchaseNum;
            this.cart = cart;
            this.dateTimeSale = dateTimeSale;
        }        
    }
}