using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace IMS.Models.SC.SCDocUpload
{

    public class ScUploadDoc
    {
        public int SrNO { get; set; }
        public int SCWODocId { get; set; }
        public string? AttachDocName { get; set; }
        public Byte[]? AttachDoc { get; set; }
        public int DocId { get; set; }
        public int DocType { get; set; }
        public int PartNo { get; set; }
        public int NoOfPage { get; set; }
        public int PageFrom { get; set; }
        public int PageTo { get; set; }
        public int AttId { get; set; }
        public int ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public int WOAmendNo { get; set; }     
        public string? Path { get; set; }
    }
}
