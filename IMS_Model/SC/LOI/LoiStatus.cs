namespace IMS_Model.SC.LOI
{
    public class LoiStatus
    {
        public int ActLOIID { get; set; }
        public int LOINO { get; set; }
        public int Action { get; set; }
        public int ActionBy { get; set; }
        public DateTime ActionDateTime { get; set; }
        public string? ActionIP { get; set; }
        public string? Remark { get; set; }

    }
}
