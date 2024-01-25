using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace IMS_Model.SC.SCWorkOrder
{
    public class PostvmWo
    {
        public SCWoMst SCWoMst { get; set; }

        [ValidateNever]
        public List<SCWoDtl> SCWoDtl { get; set; }

        [ValidateNever]
        public List<SCWoTNC> SCWoTerm { get; set; }

        [ValidateNever]
        public List<SCWoTNCDtl> SCWoSpTerm { get; set; }

        [ValidateNever]
        public List<SCWoDocUpload> SCWoupload { get; set; }

        [ValidateNever]
        public List<SCWoResDep> ScWoResource { get; set; }

        [ValidateNever]
        public List<WoStatus> WoStatus { get; set; }

    }
}
