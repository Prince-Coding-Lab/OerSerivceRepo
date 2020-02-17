using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class ProfileAppData
    {
        public List<Title> Titles { get; set; }
        public List<Country> Countries { get; set; }
        public List<State> States { get; set; }
        public List<SocialMedia> SocialMedias { get; set; }
        public List<Gender> Genders { get; set; }
        public List<PortalLanguage> PortalLanguages { get; set; }

    }

    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }       
    }

    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }      
    }

    public class SocialMedia
    {
        public int Id { get; set; }
        public string Name { get; set; }     
    }

    public class State
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }       
    }

    public class TitleMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class CountryMaster
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class StateMaster
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class SocialMediaMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }


    public class PortalLanguage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
