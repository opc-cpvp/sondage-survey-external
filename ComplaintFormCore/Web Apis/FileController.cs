using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
using ComplaintFormCore.Models;
//using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly List<string> _allowedFileTypes = new List<string>() { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".wpd", ".csv", ".pdf", ".jpg", ".jpeg", ".gif", ".txt", ".rtf", ".tif", ".tiff" };

        private readonly string FileUploadeFolderName = "FileUploads";

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string complaintI, string questionName)
        {
            //  NOTE: The complaintId should probably be used to validate that this is a proper request

            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), FileUploadeFolderName);

                var files = new Dictionary<string,SurveyFile>();
                OPCProblemDetails problem = new OPCProblemDetails
                {
                    Status = 400
                };

                foreach (var file in Request.Form.Files)
                {
                    var extension = Path.GetExtension(file.FileName);

                    if (!_allowedFileTypes.Contains(extension))
                    {
                        problem.Errors.Add(questionName, new List<string> { $"Extension {extension} not allowed" });
                        continue;
                    }

                    if (file.Length < 0)
                    {
                        problem.Errors.Add(questionName, new List<string> { "The file is empty" });
                        continue;
                    }

                    var uniqueId = Guid.NewGuid().ToString();
                    var folder = Path.Combine(pathToSave, uniqueId);
                    var fullPath = Path.Combine(folder, file.FileName);

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    files.Add(file.FileName, new SurveyFile { name = file.FileName, type = file.ContentType, content = uniqueId, size = file.Length });
                }

                if (problem.Errors.Count > 0)
                    return BadRequest(problem);

                return Ok(files);
            }
            catch (Exception ex)
            {
                //  TODO: Log this somewhere
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Fetch a file associated to a complaint
        /// </summary>
        [HttpGet]
        [ActionName("Get")]
        public IActionResult Get([FromQuery] string complaintId, [FromQuery] string fileUniqueId, [FromQuery] string fileName)
        {
            //  NOTE: The complaintId should probably be used to validate that this is a proper request

            try
            {
                // Files are organized by complaint id in the folder FileUploads
                var folderName = Path.Combine("FileUploads", fileUniqueId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fullPath = Path.Combine(pathToSave, fileName);

                var net = new System.Net.WebClient();
                var data = net.DownloadData(fullPath);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";

                return File(content, contentType, fileName);
            }
            catch (WebException)
            {
                //  TODO: Log this somewhere
                OPCProblemDetails problem = new OPCProblemDetails
                {
                    Detail = "The file is not found or has been removed from the server",
                    Status = 400,
                    Title = ""
                };

                return BadRequest(problem);
            }
            catch (Exception)
            {
                //  TODO: Log this somewhere
                OPCProblemDetails problem = new OPCProblemDetails
                {
                    Detail = "The file is empty",
                    Status = 400,
                    Title = ""
                };

                return BadRequest(problem);
            }
        }
    }
}
