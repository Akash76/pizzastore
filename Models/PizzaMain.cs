namespace pizzastore.Models
{
    public abstract class PizzaMain
    {
        private string name;
        public void SetName(string name) {
            this.name = name;
        }
        public string GetName() {
            return name;
        }
        public abstract int GetCost();
    }
}