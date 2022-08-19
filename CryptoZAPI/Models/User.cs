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

        public User(string name, string email, string password, string salt)
        {
            Name = name;
            Email = email;
            Password = password;
            Salt = salt;
        }
    }
}
