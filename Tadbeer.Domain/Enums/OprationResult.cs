using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracking.Domain.Enums
{
    public enum OperationResult
    {
        Success = 0,
        NotFound = 1,
        ValidationError = 2,
        Unauthorized = 3,
        Forbidden = 4,
        Conflict = 5,
        Error = 6
    }
}
