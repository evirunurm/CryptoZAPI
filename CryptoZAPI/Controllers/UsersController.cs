using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repo;

namespace CryptoZAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Logging
        private readonly ILogger<UsersController> _logger;
        private readonly IRepository repository;

        public UsersController(ILogger<UsersController> logger, IRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        // POST users
        [HttpPost]
        public User? Post([FromBody] User user)
        {
            // TODO: Convert user.password to Hash + salt
            // TODO: Add user.Salt
            try
            {
                repository.CreateUser(user);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e.Message);
                return null;
            }

            return user;
        }


        // PUT users/5
        [HttpPut("{id}")]
        public User? Put(int id, [FromBody] User newUser)
        {
            try
            {
                repository.ModifyUser(id, newUser);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // TODO: Send a code 
                Console.WriteLine(e.Message);
                return null;
            }
            return newUser;
        }


        // GET
        [HttpGet("{id}")]
        public User? FindOne(int id)
        {
            User? user;
            try
            {
                user = repository.GetOneUser(id);
                // TODO: Send a code 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                // Send a code 
                Console.WriteLine(e.Message);
                return null;
            }

            return user;
        }

    }
}
