using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildWatchEmployee.Controllers
{
    public class HelpController : Controller
    {
        // GET: Help
        public ActionResult HelpFile()
        {
            return View();
        }
    }
}