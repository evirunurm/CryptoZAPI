using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Roles {
    public class UserRole : IdentityRole<int> {
        [Required]
        public string Notes { get; set; }
    }
}
