﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Models.pia;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Http;
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
            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            //throw new Exception("this is a test exception", new Exception("this is the inner exception"));

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
