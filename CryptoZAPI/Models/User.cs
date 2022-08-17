using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class User
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required]
		public string Name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public string Salt { get; set; }

	}
}
