using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class CourseComment
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

    public class CourseCommentRequest
    {       
        public decimal CourseId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }       
    }

    public class CourseCommentUpdateRequest
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
    }

    public class CourseReportAbuseWithComment
    {
        public decimal CourseId { get; set; }
        public string ReportReasons { get; set; }
        public string Comments { get; set; }
        public int ReportedBy { get; set; }
    }

    public class CourseRatingRequest
    {
        public decimal CourseId { get; set; }
        public float Rating { get; set; }
        public string Comments { get; set; }
        public int RatedBy { get; set; }
    }
}
