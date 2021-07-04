namespace pizzastore.Models
{
    public class Margharita: PizzaMain
    {
        public Margharita() => SetName("Margharita");
        public override int GetCost() => 100;
    }
}