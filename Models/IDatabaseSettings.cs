namespace pizzastore.Models
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UserCollection { get; set; }

        string PizzaCollection { get; set; }
    }
}