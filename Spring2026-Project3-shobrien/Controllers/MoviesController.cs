using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_shobrien.Data;
using Spring2026_Project3_shobrien.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using VaderSharp2;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace Spring2026_Project3_shobrien.Controllers
{
    public class MoviesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public MoviesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            var endpoint = _configuration["gpt-4.1-mini:API_Endpoint"];
            var apiKey = _configuration["gpt-4.1-mini:API_Key"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
            {
                throw new Exception("API config not loading correctly");
            }

            ChatClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey)).GetChatClient("gpt-4.1-mini");

            var testMessages = new ChatMessage[]
            {
                new UserChatMessage("Say hello in one sentence.")
            };

            var testResult = await client.CompleteChatAsync(testMessages);

            var testOutput = testResult.Value.Content[0].Text;

            ViewBag.Test = testOutput;
            return View(movie);

            //var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            //if (movie == null) return NotFound();

            //ChatClient client = new AzureOpenAIClient(APIEndpoint, APICredential).GetChatClient("azureml://registries/azure-openai/models/gpt-4.1-mini/versions/2025-04-14");

            //var messages = new ChatMessage[]
            //{
            //    new SystemChatMessage("You will output exactly 5 short reviews in valid JSON format, nothing else."),
            //    new UserChatMessage($@"
            //    Generate exactly five short movie reviews (2-3 sentences each) for the movie '{movie.Title}' released in {movie.ReleaseYear}. 
            //    Return the output in valid JSON with this structure:
            //    {{
            //      ""reviews"": [
            //        ""Review 1"",
            //        ""Review 2"",
            //        ""Review 3"",
            //        ""Review 4"",
            //        ""Review 5""
            //      ]
            //    }}
            //    Do not include extra text, explanations, or formatting outside this JSON. Each review should be unique and reflect a realistic audience perspective.
            //    ")
            //};

            //ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages);

            //return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("MovieID,Title,Genre,ReleaseYear,IMDBLink")] Movie movie, IFormFile Poster)
        {
            ModelState.Remove("Poster");

            if (id != movie.MovieID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var movieToUpdate = await _context.Movies.FindAsync(id);

                if (movieToUpdate == null)
                    return NotFound();

                movieToUpdate.Title = movie.Title;
                movieToUpdate.Genre = movie.Genre;
                movieToUpdate.ReleaseYear = movie.ReleaseYear;
                movieToUpdate.IMDBLink = movie.IMDBLink;

                if (Poster != null && Poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Poster.CopyToAsync(memoryStream);
                    movieToUpdate.Poster = memoryStream.ToArray();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
