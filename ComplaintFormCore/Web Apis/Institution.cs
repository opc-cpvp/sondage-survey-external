using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Web_Apis.Models
{
    [Serializable]
    public class Institution
    {
        public string Value { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }
        public string FrenchName { get; set; }

    }

    
}
