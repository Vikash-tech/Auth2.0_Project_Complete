using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Model.CommonDropDown
{
    public class CompanyAddress
    {
       
        public int? SRNO { get; set; }
        [ValidateNever]
        public int? CAddId { get; set; }
        [ValidateNever]
        public int? CompId { get; set; }
        public int? AtId { get; set; }
        public string? Add1 { get; set; }
        public string? Add2 { get; set; }
        public string? Add3 { get; set; }
        public string? City { get; set; }
        public int? StId { get; set; }
        public int? CtId { get; set; }
        [ValidateNever]
        public int? CaPin { get; set; }
        public string? Phone { get; set; }
        public bool ActStatus { get; set; }
        public string? AddedBy { get; set; }
        public DateTime AddedOn { get; set; }

        //
        public string? AtDesc { get; set; }
        public string? StName { get; set; }
        public string? CtName { get; set; }

    }
}
