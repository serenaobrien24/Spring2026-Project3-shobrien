using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_shobrien.Data;
using Spring2026_Project3_shobrien.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// seeding database (without pictures), also I didn't know how to do this so credit to the Internet
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    // Seed Actors
    if (!context.Actors.Any())
    {
        var actors = new List<Actor>
        {
            new Actor { Name = "Leonardo DiCaprio", Gender = "Male", Age = 48, IMDBLink = "https://www.imdb.com/name/nm0000138/" },
            new Actor { Name = "Meryl Streep", Gender = "Female", Age = 73, IMDBLink = "https://www.imdb.com/name/nm0000658/" },
            new Actor { Name = "Tom Hanks", Gender = "Male", Age = 66, IMDBLink = "https://www.imdb.com/name/nm0000158/" },
            new Actor { Name = "Scarlett Johansson", Gender = "Female", Age = 38, IMDBLink = "https://www.imdb.com/name/nm0424060/" },
            new Actor { Name = "Brad Pitt", Gender = "Male", Age = 59, IMDBLink = "https://www.imdb.com/name/nm0000093/" },
            new Actor { Name = "Emma Stone", Gender = "Female", Age = 34, IMDBLink = "https://www.imdb.com/name/nm1297015/" },
            new Actor { Name = "Morgan Freeman", Gender = "Male", Age = 86, IMDBLink = "https://www.imdb.com/name/nm0000151/" },
            new Actor { Name = "Jennifer Lawrence", Gender = "Female", Age = 32, IMDBLink = "https://www.imdb.com/name/nm2225369/" },
            new Actor { Name = "Robert Downey Jr.", Gender = "Male", Age = 58, IMDBLink = "https://www.imdb.com/name/nm0000375/" },
            new Actor { Name = "Anne Hathaway", Gender = "Female", Age = 40, IMDBLink = "https://www.imdb.com/name/nm0004266/" }
        };
        context.Actors.AddRange(actors);
        context.SaveChanges();
    }

    // Seed Movies
    if (!context.Movies.Any())
    {
        var movies = new List<Movie>
        {
            new Movie { Title = "Inception", Genre = "Sci-Fi", ReleaseYear = 2010, IMDBLink = "https://www.imdb.com/title/tt1375666/" },
            new Movie { Title = "The Post", Genre = "Drama", ReleaseYear = 2017, IMDBLink = "https://www.imdb.com/title/tt6294822/" },
            new Movie { Title = "Forrest Gump", Genre = "Drama", ReleaseYear = 1994, IMDBLink = "https://www.imdb.com/title/tt0109830/" },
            new Movie { Title = "Avengers: Endgame", Genre = "Action", ReleaseYear = 2019, IMDBLink = "https://www.imdb.com/title/tt4154796/" },
            new Movie { Title = "La La Land", Genre = "Musical", ReleaseYear = 2016, IMDBLink = "https://www.imdb.com/title/tt3783958/" }
        };
        context.Movies.AddRange(movies);
        context.SaveChanges();
    }

    // Seed MovieActor relationships
    if (!context.MovieActors.Any())
    {
        var leonardo = context.Actors.First(a => a.Name == "Leonardo DiCaprio");
        var meryl = context.Actors.First(a => a.Name == "Meryl Streep");
        var tom = context.Actors.First(a => a.Name == "Tom Hanks");
        var scarlett = context.Actors.First(a => a.Name == "Scarlett Johansson");
        var brad = context.Actors.First(a => a.Name == "Brad Pitt");
        var emma = context.Actors.First(a => a.Name == "Emma Stone");
        var morgan = context.Actors.First(a => a.Name == "Morgan Freeman");
        var jennifer = context.Actors.First(a => a.Name == "Jennifer Lawrence");
        var robert = context.Actors.First(a => a.Name == "Robert Downey Jr.");
        var anne = context.Actors.First(a => a.Name == "Anne Hathaway");

        var inception = context.Movies.First(m => m.Title == "Inception");
        var thePost = context.Movies.First(m => m.Title == "The Post");
        var forrest = context.Movies.First(m => m.Title == "Forrest Gump");
        var endgame = context.Movies.First(m => m.Title == "Avengers: Endgame");
        var laland = context.Movies.First(m => m.Title == "La La Land");

        var movieActors = new List<MovieActor>
        {
            new MovieActor { Actor = leonardo, Movie = inception },
            new MovieActor { Actor = scarlett, Movie = inception },
            new MovieActor { Actor = meryl, Movie = thePost },
            new MovieActor { Actor = tom, Movie = forrest },
            new MovieActor { Actor = morgan, Movie = forrest },
            new MovieActor { Actor = robert, Movie = endgame },
            new MovieActor { Actor = scarlett, Movie = endgame },
            new MovieActor { Actor = emma, Movie = laland },
            new MovieActor { Actor = anne, Movie = laland },
            new MovieActor { Actor = brad, Movie = inception } 
        };

        context.MovieActors.AddRange(movieActors);
        context.SaveChanges();
    }
}

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
