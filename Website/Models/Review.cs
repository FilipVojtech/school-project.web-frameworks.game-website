using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models;

public class Review
{
    public long Id { get; set; }

    [StringLength(40)]
    public string? Title { get; set; }

    [Column(TypeName = "text")]
    [StringLength(int.MaxValue)]
    [Required]
    public string Body { get; set; }

    [Required]
    public Game Game { get; set; }

    [Required]
    [Range(0, 10)]
    public int Rating { get; set; }

    [Required]
    public User Author { get; set; }
}