namespace pizzastore.Models
{
    public class FreshTomato:ToppingsDecorator
    {
        PizzaMain pizza;
        public FreshTomato(PizzaMain pizza) {
            this.pizza = pizza;
        }
        public override string GetName()
        {
            return "Fresh Tomato, " + pizza.GetName();
        }
        public override int GetCost()
        {
            return 20 + pizza.GetCost();
        }
    }
}