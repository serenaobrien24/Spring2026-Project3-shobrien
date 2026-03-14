using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_shobrien.Data;
using Spring2026_Project3_shobrien.Models;
using System.IO;
using System.Threading.Tasks;

namespace Spring2026_Project3_shobrien.Controllers
{
    public class MoviesController : Controller
    {

        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();  
            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Genre,ReleaseYear,IMDBLink")] Movie movie, IFormFile Poster)
        {
            if (ModelState.IsValid)
            {
                if (Poster != null && Poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray(); 
                }
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
               
            }
            return View(movie);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);
            
            if (movie == null) return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null) return NotFound();

            return View(movie);
        }
    }
}
