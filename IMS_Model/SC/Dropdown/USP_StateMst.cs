using System.ComponentModel.DataAnnotations;

namespace IMS_Model.SC.Dropdown
{ 
    public class USP_StateMst
    {


        //for country and state  USP_StateMst_GridView

        
        public int StName { get; set; }
        public int StId { get; set; }
        public int StCode { get; set; }
        public int CtCode { get; set; }

        [Key]
        public int CtId { get; set; }
    }
}
