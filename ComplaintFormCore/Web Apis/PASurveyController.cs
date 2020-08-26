using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
using ComplaintFormCore.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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
            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            //throw new Exception("this is a test exception", new Exception("this is the inner exception"));

            return Ok();
        }

        [HttpPost]
        public IActionResult ValidateAttachments([FromBody] PAFilePageData files, [FromQuery] string complaintId)
        {
            //throw new Exception("this is a test exception", new Exception("this is the inner exception"));

            //ValidationProblemDetails valid = new ValidationProblemDetails();
            //valid.Detail = "There is errors with the validation, see error list";
            //valid.Title = "Validation errors";
            //valid.Errors.Add("mykey", new string[] { "value1", "value2" });
            //valid.Errors.Add("another mykey", new string[] { "more value1", "stuff" });
            //return BadRequest(valid);

            if (files.Documentation_type == "upload" || files.Documentation_type == "both")
            {
                List<SurveyFile> allFiles = new List<SurveyFile>();

                if(files.Documentation_file_upload != null)
                {
                    allFiles.AddRange(files.Documentation_file_upload);
                }               

                if (files.Documentation_file_upload_rep != null)
                {
                    allFiles.AddRange(files.Documentation_file_upload_rep);
                }

                if (allFiles.Count > 0)
                {
                    long totalSizes = 0;
                    long multipleFileMaxSize = 26214400;

                    var folderName = Path.Combine("FileUploads", complaintId);
                    var folderpath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(folderpath))
                    {
                        Directory.CreateDirectory(folderpath);
                    }

                    DirectoryInfo directory = new DirectoryInfo(folderpath);

                    FileInfo[] filesStored = directory.GetFiles();

                    //  We need to make sure the list of files sent to us saved in local storage (in other words the files the users think he is 
                    //  uploading) are all saved on the server properly with the right size
                    OPCProblemDetails fileMissingProblem = new OPCProblemDetails
                    {
                        Status = 400,
                        Detail = _localizer.GetLocalizedStringGeneral("FileNotFound"),
                        Title = _localizer.GetLocalizedStringGeneral("ValidationIssues")
                    };

                    foreach (SurveyFile file in allFiles)
                    {
                        long.TryParse(file.content, out long fileSize);

                        if (fileSize == 0 || filesStored.Where(f => f.Name == file.name && f.Length == fileSize).Any() == false)
                        {
                            fileMissingProblem.AddError(_localizer.GetLocalizedStringGeneral("FileNotFound"), string.Format(_localizer.GetLocalizedStringGeneral("FileMissing"), file.name));
                        }
                        else
                        {
                            //  We are adding to the total file size that we will process later
                            totalSizes += fileSize;
                        }
                    }

                    if (fileMissingProblem.Errors.Count > 0)
                    {
                        return BadRequest(fileMissingProblem);
                    }

                    //  Next step is to validate for total file size.
                    if (totalSizes > multipleFileMaxSize)
                    {
                        OPCProblemDetails problem = new OPCProblemDetails
                        {
                            Detail = _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded"),
                            Status = 400,
                            Title = _localizer.GetLocalizedStringGeneral("ValidationIssues")
                        };

                        problem.Errors.Add(_localizer.GetLocalizedStringSubmission("Attachments"), new List<string>() { _localizer.GetLocalizedStringSubmission("SizeOfFilesExceeded") });

                        return BadRequest(problem);
                    }
                }
                else
                {
                    OPCProblemDetails problem = new OPCProblemDetails
                    {
                        Detail = "There is no files uploaded",
                        Status = 400,
                        Title = _localizer.GetLocalizedStringGeneral("ValidationIssues")
                    };

                    return BadRequest(problem);
                }
            }

            return Ok();
        }      
    }
}
