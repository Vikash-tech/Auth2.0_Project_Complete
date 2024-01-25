using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.SettingModel
{
    public class IMSClaimMaster
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ClaimValue cannot ne Empty")]
        public string ClaimValue { get; set; }
        [Required(ErrorMessage = "ClaimType cannot ne Empty")]
        public string ClaimType { get; set; }
        public bool ActStatus { get; set; } = true;
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;

        public int MenuID { get; set; }//added for itrating and not related to entity
    }
}
