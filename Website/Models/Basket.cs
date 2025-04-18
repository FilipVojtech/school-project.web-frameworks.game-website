namespace Website.Models;

public class Basket
{
    public long Id { get; set; }

    public string UserId { get; set; }

    public virtual IList<BasketItem> Items { get; set; } = [];

    public decimal TotalPrice() => Items.Sum(item => item.Game.Price * item.Count);
}