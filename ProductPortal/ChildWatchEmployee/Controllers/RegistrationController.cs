using ChildWatchEmployee.Models;
using System.Web.Mvc;
using ChildWatchApi.Data;
using System.Configuration;

namespace ChildWatchEmployee.Controllers
{
    
    public class RegistrationController : Controller
    {
        MembershipManager membership = new MembershipManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));
        // YMCAWebService ymcaService = new YMCAWebService();
        // GET: Registration
        public ActionResult Register()
        {
            ViewBag.Title = "Employee - Register New Child";

            var member = new Models.Member();
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.Member member)
        {
            if (ModelState.ContainsKey("{ServerError}"))
            {
                ModelState.Remove("{ServerError}");
            }
            if (ModelState.IsValid)
            {
                TempData["member"] = member.MemberID;
                bool register = membership.SaveMember(member.toServer());
                if (register)
                {
                    return Redirect("~/ChildAdd/AddChild");
                }
                /** 
                YMCAServiceResponse ymcaserverResponse = ymcaService.RegisterMember(member.toServer());
                if (ymcaserverResponse.Error)
                {
                    ModelState.AddModelError("ServerError", ymcaserverResponse.Message);
                    return View(member);
                }
                return Redirect("~/Home/Index"); 
                **/
            }
            return View(member);
        }
    }
}