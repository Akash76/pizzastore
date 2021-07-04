namespace pizzastore.Models
{
    public class SimplePizza: PizzaMain
    {
        public SimplePizza() => SetName("Simple Pizza");
        public override int GetCost() => 50;
    }
}