namespace StoreApi.Models
{
    public sealed class PurchaseNumber
    {
        public PurchaseNumber(string purchaseNumber)
        {
            this.purchaseNumber = purchaseNumber;
        }

        public string purchaseNumber {get; set;}
    }
}