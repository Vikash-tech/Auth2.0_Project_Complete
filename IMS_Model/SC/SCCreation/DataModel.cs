namespace IMS_Model.SC.SCCreation
{
    public class DataModel
    {
        public List<SCMst> scmstlist { get; set; }
        public List<SCAddress> SCAddresses { get; set; }
        public List<SCMstReq> SCmstreq { get; set; }
        public List<SCAssComp> SCAssComp { get; set; }
        public List<SCEquipDtl> SCEquipDtl { get; set; } 
        public List<SCProfileWODtl> SCProfileWODtl { get; set; }
        public List<AttachDtl> AttachFile { get; set; }

    }
}
