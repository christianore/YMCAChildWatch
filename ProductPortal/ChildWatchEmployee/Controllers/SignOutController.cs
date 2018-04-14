using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchEmployee.Models;
using ChildWatchApi.Data;
using System.Configuration;

namespace ChildWatchEmployee.Controllers
{
    public class SignOutController : Controller
    {

        SignInManager signin = new SignInManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));
        // GET: SignOut
        public ActionResult SignOut()
        {
            ViewBag.Title = "Employee - Sign Out Child";
            return View();

        }

        [HttpPost]
        public ActionResult SignOut(Models.SignOut signOut)
        {
            if (ModelState.IsValid)
            {
                if (signin.SignOut(int.Parse(signOut.BandNum)))
                {
                    return View(signOut);
                }

            }
            return View(signOut);
        }
    }
}