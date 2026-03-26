using System.Text.Json.Serialization;

namespace Spring2026_Project3_shobrien.Models
{
    public class MovieReviewResponse
    {
        [JsonPropertyName("reviews")]
        public List<string> Reviews { get; set; } = new(); 
    }
}
