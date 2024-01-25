using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class ParameterMstController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int? iSiteId;
        public static int iMenuId;
        public ParameterMstController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
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

        public static List<ParameterMst> parameterMstslist = new List<ParameterMst>();
        public static List<ParameterVal> parameterVallist = new List<ParameterVal>();
        public static ParameterMst ParaMst = new ParameterMst();

        public async Task<IActionResult> Index(int Menuid)
        {
            try
            {
                sUserCode = HttpContext.Session.GetString("UserCode");
                iSiteId = HttpContext.Session.GetInt32("SiteId");
                iMenuId = Menuid;

                ViewBag.ParaMst = parameterMstslist;

                GetBaseUrl();
                string sBaseUrl = sBU + "GetParameterMst";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    parameterMstslist = JsonConvert.DeserializeObject<List<ParameterMst>>(response.Result);
                    return View(parameterMstslist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }

            return View(parameterMstslist);
        }




        public async Task<IActionResult> GroupCreate(string Paramcd, string GrpDisc, string GrpDec)
        {

            if (GrpDisc != null || GrpDec != null)
            {
                if (GrpDec == GrpDisc)
                {
                    TempData["success"] = ("Name", "Group Description And Group Display Name Cannot Be Same!");
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    ParameterMst ParaMst = new ParameterMst()
                    {
                        ParamCd = Paramcd,
                        ParamDispTxt = GrpDisc,
                        ParamDesc = GrpDec,
                        AddedBy = sUserCode,
                        ActStatus = true,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = ParaMst;
                    GetBaseUrl();
                    string sBaseUrl = sBU + "InsertParameterMst";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GroupEdit(string Paramcd, int GrpID, string GrpDisc, string GrpDec)
        {
            if (GrpID > 0)
            {
                if (ModelState.IsValid)
                {

                    ParameterMst ParaUpdate = new ParameterMst()
                    {
                        ParamId = GrpID,
                        ParamCd = Paramcd,
                        ParamDispTxt = GrpDisc,
                        ParamDesc = GrpDec,
                        AddedBy =sUserCode,
                        ActStatus = true,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = ParaUpdate;
                    GetBaseUrl();
                    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                    string sBaseUrl = sBU + "UpdateParameterMst";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GroupDelete(string Paramcd, int GrpID, string GrpDisc, string GrpDec)
        {
            if (GrpID > 0)
            {
                if (ModelState.IsValid)
                {
                    ParameterMst ParaDelete = new ParameterMst()
                    {
                        ParamId = GrpID,
                        ParamDispTxt = GrpDisc,
                        ParamCd = Paramcd,
                        ParamDesc = GrpDec,
                        ActStatus = false,
                        AddedBy = sUserCode,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = ParaDelete;
                    GetBaseUrl();
                    string sBaseUrl = sBU + "DeleteParameterMst";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ChildDatabyID(string Id)
        {
            try
            {
                ParameterMst parameter = new ParameterMst();
                if (Convert.ToInt32(Id) > 0)
                {
                    parameter = parameterMstslist.FirstOrDefault(x => x.ParamId == Convert.ToInt32(Id));
                }

                GetBaseUrl();
                string sBaseUrl = sBU + "GetParameterVal";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    parameterVallist = JsonConvert.DeserializeObject<List<ParameterVal>>(response.Result);
                    if (parameterVallist.Count > 0)
                    {
                        var list = parameterVallist.Where(x => x.ParamCd == parameter.ParamCd);
                        return Json(list);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return Json(parameterVallist);
        }
        public async Task<IActionResult> InsertSubGrp(string GrpName, int GrpSubName, string GrpSubDis)
        {
            try
            {
                if (GrpName != null)
                {
                    ParameterVal bOQSubGrp = new ParameterVal()
                    {
                        ParamCd = GrpName,
                        ParamValueTxt = Convert.ToInt32(GrpSubName),
                        ParamValDispTxt = GrpSubDis,
                        AddedBy = sUserCode,
                        ActStatus = true,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = bOQSubGrp;
                    GetBaseUrl();
                    string sBaseUrl = sBU + "InsertParameterVal";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetSubGroupData(int SubGrpId)
        {

            var sUBgrpselectedlist = parameterVallist.Where(m => m.ParamValId == SubGrpId).ToList();
            return Json(sUBgrpselectedlist);
        }


        public async Task<IActionResult> UpdateSubGrp(int SubGrpId, string GrpName, int GrpSubName, string GrpSubDis)
        {
            try
            {
                if (SubGrpId > 0)
                {
                    ParameterVal bOQSubGrp = new ParameterVal()
                    {
                        ParamValId = SubGrpId,
                        ParamCd = GrpName,
                        ParamValueTxt = GrpSubName,
                        ParamValDispTxt = GrpSubDis,
                        AddedBy = sUserCode,
                        ActStatus = true,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = bOQSubGrp;
                    GetBaseUrl();
                    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                    string sBaseUrl = sBU + "UpdateParameterVal";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteSubGrp(int SubGrpId)
        {
            try
            {
                if (SubGrpId > 0)
                {
                    ParameterVal bOQSubGrp = new ParameterVal()
                    {
                        ParamValId = SubGrpId,
                        ActStatus = false,
                        AddedBy = sUserCode,
                        AddedOn = DateTime.Now,
                    };
                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = bOQSubGrp;
                    GetBaseUrl();
                    string sBaseUrl = sBU + "DeleteParameterVal";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Result;
                        return Json(message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}

