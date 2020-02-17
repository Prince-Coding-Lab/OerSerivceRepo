using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{
    public class QRCUserMapping
    {
        public int Id { get; set; }
        public int QRCId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Active { get; set; }
    }
    public class QRCUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ResourceContributed { get; set; }
        public int CourseCreated { get; set; }
        public int CurrentQRCS { get; set; }
        public int NoOfReviews { get; set; }
    }
}
