using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMS_Model.UserMaster
{
    public class UserMst
    {
      
        public string? Id { get; set; } 

        [Required(ErrorMessage = "User First Name is required!")]
        [MaxLength(50)]
        public string UsrFN { get; set; }
        [Required(ErrorMessage = "User Last Name is required!")]
        [MaxLength(50)]
        public string UsrLN { get; set; }

        [StringLength(6)]
        [Required(ErrorMessage = "User Code is required!")]
        public string UsrCode { get; set; }
        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "User Name is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string? Password { get; set; }

        public string? AddedBy { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage ="Select User Type")]
        public int HOUser { get; set; }

        public string? RevokeBy { get; set; }
        public DateTime? RevokeOn { get; set; }
        public bool ActStatus { get; set; }

        public string? NormalizedUserName { get; set; }  
        public DateTime AddedOn {  get; set; }   

    }
}
