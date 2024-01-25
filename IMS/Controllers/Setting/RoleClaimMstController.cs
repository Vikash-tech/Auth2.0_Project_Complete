using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonModel;
using IMS_Model.SettingModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class RoleClaimMstController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;

        public static List<IMSRoleClaimMaster> iMSRoleClaimMasters = new List<IMSRoleClaimMaster>();
        public RoleClaimMstController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
        {
            _configuration = configuration;
            _apiservice = aPTService;
        }

        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<PartialViewResult> _RoleClaimMstList()
        {
            GetBaseUrl();
            string sBaseUrl = sBU + "api/RoleClaim/ReadAllRoleClaimMaster";
            APIResponse response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    iMSRoleClaimMasters = JsonConvert.DeserializeObject<List<IMSRoleClaimMaster>>(response.Result);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return PartialView("_RoleClaimMstList", iMSRoleClaimMasters);
        }


        public async Task<IActionResult> RoleClaimCreation()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleClaimCreation(IMSRoleClaimMaster iMSRoleClaim)
        {
            if (ModelState.IsValid)
            {
                try
                {

                }catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return View(iMSRoleClaim);
        }
    }
}
