using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Models
{
    public class PAFilePageData
    {
        public string Documentation_type { get; set; }

        public List<SurveyFile> Documentation_file_upload { get; set; }

        public List<SurveyFile> Documentation_file_upload_rep { get; set; }
    }
}
