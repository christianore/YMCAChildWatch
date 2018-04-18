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
        public ActionResult RegisterChild(Models.ChildLocal child, string AddChild)
        {
            if (ModelState.ContainsKey("{ServerError}"))
            {
                ModelState.Remove("{ServerError}");
            }
            if (ModelState.IsValid)
            {
                Child newChild = new Child();
                newChild.FirstName = child.FirstName;
                newChild.LastName = child.LastName;
                newChild.BirthDate = child.Birthday;
                string guardianID = child.Guardian.ToString();
                if (membership.InsertChild(newChild, guardianID) > 0)
                {
                    if (AddChild.Equals("Add Another Child"))
                    {
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(child);
        }
    }
}
