using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace pizzastore.Models
{
    public class Pizza
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string PizzaType { get; set; }
        public int Cost { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public enum Status {
            PLACED,
            COMPLETE,
            CANCELED
        }
    }
}