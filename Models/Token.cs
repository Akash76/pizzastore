using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pizzastore.Models
{
    public class Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        public string _Token { get; set; }
        public bool Status { get; set; }
    }
}