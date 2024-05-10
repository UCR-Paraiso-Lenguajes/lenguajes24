using System.ComponentModel.DataAnnotations;

namespace ApiLab7;

public sealed class Cart
{
    [Required]
    public List<string> ProductIds { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public PaymentMethods.Type PaymentMethod { get; set; }

    public string ConfirmationNumber { get; set; }
}
