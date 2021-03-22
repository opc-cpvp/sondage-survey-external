using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using ComplaintFormCore.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Web_Apis
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class PIDSurveyController : ControllerBase
	{
		private readonly SharedLocalizer _localizer;

		public PIDSurveyController(IStringLocalizerFactory factory)
		{
			_localizer = new SharedLocalizer(factory);
		}

		[HttpPost]
		public IActionResult Complete([FromBody] SurveyPIDModel model, [FromQuery] string complaintId, [FromQuery] bool isLongSurvey)
		{
			model.IsLongSurvey = isLongSurvey;

			OPCProblemDetails problems = _Validate(model);

			if (problems != null)
			{
				return BadRequest(problems);
			}

			return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
		}

		[HttpPost]
		public IActionResult Validate([FromBody] SurveyPIDModel model, [FromQuery] string complaintId, [FromQuery] bool isLongSurvey)
		{
			model.IsLongSurvey = isLongSurvey;

			OPCProblemDetails problems = _Validate(model);

			if (problems != null)
			{
				return BadRequest(problems);
			}

			return Ok();
		}

		private OPCProblemDetails _Validate(SurveyPIDModel model)
		{
			#region Test Data
			//ValidationProblemDetails valid = new ValidationProblemDetails();
			//valid.Detail = "There is errors with the validation, see error list";
			//valid.Title = "Validation errors";
			//valid.Errors.Add("mykey", new string[] { "value1", "value2" });
			//valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
			//return BadRequest(valid);

			//throw new Exception("this is a test exception", new Exception("this is the inner exception"));
			#endregion

			var validator = new SurveyPIDModelValidator(_localizer);
			var results = validator.Validate(model);

			if (!results.IsValid)
			{
				OPCProblemDetails valid = new OPCProblemDetails();
				valid.Detail = "There is errors with the validation, see error list";
				valid.Title = "Validation errors";

				foreach (var error in results.Errors)
				{
					valid.AddError(error.PropertyName, error.ErrorMessage);
				}

				return valid;
			}

			return null;
		}
	}
}
