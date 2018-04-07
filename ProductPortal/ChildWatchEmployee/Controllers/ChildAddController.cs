using System.Web.Mvc;
using ChildWatchApi;
using System.Configuration;


namespace ChildWatchEmployee.Controllers
{
    public class ChildAddController : Controller
    {
        ChildWatchApi.Data.MembershipManager membership = new ChildWatchApi.Data.MembershipManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));
        // GET: ChildAdd
        public ActionResult AddChild()
        {
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

                /** bool success = membership.AttachChild();
                if (success)
                {
                    return Redirect("~/Home/Index");
                }
                **/
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