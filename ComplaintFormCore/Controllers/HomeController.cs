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
		public IActionResult BreachPA([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "Breach PA", Href = $"/Home/BreachPA?token={token}" });

			ViewBag.token = token;
			return View();
		}
		public IActionResult ContactInfo([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "Contact Info", Href = $"/Home/ContactInfo?token={token}" });

			ViewBag.token = token;
			return View();
		}
		public IActionResult DetailsPA([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "Details PA", Href = $"/Home/DetailsPA?token={token}" });

			ViewBag.token = token;
			return View();
		}
		public IActionResult Pbr([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "PBR", Href = $"/Home/Pbr?token={token}" });

			ViewBag.token = token;
			return View();
		}
		public IActionResult PiaETool([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "PIA", Href = $"/Home/PiaETool?token={token}" });

			ViewBag.token = token;
			return View();
		}
		public IActionResult Pipeda([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "PIPEDA", Href = $"/Home/Pipeda?token={token}" });

			ViewBag.token = token;
			return View();
		}

		public IActionResult Privacy()
        {
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb() { Title = "Privacy", Href = $"/Home/Privacy" });

			return View();
        }

		public IActionResult TellOPC([FromQuery(Name = "token")] string token)
		{
			WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Title = "Tell OPC", Href = $"/Home/TellOPC?token={token}" });

			ViewBag.token = token;
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
