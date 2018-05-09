using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using test2.Models;
using System.Web.Configuration;
using System.Configuration;
using System.Web.Helpers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

namespace test2.Controllers
{
    public class SpecialController : Controller
    {
        [Route("api.current.user.getinfo")]
        [HttpGet]
        public ActionResult CurrentUser(string url)
        {
            if (Request.Cookies["UserSettings"] != null)
            {
                Role userRole = Role.User;
                try
                {
                    userRole = (Role)Convert.ToInt32(Request.Cookies["UserSettings"]["role"]);
                }
                catch (Exception e) { }
                try
                {
                    return Json(new
                    {
                        email = Request.Cookies["UserSettings"]["email"],
                        firstName = Request.Cookies["UserSettings"]["fName"],
                        lastName = Request.Cookies["UserSettings"]["sName"],
                        role = userRole.ToString()
                    }, JsonRequestBehavior.AllowGet);
                }
                catch { }
            }
            ViewData["Message"] = "Error:Please sigh in";
            return View();
        }

        [Route("api.{fName}.{sName}.getinfo")]
        [HttpGet]
        public ActionResult CurrentNamedUser(string fName, string sName)
        {
            if (Request.Cookies["UserSettings"] != null)
            {
                if (fName != Request.Cookies["UserSettings"]["fName"] ||
                    sName != Request.Cookies["UserSettings"]["sName"])
                {
                    ViewData["Message"] = "Error: Now such user";
                    return View();
                }

                Role userRole = Role.User;
                try
                {
                    userRole = (Role)Convert.ToInt32(Request.Cookies["UserSettings"]["role"]);
                }
                catch (Exception e) { }
                try
                {
                    return Json(new
                    {
                        email = Request.Cookies["UserSettings"]["email"],
                        firstName = Request.Cookies["UserSettings"]["fName"],
                        lastName = Request.Cookies["UserSettings"]["sName"],
                        role = userRole.ToString()
                    }, JsonRequestBehavior.AllowGet);
                }
                catch { }
            }
            return null;
        }

        [Route("api.user.getall")]
        [HttpGet]
        public ActionResult AllUser(string url)
        {
            if (Request.Cookies["UserSettings"] != null)
            {
                Role userRole = Role.User;
                try
                {
                    userRole = (Role)Convert.ToInt32(Request.Cookies["UserSettings"]["role"]);
                }
                catch (Exception e) 
                { 
                }
                if (userRole!=Role.Admin)
                {
                    ViewData["Message"] = "Error:Sorry, admin option only";
                    return View();
                }

                try
                {
                    Configuration rootWebConfig1 = WebConfigurationManager.OpenWebConfiguration("~/")
                        as System.Configuration.Configuration;
                    List<User> users = new List<User>();

                    if (rootWebConfig1.AppSettings.Settings.Count > 0)
                    {
                        foreach (KeyValueConfigurationElement setting in rootWebConfig1.AppSettings.Settings)
                        {
                            if (setting.Key.Contains("user"))
                            {
                                User temp = new User();
                                var parts = setting.Value.Split(';');
                                try
                                {
                                    temp.email = parts[0];
                                    temp.firstName = parts[2];
                                    temp.lastName = parts[3];
                                }
                                catch
                                {
                                    throw;
                                }
                                try
                                {
                                    temp.role = (Role)Convert.ToInt32(parts[4]);
                                }
                                catch
                                {
                                    throw;
                                }
                                users.Add(temp);
                            }
                        }
                        var jsonSerialiser = new JavaScriptSerializer();
                        var json = jsonSerialiser.Serialize(users);
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                }
            }
            ViewData["Message"] = "Error:Please sign in like admin";
            return View();
        }
	}
}