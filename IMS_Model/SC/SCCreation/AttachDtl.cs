using System.ComponentModel.DataAnnotations;

namespace IMS_Model.SC.SCCreation

{
    public class AttachDtl
    {
        [Required]
        public int SrNo { get; set; }

        [Required]      
        public string FileName { get; set; }
        public string? Path { get; set; }
        public int AttId { get; set; }
        public string? AttachDocName { get; set; }

       
        [Required(ErrorMessage = "Please select a file.")]
        [FileExtensions(Extensions = ".pdf", ErrorMessage = "Only PDF files are allowed")]
        public byte[]? AttachDoc { get; set; }
        public int ActStatus { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
