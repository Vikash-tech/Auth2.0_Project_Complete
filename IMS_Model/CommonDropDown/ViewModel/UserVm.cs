using IMS_Model.SettingModel;
using IMS_Model.UserMaster;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS_Model.CommonDropDown.ViewModel
{
    public class UserVm
    {
        public UserMst UserMst { get; set; }

        public List<IMSUsrRole> imsUsrRoles { get; set; }   
        public List<IMSUserClaim> imsUserClaims { get; set; } 


        public List<IMSRoleMaster> roleMasters { get; set; }
        public List<SiteMst> siteMasters { get; set; }


        public List<TMenuMst> TMenuMsts { get; set; } //List to Assign For Insert
        public List<IMSClaimMaster> IMSClaims { get; set; } //List to Assign For Insert

        public List<SelectListItem> AllSite { get; set; }
        public List<SelectListItem> AddYesNo { get; set; }
    }
}
