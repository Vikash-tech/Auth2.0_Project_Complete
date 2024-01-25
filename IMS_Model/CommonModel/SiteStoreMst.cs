using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonModel
{
    public class SiteStoreMst
    {
        public int SrNo { get; set; }
        public int StoreId { get; set; }
        public string SiteCode { get; set; }
        public string StoreCode { get; set; }
        public string StoreDesc { get; set; }
        public int AddedBy { get; set; }
        public int ActStatus { get; set; }
        public DateTime AddedOn { get; set; }
        public bool? BulkStore { get; set; }
        public int iBulkSt { get; set; }
    }
}
