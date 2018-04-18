using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class ChangePassword :UpdateEmployee 
    {
        public string AdminPassword { get; set; }

        public ChangePassword() : base()
        {
            AdminPassword = "";
        }
    }
}