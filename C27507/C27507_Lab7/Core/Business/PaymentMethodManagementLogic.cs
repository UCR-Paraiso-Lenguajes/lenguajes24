//API
using Core;
using MyStoreAPI.DB;
using MyStoreAPI.Models;

namespace MyStoreAPI.Business
{
    public class PaymentMethodManagementLogic{
        
        private DB_PaymentMethod db_paymentMethod;
        public delegate void PaymentUpdatedDelegate (int paymentMethodId, int newStatus);
        public PaymentUpdatedDelegate  onPaymentUpdated;
        
        public PaymentMethodManagementLogic(){
            this.db_paymentMethod = new DB_PaymentMethod();
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsFromDBAsync(){
            IEnumerable<PaymentMethod> paymentMethods = await db_paymentMethod.GetAllPaymentMethodsAsync();

            if (paymentMethods == null || !paymentMethods.Any())
                throw new BussinessException("No se encontraron métodos de pago.");

            //Validar cada uno de los pagos
            foreach (var paymentMethod in paymentMethods)
            {
                if (paymentMethod == null)
                    throw new BussinessException("El método de pago no puede ser nulo");
                
                if (paymentMethod.verify == null)
                    throw new BussinessException("El campo de verificación del método de pago no puede ser nulo");
            }
            return paymentMethods;
        }


        public async Task UpdatePaymentMethodStatusInBDAsync(int paymentMethodId, int newStatus,PaymentUpdatedDelegate onPaymentUpdated){

            if (newStatus == null)
                throw new BussinessException($"{nameof(newStatus)} no debe ser nulo.");            

            if (paymentMethodId == null)
                throw new BussinessException($"{nameof(paymentMethodId)} no debe ser nulo.");
                                                
            if (newStatus != 0 && newStatus != 1)
                throw new BussinessException($"{nameof(newStatus)} debe ser 0 o 1.");

            //Si la actualizacion falla, se captura en Controller
                //actualizamos la BD
            await db_paymentMethod.UpdatePaymentMethodStatusAsync(paymentMethodId,newStatus);
                //actualizamos la lista en memoria de metodo de pago
            onPaymentUpdated?.Invoke(paymentMethodId,newStatus);
        }
    }
}