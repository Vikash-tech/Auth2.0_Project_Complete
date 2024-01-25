using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS_Model.CommonDropDown
{

    public class ParameterVal
    {
        public int ParamValId { get; set; }
        public string ParamCd { get; set; }
        public string ParamValDispTxt { get; set; }
        public int ParamValueTxt { get; set; }
        public bool ActStatus { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
    }
}
