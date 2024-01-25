using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.UserMaster
{
  
    public class SiteMaster
    {
        [ValidateNever]
        public int SiteId { get; set; }

        public int CompId { get; set; } = 1;
        [Required(ErrorMessage ="Site Code Cannot be Empty")]
        public string SiteCode { get; set; }
        [Required(ErrorMessage = "Site Name Cannot be Empty")]
        public string SiteName { get; set; }
        [Required(ErrorMessage = "Site Start Date Cannot be Empty")]
        public DateTime SiteStdt { get; set; }

        [Display(Name = "Site GST No:")]
        [Required(ErrorMessage = "GST Number Cannot be Blank")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST Number.")]

        //[Required(ErrorMessage = "Site GST No Date Cannot be Empty")]
        //[RegularExpression("^d{2}[A-Z]{5}d{4}[A-Z]d{2}$")]
        public string SiteGSTN { get; set; }
        public bool ActStatus { get; set; }

        [ValidateNever]
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }=DateTime.Now;
        public string? BANKCHQREF { get; set; }
    }

}
