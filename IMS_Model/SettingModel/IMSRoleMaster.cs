using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMS_Model.SettingModel
{
    public class IMSRoleMaster
    {

        [Key]
        public Guid? Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Lock Period cannot be null")]
        [DisplayName("Lock Period(In Days)")]
        public int LockPeriod { get; set; }

        public bool? ActStatus { get; set; } = true;
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        [Required(ErrorMessage = "Lock Period For Month End cannot be null")]
        [DisplayName("Lock Period For Month End")]
        public int LockPeriodAdd { get; set; }

        [Required(ErrorMessage ="Role Name Cannot be null")]
        [DisplayName("Role Name")]
        public string Name { get; set; }


        [DisplayName("Normalized Role Name")]
        [Required(ErrorMessage = "Normalized Role Name Cannot be null")]
        public string NormalizedName { get; set; }

        public string? ConcurrencyStamp { get; set; }
    }
}
