using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class ResourceAbuseReports
    {
        public decimal? Id { get; set; }
        public decimal? ResourceId { get; set; }
        public int? ReportedBy { get; set; }
        public string ReportReasons { get; set; }
        public string Comments { get; set; }
    }
}
