using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.CommonDropDown
{

    public class CountryMst
    {
        [Key]
        [ValidateNever]
        public int CtId { get; set; } = 0;

        [Required(ErrorMessage = "Pleasee enter Country Code ")]
        [DisplayName("Country Code")]
        public string CtCode { get; set; }

        [Required(ErrorMessage = "Pleasee enter Country Name")]
        [DisplayName("Country Name")]
        public string CtName { get; set; }

        [Required(ErrorMessage = "Pleasee enter CtIsdCod")]
        [DisplayName("Country ISD Code")]
        public string CtIsdCod { get; set; }

        public bool ActStatus { get; set; } = true;
        public string? AddedBy { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.Now;


    }

}
