using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string complaintId)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("FileUploads", complaintId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

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

                    string apiUrl = folderName + fileName;



                    return Ok(new { dbPath, size = file.Length });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
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

        /// <summary>
        /// Fetch a file associated to a complaint
        /// </summary>
        [HttpGet]
        [ActionName("Get")]
        public IActionResult Get([FromQuery] string complaintId, [FromQuery] string filename)
        {
            //  Files are organized by complaint id in the folder FileUploads
            var folderName = Path.Combine("FileUploads", complaintId);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, filename);

            var net = new System.Net.WebClient();
            var data = net.DownloadData(fullPath);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
  
            return File(content, contentType, filename);
        }
    }
}
