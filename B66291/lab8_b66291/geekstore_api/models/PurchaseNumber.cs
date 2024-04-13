using System;

namespace geekstore_api
{
    public class PurchaseNumber
    {
         private static Random random = new Random();

        public int generarNumeroCompra()
        {
            int purchaseNumber = random.Next(10000, 90000);
            return purchaseNumber;
        }
    }
}




