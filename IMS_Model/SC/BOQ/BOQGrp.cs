using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace IMS_Model.SC.BOQ
{
    public class BOQGrp
    {
        [ValidateNever]
        public int BOQGrpId { get; set; }

        [Required]
        public string? GrpDisp { get; set; }

        [Required]
        public string? GrpDesc { get; set; }

        public int? ActStatus { get; set; }

        [ValidateNever]
        public string? AddedBy { get; set; }

        public DateTime? AddedOn { get; set; }
    }
}
