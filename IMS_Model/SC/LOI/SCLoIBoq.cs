

namespace IMS_Model.SC.LOI
{
    public class SCLoIBoq
    {
        public int BoqItemId { get; set; }
        public string? BOQItemCode { get; set; }
        public string? ItemDesc { get; set; }
        public int PUOM { get; set; }
        public int SUOM { get; set; }
        public string? HSNCode { get; set; }
        public string? GrpDisp { get; set; }
        public string? SubGrpDisp { get; set; }
        public string? ItemGrpDisp { get; set; }
        public string? TallyLedger { get; set; }
    }
}
