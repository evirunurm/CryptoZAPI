using CryptoZAPI.Models;
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            // TODO: Convert user.password to Hash + salt
            // TODO: Add user.Salt
            try
            {
                await repository.CreateUser(user);
            }
            catch (Exception e) // TODO: Change Exception type
            { 
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }

            return Created($"/users/{user.Id}", user); // TODO: Idk what is this??? 
            // return Ok(user);
        }


        // PUT users/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Put(int id, [FromBody] User newUser)
        {

            User user = null;
            try
            {
                user = await repository.ModifyUser(id, newUser); 
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }
            return Ok(user);
        }


        // GET
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(int id)
        {
            User user;
            try
            {
                user = await repository.GetOneUser(id);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }


            return Ok(user);
        }

    }
}
