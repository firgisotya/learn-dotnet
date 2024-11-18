using learn.Entities;

namespace learn.Models.Auth
{
    public class AuthResponse
    {

        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthResponse(User user, string token)
        {
            Id = user.id;
            Fullname = user.Fullname;
            Username = user.Username;
            Email = user.Email;
            Token = token;
        }
    }
}
