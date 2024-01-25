using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class CompTypeMstController : CommonController
    {
        private readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;

        public CompTypeMstController(IConfiguration configuration, IAPIService aPIService) : base(configuration, aPIService)
        {
            _configuration = configuration;
            _apiservice = aPIService;
        }

        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }
        public static List<CompTypMst> CompTypMsts = new List<CompTypMst>();

        public IActionResult CompTypeIndex(int CtmId, int MenuId)
        {
            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            if (CtmId > 0)
            {
                CompTypMst master = new CompTypMst();
                master = CompTypMsts.FirstOrDefault(x => x.CtmId == CtmId);
                if (master != null)
                {
                    return View(master);
                }
            }
            return View();
        }

        public async Task<PartialViewResult> _CompTypMstList()
        {
            ViewBag.btnAccControl = GetValues(iMenuId);
            GetBaseUrl();
            string sBaseUrl = sBU + "api/CompTypMst/GetCompTypMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    CompTypMsts = JsonConvert.DeserializeObject<List<CompTypMst>>(response.Result);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return PartialView("_CompTypMstList", CompTypMsts);
        }

        [HttpPost]
        public async Task<IActionResult> CompTypeIndex(CompTypMst CompTypMst)
        {
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = CompTypMst;
            if (ModelState.IsValid)
            {
                try
                {
                    string sBaseUrl = string.Empty;
                    CompTypMst.AddedBy = (sUserCode);
                    CompTypMst.AddedOn = DateTime.Now;
                    GetBaseUrl();
                    if (CompTypMst.CtmId > 0)
                    {
                        sBaseUrl = sBU + "api/CompTypMst/UpdateCompTypMst";
                    }
                    else
                    {
                        sBaseUrl = sBU + "api/CompTypMst/InsertCompTypMst";
                    }
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        if (response.Result == 1)
                        {
                            TempData["Success"] = response.Message;
                        }
                        return View(CompTypMst);
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View(CompTypMst);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

            }
            return View(CompTypMst);

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int CtmId)
        {
            CompTypMst master = new CompTypMst();
            master = CompTypMsts.FirstOrDefault(x => x.CtmId == CtmId);
            if (master != null)
            {
                master.AddedBy = (sUserCode);
                master.AddedOn = DateTime.Now;
                master.ActStatus = false;

                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = master;

                GetBaseUrl();
                string sBaseUrl = sBU + "api/CompTypMst/DeleteCompTypMst";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Result == 1)
                    {
                        TempData["Success"] = response.Message;
                    }
                    return RedirectToAction("CompTypeIndex");
                }
                else
                {
                    TempData["Error"] = response.Message;
                    return View(master);
                }
            }
            return View(master);
        }
    }
}
