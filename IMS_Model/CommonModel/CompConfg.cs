

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IMS_Model.CommonModel
{
    public class CompConfg
    {
        public int? ConfgId { get; set; }
        public int? CompId { get; set; }

        [Required(ErrorMessage ="Kindly Upload Logo")]
        public IFormFile? LogoFile { get; set; }

        public string? FileName { get; set; }
        public byte[]? Logo { get; set; }

        [Required(ErrorMessage = "Financial Start Year Should not be Blank")]
        public int FinStYr { get; set; }
        [Required(ErrorMessage = "Financial End Year Should not be Blank")]
        public int FinEdYr { get; set; }
        public string? DtFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? NumFormat { get; set; }
        public string? CurrFormat { get; set; }
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        //
        [Required(ErrorMessage = "Kindly Upload Letter Head Logo")]
        public IFormFile? LetterHeadLogoFile { get; set; }

        public byte[]? LetterHeadLogo { get; set; }
    }
}
 					