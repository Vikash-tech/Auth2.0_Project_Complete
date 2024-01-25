
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace IMS_Model.SC.LOI
{
    public class VmLOI
    {

        public SCLoIMst SCloiMst { get; set; }
        public List<SCLOIDtl> SCloiDtl { get; set; }
        public List<SCLOITNC> SCloiTerm { get; set; }
        public List<SCDocUpload> SCloiupload { get; set; }
        public List<SCLOIResDep> ScloiResource { get; set; }
        public List<LoiStatus> ScloiStatus { get; set; }

        public List<SelectListItem> GSTStatus { get; set; }
        public List<SelectListItem> GST { get; set; }
        public List<SelectListItem> workshift { get; set; }
        public List<SelectListItem> for_fob { get; set; }
        public List<SelectListItem> yesno { get; set; }
        public List<SelectListItem> Pbgreq { get; set; }
        public List<SelectListItem> HsdScope { get; set; }
        public List<SelectListItem> HSDfactor { get; set; }
        public List<SelectListItem> Pfscope { get; set; }
        public List<SelectListItem> FoodScope { get; set; }
        public List<SelectListItem> Safety { get; set; }
        public List<SelectListItem> AccomScope { get; set; }
        public List<SelectListItem> FOScope { get; set; }
        public List<SelectListItem> Bitumen { get; set; }
        public List<SelectListItem> License_legal { get; set; }
        public List<SelectListItem> BillSudle { get; set; }
        public List<SelectListItem> unitmst { get; set; }
        public List<SelectListItem> Hsdunit { get; set; }
        public List<SelectListItem> ExcesHrkm { get; set; }
        public List<SelectListItem> DocumenTyp { get; set; }
        public List<SelectListItem> PartNo { get; set; }
        public List<SelectListItem> WoGettyp { get; set; }
        public List<SelectListItem> Client { get; set; }
        public List<SelectListItem> ddlTDS { get; set; }

    }
}
