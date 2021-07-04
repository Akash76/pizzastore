namespace pizzastore.Models
{
    public class Pizza
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string PizzaType { get; set; }
        public int Cost { get; set; }
        public string Status { get; set; }
    }
}