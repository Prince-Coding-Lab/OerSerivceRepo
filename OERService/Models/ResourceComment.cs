using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class ResourceComment
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

    public class ResourceCommentRequest
    {       
        public decimal ResourceId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }       
    }

    public class ResourceCommentUpdateRequest
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
    }

    public class ResourceReportAbuseWithComment
    {
        public decimal ResourceId { get; set; }
        public string ReportReasons { get; set; }
        public string Comments { get; set; }
        public int ReportedBy { get; set; }
    }

    public class ResourceRatingRequest
    {
        public decimal ResourceId { get; set; }
        public float Rating { get; set; }
        public string Comments { get; set; }
        public int RatedBy { get; set; }
    }
}
