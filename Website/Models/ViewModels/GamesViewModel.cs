namespace Website.Models.ViewModels;

public class GamesViewModel
{
    public string? Query { get; set; }

    public ICollection<Game> Games { get; set; } = [];
}