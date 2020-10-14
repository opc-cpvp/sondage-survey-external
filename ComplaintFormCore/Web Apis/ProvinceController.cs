using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComplaintFormCore.Web_Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        // GET: api/Province
        [HttpGet]
        public IEnumerable<Province> Get()
        {
            string language = "en";
            var lang = HttpContext.Request.Query.Where(k => k.Key == "lang").Select(v => v.Value).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(lang) == false)
            {
                language = lang;
            }

            var provinces = new List<Province>();

            var addOtherArg = HttpContext.Request.Query.Where(k => k.Key == "addOther").Select(v => v.Value).FirstOrDefault();
            var addOther = false;

            if (!string.IsNullOrWhiteSpace(addOtherArg))
            {
                bool.TryParse(addOtherArg, out addOther);
            }

            if (language == "fr")
            {
                provinces.AddRange(GetFrenchProvinces().OrderBy(x => x.Text));

                if (addOther)
                {
                    provinces.Add(new Province { Value = "14", Text = "Autre (à l’étranger)" });
                }
            }
            else
            {
                provinces.AddRange(GetProvinces().OrderBy(x => x.Text));

                if (addOther)
                {
                    provinces.Add(new Province { Value = "14", Text = "Other (Outside of Canada)" });
                }
            }

            return provinces;
        }

        private static List<Province> GetFrenchProvinces()
        {
            return new List<Province>
            {
                new Province {Value = "1", Text = "Ontario"},
                new Province {Value = "2", Text = "Québec"},
                new Province {Value = "3", Text = "Nouvelle Écosse"},
                new Province {Value = "4", Text = "Nouveau-Brunswick"},
                new Province {Value = "5", Text = "Manitoba"},
                new Province {Value = "6", Text = "Colombie Britanique"},
                new Province {Value = "7", Text = "Ile du Prince Édouard"},
                new Province {Value = "8", Text = "Saskatchewan"},
                new Province {Value = "9", Text = "Alberta"},
                new Province {Value = "10", Text = "Terre-Neuve-et-Labrador"},
                new Province {Value = "11", Text = "Nunavut"},
                new Province {Value = "12", Text = "Yukon"},
                new Province {Value = "13", Text = "Territoires du Nord-Ouest"}
            };
        }

        private static List<Province> GetProvinces()
        {
            return new List<Province>
            {
                new Province {Value = "1", Text = "Ontario"},
                new Province {Value = "2", Text = "Quebec"},
                new Province {Value = "3", Text = "Nova Scotia"},
                new Province {Value = "4", Text = "New Brunswick"},
                new Province {Value = "5", Text = "Manitoba"},
                new Province {Value = "6", Text = "British Columbia"},
                new Province {Value = "7", Text = "Prince Edward Island"},
                new Province {Value = "8", Text = "Saskatchewan"},
                new Province {Value = "9", Text = "Alberta"},
                new Province {Value = "10", Text = "Newfoundland And Labrador"},
                new Province {Value = "11", Text = "Nunavut"},
                new Province {Value = "12", Text = "Yukon"},
                new Province {Value = "13", Text = "Northwest Territories"}
            };
        }
    }
}
