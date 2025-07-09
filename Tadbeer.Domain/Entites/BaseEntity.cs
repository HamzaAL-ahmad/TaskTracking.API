using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Enums;

namespace TaskTracking.Domain.Entites
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset Completion { get; set; }
        public int status { get; set; }
    }
}
