using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.SettingModel
{
    public class IMSUserClaim
    {

        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int SiteId { get; set; }
        public int MenuId { get; set; }
        public bool ActiveStatus { get; set; } = true;
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public string? RevokeBy { get; set; }
        public DateTime? RevokeOn { get; set; }
    }
}
