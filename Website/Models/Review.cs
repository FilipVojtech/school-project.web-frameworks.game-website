using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models;

public class Review
{
    public int Id { get; set; }

    [StringLength(40)]
    public string Title { get; set; }

    [Column(TypeName = "text")]
    [StringLength(int.MaxValue)]
    public string Body { get; set; }
}