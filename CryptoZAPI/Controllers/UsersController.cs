using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CryptoZAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IRepository<User> repository;
        private readonly IMapper _mapper;

        public UsersController(IRepository<User> repository, IMapper mapper) {
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
  
                
                userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userToAdd.Password);

                UserForViewDto user = _mapper.Map<UserForViewDto>(await repository.Create(userToAdd));

                await repository.SaveDB();
                return Created($"/users", user);
            }
            catch (OperationCanceledException e)
            {
                Log.Error(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }
        }


        // PUT users/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Put(int id, [FromBody] UserForUpdateDto updateUser) {
            try {

                //if (foundUsers.Count == 0)
                //{
                //    return NotFound();
                //}

                //if (foundUsers.Count > 1)
                //{
                //    // Error de argumentos
                //}

                //int userId = foundUsers[0].Id;

                User user = _mapper.Map<User>(updateUser);
                user.Id = id;

                UserForViewDto updatedUser = _mapper.Map<UserForViewDto>(await repository.Update(user));
                await repository.SaveDB();
                return Ok(updatedUser);
            }

            catch (KeyNotFoundException e)
            {
                Log.Warning("No content found");
                return NotFound();
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.InnerException.Message);
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
                    Log.Warning("No content found");
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
                Log.Error(e.Message);
                return NotFound();
            }
            catch (Exception e) {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed"); ;
            }
        }

    }
}
