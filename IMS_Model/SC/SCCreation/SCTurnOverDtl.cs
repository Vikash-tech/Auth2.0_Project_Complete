using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.SCCreation
{
    public class SCTurnOverDtl
    {
        public int SCFYTId { get; set; }
        public int ScTmpId { get; set; }
        public string? ScRegnNo { get; set; }
        public string? FinYear { get; set; }
        public int TurnOver { get; set; }


        [ForeignKey("TmpScId")]
        public virtual SCMstReq? SCMstReq { get; set; }
    }
}
