namespace IMS.Models.SC.SCRetentionMst
{
    public class RetentionMst
    {
        public int RetId { get; set; }
        public string? SiteCode { get; set; }
        public string? RelPeriod { get; set; }
        public int RelCond { get; set; }
        public string? RelDesc { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
