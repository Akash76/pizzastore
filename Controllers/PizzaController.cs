using System;
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

        public ToppingsDecorator ToppPizza(PizzaMain pizza, string topping) {
            if(topping.ToUpper() == "PANEER") {
                return new Paneer(pizza);
            } else if(topping.ToUpper() == "FRESH_TOMATO") {
                return new FreshTomato(pizza);
            }
            return null;
        }

        [AllowAnonymous]
        [HttpPost("estimateCost")]
        public ActionResult EstimateCost([FromBody] Order order) {
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

            return Ok(new {cost, name});
        }
        
    }
}