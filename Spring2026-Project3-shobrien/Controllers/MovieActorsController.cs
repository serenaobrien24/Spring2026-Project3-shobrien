using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_shobrien.Data;
using Spring2026_Project3_shobrien.Models;

namespace Spring2026_Project3_shobrien.Controllers
{
    public class MovieActorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieActorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var links = await _context.MovieActors
                .Include(ma => ma.Movie)
                .Include(ma => ma.Actor)
                .Select(ma => new MovieActorViewModel
                {
                    MovieID = ma.MovieID,
                    ActorID = ma.ActorID,
                    MovieTitle = ma.Movie.Title,
                    ActorName = ma.Actor.Name
                })
                .ToListAsync();

            return View(links);
        }
    }
}
