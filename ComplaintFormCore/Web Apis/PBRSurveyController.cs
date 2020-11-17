using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Models.pipeda;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PBRSurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public PBRSurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPBRModel model, [FromQuery] string complaintId)
        {
            var validator = new SurveyPBRModelValidator(_localizer);
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
        public IActionResult Complete([FromBody] SurveyPBRModel model, [FromQuery] string complaintId)
        {
            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            return Ok();
        }


    }
}
