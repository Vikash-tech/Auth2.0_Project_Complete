using IMS_Model.SettingModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model
{
    public class ClaimsToRole
    {
        public int MenuId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public bool IsChecked { get; set; }
    }
}
