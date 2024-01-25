
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.CommonDropDown.ViewModel
{
    public class CompanyMst_VM
    {
        public CompMst CompMst { get; set; }

        [ValidateNever]
        public CompanyAddress CompanyAddress { get; set; }
        [ValidateNever]
        public List<CompanyAddress>? CompanyAddresslist { get; set; }
        [ValidateNever]
        public CompConfg CompConfg { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CompTypeMst { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? AddressType { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? StateMst { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CountryMst { get; set; }

    }
}
