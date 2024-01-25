using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonDropDown
{
    public class CityMst
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityDispName { get; set; }
        public int StCode { get; set; }
        public string CityAirCode { get; set; }
        public string CityTrainCode { get; set; }
        public string CitySTDCODE { get; set; }
        public int Zone { get; set; }
        public int ActStatus { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
