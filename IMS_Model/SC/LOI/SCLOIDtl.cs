namespace IMS_Model.SC.LOI
{
    public class SCLOIDtl
    {

        public int SrNo { get; set; }
        public int SCLOIDtlId { get; set; }
        public int SCLOIId { get; set; }
        public string? BOQItemCode { get; set; }
        public int ClientBOQDtlId { get; set; }
        public int Unit { get; set; }
        public string? HSNCode { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double TaxAmt { get; set; }
        public int GSTId { get; set; }
        public double SGSTRate { get; set; }
        public double CGSTRate { get; set; }
        public double IGSTRate { get; set; }
        public double SGSTAmt { get; set; }
        public double CGSTAmt { get; set; }
        public double IGSTAmt { get; set; }
        public double TotalAmt { get; set; }
        public bool TCSAppl { get; set; }
        public bool TDSAppl { get; set; }
        public double TDSRate { get; set; }
        public string? ShortDesc { get; set; }
        public string? AddlDesc { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public double MaintDays { get; set; }
        public double EquipAvlDays { get; set; }
        public double HSDPerfFrom { get; set; }
        public double HSDPerfTo { get; set; }
        public int HSDPerUnit { get; set; }
        public bool DryRentAppl { get; set; }
        public double DryRentPer { get; set; }
        public double DryRentAmt { get; set; }
        public bool RateEscl { get; set; }
        public int OperatorScope { get; set; }
        public int OperatorNos { get; set; }
        public int HelperNos { get; set; }
        public double LeadFrom { get; set; }
        public double LeadTo { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public double Discount { get; set; }
        public double SubTotal { get; set; }
        public double DeductRatePerHour { get; set; }
        public int GSTOnRCM { get; set; }
        public int BOQHSDScope { get; set; }
        public double BOQHSDFactor { get; set; }
        public int ReconPeriod { get; set; }
        public bool AllowMarching { get; set; }
        public bool AllowBreakDown { get; set; }
        public int BreakDownHrs { get; set; }
        public int MarchingKMs { get; set; }
        public string? BreakDownHrsDays { get; set; }

        public string? GstRate { get; set; }
        public string? ItemGrpDisp { get; set; }
        public string? SubGrpDisp { get; set; }
        public string? GrpDisp { get; set; }
        public string? BoqDesc { get; set; }
        public string? TallyLedger { get; set; }
        public string? DispUnit { get; set; }
        public string? ReconPeriodTxt { get; set; }
    }
}
