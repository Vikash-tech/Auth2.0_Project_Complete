using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.SettingModel
{
    public class AllClaimsType
    {
        public int MenuID { get; set; }
        public bool IsGet { get; set; }
        public bool IsPut { get; set; }
        public bool IsPost { get; set; }
        public bool IsSiteVerify { get; set; }
        public bool IsPMVerify { get; set; }
        public bool IsHOVerify { get; set; }
        public bool IsApprove { get; set; }
        public bool IsCancel { get; set; }
        public bool IsApprove25 { get; set; }
        public bool IsApprove50 { get; set; }
        public bool IsPayPost { get; set; }
        public bool IsSetting { get; set; }


    }
}
