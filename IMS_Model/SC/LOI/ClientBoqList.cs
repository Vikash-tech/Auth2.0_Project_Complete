namespace IMS_Model.SC.LOI
{
    public class ClientBoqList
    {
        public int DtlId { get; set; }
        public int ClientLOIWOID { get; set; }
        public double ClientBOQNo { get; set; }
        public string? ItemGrp { get; set; }
        public string? SubGrp { get; set; }
        public string? Grp { get; set; }
        public string? ShortDesc { get; set; }
        public string? BOQDesc { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
    }
}
