using ChildWatchEmployee.Models;
using System.Web.Mvc;
using ChildWatchApi.Data;
using System.Configuration;

namespace ChildWatchEmployee.Controllers
{
    
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
                    return RedirectToAction("AddChild");
                }
                else
                {
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
                    return View();
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
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(child);
        }
    }
}
