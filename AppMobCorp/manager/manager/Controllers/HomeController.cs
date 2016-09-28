using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager.Models;

namespace Manager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            // Helpers.SendMails.SendMail("salinoit@gmail.com", "jshgdajshgf");

            ViewBag.Title = "Painel Bradesco";

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
