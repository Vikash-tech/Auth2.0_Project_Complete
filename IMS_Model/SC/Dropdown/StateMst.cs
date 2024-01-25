using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.Dropdown
{
    [Table("tStateMst")]
    public class StateMst
    {
        [Key]
        public int StId { get; set; }
        public int CtId { get; set; }
        public string StCode { get; set; }
        public string? StName { get; set; }


        [ForeignKey("CtId")]
        public virtual CountryMst? Country { get; set; }



        [ForeignKey("StCode")]
        public virtual StateMst? State { get; set; }

    }
}
