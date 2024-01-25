namespace IMS_Model.SC.BOQItemCreation
{
    public class BOQItemMstTmp
    {
        public int BoqItemId { get; set; }
        public string? BOQItemCode { get; set; }
        public string? ItemGrpCode { get; set; }
        public string? ItemDesc { get; set; }
        public int PUOM { get; set; }
        public int SUOM { get; set; }
        public string? HSNCode { get; set; }
        public bool ITCApplicable { get; set; }
        public bool TCSApplicable { get; set; }
        public bool TDSApplicable { get; set; }
        public int ApprStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string? ApprBy { get; set; }
        public DateTime ApprOn { get; set; }
        public string? ApprRem { get; set; }
        public string? SiteCode { get; set; }

        public string? ItemGrpDisp { get; set; }
        public string? SubGrpDisp { get; set; }
        public string? GrpDisp { get; set; }

    }
}
