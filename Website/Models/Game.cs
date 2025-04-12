using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class Game
{
    public Game()
    {
    }

    // public Game(IGDB.Models.Game igdbGame)
    // {
    //     Id = igdbGame.Id ?? throw new MissingFieldException($"Property Id missing on {nameof(igdbGame)}");
    //     Name = igdbGame.Name;
    //     Url = igdbGame.Url;
    //     ImageUrl = igdbGame.Cover.Value.Url;
    //     ReleaseDate = igdbGame.FirstReleaseDate?.DateTime ?? throw new MissingFieldException($"Property ReleaseDate missing on {nameof(igdbGame)}");
    //     Genres = igdbGame.Genres.Values.Select(g => new Genre(g)).ToList();
    // }

    public long Id { get; set; }

    [StringLength(80)]
    [Required]
    public string Name { get; set; }

    [DataType(DataType.Url)]
    [StringLength(255)]
    public string? Url { get; set; }

    [DataType(DataType.Url)]
    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public virtual ICollection<Genre> Genres { get; set; } = [];

    public virtual ICollection<Review> Reviews { get; set; } = [];

    public double? AverageRating() =>
        Reviews.Count switch
        {
            0 => null,
            1 => Reviews.First().Rating,
            _ => Reviews.Average(review => review.Rating)
        };
}