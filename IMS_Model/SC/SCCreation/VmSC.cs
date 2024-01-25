using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.SC.SCCreation
{
    public class VmSC
    {
        public SCMst scmst { get; set; }
        public List<SCMst> SCmstlist { get; set; }
        public List<SCAddress> SCAddresses { get; set; }
        public List<SCAssComp> SCAssComp { get; set; }
        public List<SCEquipDtl> SCEquipDtl { get; set; }
        public List<SCProfileWODtl> SCProfileWODtl { get; set; }
        public List<AttachDtl> AttachFile { get; set; }

        public List<SelectListItem> Msme { get; set; }
        public List<SelectListItem> GST { get; set; }
        public List<SelectListItem> Site { get; set; }
        public List<SelectListItem> Enterprise { get; set; }
        public List<SelectListItem> Attachfilepara { get; set; }
        public List<SelectListItem> Compty { get; set; }
        public List<SelectListItem> Statelist { get; set; }
        public List<SelectListItem> Banklists { get; set; }
    }
}
