using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models.pipeda;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PipedaSurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public PipedaSurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPipedaModel model, [FromQuery] string complaintId)
        {
            var validator = new SurveyPipedaModelValidator(_localizer);
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
        public IActionResult Complete([FromBody] SurveyPipedaModel model, [FromQuery] string complaintId)
        {
            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }
    }
}
