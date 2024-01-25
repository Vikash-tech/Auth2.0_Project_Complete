namespace IMS_Model.SC.LOI
{
    public class PostVm
    {
        public SCLoIMst SCloiMst { get; set; }
        public List<SCLOIDtl> SCloiDtl { get; set; }
        public List<SCLOITNC> SCloiTerm { get; set; }
        public List<SCDocUpload> SCloiupload { get; set; }
        public List<SCLOIResDep> ScloiResource { get; set; }
        public List<LoiStatus> ScloiStatus { get; set; }
        public List<SCLOITNCDtl> SCloiTermdtl { get; set; }
    }
}
