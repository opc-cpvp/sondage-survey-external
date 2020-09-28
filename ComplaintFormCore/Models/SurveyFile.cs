using System;

namespace ComplaintFormCore.Models
{
    [Serializable]
    public class SurveyFile
    {
        public string name { get; set; }

        public string type { get; set; }

        public string content { get; set; }

        public long size { get; set; }
    }
}
