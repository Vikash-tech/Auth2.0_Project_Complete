using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.SCCreation
{

    [Table("tSCEquipDtl")]
    public class SCEquipDtl
    {

        [Key]
        public int SCEquipId { get; set; }
        public int ScTmpId { get; set; }
        public int SrNo { get; set; }
        public string? ScRegnNo { get; set; }
        public string? ScEquipDesc { get; set; }
        public string? ScEquipMake { get; set; }
        public string? ScEquipModel { get; set; }
        public string? ScEquipNo { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }


        [ForeignKey("TmpScId")]
        public virtual SCMstReq? SCMstReq { get; set; }
    }
}
