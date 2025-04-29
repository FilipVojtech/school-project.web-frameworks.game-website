using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Models.ViewModels;

public class CheckoutViewModel
{
    public CheckoutViewModel()
    {
    }

    public CheckoutViewModel(Basket basket, string emailAddress)
    {
        Basket = basket;
        EmailAddress = emailAddress;
    }

    [ValidateNever]
    public Basket Basket { get; set; }

    [DisplayName("Name on Card")]
    [Required(ErrorMessage = "Please enter the name on card")]
    public string NameOnCard { get; set; }

    [DisplayName("Card Number")]
    [Required(ErrorMessage = "Please enter a card number")]
    [DataType(DataType.CreditCard)]
    [CreditCard(ErrorMessage = "Please enter a valid card number")]
    public string CardNumber { get; set; }

    [DisplayName("Security Code")]
    [Required(ErrorMessage = "Please enter security code")]
    [Range(100, 999, ErrorMessage = "Please enter a valid security code")]
    public int? SecurityCode { get; set; }

    [DisplayName("Exp. Month")]
    [Required(ErrorMessage = "Please select expiration month")]
    [Range(1,12, ErrorMessage = "Please choose a valid month")]
    public int? ExpMonth { get; set; }

    [DisplayName("Exp. Year")]
    [Required(ErrorMessage = "Please select expiration year")]
    public int? ExpYear { get; set; }

    [AllowedValues(true, ErrorMessage = "You need to agree to the refund policy.")]
    [DefaultValue(false)]
    public bool AcceptRefundPolicy { get; set; }

    [ValidateNever]
    public SelectList ExpYearSelectList
    {
        get
        {
            List<string> years = [];
            for (var i = DateTime.Today.Year; i < DateTime.Today.Year + 12; i++)
            {
                years.Add(i.ToString());
            }

            return new SelectList(years);
        }
    }

    [ValidateNever]
    public string EmailAddress { get; set; }
}