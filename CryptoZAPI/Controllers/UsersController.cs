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
using System.Text.RegularExpressions;

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

                if (!Regex.Match(newUser.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success)
                    ModelState.AddModelError("Email", "Please enter a valid email");               

                if (!ModelState.IsValid) {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var foundCountry = await repositoryCountry.FindBy(c => c.CountryCode == newUser.CountryCode.ToUpper()).ToListAsync();

                /* DUDA PREGUNTAR EN CLASE */
                if (!foundCountry.Any()) {
                    ModelState.AddModelError("Country", "Please enter a Country Code (2 characters)");
                    return BadRequest(new UnprocessableEntityObjectResult(ModelState));
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


        // TODO: newPassword, countryCode and name should be optional. 
        // PUT users/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Put(int id, [FromBody] UserForUpdateDto updateUser) {
            try {

                if (!ModelState.IsValid) {
                    // return 422 - Unprocessable Entity when validation fails
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                List<User> foundUsers = await repository.FindBy(u => u.Id == id).ToListAsync();

                if (!foundUsers.Any()) {
                    return BadRequest();
                }
             
                List<Country> foundCountry = await repositoryCountry.FindBy(u => u.CountryCode == updateUser.CountryCode).ToListAsync();

                if (!foundCountry.Any()) {
                    return BadRequest();
                }

                Country country = foundCountry[0];
                User userToUpdate = foundUsers[0];

                User user = _mapper.Map<User>(updateUser);

                user.Country = country;
                user.CountryId = country.Id;

                userToUpdate.UpdateFromUser(user);

                UserForViewDto updatedUser = _mapper.Map<UserForViewDto>(repository.Update(userToUpdate));

                await repository.SaveDB();

                return Ok(updatedUser);
            }

            catch (KeyNotFoundException e) {
                Log.Warning("No content found: "+e.Message);
                return NotFound();
            }
            catch (Exception e) // TODO: Change Exception type
            {
                Log.Error(e.Message);
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
            }
        }

        // TODO: Should return user Id
        // GET
        // TODO REGEX
        [HttpGet("{UserEmail}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> FindByMail(string UserEmail) {
            try {
                if (!Regex.Match(UserEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success) {
                    ModelState.AddModelError("Email", "Please enter a valid email");
                }

                if (!ModelState.IsValid) {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                List<User> foundUsers = await repository.FindBy(u => u.Email == UserEmail).ToListAsync();

                if (!foundUsers.Any()) {
                    Log.Warning("No content found");
                    return NotFound();
                }

                User foundUser = foundUsers[0];

                /*  
                    Tenemos activado el Lazy Loading de Entity FrameWork, así que
                    no obtenemos los objetos Country al hacer el fetch de User
                */
                foundUser.Country = await repositoryCountry.GetById(foundUser.CountryId);

                UserForViewDto user = _mapper.Map<UserForViewDto>(foundUser);
                return Ok(user);
            }
            catch (KeyNotFoundException e) {
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
                User foundUser = await repository.GetById(id);

                /*  
                    Tenemos activado el Lazy Loading de Entity FrameWork, así que
                    no obtenemos los objetos Country al hacer el fetch de User
                */
                foundUser.Country = await repositoryCountry.GetById(foundUser.CountryId);

                UserForViewDto user = _mapper.Map<UserForViewDto>(foundUser);
                return Ok(user);
            }
            catch (KeyNotFoundException e) {
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
