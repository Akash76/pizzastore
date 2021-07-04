namespace pizzastore.Models
{
    public abstract class ToppingsDecorator: PizzaMain
    {
        public abstract new string GetName();
    }
}