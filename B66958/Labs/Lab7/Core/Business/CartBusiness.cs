using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiLab7;

public class CartBusiness
{
    private CartData cartData;
    public IEnumerable<Product> Products { get; }

    public CartBusiness()
    {
        this.cartData = new CartData();
    }

    public async Task<Sale> PurchaseAsync(Cart cart)
    {
        ValidateCart(cart);

        // Find matching products based on the product IDs in the cart
        IEnumerable<Product> matchingProducts = Store
            .Instance.ProductsInStore.Where(p => cart.ProductIds.Contains(p.Uuid.ToString()))
            .ToList();

        // Create shadow copies of the matching products
        IEnumerable<Product> shadowCopyProducts = matchingProducts
            .Select(p => (Product)p.Clone())
            .ToList();

        // Calculate purchase amount by multiplying each product's price with the store's tax percentage
        decimal purchaseAmount = 0;
        foreach (var product in shadowCopyProducts)
        {
            product.Price *= (1 + (decimal)Store.Instance.TaxPercentage / 100);
            purchaseAmount += product.Price;
        }

        PaymentMethods paymentMethod = PaymentMethods.Find(cart.PaymentMethod);

        string receiptNumber = GeneratePurchaseNumber();

        var sale = Sale.Build(
            shadowCopyProducts,
            cart.Address,
            purchaseAmount,
            paymentMethod,
            receiptNumber,
            cart.ConfirmationNumber
        );

        ValidateSale(sale);

        await cartData.SaveAsync(sale);

        return sale;
    }

    private void ValidateCart(Cart cart)
    {
        if (cart.ProductIds.Count == 0)
            throw new ArgumentException("Cart must contain at least one product.");
        if (string.IsNullOrWhiteSpace(cart.Address))
            throw new ArgumentException("Address must be provided.");
        if (cart.PaymentMethod == null)
            throw new ArgumentException("A payment method should be provided");
    }

    private void ValidateAddress(String address){
        
    }

    private void ValidateSale(Sale sale)
    {
        if (sale.Products.Count() == 0)
            throw new ArgumentException("Sale must contain at least one product.");
        if (string.IsNullOrWhiteSpace(sale.Address))
            throw new ArgumentException("Address must be provided.");
        if (sale.Amount <= 0)
            throw new ArgumentException("The final amount should be above 0");
        if (sale.PaymentMethod == null)
            throw new ArgumentException("A payment method should be provided");
        if (string.IsNullOrWhiteSpace(sale.PurchaseNumber))
            throw new ArgumentException("A purchase number must be provided");
        if (
            sale.PaymentMethod.PaymentType == PaymentMethods.Type.SINPE
            && sale.ConfirmationNumber == null
        )
            throw new ArgumentException("The confirmation number must be provided");
    }

    private string GeneratePurchaseNumber()
    {
        DateTime now = DateTime.Now;

        string dateTimeString = now.ToString("yyyyMMddHHmmss");

        byte[] hashBytes;
        using (SHA256 sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dateTimeString));
        }

        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        string receipt = sb.ToString().Substring(0, 6);

        return receipt;
    }
}
