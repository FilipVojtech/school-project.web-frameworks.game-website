using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class Genre
{
    public int Id { get; set; }

    [StringLength(70)]
    [Required]
    public string Name { get; set; } = "";

    [StringLength(80)]
    [Required]
    public string Slug { get; set; } = "";
}