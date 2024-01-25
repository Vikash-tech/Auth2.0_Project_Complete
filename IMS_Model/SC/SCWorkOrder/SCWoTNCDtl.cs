namespace IMS_Model.SC.SCWorkOrder
{ 
    public class SCWoTNCDtl
    {
        public int SRNO { get; set; }
        public int SCWOTNCDTLID { get; set; }
        public int SCWOID { get; set; }
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
