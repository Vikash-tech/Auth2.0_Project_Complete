using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.SettingModel
{
    public class IMSRoleClaimMaster
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Role Cannot be null")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Claim Type Cannot be null")]
        public string ClaimType { get; set; }

        [Required(ErrorMessage = "Claim Value Cannot be null")]
        public string ClaimValue { get; set; }
        [Required]
        public bool ActiveStatus { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        [Required]
        public int MenuId { get; set; }
        public string? RevokeBy { get; set; }
        public DateTime? RevokeOn { get; set; }
    }
}
