namespace IMS_Model.SC.SCWorkOrderClose
{
    public class ScWoClose
    {
        public int SCWOId { get; set; }
        public string? SiteCode { get; set; }
        public string? SCRegNo { get; set; }
        public string? SCName { get; set; }
        public string? SCWONO { get; set; }
        public string? TypeDisp { get; set; }
        public decimal TotalWOValue { get; set; }
        public string? ProjectName { get; set; }
        public string? SCWoDt { get; set; }
        public string? ValidFrom { get; set; }
        public string? ValidUpto { get; set; }
        public string? BillPeriod { get; set; }
        public string? InvDate { get; set; }
        public string? AccountingDate { get; set; }
        public string? ThisBillAmt { get; set; }
    }
}
