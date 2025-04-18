using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games { get; set; } = null!;

    public DbSet<Genre> Genres { get; set; } = null!;

    public DbSet<Review> Reviews { get; set; } = null!;

    public DbSet<Platform> Platforms { get; set; } = null!;

    public DbSet<BasketItem> Items { get; set; } = null!;

    public DbSet<Basket> Baskets { get; set; } = null!;
}