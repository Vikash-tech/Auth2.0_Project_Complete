namespace IMS_Model.SC.BOQItemGroup
{
    public class BOQItemGrp
    {
    
        public int BOQItemGrpId { get; set; }
        public string? ItemGrpCode { get; set; }
        public int BOQGrp { get; set; }
        public int BOQSubGrp { get; set; }
        public string? ItemGrpDisp { get; set; }
        public string? ItemGrpDesc { get; set; }
        public int ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string? BOQGrpDisplay { get; set; }
        public string? BOQSubGrpDisplay { get; set; }

    }
}
