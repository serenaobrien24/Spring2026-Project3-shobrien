namespace Spring2026_Project3_shobrien.Models
{
    public class Actor
    {
        public int ActorID { get; set; }

        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string IMDBLink { get; set; }

        public byte[] Photo { get; set; }

        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
