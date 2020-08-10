using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using PhoneNumbers;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PASurveyController : ControllerBase
    {
        private readonly SharedLocalizer _localizer;

        public PASurveyController(IStringLocalizerFactory factory)
        {
            _localizer = new SharedLocalizer(factory);
        }

        [HttpPost]
        public IActionResult Complete([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            return Ok(new { ReferenceNumber = Guid.NewGuid().ToString() });
        }

        [HttpPost]
        public IActionResult Validate([FromBody] SurveyPAModel model, [FromQuery] string complaintId)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult ValidateAttachments([FromBody] PAFilePageData files, [FromQuery] string complaintId)
        {

            //throw new Exception("glkdfjgldfghldhg", new Exception("this is the inner exception"));
            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            List<SurveyFile> allFiles = files.Documentation_file_upload;

            if(files.Documentation_file_upload_rep != null)
            {
                allFiles.AddRange(files.Documentation_file_upload_rep);
            }

            OPCProblemDetails problem = ValidateDocumentations(allFiles, files.Documentation_type, complaintId);

            if (problem.Status != 200 || problem.Errors.Count > 0)
            {
                problem.Title = "Validation issues";
                return BadRequest(problem);
            }

            return Ok();
        }

        private OPCProblemDetails ValidateDocumentations(List<SurveyFile> documentationFileUpload, string documentType, string complaintId)
        {
            OPCProblemDetails problems = new OPCProblemDetails();

            if (documentType == "upload" || documentType == "both")
            {
                OPCProblemDetails attachmentSizeProblems = ValidateAttachmentsSize(documentationFileUpload, complaintId);

                if (attachmentSizeProblems != null)
                {
                    problems.Errors.Union(attachmentSizeProblems.Errors);
                }
            }

            return problems;
        }

        private OPCProblemDetails ValidateAttachmentsSize(List<SurveyFile> documentationFileUpload, string complaintId)
        {
            long totalSizes = 0;
            long multipleFileMaxSize = 26214400;

            var folderName = Path.Combine("FileUploads", complaintId);
            var folderpath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo directory = new DirectoryInfo(folderpath);

            foreach (var file in directory.GetFiles())
            {
                if (documentationFileUpload.Where(f => f.name == file.Name).Any())
                {
                    totalSizes += file.Length;
                }
            }

            if (totalSizes > multipleFileMaxSize)
            {
                OPCProblemDetails problem = new OPCProblemDetails
                {
                    Detail = _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded"),
                    Status = 400,
                    Title = ""
                };

                problem.Errors.Add(_localizer.GetLocalizedStringSubmission("Attachments"), new List<string>() { _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded") });

                return problem;
            }

            return null;
        }

      
    }
}
