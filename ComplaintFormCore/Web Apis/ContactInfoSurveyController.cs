using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Models.pa;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactInfoSurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public ContactInfoSurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Complete([FromBody] SurveyContactInfoModel model, [FromQuery] string complaintId)
        {
            OPCProblemDetails problems = _Validate(model);

            if (problems != null)
            {
                return BadRequest(problems);
            }

            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyContactInfoModel model, [FromQuery] string complaintId)
        {
            OPCProblemDetails problems = _Validate(model);

            if (problems != null)
            {
                return BadRequest(problems);
            }

            return Ok();
        }

        private OPCProblemDetails _Validate(SurveyContactInfoModel model)
        {
            var validator = new SurveyContactInfoModelValidator(_localizer);
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
