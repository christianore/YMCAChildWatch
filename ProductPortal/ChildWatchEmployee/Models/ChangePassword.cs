using ChildWatchApi.Data;
using ChildWatchApi.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildWatchEmployee.Models
{
    public class ChangePassword 
    {
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string AdminPassword { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        public ChangePassword() : base()
        {
            AdminPassword = "";
            Password = "";
            EmployeeID = -1;
        }

        public SelectList CurrentEmployees()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            OrganizationManager manager =
                new OrganizationManager(ConfigurationManager.ConnectionStrings["database"].ToString());

            List<Employee> employees = manager.GetEmployees();

            foreach (Employee e in employees)
            {
                items.Add(new SelectListItem()
                {
                    Value = e.ID.ToString(),
                    Text = e.FirstName + " " + e.LastName,
                    Selected = e.ID == EmployeeID
                });
            }

            return new SelectList(items, "Value", "Text");
        }
    }
}