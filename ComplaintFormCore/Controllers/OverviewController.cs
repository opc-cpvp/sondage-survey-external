using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintFormCore.Controllers
{
    public class OverviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

     //   [Route("privacy-act")]
        public ActionResult Pa()
        {
            // Keep the token until the start of the workflow
           // TempData.Keep("tempToken");

            return View();
        }
    }
}
