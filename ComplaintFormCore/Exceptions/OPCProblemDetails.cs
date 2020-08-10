using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Exceptions
{
    public class OPCProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public OPCProblemDetails()
        {
            Status = 200;
            Errors = new Dictionary<string, List<string>>();
        }

        public Dictionary<string, List<string>> Errors { get; set; }

        public void AddError(string key, string value)
        {
            if (Errors.ContainsKey(key))
            {
                Errors[key].Add(value);
            }
            else
            {
                Errors.Add(key, new List<string>() { value });
            }            
        }
    }
}
