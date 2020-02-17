using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{    
    public class ResourceMaster
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Thumbnail { get; set; }
        public string ProfileDescription { get; set; }
        public string Keywords { get; set; }
        public string CourseContent { get; set; }
        public int MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }
        public bool IsDraft { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public double AlignmentRating { get; set; }
        public int ReportAbuseCount { get; set; }
    }

    public class Resource
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public ShortCategory Category { get; set; }
        public ShortSubCategory SubCategory { get; set; }
        public string Thumbnail { get; set; }
        public string ResourceDescription { get; set; }
        public string Keywords { get; set; }
        public string ResourceContent { get; set; }
        public ShortMaterialType MaterialType { get; set; }
        public ShortCopyright CopyRight { get; set; }
        public bool IsDraft { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
        public double Rating { get; set; }
        public double AlignmentRating { get; set; }
        public int ReportAbuseCount { get; set; }
        public List<ReferenceMaster> References { get; set; }
        public List<ResourceFileMaster> ResourceFiles { get; set; }

        public List<ResourceComments> ResourceComments { get; set; }
    }
    public class CreateResourceRequest
    {

        [Required(ErrorMessage = "Title Required")]
        [MaxLength(250, ErrorMessage = "Maximum length 250")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CategoryId Required")]
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public string Thumbnail { get; set; }

        [MaxLength(2000, ErrorMessage = "Maximum length 250")]
        public string ResourceDescription { get; set; }

        [MaxLength(1500, ErrorMessage = "Maximum length 250")]
        public string Keywords { get; set; }       
        public string ResourceContent { get; set; }

        [Required(ErrorMessage = "MaterialTypeId Required")]
        public int MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }

        [Required(ErrorMessage = "IsDraft Required")]
        public bool IsDraft { get; set; }

        [Required(ErrorMessage = "CreatedBy Required")]
        public int CreatedBy { get; set; }

        public List<Reference> References { get; set; }

        public List<ResourceFile> ResourceFiles { get; set; }        


    }

    public class UpdateResourceRequest
    {
        [Required(ErrorMessage = "Id Required")]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "Title Required")]
        [MaxLength(250, ErrorMessage = "Maximum length 250")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CategoryId Required")]
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }

        public string Thumbnail { get; set; }

        [MaxLength(2000, ErrorMessage = "Maximum length 250")]
        public string ResourceDescription { get; set; }

        [MaxLength(1500, ErrorMessage = "Maximum length 250")]
        public string Keywords { get; set; }
        public string ResourceContent { get; set; }

        [Required(ErrorMessage = "MaterialTypeId Required")]
        public int MaterialTypeId { get; set; }
        public int? CopyRightId { get; set; }

        [Required(ErrorMessage = "IsDraft Required")]
        public bool IsDraft { get; set; }

        [Required(ErrorMessage = "CreatedBy Required")]
        public int CreatedBy { get; set; }
        public List<Reference> References { get; set; }
        public List<ResourceFile> ResourceFiles { get; set; }
    }

    public class ReferenceMaster
    {
        public decimal Id { get; set; }       
        public decimal ResourceId { get; set; }
        public string URLReference { get; set; }
        public DateTime CreatedOn { get; set; }

    }
    public class ResourceFileMaster
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string AssociatedFile { get; set; }
        public DateTime UploadedDate { get; set; }

    }

    public class Reference
    {
        [Required(ErrorMessage = "URLReferenceId Required")]     
        public int URLReferenceId { get; set; }
    }

    public class ResourceFile
    {
        [Required(ErrorMessage = "AssociatedFile Required")]
        [MaxLength(50, ErrorMessage = "Maximum length 50")]
        public string AssociatedFile { get; set; }
    }

    public class ResourceApprovals
    {
        public int Id { get; set; }
        public decimal ResourceId { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
    public class ResourceComments
    {
        public decimal Id { get; set; }
        public decimal ResourceId { get; set; }
        public string Comments { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReportAbuseCount { get; set; }
    }

   

}
