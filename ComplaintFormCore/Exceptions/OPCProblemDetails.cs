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
            ErrorMessages = new List<string>();
        }

        public List<string> ErrorMessages { get; set; }
    }
}
