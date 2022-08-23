﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Required]
        [MaxLength(64)]
        public string Name { get; set; }
		[Required]
        [MaxLength(320)]
        public string Email { get; set; }
		[Required]
        public string Password { get; set; }
        public string Salt { get; set; }

        
    }
}
