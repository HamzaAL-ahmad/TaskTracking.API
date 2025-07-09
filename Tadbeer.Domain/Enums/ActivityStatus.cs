using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracking.Domain.Enums
{
    public enum ActivityStatus
    {
        Pending = 1,
        InProgress,
        Completed,
        Cancelled
    }
}
