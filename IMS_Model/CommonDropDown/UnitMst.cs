using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.CommonDropDown
{
    public class UnitMst
    {
        public int UnitId { get; set; }
        public string? DispUnit { get; set; }
        public bool ActStatus { get; set; }
        public string? GSTUnit { get; set; }
    }
}
