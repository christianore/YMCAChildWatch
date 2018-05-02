using ChildWatchApi.Data;
using ChildWatchApi.Entity;
using ChildWatchApi.Web;
using ChildWatchEmployee.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChildWatchEmployee.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        OrganizationManager manager = new OrganizationManager(new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString()));

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View(new RegisterEmployee());
        }
        [HttpPost]
        public ActionResult Register(RegisterEmployee model)
        {
            if(ModelState.IsValid)
            {
                if(manager.RegisterEmployee(model.FirstName, model.LastName, model.Administrator, model.Username, model.Password))
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = false,
                        Message = "Employee was successfully added to the ChildWatch database."
                    };
                }
                else
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = true,
                        Message = "Employee was unable to be added to the database."
                    };
                    return View(model);
                }  
                
            }
            
            return View(model);
        }

        public ActionResult Unlock()
        {

            return View(new ChangePassword());
        }
        [HttpPost]
        public ActionResult Unlock(ChangePassword model)
        {
            if(ModelState.IsValid)
            {
                string connectString = ConfigurationManager.ConnectionStrings["database"].ToString();
                SqlConnection connection = new SqlConnection(connectString);
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                bool success = false;

                if(ticket.Name == "root")
                {
                    if (model.AdminPassword == Properties.Settings.Default.root)
                        success = true;
                }
                else
                {
                    var auth = ChildwatchAuthentication.Authenticate(ticket.Name,
                        model.AdminPassword,
                        connection);

                    if (auth.Authentication == AuthContext.Admin)
                    {
                        using (connection = new SqlConnection(connectString))
                        {
                            try
                            {
                                connection.Open();
                                using (SqlCommand command = new SqlCommand("p_employee_update_login", connection))
                                {
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.Parameters.AddRange(new SqlParameter[]
                                    {
                                    new SqlParameter("password", model.Password),
                                    new SqlParameter("id", model.EmployeeID)
                                    });

                                    success = command.ExecuteNonQuery() > 0;
                                }
                            }
                            catch
                            {
                                success = false;
                            }
                            finally
                            {
                                if (connection.State == System.Data.ConnectionState.Open)
                                    connection.Close();
                            }
                        }
                    }
                }

                if(success)
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = false,
                        Message = "Password successfully changed."
                    };
                    return View(new ChangePassword());
                }
                else
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = true,
                        Message = "Unable to change password."
                    };
                    return View(model);
                }
            }

            return View(model);
        }
        public ActionResult Update()
        {
            var model = new UpdateEmployee();
            ViewBag.isAdmin = model.Employee.Administrator;
            ViewBag.isReset = model.Employee.NeedsReset;
            ViewBag.isBlocked = model.Employee.Blocked;
            return View(model);
        }
        [HttpPost]
        public ActionResult Update(UpdateEmployee model)
        {
            if(ModelState.IsValid)
            {
                if (manager.SaveEmployee(model.Employee.ToEmployee()))
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = false,
                        Message = "Employee was successfully saved to the ChildWatch database."
                    };
                   
                }
                else
                {
                    TempData["Message"] = new ResponseMessage()
                    {
                        Error = true,
                        Message = "Employee was unable to be saved to the database."
                    };
           
                }
                
            }
            ViewBag.isAdmin = model.Employee.Administrator;
            ViewBag.isReset = model.Employee.NeedsReset;
            ViewBag.isBlocked = model.Employee.Blocked;

            return View(model);

        }
        [HttpPost]
        public JsonResult GetEmployeeInfo(string id)
        {
            try
            {
                Employee e = manager.GetEmployee(int.Parse(id));
                if(e != null)
                {
                    return Json(e, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {

            }

            return Json(new { error = true, message = "No data for employee found." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}