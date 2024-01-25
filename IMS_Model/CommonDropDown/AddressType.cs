using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonDropDown
{
    public class AddressType
    {
        [Key]
        public int AtId { get; set; }
        [Required(ErrorMessage ="Value Cannot be null")]
        [DisplayName("Address Description")]
        public string AtDesc { get; set; }
        public bool ActStatus { get; set; } = true;
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}
