using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{ 
    public class CopyrightMaster
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
    }

    public class ShortCopyright
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }       
    }
    public class CreateCopyrightMaster
    {
        [Required(ErrorMessage = "Title Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [MaxLength(1000, ErrorMessage = "Maximum length 1000")]
        public string Description { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateCopyrightMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description Required")]
        [MaxLength(1000, ErrorMessage = "Maximum length 1000")]
        public string Description { get; set; }      
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
    }

}
