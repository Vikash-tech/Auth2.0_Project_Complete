using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.SC.SCWorkOrder
{
    public class VmWork
    {
        public SCWoMst SCWoMst { get; set; }
        public List<SCWoDtl> SCWoDtl { get; set; }
        public List<SCWoTNC> SCWoTerm { get; set; }
        public List<SCWoDocUpload> SCWoupload { get; set; }
        public List<SCWoResDep> ScWoResource { get; set; }
        public List<WoStatus> WoStatus {  get; set; } 


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
        public List<SelectListItem> ExcesHrkm { get; set; }
        public List<SelectListItem> DocumenTyp { get; set; }
        public List<SelectListItem> PartNo { get; set; }
        public List<SelectListItem> WoGettyp { get; set; }
        public List<SelectListItem> Client { get; set; }
        public List<SelectListItem> ddlTDS { get; set; }
    }
}
