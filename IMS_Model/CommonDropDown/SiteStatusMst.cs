using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonDropDown
{
    public class SiteStatusMst
    {
        public int SrNo { get; set; }
        public int SiteStatusId { get; set; }
        public string SiteCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }
        public int AddedBy { get; set; }
        public bool ActStatus { get; set; } = true;
        public DateTime AddedOn { get; set; }
    }
}
