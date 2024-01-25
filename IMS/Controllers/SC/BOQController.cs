using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IMS.Controllers.Common;
using IMS_Model.SC.BOQ;
using IMS_Model.CommonModel;
using IMS.Service;

namespace IMS.Controllers.SC
{
    public class BOQController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public static string? sUserCode;
        public static int? iSiteId;
        public static string Ucode = string.Empty;
        public static string Site = string.Empty;
        public static string RoleId = string.Empty;



        public BOQController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
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


        public static List<BOQGrp> BOQGrpsList = new List<BOQGrp>();
        public static List<VmBOQ> vmList = new List<VmBOQ>();
        public static List<BOQSubGrp> BOQSubGrpsList = new List<BOQSubGrp>();
        public static BOQGrp bOQGrp = new BOQGrp();


        public async Task<IActionResult> Index()
        {
            try
            {
                //RoleId = HttpContext.Session.GetString("RoleId");
                //Site = HttpContext.Session.GetString("SiteCode");

                Ucode = HttpContext.Session.GetString("UserCode");
                iSiteId = HttpContext.Session.GetInt32("SiteId");
                sUserCode = Ucode;


                var GrpsList = BOQGrpsList;
                ViewBag.Grplist = GrpsList;

                var myData = await GetTallyLedgers();
                ViewBag.GetTally = myData.Select(TallyLedger => TallyLedger.LedDispName).ToList();

                GetBaseUrl();
                string sBaseUrl = sBU + "api/SCBOQ/BOQGrpsList?";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BOQGrpsList = JsonConvert.DeserializeObject<List<BOQGrp>>(response.Result);
                    return View(BOQGrpsList);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }

            return View(BOQGrpsList);
        }

        public async Task<IActionResult> GroupCreate(string GrpDisc, string GrpDec)
        {
            BOQGrpsList.Clear();

            if (GrpDisc != null || GrpDec != null)
            {
                if (GrpDec == GrpDisc)
                {
                    TempData["success"] = ("Name", "Group Description And Group Display Name Cannot Be Same!");//Adding error to property
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    BOQGrp bOQGrp = new BOQGrp()
                    {
                        GrpDesc = GrpDec,
                        GrpDisp = GrpDisc,
                    };

                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQGrp };  

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/SCBOQ/InsertBOQ?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
                        return Json(message);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GroupEdit(int GrpID, string GrpDisc, string GrpDec)
        {
            if (GrpID > 0)
            {
                if (ModelState.IsValid)
                {
                    BOQGrp bOQGrp = new BOQGrp()
                    {
                        BOQGrpId = GrpID,
                        GrpDesc = GrpDec,
                        GrpDisp = GrpDisc,
                        AddedBy = Ucode,
                    };

                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQGrp };

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/SCBOQ/UpdateBOQ?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
                        return Json(message);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GroupDelete(int GrpID, string GrpDisc, string GrpDec)
        {
            if (GrpID > 0)
            {
                if (ModelState.IsValid)
                {
                    BOQGrp bOQGrp = new BOQGrp()
                    {
                        BOQGrpId = GrpID,
                        GrpDesc = GrpDec,
                        GrpDisp = GrpDisc,
                        AddedBy = Ucode,
                    };

                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQGrp };

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/SCBOQ/DeleteBOQ?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
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
                GetBaseUrl();
                string sBaseUrl = sBU + "api/SCBOQ/BOQSubGrpList?GrpID=" + Id;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BOQSubGrpsList = JsonConvert.DeserializeObject<List<BOQSubGrp>>(response.Result);
                    return Json(BOQSubGrpsList);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return Json(BOQSubGrpsList);
        }

        public async Task<IActionResult> InsertSubGrp(int GrpName, string GrpSubName, string GrpSubDis, string GrpTally)
        {
            try
            {
                if (GrpName > 0)
                {
                    BOQSubGrpsList.Clear();
                    BOQSubGrp bOQSubGrp = new BOQSubGrp()
                    {
                        GrpId = GrpName,
                        SubGrpDesc = GrpSubName,
                        SubGrpDisp = GrpSubDis,
                        TallyLedger = GrpTally,

                    };

                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQSubGrp };

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/SCBOQ/InsertSubGrp?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
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
            var sUBgrpselectedlist = BOQSubGrpsList.Where(m => m.BOQSubGrpId == SubGrpId).ToList();

            return Json(sUBgrpselectedlist);
        }

        public async Task<IActionResult> UpdateSubGrp(int SubGrpId, int GrpName, string GrpSubName, string GrpSubDis, string GrpTally)
        {
            try
            {
                if (SubGrpId > 0)
                {
                    BOQSubGrpsList.Clear();
                    BOQSubGrp bOQSubGrp = new BOQSubGrp()
                    {
                        BOQSubGrpId = SubGrpId,
                        GrpId = GrpName,
                        SubGrpDesc = GrpSubName,
                        SubGrpDisp = GrpSubDis,
                        TallyLedger = GrpTally,

                    };
                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQSubGrp };

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/SCBOQ/UpdateSubGrp?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
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
                    BOQSubGrpsList.Clear();
                    BOQSubGrp bOQSubGrp = new BOQSubGrp()
                    {
                        BOQSubGrpId = SubGrpId,
                    };

                    APIResponse_IO ApiPost = new APIResponse_IO() { Result = bOQSubGrp };

                    GetBaseUrl();
                    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                    string sBaseUrl = sBU + "api/SCBOQ/DeleteSubGrpQ?";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, ApiPost);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var message = response.Message;
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
