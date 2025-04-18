using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Services;

namespace Website;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"] ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        }

        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
                                   ?? throw new InvalidOperationException("Could not find Google Client ID");
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
                                       ?? throw new InvalidOperationException("Could not find Google Client Secret");
            })
            .AddFacebook(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Facebook:AppId"]
                                   ?? throw new InvalidOperationException("Could not find Facebook App ID");
                options.ClientSecret = builder.Configuration["Authentication:Facebook:AppSecret"]
                                       ?? throw new InvalidOperationException("Could not find Facebook App Secret");
            })
            .AddTwitter(options =>
            {
                options.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerAPIKey"]
                                      ?? throw new InvalidOperationException("Could not find Twitter Consumer Key");
                options.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"]
                                         ?? throw new InvalidOperationException("Could not find Twitter Consumer Secret");
            });

        var mvcBuilder = builder.Services.AddControllersWithViews();

        if (builder.Environment.IsDevelopment())
        {
            mvcBuilder.AddRazorRuntimeCompilation();
        }

        // IGDB Client
        // builder.Services.AddSingleton<IGDBClient>(_ =>
        // {
        //     var clientId = builder.Configuration["twitch:clientId"] ??
        //                    throw new InvalidOperationException("Could not get Twitch Client ID.");
        //     var clientSecret = builder.Configuration["twitch:clientSecret"] ??
        //                        throw new InvalidOperationException("Could not get Twitch Client Secret");
        //     return new IGDBClient(clientId, clientSecret);
        // });

        builder.Services.AddScoped<IBasketService, BasketService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        app.Run();
    }
}