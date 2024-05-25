namespace StoreApi.Models
{
    public sealed class Cart
    {
        public List<string> ProductIds { get; set; }
        public string Address { get; set; }
        public int PaymentMethod { get; set; }

    }
}