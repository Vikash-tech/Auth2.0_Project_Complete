using System.Data;
namespace IMS_Model.SC.SCWorkOrder
{
    public class WOPrint
    {
        public List<SCWoMst> ScwoMstlist { get; set; }
        public DataTable DtWomst { get; set; }
        public DataTable DtWodtl { get; set; }
        public DataTable DtWoResource { get; set; }
        public DataTable DtWoTNCdtl { get; set; }
    }
}
