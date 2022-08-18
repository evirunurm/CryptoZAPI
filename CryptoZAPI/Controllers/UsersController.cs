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


        //POST
        [HttpPost(Name = "PostUser")]
        public User Post([FromBody] User user)
        {
            // Save in DB
            return user;
        }


        // PUT
        // [Route("users/{id}")]
        [HttpPut(Name = "PutUser")]
        public User Put(int id, [FromBody] User newUser)
        {
            // hara cosas
            return newUser;
        }


        // GET
        [HttpGet(Name = "GetUser")]
        public User Get(int id)
        {
            // hara cosas
            return new User { Id = Guid.NewGuid(), Email = null, Name = null, Password = null, Salt = null };
        }




    }
}
