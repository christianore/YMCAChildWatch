using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class ChangePassword :UpdateEmployee 
    {
        [Required]
        public string AdminPassword { get; set; }

        public ChangePassword() : base()
        {
            AdminPassword = "";
        }
    }
}