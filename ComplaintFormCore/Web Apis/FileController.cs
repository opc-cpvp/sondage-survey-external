using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ComplaintFormCore.Exceptions;
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
        private List<string> _allowedFileTypes = new List<string>() { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".wpd", ".csv", ".pdf", ".jpg", ".jpeg", ".gif", ".txt", ".rtf", ".tif", ".tiff" };

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string complaintId, string opcSurveyType, string subFolder)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("FileUploads", opcSurveyType, subFolder, complaintId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                var extension = Path.GetExtension(file.FileName);

                if(_allowedFileTypes.Contains(extension) == false)
                {
                    OPCProblemDetails problem = new OPCProblemDetails
                    {
                        Detail = "Extension " + extension + " not allowed",
                        Status = 400,
                        Title = ""
                    };

                    return BadRequest(problem);
                }

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    byte[] fileData = await GetByteArrayFromImageAsync(file);
                    long size = file.Length;

                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath, size = file.Length });
                }
                else
                {
                    OPCProblemDetails problem = new OPCProblemDetails
                    {
                        Detail = "The file is empty",
                        Status = 400,
                        Title = ""
                    };

                    return BadRequest(problem);
                }
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
