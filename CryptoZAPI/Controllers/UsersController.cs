using AutoMapper;
using CryptoZAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
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
		private readonly IMapper _mapper;

		public UsersController(ILogger<UsersController> logger, IRepository repository, IMapper mapper)
		{
			_logger = logger;
			this.repository = repository;
			this._mapper = mapper;
			_mapper = mapper;
		}

		// POST users
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserForViewDto))]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		public async Task<IActionResult> Post([FromBody] UserForCreationDto user)
		{
			// TODO: Convert user.password to Hash + salt
			// TODO: Add user.Salt
			try
			{
				User userToAdd = _mapper.Map<User>(user);
				UserForViewDto userForView = _mapper.Map<UserForViewDto>(await repository.CreateUser(userToAdd));
				return Created($"/users", userForView);
			}
			catch (Exception e) // TODO: Change Exception type
			{ 
				Console.WriteLine(e.Message);
				return StatusCode(StatusCodes.Status503ServiceUnavailable, "Database couldn't be accessed");
			}
		}


		// PUT users/5
		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
		[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
		public async Task<IActionResult> Put([FromBody] UserForUpdateDto newUser)
		{

			User user = null;
			try
			{
				user = await repository.ModifyUser(newUser); 
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
