namespace Website.Models.ViewModels;

public class ViewGame
{
    public ViewGame()
    {
        AddReviewModel = new AddReviewModel();
    }

    public Game Game { get; set; }

    public AddReviewModel AddReviewModel { get; set; }
}