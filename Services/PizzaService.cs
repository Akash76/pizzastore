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
    public class PizzaService
    {
        private readonly IMongoCollection<Pizza> _pizza;

        public PizzaService(IDatabaseSettings settings, IConfiguration configuration) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _pizza = database.GetCollection<Pizza>(settings.PizzaCollection);
        }

        public Pizza CreateOrder(Pizza pizza) {
            _pizza.InsertOne(pizza);

            return pizza;
        }

        public Pizza GetPizzaByOrderId(string id) => _pizza.Find(pizza => pizza.OrderId == id).SingleOrDefault();

        public void CompleteOrder(Pizza newPizza) => _pizza.ReplaceOne(pizza => pizza.Id == newPizza.Id, newPizza);
    }
}