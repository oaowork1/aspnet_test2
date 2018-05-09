using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using test2.Models;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;

namespace test2.Controllers
{
    public class Default1Controller : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies["UserSettings"] == null)
            {
                return RedirectToAction("Autorisation");
            }
            User user = new User();
            user.firstName = Request.Cookies["UserSettings"]["fName"];
            user.lastName = Request.Cookies["UserSettings"]["sName"];
            ViewData["firstName"] = Request.Cookies["UserSettings"]["fName"];
            ViewData["lastName"] = Request.Cookies["UserSettings"]["sName"];
            return View();
        }

        [HttpPost]
        public ActionResult Index(string s)
        {
            if (Request.Cookies["UserSettings"]["email"] != null)
            {
                var cookie = new HttpCookie("UserSettings")
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Autorisation");
        }

        //[Route("Test")]
        [HttpGet]
        public ActionResult Autorisation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autorisation(User smth)
        {
            var settings = WebConfigurationManager.GetWebApplicationSection("mySection");
            try
            {
                Configuration rootWebConfig1 = WebConfigurationManager.OpenWebConfiguration("~/")
                    as System.Configuration.Configuration;
                if (rootWebConfig1.AppSettings.Settings.Count > 0)
                {
                    foreach (KeyValueConfigurationElement setting in rootWebConfig1.AppSettings.Settings)
                    {
                        if (setting.Key.Contains("user"))
                        {
                            var parts = setting.Value.Split(';');
                            try
                            {
                                if (smth.email == parts[0] && smth.password == parts[1])
                                {
                                    HttpCookie myCookie = new HttpCookie("UserSettings");
                                    myCookie["email"] = smth.email;
                                    myCookie["fName"] = parts[2];
                                    myCookie["sName"] = parts[3];
                                    myCookie["role"] = parts[4];
                                    myCookie.Expires = DateTime.Now.AddDays(1d);
                                    Response.Cookies.Add(myCookie);

                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                                }
                            }
                            catch
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return View(smth);
        }
	}
}