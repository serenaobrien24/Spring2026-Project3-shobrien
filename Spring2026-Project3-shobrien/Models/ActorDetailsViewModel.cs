

namespace Spring2026_Project3_shobrien.Models
{
    public class ActorDetailsViewModel
    {
        public Actor Actor { get; set; }
        public List<string> Tweets { get; set; } = new();
        public List<double> Sentiments { get; set; } = new();
        public double AverageSentiment { get; set; }

        public List<Movie> Movies { get; set; } = new();
    }

        
}
