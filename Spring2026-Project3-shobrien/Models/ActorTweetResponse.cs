using System.Text.Json.Serialization;

namespace Spring2026_Project3_shobrien.Models
{
    public class ActorTweetResponse
    {
        [JsonPropertyName("tweets")]
        public List<string> Tweets { get; set; } = new(); 
    }
}
