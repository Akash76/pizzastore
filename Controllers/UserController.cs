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
    [Route("api/users")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly TokenService _token;
        private readonly UserService _user;
        public UserController(UserService user) {
            _user = user;
        }
        public string GetDetails() {
            var username = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                username = identity.FindFirst(ClaimTypes.Email).Value;
            }
            return username;
        }

        [AllowAnonymous]
        [HttpPost("createUser")]
        public ActionResult<User> CreateUser([FromBody] User body) {
            _user.Create(body);

            return body;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] User user) {
            var token = _user.Authenticate(user.Username, user.Password);
            if(token == null) {
                return Unauthorized();
            }
            _token.SetToken(token);
            return Ok(new {token, user});
        }

        [HttpGet("logout")]
        public ActionResult Logout() {
            var request = Request;
            var headers = request.Headers;
            var handler = new JwtSecurityTokenHandler();
            var token = headers["Authorization"].ToString().Replace("Bearer ", "");

            _token.Deactivate(token);
            string message = "Successfully logged out";
            return Ok(new {message});
        }

        [HttpGet("getUsers")]
        public ActionResult<List<User>> GetUsers() {
            List<User> result = _user.GetAllUsers();
            return result;
        }

        [HttpGet("getUserById/{id:length(24)}")]
        public ActionResult<User> GetUserById(string id) {
            User user = _user.Find(id);

            return user;
        }

        [HttpGet("getUserByUsername")]
        public ActionResult<User> GetUserByUsername() {
            string username = GetDetails();
            User user = _user.FindByUsername(username);

            return user;
        }

        [HttpGet("testToken")]
        public ActionResult TestToken() {
            var request = Request;
            var headers = request.Headers;
            var handler = new JwtSecurityTokenHandler();
            var token = headers["Authorization"].ToString().Replace("Bearer ", "");
            // authHeader = authHeader.Replace("Bearer ", "");
            // var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
            // var claims = tokenS.Claims;
            
            string user = GetDetails();
            return Ok(new {user});
        }
    }
}