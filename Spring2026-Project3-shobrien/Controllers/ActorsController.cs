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
using System.Text.Json;
using VaderSharp2;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace Spring2026_Project3_shobrien.Controllers
{
    public class ActorsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ActorsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var actors = await _context.Actors.ToListAsync();  
            return View(actors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Gender,Age,IMDBLink")] Actor actor, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null && Photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Photo.CopyToAsync(memoryStream);
                    actor.Photo = memoryStream.ToArray(); 
                }
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
               
            }
            return View(actor);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.ActorID == id);

            if (actor == null) return NotFound();

            string json = @"{
              ""tweets"": [
                ""Just watched Pedro Pascal in his latest series—he absolutely steals every scene!"",
                ""Pedro Pascal’s charm and talent never fail to impress. Truly one of a kind."",
                ""I feel like Pedro Pascal could play any role and make it iconic."",
                ""Can we talk about Pedro Pascal’s wardrobe choices in the new show? Chef’s kiss!"",
                ""Every time Pedro Pascal smiles on screen, my day gets better."",
                ""Pedro Pascal deserves every award coming his way. What a performer!"",
                ""The intensity Pedro Pascal brings to his roles is unmatched."",
                ""I could watch Pedro Pascal deliver monologues all day, honestly."",
                ""Pedro Pascal + action scenes = pure perfection. Never disappoints!"",
                ""Why is Pedro Pascal so effortlessly cool in every role he takes on?""
              ]
            }";

            //var endpoint = _configuration["gpt-4.1-mini:API_Endpoint"];
            //var apiKey = _configuration["gpt-4.1-mini:API_Key"];

            //if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(endpoint))
            //{
            //    throw new Exception("API config not loading correctly");
            //}

            //ChatClient client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(apiKey)).GetChatClient("gpt-4.1-mini");

            //var messages = new ChatMessage[]
            //{
            //    new SystemChatMessage("You will output exactly 10 short simulated tweets in valid JSON format, nothing else."),
            //    new UserChatMessage($@"
            //    Generate exactly ten tweets about actor '{actor.Name}'. 
            //    Return the output in valid JSON with this structure:
            //    {{
            //      ""tweets"": [
            //        ""Tweet 1"",
            //        ""Tweet 2"",
            //        ""Tweet 3"",
            //        ""Tweet 4"",
            //        ""Tweet 5"",
            //        ""Tweet 6"",
            //        ""Tweet 7"",
            //        ""Tweet 8"",
            //        ""Tweet 9"",
            //        ""Tweet 10"",
            //      ]
            //    }}
            //    Do not include extra text outside the JSON.
            //    ")
            //};

            //var result = await client.CompleteChatAsync(messages);

            //string json = result.Value.Content.FirstOrDefault()?.Text ?? "{\"tweets\":[]}";

            var tweets = JsonSerializer.Deserialize<ActorTweetResponse>(json);

            var viewModel = new ActorDetailsViewModel
            {
                Actor = actor,
                Tweets = tweets?.Tweets ?? new List<string>()
            };

            var analyzer = new SentimentIntensityAnalyzer();
            foreach (var review in viewModel.Tweets)
            {
                var score = analyzer.PolarityScores(review).Compound;
                viewModel.Sentiments.Add(score);
            }

            viewModel.AverageSentiment = viewModel.Sentiments.Count > 0 ? viewModel.Sentiments.Average() : 0;

            return View(viewModel);

            //var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieID == id);

            //if (movie == null) return NotFound();

            //ChatClient client = new AzureOpenAIClient(APIEndpoint, APICredential).GetChatClient("azureml://registries/azure-openai/models/gpt-4.1-mini/versions/2025-04-14");

            //ClientResult<ChatCompletion> result = await client.CompleteChatAsync(messages);

            //return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.ActorID == id);

            if (actor == null) return NotFound();

            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ActorID,Name,Gender,Age,IMDBLink")] Actor actor, IFormFile Photo)
        {
            ModelState.Remove("Photo");

            if (id != actor.ActorID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var actorToUpdate = await _context.Actors.FindAsync(id);

                if (actorToUpdate == null)
                    return NotFound();

                actorToUpdate.Name = actor.Name;
                actorToUpdate.Gender = actor.Gender;
                actorToUpdate.Age = actor.Age;
                actorToUpdate.IMDBLink = actor.IMDBLink;

                if (Photo != null && Photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Photo.CopyToAsync(memoryStream);
                    actorToUpdate.Photo = memoryStream.ToArray();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Actors.FirstOrDefaultAsync(m => m.ActorID == id);

            if (movie == null) return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor != null)
            {
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
