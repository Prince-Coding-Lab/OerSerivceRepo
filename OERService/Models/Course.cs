using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class CourseMaster
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public int ReportAbuseCount { get; set; }
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
    }


    public class Course
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public ShortCategory Category { get; set; }
        public ShortSubCategory SubCategory { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public ShortCopyright CopyRight { get; set; }
        public Education Education { get; set; }
        public Profession Profession { get; set; }
        public bool IsDraft { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public int ReportAbuseCount { get; set; }
        public List<CourseURLReferences> References { get; set; }
        public List<CourseAssociatedFiles> AssociatedFiles { get; set; }

    }


    public class CreateCourseRequest
    {       
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; } 
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
        public int CreatedBy { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }


    }

    public class UpdateCourseRequest
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string CourseDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int? EducationId { get; set; }
        public int? ProfessionId { get; set; }
        public int CreatedBy { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }


    }
    public class CourseURLReferences
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string URLReference { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CourseApprovals
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }

    public class CourseAssociatedFiles
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string AssociatedFile { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CourseComments
    {
        public decimal Id { get; set; }
        public decimal CourseId { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

}
