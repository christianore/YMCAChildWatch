using ChildWatchApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class RegisterEmployee 
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Administrator { get; set; }
        public bool Blocked { get; set; }
        public bool NeedsReset { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public RegisterEmployee()
        {
            FirstName = "";
            LastName = "";
            Administrator = false;

            Password = "";
        }

        public Employee ToEmployee()
        {
            return new Employee()
            {
                ID = this.ID,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Administrator = this.Administrator,
                Blocked = this.Blocked,
                NeedsReset = this.NeedsReset,
                UserName = this.Username
            };
        }
    }
}