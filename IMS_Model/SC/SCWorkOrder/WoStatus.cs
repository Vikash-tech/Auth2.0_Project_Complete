namespace IMS_Model.SC.SCWorkOrder
{
    public class WoStatus
    {
        public int ActWOID { get; set; }
        public int WOId { get; set; }
        public int Action { get; set; }
        public int ActionBy { get; set; }
        public DateTime ActionDateTime { get; set; }
        public string? ActionIP { get; set; }
        public string? Remark { get; set; }

    }
}
