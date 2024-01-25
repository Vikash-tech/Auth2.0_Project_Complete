namespace IMS_Model.SC.BOQ
{
    public class BOQSubGrp
    {
        public int BOQSubGrpId { get; set; }
        public int GrpId { get; set; }
        public string? SubGrpDisp { get; set; }
        public string? SubGrpDesc { get; set; }

        public int ActStatus { get; set; }

        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        public string? TallyLedger { get; set; }

    }
}
