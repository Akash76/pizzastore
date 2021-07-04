namespace pizzastore.Models
{
    public class ChickenFiesta: PizzaMain
    {
        public ChickenFiesta() => SetName("Chicken Fiesta");
        public override int GetCost() => 200;
    }
}