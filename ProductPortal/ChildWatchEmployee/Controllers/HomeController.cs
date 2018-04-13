using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchApi.Data.Report;
using System.Data.SqlClient;
using System.Configuration;

namespace ChildWatchEmployee.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "Employee Main Menu";
            string connectionString = ConfigurationManager.ConnectionStrings["database"].ToString();

            SqlConnection databaseConnection = new SqlConnection(connectionString);
            ReportManager report = new ReportManager(databaseConnection);
            SigninReport signin = report.GetChildrenSignedIn();

            List<SignIn> current = new List<SignIn>();

            foreach (SigninRecord record in signin.Rows)
                current.Add(new SignIn()
                {
                    ChildID = record.ChildID,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    LocationName = record.Location,
                    BirthDate = record.BirthDate
                });

            return View(current);
        }
    }
}