namespace IMS_Model.SC.SCCreation
{
    public class SCViewModel
    {

        public SCMst SCMst { get; set; }
        public IEnumerable<SCMst> sCMstLST { get; set; } = new List<SCMst>();


        public List<SCAddress> SCAddresses { get; set; }
        public List<SCMstReq> SCmstreq { get; set; }
        public List<SCAssComp> SCAssComp { get; set; }
        public List<SCEquipDtl> SCEquipDtl { get; set; }
        public List<SCProfileWODtl> SCProfileWODtl { get; set; }
        public List<AttachDtl> AttachFile { get; set; }

    }
}
