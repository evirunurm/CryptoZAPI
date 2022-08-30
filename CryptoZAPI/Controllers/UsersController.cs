using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using System.Data.Entity;

namespace CryptoZAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase {
        // Logging
        private readonly ILogger<UsersController> _logger;
        private readonly IRepository<User> repository;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IRepository<User> repository, IMapper mapper) {
            this._logger = logger;
            this.repository = repository;
            this._mapper = mapper;
        }

        // POST users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] UserForCreationDto newUser) {

            try {
                User userToAdd = _mapper.Map<User>(newUser);

                // TODO: Convert user.password to Hash + salt
                // TODO: Add user.Salt

                UserForViewDto user = _mapper.Map<UserForViewDto>(await repository.Create(userToAdd));
                return Created($"/users", user);
            }
            catch (OperationCanceledException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }
        }


        // PUT users/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Put([FromBody] UserForUpdateDto updateUser) {
            try {
                var foundUsers = await repository.FindBy(u => u.Email == updateUser.Email).ToListAsync();

                if (foundUsers.Count == 0)
                {
                    return NotFound();
                }

                if (foundUsers.Count > 1)
                {
                    // Error de argumentos
                }

                int userId = foundUsers[0].Id;

                User user = _mapper.Map<User>(updateUser);


                UserForViewDto updatedUser = _mapper.Map<UserForViewDto>(await repository.Update(user, userId));
                return Ok(updatedUser);
            }

            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }
        }

        // GET
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindOne(string UserEmail) {
            try {
                var foundUsers = await repository.FindBy(u => u.Email == UserEmail).ToListAsync();

                if (foundUsers.Count == 0)
                {
                    return NotFound();
                }

                if (foundUsers.Count > 1)
                {
                    // Error de argumentos
                }

                UserForViewDto user = _mapper.Map<UserForViewDto>(foundUsers[0]);
                return Ok(user);
            }
            catch (ArgumentNullException e) {
                Console.WriteLine(e.Message);
                return NotFound();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

    }
}
