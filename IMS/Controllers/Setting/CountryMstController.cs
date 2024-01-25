using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class CountryMstController : CommonController
    {

        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;
        public CountryMstController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;
        }

        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        public static List<CountryMst> CountryMsts = new List<CountryMst>();
        public IActionResult CountryIndex(int Ctid, int MenuId)
        {
            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            if (Ctid > 0)
            {
                CountryMst master = new CountryMst();
                master = CountryMsts.FirstOrDefault(x => x.CtId == Ctid);
                if (master != null)
                {
                    return View(master);
                }
            }
            return View();
        }
        public async Task<PartialViewResult> _CountryMstList()
        {

            GetBaseUrl();
            string sBaseUrl = sBU + "api/CountryMaster/GetCountryMaster";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    CountryMsts = JsonConvert.DeserializeObject<List<CountryMst>>(response.Result);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return PartialView("_CountryMstList", CountryMsts);
        }


        [HttpPost]
        public async Task<IActionResult> CountryIndex(CountryMst countryMst)
        {
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = countryMst;
            if (ModelState.IsValid)
            {
                try
                {
                    string sBaseUrl = string.Empty;
                    countryMst.AddedBy = (sUserCode);
                    countryMst.AddedOn = DateTime.Now;
                    GetBaseUrl();
                    if (countryMst.CtId > 0)
                    {
                        sBaseUrl = sBU + "api/CountryMaster/UpdateCountryMst";
                    }
                    else
                    {
                        sBaseUrl = sBU + "api/CountryMaster/CountryMstInsert";
                    }
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        if (response.Result == 1)
                        {
                            TempData["Success"] = response.Message;
                        }
                        return View(countryMst);
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View(countryMst);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

            }
            return View(countryMst);

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Ctid)
        {
            CountryMst master = new CountryMst();
            master = CountryMsts.FirstOrDefault(x => x.CtId == Ctid);
            if (master != null)
            {
                master.AddedBy = (sUserCode);
                master.AddedOn = DateTime.Now;
                master.ActStatus = false;

                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = master;

                GetBaseUrl();
                string sBaseUrl = sBU + "api/CountryMaster/DeleteCountryMst";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("CountryIndex");
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
