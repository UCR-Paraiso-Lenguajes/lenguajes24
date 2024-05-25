//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{
    public class CartLogic{

        private Cart newCart {get;}


        public CartLogic(Cart newCart){
            this.newCart = newCart;
        }

        public bool validateCart(){

            //Validaciones manejadas a partir de ahora con try catch
             if (newCart == null){
                throw new BussinessException("El carrito no puede ser nulo");
            }

            if (!validatePaymentMethodFromCart(newCart.PaymentMethod)){
                throw new BussinessException("El metodo de pago no es valido por el momento");
            }
            if (!validateProductsFromCart(newCart.allProduct)){
                throw new BussinessException("La lista de productos del carrito no puede ser nula ni estar vacia");
            }
            if (!validateDirectionFromCart(newCart.Direction)){
                throw new BussinessException("La direccion debe contener informacion verdadera");
            }
            if (!validateTotalFromCart(newCart.Total)){
                throw new BussinessException("El total de los productos del carrito no puede ser igual o menor a cero");
            }            
            //El carrito podra ser procesado con la BD
            return true;
        }
        
        private bool validateProductsFromCart(List<Product> products){
            if(products == null || products.Count == 0){
                return false;
            } 
            return true;
        }

        private bool validatePaymentMethodFromCart(PaymentMethod paymentFromCart){
            foreach (var paymentItemList in PaymentMethods.paymentMethodsList){
                if(paymentFromCart.payment == paymentItemList.payment){
                    return true;
                }                
            }
            return false;
        }

        private bool validateTotalFromCart(decimal total){
            return total > 0;
        }

        private bool validateDirectionFromCart(string direction){
            return !string.IsNullOrEmpty(direction);
        }        
    }
}