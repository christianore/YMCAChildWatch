using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchEmployee.Models;


namespace ChildWatchEmployee.Controllers
{
    public class SignOutController : Controller
    {
  
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
               
                return Redirect("~/Home/Index");
            }
            return View(signOut);
        }
    }
}