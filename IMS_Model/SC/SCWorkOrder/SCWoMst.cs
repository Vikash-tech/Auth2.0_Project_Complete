namespace IMS_Model.SC.SCWorkOrder
{
    public class SCWoMst
    {
        public string? SCName { get; set; }

        public string? LOINO { get; set; }
        public string? WoStatusDes { get; set; }

        public string? SCWOTypeDesc { get; set; }

        public int SCWOId { get; set; }

        public string? SiteCode { get; set; }

        public string? SCRegNo { get; set; }

        public int SCWOType { get; set; }

        public string? SCWONO { get; set; }

        public int AmendNo { get; set; }

        public DateTime SCWoDt { get; set; }

        public int SCLOIID { get; set; }

        public int QuoteRecd { get; set; }

        public string? ProjectName { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidUpto { get; set; }

        public string? OrgWONo { get; set; }

        public DateTime OrgWODate { get; set; }

        public string? RevReason { get; set; }

        public double TotalWOValue { get; set; }

        public int WOStatus { get; set; }

        public int RetntionAllow { get; set; } = 1;

        public double RetntionPer { get; set; }

        public int RetRelCond { get; set; }

        public double MoblAdv { get; set; }

        public string? MoblAdvCond { get; set; }

        public int BGReq { get; set; }

        public double BGPer { get; set; }

        public int WorkShift { get; set; }

        public string? PreparedBy { get; set; }

        public DateTime PreparedOn { get; set; }

        public string? ApprBy { get; set; }

        public DateTime ApprOn { get; set; }

        public string? ApprRem { get; set; }

        public int MtrlScope { get; set; }

        public double MtrlHandlingPer { get; set; }

        public int HSDScope { get; set; }

        public double HSDFixRate { get; set; }

        public double HSDEscPer { get; set; }

        public double HSDHandlingPer { get; set; }

        public int BitumenScope { get; set; }

        public int FOScope { get; set; }

        public double FORate { get; set; }

        public int FoodExp { get; set; }

        public int SafetyItemScope { get; set; }

        public double SafetyItemHandPer { get; set; }

        public int PFESICScope { get; set; }

        public int TransCondition { get; set; }

        public int AccoScope { get; set; }

        public DateTime AddedOn { get; set; }

        public double FoodPer { get; set; }

        public int ESICScope { get; set; }

        public double BitumenMixPer { get; set; }

        public double BitumenUtilizedPer { get; set; }

        public double AccoMnthAmt { get; set; }

        public string? Remarks { get; set; }

        public int HSDVarianceBasedOn { get; set; }

        public double HSDEscVal { get; set; }

        public double HSDFactor { get; set; }

        public int ClientLoiId { get; set; }

        public int ClientWOId { get; set; }

        public int BacktoBack { get; set; }

        public int HSDAppl { get; set; }

        public int MBGReq { get; set; }

        public double MBGPer { get; set; }

        public int DLPMonths { get; set; }

        public int WOMth { get; set; }

        public int TerminationDays { get; set; }

        public int ShippingAddress { get; set; }

        public int BillingAddress { get; set; }

        public string? SHTApprovedBy { get; set; }

        public DateTime SHTApprovedOn { get; set; }

        public string? SHMApprovedBy { get; set; }

        public DateTime SHMApprovedOn { get; set; }

        public string? RejBy { get; set; }

        public DateTime RejOn { get; set; }

        public string? RejRemarks { get; set; }

        public int HSDCalcOn { get; set; }

        public int PaymentTerms { get; set; }

        public int PaymentDueDays { get; set; }

        public string? ApprRemHOSCTeam { get; set; }

        public string? ApprRemHOSCMgr { get; set; }

        public int TDSSecId { get; set; }

        public int LicLegMatter { get; set; }

        public string? ScopeOfWork { get; set; }

        public double RetLieuPer { get; set; }

        public int MobAppl { get; set; }

        public int BillSubmSchd { get; set; }

        public string? QuotationNo { get; set; }

        public DateTime QuotationDt { get; set; }

        public double RetLimit { get; set; }

        public int NoUndatedChq { get; set; }

        public double ValOfSigleChq { get; set; }

        public int RetMthPostDLP { get; set; }

        public int MobNoOfInst { get; set; }

        public double MobDedPer { get; set; }

        public int FORFOB { get; set; }

        public double AmtFORFOB { get; set; }

        public double RoyalStatPer { get; set; }

    }
}
