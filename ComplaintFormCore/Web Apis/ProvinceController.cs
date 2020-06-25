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
            return new List<Province>
            {
                new Province {Value = "1", Text = "Ontario"},
                new Province {Value = "2", Text = "Quebec"},
                new Province {Value = "3", Text = "Nova_Scotia"},
                new Province {Value = "4", Text = "New_Brunswick"},
                new Province {Value = "5", Text = "Manitoba"},
                new Province {Value = "6", Text = "British_Columbia"},
                new Province {Value = "7", Text = "Prince_Edward_Island"},
                new Province {Value = "8", Text = "Saskatchewan"},
                new Province {Value = "9", Text = "Alberta"},
                new Province {Value = "10", Text = "Newfoundland_And_Labrador"},
                new Province {Value = "11", Text = "Nunavut"},
                new Province {Value = "12", Text = "Yukon"},
                new Province {Value = "13", Text = "Northwest_Territories"}
            };
        }
    }
}
