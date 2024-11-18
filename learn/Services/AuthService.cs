using learn.Entities;
using learn.Helpers;
using learn.Models.Auth;
using learn.Models.Users;
using learn.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace learn.Services
{

   public interface IAuthService
    {
        Task<AuthResponse> Authenticate(AuthRequest model);
        Task<AuthResponse> Register(CreateRequest model);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _config = configuration;
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> Authenticate(AuthRequest model)
        {
            var user = await _userRepository.GetByEmail(model.Email);
            var password = VerifyPassword(model.Password, user.Password);
            if (user == null || password == null)
                return null;
            var token = GenerateJwtToken(user);
            return new AuthResponse(user, token);
        }

        public async Task<AuthResponse> Register(CreateRequest model)
        {
            var user = await _userRepository.GetByEmail(model.Email);
            if (user != null)
                return null;
            user = new User
            {
                Fullname = model.Fullname,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Role = Role.User
            };
            await _userRepository.Create(user);
            var token = GenerateJwtToken(user);
            return new AuthResponse(user, token);
        }

        private string VerifyPassword(string typePassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(typePassword, storedPassword) ? storedPassword : null;
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Secret").Value));


            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
   }
