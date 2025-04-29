using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Website.Models.ViewModels;

public class AddReviewModel
{
    public AddReviewModel()
    {
        RatingItems = new List<SelectListItem>();
        for (var i = 0; i < 10; i++)
        {
            RatingItems.Add(new SelectListItem($"{i + 1}", $"{i + 1}", i == 4));
        }
    }

    public IList<SelectListItem> RatingItems { get; set; }

    public long GameId { get; set; }

    [StringLength(40)]
    public string? Title { get; set; }

    [Required]
    public string Body { get; set; }

    [Range(1, 10)]
    [Required]
    public int Rating { get; set; }
}