using IMS_Model.SettingModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model
{
    public class ClaimToUser
    {
        public int SiteId { get; set; }
        public string RoleId { get; set; }
        public bool IsChecked { get; set; }
    }
}
