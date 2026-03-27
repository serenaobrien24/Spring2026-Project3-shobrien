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

        public IActionResult Create()
        {
            ViewBag.Actors = _context.Actors.ToList();
            ViewBag.Movies = _context.Movies.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieActorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var link = new MovieActor
                {
                    ActorID = model.ActorID,
                    MovieID = model.MovieID
                };

                _context.Add(link);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
