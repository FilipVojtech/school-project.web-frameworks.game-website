using Website.Helpers;

namespace Website.Models.ViewModels;

public class ViewGame
{
    public ViewGame()
    {
        AddReviewModel = new AddReviewModel();
    }

    public Game Game { get; set; }

    public double? AverageRating { get; set; }

    public int ReviewCount { get; set; } = 0;

    public AddReviewModel AddReviewModel { get; set; }

    public PaginatedList<Review> Reviews { get; set; }
}