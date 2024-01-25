using System.ComponentModel.DataAnnotations;

namespace IMS_Model.LoginModel
{
    public class Login
    {
   
        [Required(ErrorMessage = "User Id Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
