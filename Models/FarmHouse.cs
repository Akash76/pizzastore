namespace pizzastore.Models
{
    public class FarmHouse: PizzaMain
    {
        public FarmHouse() => SetName("Farm House");
        public override int GetCost() => 150;
    }
}