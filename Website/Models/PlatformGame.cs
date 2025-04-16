using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Website.Models;

[PrimaryKey(nameof(GameId), nameof(PlatformId))]
public class PlatformGame
{
    [Microsoft.Build.Framework.Required]
    public long GameId { get; set; }

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    [Microsoft.Build.Framework.Required]
    public long PlatformId { get; set; }

    [ForeignKey(nameof(PlatformId))]
    public virtual Platform Platform { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public double Price { get; set; }
}