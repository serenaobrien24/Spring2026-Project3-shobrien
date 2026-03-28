using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_shobrien.Data;
using Spring2026_Project3_shobrien.Models;
using VaderSharp2;

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

                var exists = await _context.MovieActors.AnyAsync(ma => ma.MovieID == model.MovieID && ma.ActorID == model.ActorID);

                if (exists)
                {
                    ModelState.AddModelError("", "This movie-actor relationship already exists.");

                    ViewBag.Actors = _context.Actors.ToList();
                    ViewBag.Movies = _context.Movies.ToList();
                    return View(model);
                }

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

        public async Task<IActionResult> Details(int movieID, int actorID)
        {
            var link = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Include(ma => ma.Movie)
                .FirstOrDefaultAsync(ma => ma.MovieID == movieID && ma.ActorID == actorID);

            if (link == null) return NotFound();

            var viewModel = new MovieActorDetailsViewModel
            {
                movie = link.Movie,
                actor = link.Actor,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int movieID, int actorID)
        {
            var link = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Include(ma => ma.Movie)
                .FirstOrDefaultAsync(ma => ma.MovieID == movieID && ma.ActorID == actorID);

            if (link == null) return NotFound();

            ViewBag.Actors = _context.Actors.ToList();
            ViewBag.Movies = _context.Movies.ToList();

            var vm = new MovieActorViewModel
            {
                ActorID = link.ActorID,
                MovieID = link.MovieID,
                ActorName = link.Actor.Name,
                MovieTitle = link.Movie.Title
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieActorViewModel model, int oldMovieID, int oldActorID)
        {
            var link = await _context.MovieActors
                .FirstOrDefaultAsync(ma => ma.MovieID == oldMovieID && ma.ActorID == oldActorID);

            if (link == null) return NotFound();

            if (ModelState.IsValid)
            {
                _context.MovieActors.Remove(link);

                var newLink = new MovieActor
                {
                    MovieID = model.MovieID,
                    ActorID = model.ActorID
                };
                _context.MovieActors.Add(newLink);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            ViewBag.Actors = _context.Actors.ToList();
            ViewBag.Movies = _context.Movies.ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(int movieID, int actorID)
        {
            var link = await _context.MovieActors
                .Include(ma => ma.Actor)
                .Include(ma => ma.Movie)
                .FirstOrDefaultAsync(ma => ma.MovieID == movieID && ma.ActorID == actorID);

            if (link == null) return NotFound();

            var vm = new MovieActorViewModel
            {
                ActorID = link.ActorID,
                MovieID = link.MovieID,
                ActorName = link.Actor.Name,
                MovieTitle = link.Movie.Title
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int movieID, int actorID)
        {
            var link = await _context.MovieActors
                .FirstOrDefaultAsync(ma => ma.MovieID == movieID && ma.ActorID == actorID);

            if (link != null)
            {
                _context.MovieActors.Remove(link);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
