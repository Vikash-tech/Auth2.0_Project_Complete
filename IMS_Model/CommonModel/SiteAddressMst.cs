using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonModel
{
    public class SiteAddressMst
    {
        public int AddSrNo { get; set; }    
        public int SiteAddId { get; set; }
        public int SiteId { get; set; }
        public int SiteAddType { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string City { get; set; }
        public int StId { get; set; }
        public int CtId { get; set; }
        public string SitePin { get; set; }
        public string PhoneNo { get; set; }
        public bool ActStatus { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
