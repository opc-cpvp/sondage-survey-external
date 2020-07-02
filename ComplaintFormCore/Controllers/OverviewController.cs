using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoC.WebTemplate.Components.Core.Services;
using GoC.WebTemplate.CoreMVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComplaintFormCore.Controllers
{
  
    public class OverviewController : WebTemplateBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public OverviewController(ModelAccessor modelAccessor, ILogger<HomeController> logger)
         : base(modelAccessor)
        {
            _logger = logger;

            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PA", Href = "/PA" });
        }

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
