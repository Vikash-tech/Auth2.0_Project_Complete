using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class StateMstController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int? iSiteId;
        public static int iMenuId = -1;
        public StateMstController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;
        }
        public static StateMst master = new StateMst();
        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        public async Task<IActionResult> Index(int MenuId)
        {
            var countryEnumList = await GetCountryMst();
            ViewBag.country = countryEnumList;

            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            return View();
        }

        public static List<StateMst> stateMstslist = new List<StateMst>();

        public async Task<PartialViewResult> _StateMstList()
        {
            GetBaseUrl();
            string sBaseUrl = sBU + "api/StateMst/GetStateMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                stateMstslist = JsonConvert.DeserializeObject<List<StateMst>>(response.Result);
                stateMstslist= stateMstslist.Where(x=>x.ActStatus==true).ToList();
            }
            return PartialView("_StateMstList", stateMstslist);
        }


        public async Task<IActionResult> StateCreation()
        {
            var countryEnumList = await GetCountryMst();
            ViewBag.country = countryEnumList;

            var data = GetValues(iMenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            return View(master);
        }

        [HttpPost]
        public async Task<IActionResult> StateCreation(StateMst state)
        {
            APIResponse_IO aPI = new APIResponse_IO();


            if (ModelState.IsValid)
            {
                state.AddedBy = sUserCode;
                state.ActStatus = true;
                aPI.Result = state;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/StateMst/InsertStateMst";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = response.Message;
                    return View(state);
                }
            }
            return View(state);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var countryEnumList = await GetCountryMst();
            ViewBag.country = countryEnumList;

            master = stateMstslist.FirstOrDefault(x => x.StId == id);
            if (master != null)
            {
                return RedirectToAction("StateCreation", master);
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Edit(StateMst state)
        {
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = state;


            if (ModelState.IsValid)
            {
                state.AddedBy = (sUserCode);
                GetBaseUrl();

                string sBaseUrl = sBU + "api/StateMst/UpdateStateMst";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = response.Message;
                    return View(state);
                }
            }
            return View(state);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            StateMst master = new StateMst();
            master = stateMstslist.FirstOrDefault(x => x.StId == id);

            master.AddedBy = (sUserCode);
            master.AddedOn = DateTime.Now;
            master.ActStatus = false;
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = master;
            GetBaseUrl();
            string sBaseUrl = sBU + "api/StateMst/DeleteStateMst";
            APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = response.Message;
                return View();
            }

        }
    }
}
