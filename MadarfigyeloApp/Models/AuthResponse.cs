using System.Text.Json.Serialization;

namespace MadarfigyeloApp.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsSuccess => !string.IsNullOrEmpty(Token);
    }
}
