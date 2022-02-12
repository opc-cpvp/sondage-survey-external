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

            //  The token should be coming from the Complaint table
            string token = "0f3ee945-def4-4288-8a03-9459bb4890da";

            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "Test", Href= "/Home/Test?token=" + token });
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PIA", Href = "/Home/PiaETool?token=" + token });
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PA", Href = "/Home/DetailsPA?token=" + token });
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "Pipeda", Href = "/Home/Pipeda?token=" + token });
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PBR", Href = "/Home/Pbr?token=" + token });
            WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PID", Href = "/Home/Pid?token=" + token });
			WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "PID Short", Href = "/Home/PidShort?token=" + token + "&isShortSurvey=1" });
			WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "Tell OPC", Href = "/Home/TellOPC?token=" + token });
			WebTemplateModel.Breadcrumbs.Add(new GoC.WebTemplate.Components.Entities.Breadcrumb() { Title = "Contact Info", Href = "/Home/ContactInfo?token=" + token });
		}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

		public IActionResult BreachPA([FromQuery(Name = "token")] string token)
		{
			ViewBag.token = token;
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

		public IActionResult TellOPC([FromQuery(Name = "token")] string token)
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

		public IActionResult PidShort([FromQuery(Name = "token")] string token, [FromQuery(Name = "isShortSurvey")] string isShortSurvey)
		{
			ViewBag.token = token;
			ViewBag.isShortSurvey = isShortSurvey;
			return View("Pid");
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
