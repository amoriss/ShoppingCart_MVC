using System.ComponentModel.DataAnnotations;

namespace ShoppingCart_MVC.Models;

public class CheckoutViewModel
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string CreditCardNumber { get; set; }

    [Required]
    public string ExpirationDate { get; set; }
    [Required, StringLength(3, MinimumLength = 3)]
    public string CVC { get; set; }
}
