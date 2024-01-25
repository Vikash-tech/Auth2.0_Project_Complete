using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.CommonDropDown
{
    public class CompMst
    {
        [ValidateNever]
        [Key]
        public int CompId { get; set; }

        [Required(ErrorMessage = "Company Code Cannot be Null")]
        public string CompCode { get; set; }

        [Required(ErrorMessage = "Company Name Cannot be Null")]
        public string CompName { get; set; }

        [Required(ErrorMessage = "Company Display Cannot be Null")]
        public string CompDisp { get; set; }

        [Required(ErrorMessage = "Value Cannot be Null")]
        public int CtmId { get; set; }

        [Required(ErrorMessage = "Company Pan Card Cannot be Null")]
        public string CompPan { get; set; }

        [Required(ErrorMessage = "Company CIN No Cannot be Null")]
        public string CompCin { get; set; }

        [Required(ErrorMessage = "Company URL Cannot be Null")]
        public string CompUrl { get; set; }

        [Required(ErrorMessage = "Date  Cannot be Null")]
        public DateTime IncorDt { get; set; }

        [ValidateNever]
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        [ValidateNever]
        public DateTime AddedOn { get; set; }

        //
        public string? CtmDesctxt { get; set; }
    }
}
