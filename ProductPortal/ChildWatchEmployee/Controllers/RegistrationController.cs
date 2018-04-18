using ChildWatchEmployee.Models;
using System.Web.Mvc;
using ChildWatchApi.Data;
using System.Configuration;

namespace ChildWatchEmployee.Controllers
{
    [Authorize]
    public class RegistrationController : Controller
    {
        MembershipManager membership = new MembershipManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

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
                    TempData["Message"] = "Member registered";
                    return RedirectToAction("AddChild");
                }
                else
                {
                    TempData["Failure"] = "Failed to add member";
                    return View(member);
                }
            }
            return View(member);
        }

        public ActionResult AddChild()
        {
            return View();
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "AddChild", MatchFormValue = "Add Another Child")]
        public ActionResult RegisterChild(Models.ChildLocal child)
        {
            if (ModelState.ContainsKey("{ServerError}"))
            {
                ModelState.Remove("{ServerError}");
            }
            if (ModelState.IsValid)
            {
                string guardianID = child.Guardian.ToString();
                if (membership.InsertChild(child.ToServer(), guardianID) > 0)
                {
                    TempData["member"] = guardianID;
                    TempData["Message"] = "Child registered";
                    return View();
                }
                else
                {
                    TempData["Failure"] = "Failed to register";
                }
            }
            return View(child);
        }
        [HttpPost]
        [MultiButton(MatchFormKey = "AddChild", MatchFormValue = "Finish Registration")]
        public ActionResult FinishRegistration(Models.ChildLocal child)
        {
            if (ModelState.ContainsKey("{ServerError}"))
            {
                ModelState.Remove("{ServerError}");
            }
            if (ModelState.IsValid)
            {
                string guardianID = child.Guardian.ToString();
                if (membership.InsertChild(child.ToServer(), guardianID) > 0)
                {
                    TempData["Message"] = "Child registered";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Failure"] = "Failed to register";
                }
            }
            return View(child);
        }
    }
}
