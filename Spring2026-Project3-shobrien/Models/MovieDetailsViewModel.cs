

namespace Spring2026_Project3_shobrien.Models
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public List<string> Reviews { get; set; } = new();
        public List<double> Sentiments { get; set; } = new();
        public double AverageSentiment { get; set; }
        }
}
