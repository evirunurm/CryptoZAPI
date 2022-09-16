using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CryptoZAPI.Models {
    public class UserCurrency {


        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        public string Name { get; set; } = String.Empty;

        public User User { get; set; }// = new User();
        public Currency Currency { get; set; } //= new Currency();



    }
}
