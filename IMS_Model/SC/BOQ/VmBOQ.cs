namespace IMS_Model.SC.BOQ
{
    public class VmBOQ
    {
        public int BOQGrpId { get; set; }

        public string? GrpDisp { get; set; }
        public string? GrpDesc { get; set; }

       public List<BOQSubGrp> SubGrp { get; set; }
    }
}
