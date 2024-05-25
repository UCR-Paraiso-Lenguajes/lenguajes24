using System.Collections.Generic;
//namespace Core;
namespace MyStoreAPI.Models{

    public sealed class RegisteredSaleWeek{

        public string dayOfWeek  {get;set;}
        public decimal total {get;set;}


        public RegisteredSaleWeek(string dayOfWeek, decimal total){
            this.dayOfWeek = dayOfWeek;
            this.total = total;
            validateRegisteredSaleWeek();
        }

        public RegisteredSaleWeek(){

        }
        

        public void validateRegisteredSaleWeek(){
            if(string.IsNullOrWhiteSpace(dayOfWeek))
                throw new ArgumentException($"{nameof(dayOfWeek)} no puede ser nulo o vacío.");

            if(total < 0)
                throw new ArgumentException($"{nameof(total)} no puede ser negativo.");
        }
    }

}
