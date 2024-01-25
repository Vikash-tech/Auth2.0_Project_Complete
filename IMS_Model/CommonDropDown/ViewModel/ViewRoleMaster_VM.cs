using IMS_Model.SettingModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace IMS_Model.CommonDropDown.ViewModel
{
    public class ViewRoleMaster_VM
    {
        [ValidateNever]
        public List<TMenuMst> TMenuMsts { get; set; }
        [ValidateNever]
        public List<IMSClaimMaster> IMSClaims { get; set; }//all claims from database    
        [ValidateNever]
        public IMSRoleClaimMaster RoleClaimMaster { get; set; }
        [ValidateNever]
        public List<IMSRoleClaimMaster> IMSRoleClaimMasters { get; set; }
        [ValidateNever]
        public List<ClaimsToRole> CheckClaims { get; set; }
        public IMSRoleMaster IMSRoleMaster { get; set; }



    }
}



















