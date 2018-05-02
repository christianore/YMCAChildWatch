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



        public ActionResult Register(string name)
        {
            var member = new Models.Member();
            return View();
        }

        public ActionResult Update(string name)
        {
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
                    TempData.Remove("page");
                    TempData["Success"] = "Member registered";
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

        [HttpPost]
        public ActionResult Update(Models.Member member)
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
                    TempData.Remove("page");
                    TempData["Success"] = "Member updated";
                    return Redirect("~/Home/Index");
                }
                else
                {
                    TempData["Failure"] = "Failed to update member";
                    return View(member);
                }
            }
            return View(member);
        }

        public ActionResult AddChild()
        {
            ViewBag.Title = "Add New Child";
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
                    TempData["Success"] = "Child registered";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    TempData["Failure"] = "Failed to register";
                }
            }
            return View();
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
                    TempData["Success"] = "Child registered";
                    TempData["member"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Failure"] = "Failed to register";
                }
            }
            return View(new ChildLocal());
        }
    }
}
