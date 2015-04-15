using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GETweetsWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult TestMap()
        {
            return View("TestMap");
        }
        public ActionResult TestMap1()
        {
            return View("TestMap1");
        }
    }
}