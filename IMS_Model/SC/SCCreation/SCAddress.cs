using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.SC.SCCreation
{

    public class SCAddress
    {
        public int SRNO { get; set; }
        public int SCAddressId { get; set; }
        public int TmpScId { get; set; }
        public string? SCAdd1 { get; set; }
        public string? SCAdd2 { get; set; }
        public string? SCAdd3 { get; set; }
        public string? City { get; set; }
        public int Country { get; set; }
        public string? Pincode { get; set; }
        public string? PhoneNo { get; set; }
        public string? FaxNo { get; set; }
        public string? CntPerson { get; set; }
        public string? Desiganation { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailId { get; set; }

  



    }
}
