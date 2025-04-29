using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class Platform
{
    public long Id { get; set; }

    [StringLength(30)]
    [Required]
    public string Name { get; set; }

    public virtual IList<Game> Games { get; set; } = [];
}