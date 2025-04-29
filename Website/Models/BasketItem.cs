using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace Website.Models;

public class BasketItem
{
    public int Id { get; set; }

    [Required]
    public long GameId { get; set; }

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    [Required]
    public int Count { get; set; }
}