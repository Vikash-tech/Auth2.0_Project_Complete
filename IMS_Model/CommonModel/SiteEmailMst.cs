using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonModel
{
    public class SiteEmailMst
    {
        public int Srno { get; set; }
        public int SeId { get; set; }
        public int SiteId { get; set; }
        public int EmailType { get; set; }
        public string EmailId { get; set; }
        public string PersonName { get; set; }
        public string MobileNo { get; set; }
        public bool ActStatus { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
