using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonModel
{
    public class UserAccessInMenu
    {
        public bool IsGet { get; set; } = false;
        public bool IsPut { get; set; } = false;
        public bool IsPost { get; set; } = false;
        public bool IsSiteVerify { get; set; } = false;
        public bool IsPMVerify { get; set; } = false;
        public bool IsHOVerify { get; set; } = false;
        public bool IsApprove { get; set; } = false;
        public bool IsCancel { get; set; } = false;
        public bool IsApprove25 { get; set; } = false;
        public bool IsApprove50 { get; set; } = false;
        public bool IsPayPost { get; set; } = false;
        public bool IsSetting { get; set; } = false;
    }
}
