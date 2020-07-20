using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ComplaintFormCore.Models;
using GoC.WebTemplate.CoreMVC.Controllers;
using GoC.WebTemplate.Components.Core.Services;

namespace ComplaintFormCore.Controllers
{
    public class HomeController : WebTemplateBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ModelAccessor modelAccessor, ILogger<HomeController> logger)
            : base(modelAccessor)
        {
            _logger = logger;

            WebTemplateModel.HeaderTitle = "Complaint Form";

           

            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "Test", Href="/Home/Test"});
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PA", Href = "/Overview/PA" });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DetailsPA()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        // This action at /home/survey can bind JSON to a model
        [HttpPost]
        [ActionName("Survey")]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyFromBody([FromBody] PostSurveyResultModel model)
        {
            return Json(new { Id = "123" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
