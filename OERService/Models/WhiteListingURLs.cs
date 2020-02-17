using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.Models
{ 
    public class WhiteListingURLsMaster
    {
        public decimal Id { get; set; }
        public int RequestedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public string URL { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public string RejectedReason { get; set; }
    }

    public class WhiteListingURLs
    {
        public decimal Id { get; set; }
        public string RequestedBy { get; set; }
        public string VerifiedBy { get; set; }
        public string URL { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? VerifiedOn { get; set; }
        public string RejectedReason { get; set; }
    }
    public class WhiteListingRequest
    {       
        public int RequestedBy { get; set; }       
        public string URL { get; set; }       
    }

    public class WhiteListUrl
    {            
        public int VerifiedBy { get; set; }      
        public bool IsApproved { get; set; }       
        public string RejectedReason { get; set; }
    }
    public class IsWhiteListed
    {
        public string URL { get; set; }
    }
}
