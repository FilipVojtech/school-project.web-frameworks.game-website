using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    [Required]
    public long GameId { get; set; }

    [Required]
    [Range(0, 10)]
    public int Rating { get; set; }

    public string? AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public User? Author { get; set; }
}