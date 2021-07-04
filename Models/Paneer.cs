namespace pizzastore.Models
{
    public class Paneer: ToppingsDecorator
    {
        PizzaMain pizza;
        public Paneer(PizzaMain pizza) {
            this.pizza = pizza;
        }

        public override string GetName()
        {
            return "Paneer, " + pizza.GetName();
        }

        public override int GetCost()
        {
            return 40 + pizza.GetCost();
        }
    }
}