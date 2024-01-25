namespace IMS_Model.SC.LOI
{
    public class SCLOITNCDtl
    {
        public int SRNO { get; set; }
        public int SCLOITNCDTLID { get; set; }
        public int SCLOIID { get; set; }
        public int SCWOTNCId { get; set; }
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string? TermDesc { get; set; }
        public string? TermHead { get; set; }
        public int PrntSeq { get; set; }
        public int TermStatus { get; set; }

    }
}
