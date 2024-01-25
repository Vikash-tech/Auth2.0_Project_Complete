using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Models.SC.SCDocUpload
{
    public class VmDoc
    {
        public ScUploadDoc ScUploadDoc { get; set; }
        public List<ScUploadDoc> DocAttachList { get; set; }    
        public List<SelectListItem> DocumenTyp { get; set; }
        public List<SelectListItem> PartNo { get; set; }
    }
}
