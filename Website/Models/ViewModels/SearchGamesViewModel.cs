namespace Website.Models.ViewModels;

public class SearchGamesViewModel
{
    public string Query { get; set; } = "";

    public ICollection<Game>? SearchResults { get; set; }
}