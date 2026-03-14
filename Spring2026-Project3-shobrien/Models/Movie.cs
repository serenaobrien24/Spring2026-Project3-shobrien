namespace Spring2026_Project3_shobrien.Models
{
    public class Movie
    {
        public int MovieID { get; set; }

        public string Title { get; set; }
        public string IMDBLink { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }

        public byte[]? Poster { get; set; }

        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
