using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChildWatchEmployee.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public UserLogin() { }
        public UserLogin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}