using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComplaintFormCore.Web_Apis.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        // GET: api/Institution
        [HttpGet]
        public IEnumerable<Institution> Get()
        {
            string language = "en";
            var lang = HttpContext.Request.Query.Where(k => k.Key == "lang").Select(v => v.Value).FirstOrDefault();

            if(string.IsNullOrWhiteSpace(lang) == false)
            {
                language = lang;
            }

            Institution ins1 = new Institution()
            {
                Value = "1",
                EnglishName = "My Institution",
                FrenchName = "Mon institution",
                Name = language == "en" ? "My Institution" : "Mon institution"
            };

            Institution ins2 = new Institution()
            {
                Value = "2",
                EnglishName = "Second Instit",
                FrenchName = "Deuxieme institution",
                Name = language == "en" ? "Second Instit" : "Deuxieme institution"
            };

            Institution ins3 = new Institution()
            {
                Value = "3",
                EnglishName = "Third",
                FrenchName = "Troizieme",
                Name = language == "en" ? "Third" : "Troizieme"
            };

            List<Institution> list = new List<Institution>();

            list.Add(ins1);
            list.Add(ins2);
            list.Add(ins3);



            return list;
        }
    }
}
