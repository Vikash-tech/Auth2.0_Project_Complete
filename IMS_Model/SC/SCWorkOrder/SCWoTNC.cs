namespace IMS_Model.SC.SCWorkOrder
{
    public class SCWoTNC
    {
        public int SrNo { get; set; }
        public int SCWOTNCId { get; set; }
        public int SCWOId { get; set; }
        public string? TermHead { get; set; }
        public string? TermDesc { get; set; }
        public int ActiveStatus { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
