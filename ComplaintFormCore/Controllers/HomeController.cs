using ComplaintFormCore.Models;
using GoC.WebTemplate.Components.Core.Services;
using GoC.WebTemplate.Components.Entities;
using GoC.WebTemplate.CoreMVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace ComplaintFormCore.Controllers
{
	public class HomeController : WebTemplateBaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ModelAccessor modelAccessor, ILogger<HomeController> logger)
            : base(modelAccessor)
        {
            _logger = logger;

            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "Home", Href = "/Home/Index" });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult DetailsPA([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult Test([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult PiaETool([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult Pipeda([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult Pbr([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult Pid([FromQuery(Name = "token")] string token)
        {
            ViewBag.token = token;
            return View();
        }

		public IActionResult ContactInfo([FromQuery(Name = "token")] string token)
		{
			ViewBag.token = token;
			return View();
		}

        public IActionResult Test2()
        {
            return View();
        }

        // This action at /home/survey can bind JSON to a model
        [HttpPost]
        [ActionName("Survey")]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyFromBody([FromBody] SurveyPAModel model, [FromQuery] string surveyId)
        {


            return Json(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
