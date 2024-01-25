using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonModel;
using IMS_Model.SettingModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class ClaimMstSettingController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;
        public ClaimMstSettingController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;
        }


        public static List<IMSClaimMaster> IMSClaimMasters = new List<IMSClaimMaster>();
        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        public IActionResult ClaimCreation(int AddtyId,int MenuId)
        {
            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;

            ViewBag.btnAccControl = data;

            if (AddtyId > 0)
            {
                IMSClaimMaster master = new IMSClaimMaster();
                master = IMSClaimMasters.FirstOrDefault(x => x.Id == AddtyId);
                if (master != null)
                {
                    return View(master);
                }
                else
                {
                    TempData["Error"] = "Error";
                }
            }
            return View();
        }

        public async Task<PartialViewResult> _ClaimsMstList()
        {
            try
            {

                GetBaseUrl();
                string sBaseUrl = sBU + "api/ClaimMaster/ReadAllClaimMaster";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    IMSClaimMasters = JsonConvert.DeserializeObject<List<IMSClaimMaster>>(response.Result);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return PartialView("_ClaimsMstList", IMSClaimMasters);
        }


        [HttpPost]
        public async Task<IActionResult> ClaimCreation(IMSClaimMaster ClaimMaster)
        {
            try
            {
                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = ClaimMaster;
                if (ModelState.IsValid)
                {
                    ClaimMaster.AddedBy = sUserCode;
                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/ClaimMaster/ClaimMstInsert";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Success"] = response.Message;
                        ClaimMaster = null;
                        return RedirectToAction("ClaimCreation");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(ClaimMaster);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(IMSClaimMaster ClaimMaster)
        {
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = ClaimMaster;
            if (ModelState.IsValid)
            {
                ClaimMaster.AddedBy = sUserCode;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/ClaimMaster/ClaimMstUpdate";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("ClaimCreation");
                }
                else
                {
                    TempData["Error"] = response.Message;
                    return View(ClaimMaster);
                }
            }
            return View(ClaimMaster);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            IMSClaimMaster master = new IMSClaimMaster();
            master = IMSClaimMasters.FirstOrDefault(x => x.Id == id);
            try
            {
                if (master != null)
                {
                    master.AddedBy = sUserCode;
                    master.ActStatus = false;

                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = master;

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/ClaimMaster/ClaimMstDelete";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Success"] = response.Message;
                        return RedirectToAction("ClaimCreation");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "No Data Available..";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(master);
            }
        }
    }
}

