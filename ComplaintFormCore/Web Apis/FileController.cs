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

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string complaintId)
        {
            try
            {
                var folderName = Path.Combine("FileUploads", complaintId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var files = new Dictionary<string,SurveyFile>();
                var errors = new List<OPCProblemDetails>();

                foreach (var file in Request.Form.Files)
                {
                    var extension = Path.GetExtension(file.FileName);

                    if (!_allowedFileTypes.Contains(extension))
                    {
                        errors.Add(new OPCProblemDetails
                        {
                            Detail = $"Extension {extension} not allowed",
                            Status = 400,
                            Title = ""
                        });
                        continue;
                    }

                    if (file.Length < 0)
                    {
                        errors.Add(new OPCProblemDetails
                        {
                            Detail = "The file is empty",
                            Status = 400,
                            Title = ""
                        });
                        continue;
                    }

                    var fileData = await GetByteArrayFromImageAsync(file);
                    var size = file.Length;

                    var fullPath = Path.Combine(pathToSave, file.FileName);
                    var dbPath = Path.Combine(folderName, file.FileName);

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    files.Add(file.FileName, new SurveyFile { name = file.FileName, type = file.ContentType, content = Guid.NewGuid().ToString(), size = file.Length });
                }

                if (errors.Count > 0)
                    return BadRequest(errors);

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
        public IActionResult Get([FromQuery] string complaintId, [FromQuery] string filename)
        {
            // throw new Exception("The file is not found or has been removed from the server");
            //OPCProblemDetails problem2 = new OPCProblemDetails
            //{
            //    Detail = "The file is not found or has been removed from the server",
            //    Status = 400,
            //    Title = ""
            //};

            //problem2.Errors.Add("my key", new List<string>() { "The file is not found or has been removed from the server" });

            //return BadRequest(problem2);

            try
            {
                // Files are organized by complaint id in the folder FileUploads
                var folderName = Path.Combine("FileUploads", complaintId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fullPath = Path.Combine(pathToSave, filename);

                var net = new System.Net.WebClient();
                var data = net.DownloadData(fullPath);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";

                return File(content, contentType, filename);
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
            catch (Exception ex)
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

        private async Task<byte[]> GetByteArrayFromImageAsync(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                await file.CopyToAsync(target);
                return target.ToArray();
            }
        }
    }
}
