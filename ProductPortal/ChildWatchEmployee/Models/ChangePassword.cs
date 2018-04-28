using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class ChangePassword : UpdateEmployee 
    {
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string AdminPassword { get; set; }

        public new RegisterEmployee Employee { get; set; }

        public ChangePassword() : base()
        {
            AdminPassword = "";
        }
    }
}