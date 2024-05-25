namespace KEStoreApi;

public sealed class Cart{

    public List<string> ProductID{get;set;}
    public string address {get;set;}
    public PaymentMethods.Type PaymentMethod{get; set;}
}