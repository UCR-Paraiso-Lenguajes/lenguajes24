//API
using MyStoreAPI.DB;
using MyStoreAPI.Models;
namespace MyStoreAPI.Business
{
    public class StoreLogic{

        //Planeo reutilizar este codigo mas adelante.


        //Validar el status de la tienda antes de enviar su informacion por la API
        // public bool validateStatusStore(List<Product> productForCart){
            
        //     //Se indica si existen tablas o fueron creadas con exito
        //     //segun el atributo de Store
        //     bool isThereConnection = Store.Instance.StoreConnectedWithDB;
        //     if(!isThereConnection) return false;                        
        //     fillStoreData(productForCart);            
        //     return true;
        // }        

        // //Si no existe al menos un dato, rellenar las tablas
        // private void fillStoreData(List<Product> productForCart){            
        //     Console.WriteLine("Estamos en validateStatusStore, existe tabla payment: " + DB_PaymentMethod.PaymentMethodsInTableExist());
        //     if(DB_PaymentMethod.PaymentMethodsInTableExist() == false ) DB_PaymentMethod.InsertPaymentMethod();
        //     if(!DB_Product.ProductsInTableExist()){

        //         foreach (var productToStoreTable in createStoreProducts())
        //         {
        //             productForCart.Add(productToStoreTable);                    
        //         }                                
        //         DB_Product.InsertProductsStore(this.Products);
        //     }    
        // }
    }
}