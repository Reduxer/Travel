using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel.Application.Common.Interfaces;
using Travel.Application.Dtos.User;
using Travel.Domain.Entities;
using Travel.Identity.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Travel.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>()
        {
            new User()
            {
                Id = 1,
                FirstName = "Lesamel",
                LastName = "Banares",
                Username = "lesamelb",
                Password = "pw12345!"
            }
        };
        private readonly AuthSettings _authSettings;

        public UserService(IOptions<AuthSettings> authSettingOptions)
        {
            _authSettings = authSettingOptions.Value;
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            var user = _users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            
            if(user is null)
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return new AuthenticationResponse(user, token);
        }

        private string GenerateJwtToken(User user)
        {
            byte[] key = Encoding.ASCII.GetBytes(_authSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetById(int id)
        {
            return _users.SingleOrDefault(u => u.Id == id);
        }
    }
}
