using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ChildWatchApi.Entity;

namespace ChildWatchEmployee.Models
{
    public class AppEmployee
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        public bool Administrator { get; set; }
        public bool Blocked { get; set; }
        public bool NeedsReset { get; set; }

        public AppEmployee()
        {
            FirstName = "";
            LastName = "";
            Administrator = false;
            
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