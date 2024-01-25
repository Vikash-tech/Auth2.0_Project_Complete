using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.SC.SCWorkOrderClose
{
    public class ScCloseApproval
    {
        public int SCWOClsReqId { get; set; }
        public int SCWOID { get; set; }
        public string? SCWONO { get; set; }
        public string? SCRegNo { get; set; }
        public string? SCName { get; set; }
        public string? ReqBy { get; set; }
        public DateTime ReqOn { get; set; }
        public int WOStatus { get; set; }
        public List<SelectListItem> SiteList { get; set; }
    }
}
