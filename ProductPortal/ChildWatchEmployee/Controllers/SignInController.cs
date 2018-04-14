using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChildWatchEmployee.Models;
using ChildWatchApi.Data;
using System.Configuration;
using System.Web.Services;
using ChildWatchApi.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace ChildWatchEmployee.Controllers
{
    [WebService]
    public class SignInController : Controller
    {

        SignInManager signin = new SignInManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));
        // GET: SignOut
        public ActionResult SignOut()
        {
            ViewBag.Title = "Employee - Sign Out Child";

            return View(new SignOut());

        }

        [HttpPost]
        public ActionResult SignOut(SignOut signOut)
        {
            if (ModelState.IsValid)
            {
                if (signin.SignOut(int.Parse(signOut.BandNum)))
                {
                    signOut.State = SignOutState.SignedOut;
                }
                else
                    signOut.State = SignOutState.Failed;                
            }

            return View(signOut);
        }

        public ActionResult SignIn()
        {
            return View();
        }


        public JsonResult  ValidateMember(ValidationToken data)
        {           
            string connection = ConfigurationManager.ConnectionStrings["database"].ToString();
            SqlConnection sql = new SqlConnection(connection);
            SignInManager manager = new SignInManager(sql);

            Family f = manager.Validate(data.Barcode, data.Pin);
            OrganizationManager organization = new OrganizationManager(sql);
            Object o = new ValidationResponse(f != null, f, organization.GetLocations(1));

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SigninMembers(SigninToken data)
        {
            //var token = JsonConvert.DeserializeObject<SigninToken>(data);

            SignInManager manager =
                new SignInManager(new SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

            Signin value = manager.SignIn(data.MemberId, data.Assignments);

            return Json(value, JsonRequestBehavior.AllowGet);
        }


    }
}