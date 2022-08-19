using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CryptoZAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Logging
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }


        // Post
        // Put
        // FindOne


        // POST users
        [HttpPost]
        public User Post([FromBody] User user)
        {
            // Save in DB
            return user;
        }


        // PUT users/5
        [HttpPut("{id}")]
        public User Put(int id, [FromBody] User newUser)
        {
            // hara cosas
            return newUser;
        }


        // GET
        [HttpGet("{id}")]
        public User Get(int id)
        {
            // hara cosas
            return new User(null, null, null, null);
        }

    }
}
