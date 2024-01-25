
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IMS_Model.SC.SCCreation
{

    [Table("tSCAssComp")]
    public class SCAssComp
    {
        
        public int SCAssCmpId { get; set; }
        public int ScTmpId { get; set; }
        public int SrNo { get; set; }
        public string? ScRegnNo { get; set; }
        public string? AssCompName { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        [ForeignKey("TmpScId")]
        public virtual SCMstReq? SCMstReq { get; set; }
    }
}
