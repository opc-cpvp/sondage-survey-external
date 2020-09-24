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
    [Route("api/[controller]/[action]/{id?}")]
    [ApiController]
    public class InstitutionController : ControllerBase
    {
        // GET: api/Institution
        [HttpGet]
        public IEnumerable<Institution> GetAll()
        {
            string language = "en";
            var lang = HttpContext.Request.Query.Where(k => k.Key == "lang").Select(v => v.Value).FirstOrDefault();

            bool addOtherToTheList = false;
            var addOthers = HttpContext.Request.Query.Where(k => k.Key == "addOther").Select(v => v.Value).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(lang) == false)
            {
                language = lang;
            }

            if (string.IsNullOrWhiteSpace(addOthers) == false)
            {
                bool.TryParse(addOthers, out addOtherToTheList);
            }

            Institution ins1 = new Institution()
            {
                Value = "1",
                EnglishName = "My Institution",
                FrenchName = "Mon institution"
            };

            ins1.Name = language == "en" ? ins1.EnglishName : ins1.FrenchName;

            Institution ins2 = new Institution()
            {
                Value = "2",
                EnglishName = "Second Instit",
                FrenchName = "Deuxieme institution"
            };

            ins2.Name = language == "en" ? ins2.EnglishName : ins2.FrenchName;

            Institution ins3 = new Institution()
            {
                Value = "3",
                EnglishName = "Third",
                FrenchName = "Troizieme"
            };

            ins3.Name = language == "en" ? ins3.EnglishName : ins3.FrenchName;

            Institution ins4 = new Institution()
            {
                Value = "4",
                EnglishName = "This is a very long institution name, and it goes on and on",
                FrenchName = "Ceci est une institution avec un tres tres long nom, encore"
            };
            ins4.Name = language == "en" ? ins4.EnglishName : ins4.FrenchName;

            List<Institution> list = new List<Institution>();

            list.Add(ins1);
            list.Add(ins2);
            list.Add(ins3);
            list.Add(ins4);

            if (addOtherToTheList)
            {
                Institution otherInstitution = new Institution()
                {
                    Value = Institution.OTHER_INSTITUTION_ID.ToString(),
                    EnglishName = "Other",
                    FrenchName = "Autre",
                    Name = language == "en" ? "Other" : "Autre"
                };

                list.Add(otherInstitution);
            }

            return list;
        }

        [HttpGet]
        public Institution GetInstitution(string id)
        {
            string language = "en";
            if (id == "1")
            {
                return new Institution()
                {
                    Value = "1",
                    EnglishName = "My Institution",
                    FrenchName = "Mon institution",
                    Name = language == "en" ? "My Institution" : "Mon institution"
                };
            }
            else if (id == "2")
            {
                return new Institution()
                {
                    Value = "2",
                    EnglishName = "Second Instit",
                    FrenchName = "Deuxieme institution",
                    Name = language == "en" ? "Second Instit" : "Deuxieme institution"
                };
            }
            else if (id == "3")
            {
                return new Institution()
                {
                    Value = "3",
                    EnglishName = "Third",
                    FrenchName = "Troizieme",
                    Name = language == "en" ? "Third" : "Troizieme"
                };
            }
            else
            {
                return null;
            }
        }
    }
}
