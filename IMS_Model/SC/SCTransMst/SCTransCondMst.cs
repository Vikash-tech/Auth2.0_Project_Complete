namespace IMS.Models.SC.SCTransMst
{
    public class SCTransCondMst
    {
        public int TransCondId { get; set; }
        public string? TransCondition { get; set; }
        public int ActiveStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
