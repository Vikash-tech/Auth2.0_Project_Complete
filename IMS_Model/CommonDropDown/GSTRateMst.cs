

namespace IMS_Model.CommonDropDown
{

    public class GSTRateMst
    {
		public int RateId { get; set; }
		public string? GSTRateDesc { get; set; }
		public double CGSTRate { get; set; }
		public double SGSTRate { get; set; }
		public double IGSTRate { get; set; }
		public bool CessApplicable { get; set; }
		public int CessBasis { get; set; }
		public double CessRate { get; set; }
		public int CessVal { get; set; }
		public int CessFactor { get; set; }
		public bool RCMApplicable { get; set; }
		public bool ITCApplicable { get; set; }
		public DateTime StDt { get; set; }
		public DateTime EnDt { get; set; }

	}
}
