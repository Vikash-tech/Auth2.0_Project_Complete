using IMS_Model.CommonModel;
using IMS_Model.UserMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.CommonDropDown.ViewModel
{
    public class SiteMst_vm
    {
        public SiteMaster SiteMaster { get; set; }

        [ValidateNever]
        public List<SiteEmailMst> siteEmailMst { get; set; }
        [ValidateNever]
        public List<SiteStoreMst> siteStoreMst { get; set; }
        [ValidateNever]
        public List<SiteStatusMst> sStatusList { get; set; }
        [ValidateNever]
        public List<SiteAddressMst> siteAddressMst { get; set; }


        [ValidateNever]
        public List<SelectListItem> CompMstList { get; set; }
        [ValidateNever]
        public List<SelectListItem> AddTypeList { get; set; }
        [ValidateNever]
        public List<SelectListItem> Statelist { get; set; }
        [ValidateNever]
        public List<SelectListItem> CountryList { get; set; }
        [ValidateNever]
        public List<SelectListItem> EmailTypList { get; set; }
        [ValidateNever]
        public List<SelectListItem> CityMstList { get; set; }
        [ValidateNever]
        public List<SelectListItem> SiteStatusList { get; set; }
        [ValidateNever]
        public List<SelectListItem> AddYesNo { get; set; }
        [ValidateNever]
        public List<SelectListItem> SiteStatus { get; set; }
    }
}
