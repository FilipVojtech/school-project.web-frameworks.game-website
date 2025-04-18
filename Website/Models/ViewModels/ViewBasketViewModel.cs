namespace Website.Models.ViewModels;

public class ViewBasketViewModel
{
    public required decimal TotalPrice { get; set; }

    public required IList<BasketItem> Items { get; set; }
}