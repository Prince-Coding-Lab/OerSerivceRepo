using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
    public class UserMaster
    {
        public int Id { get; set; }
        public int? TitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public int? PortalLanguageId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }
        public string ProfileDescription { get; set; }
        public string SubjectsInterested { get; set; }
        public bool ApprovalStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
        public bool IsContributor { get; set; }
    }   

    public class UserCertification
    {
        public decimal Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "CertificationName Required")]
        public string CertificationName { get; set; }
        public int Year { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreateUserCertification
    {      

        [Required(ErrorMessage = "CertificationName Required")]
        public string CertificationName { get; set; }

        [Required(ErrorMessage = "Year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }

    public class UserEducation
    {
        public decimal Id { get; set; }
        public int UserId { get; set; }       
        public string UniversitySchool { get; set; }
        public string Major { get; set; }      
        string Grade { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreateUserEducation
    {      

        [Required(ErrorMessage = "UniversitySchool Required")]
        public string UniversitySchool { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }       
    }
    public class UserExperience
    {
        public decimal Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "OrganizationName Required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Designation Required")]
        public string Designation { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class CreateUserExperience
    {  
        [Required(ErrorMessage = "OrganizationName Required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "Designation Required")]
        public string Designation { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
       
    }
    public class UserLanguage
    {
        public decimal Id { get; set; }
        public int UserId { get; set; }
        public Language Language { get; set; }
        public bool IsSpeak { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public DateTime CreatedOn { get; set; }
    }

  
    public class CreateUserLanguage
    {
        [Required(ErrorMessage = "Language Required")]
        public string Language { get; set; }
        public bool IsSpeak { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
       
    }

  
    public class UserSocialMedia
    {
        public decimal Id { get; set; }
        public int UserId { get; set; }
        public SocialMedia SocialMedia { get; set; }
        public string URL { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class CreateUserSocialMedia
    {
        [Required(ErrorMessage = "SocialMediaId Required")]
        public int SocialMediaId { get; set; }

        [Required(ErrorMessage = "URL Required")]
        public string URL { get; set; }
        
    }

}
