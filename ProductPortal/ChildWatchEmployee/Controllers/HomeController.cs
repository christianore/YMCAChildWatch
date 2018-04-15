using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchApi.Data;
using System.Data.SqlClient;
using System.Configuration;
using ChildWatchEmployee.Models;
using System.Web.Security;
using System.Data;

namespace ChildWatchEmployee.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            ViewBag.Title = "Employee Main Menu";
            string connectionString = ConfigurationManager.ConnectionStrings["database"].ToString();

            SqlConnection databaseConnection = new SqlConnection(connectionString);
            ReportManager report = new ReportManager(databaseConnection);

            DataTable signin = report.GetChildrenSignedIn();

            List<SignIn> current = new List<SignIn>();

            foreach (DataRow record in signin.Rows)
                current.Add(new SignIn()
                {
                    ChildID = (int)record["child_id"],
                    FirstName = (string)record["child_fName"],
                    LastName = (string)record["child_lName"],
                    LocationName =(string)record["loc_name"],
                    BirthDate = (DateTime)record["birthdate"]
                });

            return View(current);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new UserLogin());
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLogin model)
        {
            bool validated = false;
            if (ModelState.IsValid)
            {
                if (model.Username.ToLower() == "root")
                {
                    // Application user override
                    validated = Properties.Settings.Default.root == model.Password;
                }
                else
                {
                    // Regular user login logic
                }

                if (validated)
                    FormsAuthentication.RedirectFromLoginPage("core", true);
            }

                       
            return View(new UserLogin(model.Username, null));
        }

    }
}