using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintFormCore.Exceptions
{
    public class OPCProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public List<string> ErrorMessages { get; set; }
    }
}
