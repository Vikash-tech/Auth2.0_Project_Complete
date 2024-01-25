namespace IMS_Model.SC.LOI
{
    public class SCLOITNC
    {
        public int SrNo { get; set; }
        public int SCLOITNCId { get; set; }
        public int SCLOIId { get; set; }
        public string? TermHead { get; set; }
        public string? TermDesc { get; set; }
        public int ActiveStatus { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
