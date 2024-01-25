using IMS_Model.SettingModel;
using IMS_Model.UserMaster;

namespace IMS_Model.CommonDropDown.ViewModel
{
    public class PostUserVm
    {
        public UserMst UserMst { get; set; }
        public List<ClaimsToRole> userRoleList { get; set; }  //For Inserting List In database
        public List<ClaimToUser> userClaimList { get; set; } //For Inserting List In database


        public List<IMSUsrRole> imsUsrRoles { get; set; }
        public List<IMSUserClaim> imsUserClaims { get; set; }
    }
}
