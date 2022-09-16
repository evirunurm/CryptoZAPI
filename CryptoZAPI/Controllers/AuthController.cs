using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTO;
using Repo;
using Microsoft.EntityFrameworkCore;
using Serilog;
using RestCountriesServices;
using System.Text.RegularExpressions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Models.Roles;

namespace CryptoZAPI.Controllers {

    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase {

        public static int CheckAuthorizatedUser(ClaimsPrincipal? claims, string? claim) {
            if (claims == null || claim == null || claim == "") {
                throw new ArgumentNullException();
            }
            return Int32.Parse(claims.FindFirst(claim).Value);
        }


        private readonly IRepository<User> repository;
        private readonly IRepository<Country> repositoryCountry;
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly IMapper _mapper;


        public AuthController(IRepository<User> repository, IRepository<Country> repositoryCountry, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, RoleManager<UserRole> roleManager) {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.repositoryCountry = repositoryCountry ?? throw new ArgumentNullException(nameof(repositoryCountry));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public record AuthenticateRequest(string UserEmail, string Password);

        // POST users
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Post([FromBody] UserForLoginDto newUser) {
            try {

                if (!Regex.Match(newUser.Email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").Success)
                    ModelState.AddModelError("Email", "Please enter a valid email");

                if (!ModelState.IsValid) {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var user = await userManager.FindByEmailAsync(newUser.Email);

                if (user is null || !await userManager.CheckPasswordAsync(user, newUser.Password)) {
                    return Forbid();
                }

                var claims = await CreateClaims(user);
                var jwt = CreateJWTToken(claims);

                return Created($"/me", (new {
                    AccessToken = jwt
                })
                );
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

        private async Task<List<Claim>> CreateClaims(User user) {
            var roles = await userManager.GetRolesAsync(user);

            // Generamos un token según los claims
            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                };


            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private string CreateJWTToken(List<Claim> claims) {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: this.configuration["Jwt:Issuer"],
                audience: this.configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserForViewDto))]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto newUser) {

            try {
                var foundCountry = await repositoryCountry.FindBy(c => c.CountryCode == newUser.CountryCode.ToUpper()).ToListAsync();

                /* DUDA PREGUNTAR EN CLASE */
                if (!foundCountry.Any()) {
                    ModelState.AddModelError("Country", "Please enter a Country Code (2 characters)");
                    return BadRequest(new UnprocessableEntityObjectResult(ModelState));
                }

                Country country = foundCountry[0];

                var userToAdd = this._mapper.Map<User>(newUser);
                userToAdd.CountryId = country.Id;

                var createdUser = await userManager.CreateAsync(userToAdd, newUser.Password);

                bool roleExists = await roleManager.RoleExistsAsync("User");
                if (!roleExists) {
                    // first we create User role   
                    var role = new UserRole();
                    role.Name = "User";
                    role.Notes = "test";
                    await roleManager.CreateAsync(role);
                }

                if (!createdUser.Succeeded) {
                    await userManager.AddToRoleAsync(userToAdd, "User");

                    return Created($"/me", (new {
                        Email = userToAdd.Email,
                        UserName = userToAdd.UserName,
                    }));
                }
                await this.repository.SaveDB();
                return Conflict(createdUser.Errors);
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

