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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CryptoZAPI.Controllers {
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IRepository<User> repository;
        private readonly IRepository<Country> repositoryCountry;
        private readonly IHttpContextAccessor httpContextAccessor;


        private readonly IMapper _mapper;

        public UsersController(IRepository<User> repository, IRepository<Country> repositoryCountry, IMapper mapper, IHttpContextAccessor httpContextAccessor) {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.repositoryCountry = repositoryCountry ?? throw new ArgumentNullException(nameof(repositoryCountry));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.httpContextAccessor = httpContextAccessor;
        }

        public record AuthenticateRequest(string UserEmail, string Password);

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [Authorize]
        public async Task<IActionResult> Put(int userId, [FromBody] UserForUpdateDto updateUser) {
            try {

                var tokenUserId = AuthController.CheckAuthorizatedUser(httpContextAccessor.HttpContext.User, ClaimTypes.NameIdentifier);

                if (tokenUserId != userId) {
                    return Unauthorized();
                }

                if (!ModelState.IsValid) {
                    // return 422 - Unprocessable Entity when validation fails
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                List<User> foundUsers = await repository.FindBy(u => u.Id == userId).ToListAsync();

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
                Log.Warning("No content found: " + e.Message);
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
