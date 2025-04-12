using Microsoft.AspNetCore.Identity;

namespace Website.Models;

public class User : IdentityUser
{
    public ICollection<Review> Reviews { get; set; } = [];
}