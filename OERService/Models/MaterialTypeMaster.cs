using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OERService.Models
{
    public class MaterialTypeMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
    }

    public class ShortMaterialType
    {
        public int Id { get; set; }
        public string Name { get; set; }
       
    }
    public class CreateMaterialTypeMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateMaterialTypeMaster
    {
        [Required(ErrorMessage = "Name Required")]
        [MaxLength(100, ErrorMessage = "Maximum length 100")]
        public string Name { get; set; }
        public int UpdatedBy { get; set; }
        public bool Active { get; set; }
    }

}
