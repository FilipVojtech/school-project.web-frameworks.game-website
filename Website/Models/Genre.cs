using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class Genre
{
    public Genre()
    {
    }

    // public Genre(IGDB.Models.Genre igdbGenre)
    // {
    //     Id = igdbGenre.Id ?? throw new ArgumentException($"Property Id missing on {nameof(igdbGenre)}");
    //     Name = igdbGenre.Name;
    //     Slug = igdbGenre.Slug;
    // }

    public long Id { get; set; }

    [StringLength(70)]
    [Required]
    public string Name { get; set; } = "";

    [StringLength(80)]
    [Required]
    public string Slug { get; set; } = "";

    public virtual ICollection<Game> Games { get; set; } = [];
}