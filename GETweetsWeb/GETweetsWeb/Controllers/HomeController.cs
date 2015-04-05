using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GETweetsWeb.Controllers
{
    public class HomeController : Controller
    {
        private Repository repository = new Repository();

        public ActionResult Index()
        {
            return View();
        }

    }
}