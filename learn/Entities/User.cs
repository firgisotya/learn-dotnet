using System.Text.Json.Serialization;

namespace learn.Entities
{
    public class User
    {
        public int id { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
    }
}
