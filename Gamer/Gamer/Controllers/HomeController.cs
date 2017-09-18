using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamer.Controllers
{
    public class HomeController : Controller
    {
        //principal page
        public ActionResult Index()
        {
            return View();
        }      
    }
}
