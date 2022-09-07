using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;
using RestCountriesServices;

namespace CryptoZAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IRepository<User> repository;
        private readonly IRepository<Country> repositoryCountry;


        private readonly IMapper _mapper;

        public UsersController(IRepository<User> repository, IRepository<Country> repositoryCountry, IMapper mapper) {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.repositoryCountry = repositoryCountry ?? throw new ArgumentNullException(nameof(repositoryCountry));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // POST users
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] UserForCreationDto newUser) {
            try {

                var foundCountry = await repositoryCountry.FindBy(c => c.CountryCode == newUser.CountryCode.ToUpper()).ToListAsync();

                if (foundCountry.Count < 1) {
                    // Error
                }

                Country country = foundCountry[0];

                User userToAdd = _mapper.Map<User>(newUser);
                userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userToAdd.Password);
                userToAdd.Country = country;
                userToAdd.CountryId = 0;

                UserForViewDto user = _mapper.Map<UserForViewDto>(await repository.Create(userToAdd));

                await repository.SaveDB();
                return Created($"/users", user);
            }
            catch (OperationCanceledException e) {
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
              

                var foundUsers = await repository.FindBy(u => u.Id == id).ToListAsync();

                if (foundUsers.Count < 1) {
                    // Error
                }

                User userToUpdate = foundUsers[0];

                /* TODO Obtener el Country en Users ¿? */
                var foundCountry = await repositoryCountry.FindBy(u => u.CountryCode == updateUser.CountryCode).ToListAsync();

                if (foundCountry.Count < 1) {
                    // Error
                }

                Country country = foundCountry[0];
                userToUpdate.Name = updateUser.Name;
                userToUpdate.Country = country;
                userToUpdate.CountryId = country.Id;
                userToUpdate.Password = updateUser.Password;

                UserForViewDto updatedUser = _mapper.Map<UserForViewDto>(await repository.Update(userToUpdate));
                await repository.SaveDB();
                return Ok(updatedUser);
            }

            catch (KeyNotFoundException e) {
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
        // TODO REGEX
        [HttpGet("{UserEmail}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindByMail(string UserEmail) {
            try {
                var foundUsers = await repository.FindBy(u => u.Email == UserEmail).ToListAsync();

                if (foundUsers.Count == 0) {
                    Log.Warning("No content found");
                    return NotFound();
                }

                if (foundUsers.Count > 1) {
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

        // GET
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindById(int id) {
            try {
                var foundUser = await repository.GetById(id);
                UserForViewDto user = _mapper.Map<UserForViewDto>(foundUser);
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
