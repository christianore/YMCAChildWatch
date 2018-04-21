using ChildWatchApi.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChildWatchEmployee.Models
{
    public class RegisterEmployee 
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public bool Administrator { get; set; }
        public bool Blocked { get; set; }
        public bool NeedsReset { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
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