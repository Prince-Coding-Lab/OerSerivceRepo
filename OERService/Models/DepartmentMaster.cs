using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class DepartmentMaster
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public string CreatedBy { get; set; }
       public DateTime CreatedOn { get; set; }
       public string UpdatedBy { get; set; }
       public DateTime UpdatedOn { get; set; }
       public bool Active { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
      
    }
}
