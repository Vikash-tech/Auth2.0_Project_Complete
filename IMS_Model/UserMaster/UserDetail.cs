using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.UserMaster
{
    public class UserDetail
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? UserCode { get; set; }
        public string? UserEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool ActStatus { get; set; }
        public int HOUser { get; set; }
    }
}
