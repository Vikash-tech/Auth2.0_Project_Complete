
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.SCCreation
{

    [Table("tSCProfileWODtl")]
    public class SCProfileWODtl
    {
        [Key]
        public int SCProfWOId { get; set; }
        public int ScTmpId { get; set; }
        public int SrNo { get; set; }
        public string? ScRegnNo { get; set; }
        public string? ProjSite { get; set; }
        public string? ProjName { get; set; }
        public string? ProjClient { get; set; }
        public string? States { get; set; }
        public int WOValue { get; set; }
        public string? WODesc { get; set; }

        [ForeignKey("TmpScId")]
        public virtual SCMstReq? SCMstReq { get; set; }
    }
}
