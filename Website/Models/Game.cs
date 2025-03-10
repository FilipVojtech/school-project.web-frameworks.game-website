using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class Game
{
    public long Id { get; set; }

    [StringLength(80)]
    [Required]
    public required string Name { get; set; } = "";

    [DataType(DataType.Url)]
    [StringLength(255)]
    public string? Url { get; set; }

    [DataType(DataType.Url)]
    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public Genre? Genre { get; set; } = null;

    public virtual ICollection<Review> Reviews { get; set; } = [];
}