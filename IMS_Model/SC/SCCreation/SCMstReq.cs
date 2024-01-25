namespace IMS_Model.SC.SCCreation
{
    public class SCMstReq
    {
        public int TmpScId { get; set; }
        public string? SiteCode { get; set; }
        public string? ReqSite { get; set; }
        public string? ScRegNo { get; set; }
        public string? SCName { get; set; }
        public int SCCompType { get; set; }
        public string? SCCIN { get; set; }
        public string? SCPAN { get; set; }
        public int SCPANAttach { get; set; }
        public string? SCRegdState { get; set; }
        public int SCGSTStatus { get; set; }
        public string? SCGST { get; set; }
        public int SCGSTAttach { get; set; }
        public string? SCPF { get; set; }
        public string? SCESI { get; set; }
        public string? SCBANK { get; set; }
        public string? SCBANKAcNo { get; set; }
        public string? SCBANKIFSC { get; set; }
        public string? SCAdd1 { get; set; }
        public string? SCAdd2 { get; set; }
        public string? SCAdd3 { get; set; }
        public string? SCCity { get; set; }
        public int SCCountry { get; set; }
        public string? SCPin { get; set; }
        public string? SCPhNo { get; set; }
        public string? ScFaxNo { get; set; }
        public string? SCMobNo { get; set; }
        public string? SCCntName { get; set; }
        public string? SCCntMob { get; set; }
        public string? SCCntEmail { get; set; }
        public string? SCExpConstInd { get; set; }
        public string? Remarks { get; set; }
        public string? ReferredBy { get; set; }
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string? SCCNTDesig { get; set; }
        public string? RejRemarks { get; set; }
        public int SCStatus { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime VerifiedOn { get; set; }
        public int MSMEStatus { get; set; }
        public string? RegisterNo { get; set; }
        public DateTime ValidUpto { get; set; }
        public int MSMETurnOver { get; set; }
        public int MSMEAttach { get; set; }
        public int Category { get; set; }
        public string? FinanceYear { get; set; }
        public double Invest { get; set; }
        public int SCAttch { get; set; }
        public string? BankSite { get; set; }
        public string? GSTStatus { get; set; }
        public int Year1 { get; set; }
        public double Turnover1 { get; set; } = 0.0;
        public int Year2 { get; set; }
        public double Turnover2 { get; set; } = 0.0;
        public int Year3 { get; set; }
        public double Turnover3 { get; set; } = 0.0;
        public string? UpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? SCGSTStatusvar { get; set; }
        public string? SCStatusvar { get; set; }
        public int SCPFAttach { get; set; }
        public int SCESIAttach { get; set; }
        public int SCCHQAttac { get; set; }
        public int MandateAttach { get; set; }
    }
}
