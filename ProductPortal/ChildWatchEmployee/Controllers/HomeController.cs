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
using ChildWatchApi.Utility.Logging;
using ChildWatchApi.Web;
using System.Security.Cryptography;

namespace ChildWatchEmployee.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static Logger log = new Logger(@"C:\ProgramData\YMCA\", @"ChildWatch");
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
            if (ModelState.IsValid)
            {
                // Successful root user
                if (model.Username.ToLower() == "root" && Properties.Settings.Default.root == model.Password)
                {
                    return CreateAuthToken("root", true, false);
                }
                else
                {
                    
                    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());
                    ChildwatchAuthentication auth = ChildwatchAuthentication.Authenticate(model.Username, model.Password, connection);
                    switch(auth.Authentication)
                    {
                        case AuthContext.Admin:
                        case AuthContext.User:
                            return CreateAuthToken(auth.User, auth.Authentication == AuthContext.Admin, false);
                        default:
                            TempData["Message"] = new ResponseMessage()
                            {
                                Error = true,
                                Message = "Failed to authenticate username and password."
                            };
                            break;
                    }
                }                
            } 
            
            return View(new UserLogin(model.Username, null));
        }
        private ActionResult CreateAuthToken(string user, bool admin, bool stayLoggedIn)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, user, DateTime.Now, DateTime.Now.AddHours(1), stayLoggedIn, String.Format("Admin={0}", (admin ? 1 : 0).ToString()));

            string content = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, content)
            {
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath,
                HttpOnly = true
            };

            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }
    }
}