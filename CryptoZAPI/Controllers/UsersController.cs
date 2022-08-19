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

        // POST users
        [HttpPost]
        public User Post([FromBody] User user)
        {
            // Convert user.password to Hash + salt
            // Add user.Salt
            // Save in Users table.

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
        public User? FindOne(int id)
        {
            // Cargar
            List<User> Users = new List<User>();
            User? user = null;

            try
            {
                user = Users.First(user => user.Id.Equals(id));
            }
            catch (ArgumentNullException e)
            {
                // Send a code 
                Console.WriteLine(e.Message);
            }

            return user;
        }

    }
}
