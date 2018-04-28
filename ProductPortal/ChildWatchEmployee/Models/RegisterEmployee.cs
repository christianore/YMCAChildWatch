using ChildWatchApi.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class RegisterEmployee : AppEmployee
    {
        
        [Required]
        public string Password { get; set; }
        
        public RegisterEmployee() : base()
        {
            Password = "";
        }
    }
}