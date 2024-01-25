namespace IMS_Model.SC.SCWorkOrder
{
    public class SCWoResDep
    {
        public int SrNo { get; set; }   
        public int WoResDepId { get; set; }
        public int SCWOId { get; set; }
        public string? ResourceDesc { get; set; }
        public string? ResourceNo { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
