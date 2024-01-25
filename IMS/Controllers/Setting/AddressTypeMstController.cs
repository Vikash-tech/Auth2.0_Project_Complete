using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class AddressTypeMstController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;
        public AddressTypeMstController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
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

        public static List<AddressType> AddressTypes = new List<AddressType>();

        public IActionResult AddressTypeIndex(int AtId, int MenuId)
        {
            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;

            ViewBag.btnAccControl = data;

            if (AtId > 0)
            {
                AddressType master = new AddressType();
                master = AddressTypes.FirstOrDefault(x => x.AtId == AtId);
                if (master != null)
                {
                    return View(master);
                }
            }
            return View();
        }

        public async Task<PartialViewResult> _AddressTypeList()
        {
            GetBaseUrl();
            string sBaseUrl = sBU + "api/AddressType/GetAddressType";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    AddressTypes = JsonConvert.DeserializeObject<List<AddressType>>(response.Result);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return PartialView("_AddressTypeList", AddressTypes);
        }

        [HttpPost]
        public async Task<IActionResult> AddressTypeIndex(AddressType AddressType)
        {
            APIResponse_IO aPI = new APIResponse_IO();
            aPI.Result = AddressType;

            try
            {
                if (ModelState.IsValid)
                {
                    string sBaseUrl = string.Empty;
                    AddressType.AddedBy = sUserCode;
                    GetBaseUrl();
                    if (AddressType.AtId > 0)
                    {
                        sBaseUrl = sBU + "api/AddressType/UpdateAddressType";
                    }
                    else
                    {
                        sBaseUrl = sBU + "api/AddressType/AddressTypeInsert";
                    }
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        if (response.Result == 1)
                        {
                            TempData["Success"] = response.Message;
                        }
                        else
                        {
                            TempData["Error"] = response.Message;
                        }
                        return View(AddressType);
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View(AddressType);
                    }
                }
                else
                {
                    return View(AddressType);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(AddressType);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int AtId)
        {
            AddressType master = new AddressType();
            master = AddressTypes.FirstOrDefault(x => x.AtId == AtId);
            if (master != null)
            {
                master.AddedBy = (sUserCode);
                master.AddedOn = DateTime.Now;
                master.ActStatus = false;

                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = master;

                GetBaseUrl();
                string sBaseUrl = sBU + "api/AddressType/DeleteAddressType";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction("AddressTypeIndex");
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
