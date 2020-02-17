using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{  
    public class CategoryMaster
    {
         public int Id { get; set; }
         public string Name { get; set; }
         public string CreatedBy { get; set; }
         public DateTime CreatedOn { get; set; }
         public string UpdatedBy { get; set; }
         public DateTime UpdatedOn { get; set; }
         public bool Active { get; set; }
    }

    public class ShortCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }        
    }

    public class ShortSubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
    public class CreateCategoryMaster
    {        
        [Required(ErrorMessage ="Name Required")]
        [MaxLength(100,ErrorMessage = "Maximum length 100")]
        public string Name { get; set; }       
         public int CreatedBy { get; set; }        
    }

    public class UpdateCategoryMaster
    {        
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Name { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
    }
}
