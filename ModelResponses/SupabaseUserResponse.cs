using Newtonsoft.Json;

namespace WebAPIRestaurantManagement.ModelResponses
{
    public class SupabaseUserResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("user_metadata")]
        public object UserMetadata { get; set; }

        [JsonProperty("app_metadata")]
        public object AppMetadata { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
