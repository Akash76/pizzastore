namespace pizzastore.Models
{
    public class PizzaType: ToppingsDecorator
    {
        ToppingsDecorator pizza;
        string type;
        public PizzaType(ToppingsDecorator pizza, string type) {
            this.pizza = pizza;
            this.type = type;
        }

        public override string GetName() {
            if(type.ToUpper() == "THIN_CRUST") {
                return "Thin crust: " + pizza.GetName();
            } else if(type.ToUpper() == "PAN_CRUST") {
                return "Pan crust: " + pizza.GetName();
            }

            return null;
        }

        public override int GetCost()
        {
            if(type.ToUpper() == "THIN_CRUST") {
                return 30 + pizza.GetCost();
            } else if(type.ToUpper() == "PAN_CRUST") {
                return 50 + pizza.GetCost();
            }

            return 0;
        }
    }
}