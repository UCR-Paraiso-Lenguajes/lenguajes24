using System.Collections.Generic;
//namespace Core;
namespace MyStoreAPI.Models{


    public class RegisteredSaleReport {
        public IEnumerable<RegisteredSale> salesByDay { get; set; }
        public IEnumerable<RegisteredSaleWeek> salesByWeek { get; set; }
    }
}