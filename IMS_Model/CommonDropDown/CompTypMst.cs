using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMS_Model.CommonDropDown
{

    public class CompTypMst
    {
        [Key]
        public int CtmId { get; set; }
        [Required(ErrorMessage = "Company Description cannot be Null")]
        [DisplayName("Company Description")]
        public string CtmDesc { get; set; }
        public bool? ActStatus { get; set; } = true;
        public DateTime AddedOn { get; set; }=DateTime.Now;
        public string? AddedBy { get; set; }
    }
}
