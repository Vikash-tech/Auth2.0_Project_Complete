using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.CommonDropDown
{
    public class StateMst
    {
        public int StId { get; set; }
        public int CtId { get; set; }
        public string StCode { get; set; }
        public string StName { get; set; }
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
