using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PIASurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public PIASurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPIAToolModel model, [FromQuery] string complaintId)
        {
            var validator = new SurveyPIAToolModelValidator(_localizer);
            var results = validator.Validate(model);

            if (!results.IsValid)
            {
                OPCProblemDetails valid = new OPCProblemDetails();
                valid.Detail = "There is errors with the validation, see error list";
                valid.Title = "Validation errors";

                foreach(var error in results.Errors)
                {
                    //valid.Errors.Add(error., new string[] { "more value1", "stuff" });
                    valid.AddError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(valid);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult SendEmail([FromBody] SurveyPIAToolModel model, [FromQuery] string complaintId)
        {


            return Ok();
        }
    }
}
