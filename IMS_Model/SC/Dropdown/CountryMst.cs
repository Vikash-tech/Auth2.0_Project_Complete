using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.Dropdown
{
    [Table("tCountryMst")]
    public class CountryMst

    {
        [Key]
        public int CtId { get; set; }
        public string? CtCode { get; set; }
        public string? CtName { get; set; }
        public string? CtIsdCod { get; set; }


    }

}
