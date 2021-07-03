using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Driver;
using pizzastore.Models;

namespace pizzastore.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly string key;

        public UserService(IDatabaseSettings settings, IConfiguration configuration) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _user = database.GetCollection<User>(settings.UserCollection);
            this.key = configuration.GetSection("JwtKey").ToString();
        }

        public string Authenticate(string username, string password) {
            var user = this._user.Find(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (user == null) {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, username),
                }),

                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public User Create(User user) {
            _user.InsertOne(user);

            return user;
        }

        public List<User> GetAllUsers() =>
            _user.Find(user => true).ToList();

        public User Find(string id) =>
            _user.Find(user => user.Id == id).SingleOrDefault();

        public User FindByUsername(string username) =>
            _user.Find(user => user.Username == username).SingleOrDefault();
    }
}