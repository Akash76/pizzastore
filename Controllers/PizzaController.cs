using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using pizzastore.Models;
using pizzastore.Services;

namespace pizzastore.Controllers
{
    [Authorize]
    [Route("api/order")]
    [ApiController]
    public class PizzaController: Controller
    {
        private readonly PizzaService _pizza;
        public PizzaController(PizzaService pizza) {
            _pizza = pizza;
        }

        public string GetDetails() {
            var username = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                username = identity.FindFirst(ClaimTypes.Email).Value;
            }
            return username;
        }

        public ToppingsDecorator ToppPizza(PizzaMain pizza, string topping) {
            if(topping.ToUpper() == "PANEER") {
                return new Paneer(pizza);
            } else if(topping.ToUpper() == "FRESH_TOMATO") {
                return new FreshTomato(pizza);
            }
            return null;
        }

        public dynamic CostEstimation(Order order) {
            PizzaMain pizza = null;
            int cost = 0;

            if(order.MainPizzaName == "" || order.Toppings == "" || order.PizzaType == "") {
                return Ok(new { cost });
            }

            if(order.MainPizzaName.ToUpper() == "MARGHARITA") {
                pizza = new Margharita();
            } else if(order.MainPizzaName.ToUpper() == "FARM_HOUSE") {
                pizza = new FarmHouse();
            } else if(order.MainPizzaName.ToUpper() == "CHICKEN_FIESTA") {
                pizza = new ChickenFiesta();
            } else if(order.MainPizzaName.ToUpper() == "SIMPLE") {
                pizza = new SimplePizza();
            }

            ToppingsDecorator toppedPizza = ToppPizza(pizza, order.Toppings);
            PizzaType pizzaType = new PizzaType(toppedPizza, order.PizzaType);

            cost = pizzaType.GetCost();
            string name = pizzaType.GetName();

            return new {cost, name};
        }

        public string generateID() {
            return Guid.NewGuid().ToString("N");
        }

        [AllowAnonymous]
        [HttpPost("estimateCost")]
        public ActionResult EstimateCost([FromBody] Order order) {
            var estimation = CostEstimation(order);

            return Ok(new {estimation.cost, estimation.name});
        }

        [HttpPost("placeOrder")]
        public ActionResult<Pizza> PlaceOrder([FromBody] Order order) {
            Pizza pizza = new Pizza();
            var estimation = CostEstimation(order);
            string id = generateID();
            string user = GetDetails();
            pizza.OrderId = id;
            pizza.User = user;
            pizza.Name = estimation.name;
            pizza.Cost = estimation.cost;
            pizza.PizzaType = order.PizzaType;
            pizza.OrderDate = DateTime.Now.ToUniversalTime();
            pizza.OrderStatus = Pizza.Status.PLACED.ToString();

            var response = _pizza.CreateOrder(pizza);

            return Ok(new {response});
        }

        [HttpGet("getAllOrders")]
        public ActionResult<List<Pizza>> GetAllOrders() {
            return _pizza.GetAllOrders();
        }

        [HttpGet("getOrderByOrderId/{id}")]
        public ActionResult GetOrderById(string id) {
            var response = _pizza.GetOrderByOrderId(id);

            return Ok(new {response});
        }

        [HttpGet("getOrdersByUser")]
        public ActionResult<List<Pizza>> GetOrdersByUser() {
            string user = GetDetails();
            var response = _pizza.GetOrdersByUser(user);

            return Ok(new {response});
        }

        [HttpPut("completeOrder/{id}")]
        public ActionResult CompleteOrder(string id) {
            Pizza pizza = _pizza.GetOrderByOrderId(id);
            pizza.OrderStatus = Pizza.Status.COMPLETE.ToString();

            _pizza.UpdateOrder(pizza);

            string message = "Order Completed";

            return Ok(new {message});
        }

        [HttpPut("cancelOrder/{orderId}")]
        public ActionResult CancelOrder(string orderId) {
            Pizza pizza = _pizza.GetOrderByOrderId(orderId);
            pizza.OrderStatus = Pizza.Status.CANCELED.ToString();

            _pizza.UpdateOrder(pizza);

            string message = "Order Canceled";

            return Ok(new {message});
        }
    }
}