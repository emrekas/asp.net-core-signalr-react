using ChatSample.Entities;
using ChatSample.Helpers;
using ChatSample.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ChatSample.Services
{
    public class UserService : IUserService
    {
        //TODO:Implement SQLite
        private readonly List<User> _users = new List<User> {
            new User { Username = "test", FirstName = "emre", LastName = "kas", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim (ClaimTypes.Name, user.Id.ToString ())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = "";
            return user;
        }
    }
}