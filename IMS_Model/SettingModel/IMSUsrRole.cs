using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.SettingModel
{
    public class IMSUsrRole
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleId { get; set; }

        [Required]
        public int SiteId { get; set; }
        public bool ActStatus { get; set; } = true;
        [MaxLength(10)]
        [Required]
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public string? RevokeBy { get; set; }
        public DateTime? RevokeOn { get; set; }
    }
}
