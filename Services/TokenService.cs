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
    public class TokenService
    {
        private readonly IMongoCollection<Token> _token;
        public TokenService(IDatabaseSettings settings, IConfiguration configuration) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _token = database.GetCollection<Token>("Token");
        }

        public TokenService(){}

        public void SetToken(string token) {
            Token newToken = new Token();
            newToken._Token = token;
            newToken.Status = true;

            _token.InsertOne(newToken);
        }

        public void Deactivate(string deactivatingToken) {
            Token tokenObject = _token.Find(token => token._Token == deactivatingToken).SingleOrDefault();
            tokenObject.Status = false;
        }

        public bool TokenStatus(string checkToken) {
            Token tokenObject = _token.Find(token => token._Token == checkToken).SingleOrDefault();
            return tokenObject.Status;
        }
    }
}