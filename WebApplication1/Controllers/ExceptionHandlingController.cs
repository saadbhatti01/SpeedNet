using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class ExceptionHandlingController : Controller
    {
        [HandleError]
        // GET: ExceptionHandling
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
           
            return View();
        }
    }
}