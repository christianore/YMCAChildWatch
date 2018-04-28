using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchApi.Data;
using ChildWatchApi.Entity;

namespace ChildWatchEmployee.Models
{
    public class UpdateEmployee
    {
        public AppEmployee Employee { get; set; }

        public UpdateEmployee()
        {
            Employee = new AppEmployee();
        }

        public SelectList CurrentEmployees()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            OrganizationManager manager = 
                new OrganizationManager(ConfigurationManager.ConnectionStrings["database"].ToString());

            List<Employee> employees = manager.GetEmployees();

            foreach(Employee e in employees)
            {
                items.Add(new SelectListItem()
                {
                    Value = e.ID.ToString(),
                    Text = e.FirstName + " " +  e.LastName,
                    Selected = e.ID == Employee.ID
                });
            }

            return new SelectList(items, "Value", "Text");
        }
    }
}