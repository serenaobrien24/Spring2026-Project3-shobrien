namespace Spring2026_Project3_shobrien.Models
{
    public class MovieActor
    {
        public int MovieID { get; set; }
        public int ActorID { get; set; }

        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
    }
}
