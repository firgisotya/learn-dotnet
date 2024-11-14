using System.Text.Json.Serialization;

namespace learn.Entities
{
    public class User
    {
        public int id { get; set; }
        public string? fullname { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public Role role { get; set; }

        [JsonIgnore]
        public string? password { get; set; }
    }
}
