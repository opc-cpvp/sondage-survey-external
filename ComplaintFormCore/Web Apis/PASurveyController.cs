using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PASurveyController : ControllerBase
    {
        private readonly SharedApiLocalizer _localizer;

        public PASurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedApiLocalizer(factory);
        }


        [HttpPost]
        public IActionResult Complete([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            // Thread.Sleep(4000);

            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            //var problem = new OPCProblemDetails
            //{
            //    Detail = "You object is Invalid. This is abug!!!",
            //    Status = 400,
            //    Title = ""
            //};

            //return BadRequest(problem);

            // throw new ProblemDetailsException(validation);



            //return Json(new { ReferenceNumber = Guid.NewGuid().ToString() });
            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult ValidateAttachments([FromBody] List<SurveyFile> documentation_file_upload, [FromQuery] string complaintId)
        {
            // throw new ProblemDetailsException(validation);
            long totalSizes = 0;
            long multipleFileMaxSize = 26214400;

            var folderName = Path.Combine("FileUploads", complaintId);
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo directory = new DirectoryInfo(folderpath);

            foreach (var file in directory.GetFiles())
            {                
                if (documentation_file_upload.Where(f => f.name == file.Name).Any())
                {
                    totalSizes += file.Length;
                }
            }

            if (totalSizes > multipleFileMaxSize)
            {
                var problem = new OPCProblemDetails
                {
                    Detail = _localizer.GetLocalizedString("SizeOfFilesExceeded"),
                    Status = 400,
                    Title = ""
                };

                return BadRequest(problem);
            }

            return Ok();
        }

        private OPCProblemDetails Validate(SurveyPAModel model)
        {
            OPCProblemDetails problems = new OPCProblemDetails()
            {
                ErrorMessages = new List<string>()
            };

            if (string.IsNullOrWhiteSpace(model.IsEmployee))
            {
                var problem = new OPCProblemDetails
                {
                    Detail = "You object is Invalid. This is abug!!!",
                    Status = 400,
                    Title = ""
                };

                throw new ProblemDetailsException(problem);
                problems.ErrorMessages.Add("IsEmployee:" + _localizer.GetLocalizedString("AtLeastOneItemMustBeSelected"));                
            }

            OPCProblemDetails natureOfComplaintValidation = ValidateNatureOfComplaint(model.NatureOfComplaint);


            if(problems.ErrorMessages.Count > 0)
            {
                problems.Status = 400;
            }
            else
            {
                problems.Status = 400;
            }

            return problems;
        }

        private OPCProblemDetails ValidateNatureOfComplaint(List<string> natureOfComplaint)
        {
            OPCProblemDetails problems = new OPCProblemDetails()
            {
                ErrorMessages = new List<string>()
            };

            var isAnyFirstFourSelected = natureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintDelay.ToString() || x == NatureOfComplaintType.NatureOfComplaintDenialOfAccess.ToString() || x == NatureOfComplaintType.NatureOfComplaintExtensionOfTime.ToString() || x == NatureOfComplaintType.NatureOfComplaintLanguage.ToString()).Any();

            var isAnyLastFiveSelected = natureOfComplaint.Where(x => x == NatureOfComplaintType.NatureOfComplaintOther.ToString() || x == NatureOfComplaintType.NatureOfComplaintRetentionAndDisposal.ToString() || x == NatureOfComplaintType.NatureOfComplaintUseAndDisclosure.ToString() || x == NatureOfComplaintType.NatureOfComplaintCollection.ToString() || x == NatureOfComplaintType.NatureOfComplaintCorrection.ToString()).Any();


            return problems;
        }
    }
}
