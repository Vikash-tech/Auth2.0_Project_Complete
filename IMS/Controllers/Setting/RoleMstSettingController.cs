using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model;
using IMS_Model.CommonDropDown.ViewModel;
using IMS_Model.CommonModel;
using IMS_Model.SettingModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IMS.Controllers.Setting
{
    public class RoleMstSettingController : CommonController
    {

        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;

        public static List<IMSRoleMaster> IMSRoleMasters = new List<IMSRoleMaster>();
        public static ViewRoleMaster_VM master_VM = new ViewRoleMaster_VM();
        public static List<IMSClaimMaster> IMSClaimMasters = new List<IMSClaimMaster>();
        public static List<TMenuMst> MenuItems = new List<TMenuMst>();
        List<IMSRoleClaimMaster> IMSClaimlist = new List<IMSRoleClaimMaster>();

        public static string SuccessMsg, ErrorMsg = string.Empty;

        public RoleMstSettingController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
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

        public async Task<IActionResult> Index(int MenuId)
        {
            try
            {
                if (MenuId == 0)
                {
                    MenuId = iMenuId;
                }
                var data = GetValues(MenuId);
                iMenuId = data.iMenuId;
                sUserCode = data.sUserCode;
                ViewBag.btnAccControl = data;

                if (!string.IsNullOrEmpty(SuccessMsg))
                {
                    TempData["Success"] = SuccessMsg;
                    SuccessMsg = string.Empty;
                }
                if (!string.IsNullOrEmpty(ErrorMsg))
                {
                    TempData["Error"] = ErrorMsg;
                    ErrorMsg = string.Empty;
                }
            }
            catch (Exception ex)
            {
                var error = await ProjLogException(ex);
                TempData["Error"] = error;
            }
            return View();
        }

        public async Task<PartialViewResult> _RoleMstList()
        {
            try
            {
                GetBaseUrl();
                string sBaseUrl = sBU + "api/RoleMaster/ReadAllRoleMaster";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    IMSRoleMasters = JsonConvert.DeserializeObject<List<IMSRoleMaster>>(response.Result);
                }
            }
            catch (Exception ex)
            {
                var result = ProjLogException(ex);
            }
            return PartialView("_RoleMstList", IMSRoleMasters);
        }

        //For getting data using html Action Link
        public static Guid UpdateId;
        public async Task<IActionResult> GetDatabyId(Guid Id, string EditId)
        {
            UpdateId = Id;
            GetBaseUrl();
            string sBaseUrl = sBU + "api/RoleMaster/GetRoleClamisDtlById?Id=" + Id;
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                master_VM = JsonConvert.DeserializeObject<ViewRoleMaster_VM>(response.Result);
            }

            return RedirectToAction("RoleMasterCreation");
        }

        //For ReFreshing page
        public ActionResult ReFreshPage()
        {
            master_VM.IMSRoleMaster = null;
            if (master_VM.CheckClaims != null)
            {
                master_VM.CheckClaims.Clear();
            }
            master_VM.IMSRoleClaimMasters.Clear();
            master_VM.RoleClaimMaster = null;

            return RedirectToAction("RoleMasterCreation");
        }

        public async Task<IActionResult> RoleMasterCreation(Guid Id, string EditId, string Add)
        {
          
            var data = GetValues(iMenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            if (!string.IsNullOrEmpty(EditId))
            {
                UpdateId = Id;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/RoleMaster/GetRoleClamisDtlById?Id=" + Id;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    master_VM = JsonConvert.DeserializeObject<ViewRoleMaster_VM>(response.Result);
                }
            }
            //dropdown binding
            IMSClaimMasters = await GetAllClaims();
            master_VM.IMSClaims = IMSClaimMasters;

            //dropdown binding
            MenuItems = await GetAllMenuMst();
            master_VM.TMenuMsts = MenuItems;

            if (!string.IsNullOrEmpty(Add))
            {
                ViewRoleMaster_VM vm = new ViewRoleMaster_VM();
                vm.IMSClaims = IMSClaimMasters;
                vm.TMenuMsts = MenuItems;
                return View(vm);
            }
            return View(master_VM);
        }

        [HttpPost]
        public async Task<IActionResult> RoleMasterCreation(IFormCollection fc, ViewRoleMaster_VM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    APIResponse_IO aPI = new APIResponse_IO();

                    IMSRoleMaster iMSRole = new IMSRoleMaster();
                    List<ClaimsToRole> getlist = new List<ClaimsToRole>();

                    for (int i = 0; i < fc.Count; i++)
                    {
                        string key = fc.Keys.ElementAt(i);
                        string value = fc[key];
                        if (key.EndsWith("-true") && value == "true")
                        {
                            string claimType = key.Split('.')[0];
                            string[] parts = claimType.Split('-');
                            if (parts.Length == 4)
                            {
                                ClaimsToRole class1 = new ClaimsToRole();
                                class1.MenuId = Convert.ToInt32(parts[0]);
                                class1.ClaimType = parts[1];
                                class1.ClaimValue = parts[2];
                                class1.IsChecked = Convert.ToBoolean(parts[3]);
                                getlist.Add(class1);
                            }
                        }
                    }

                    List<ClaimsToRole> getlists = new List<ClaimsToRole>();

                    int fund = -1;

                    foreach (var menu in MenuItems)
                    {
                        foreach (var claim in getlist)
                        {
                            if (menu.MenuId == claim.MenuId)
                            {
                                if (fund != menu.MenuId)
                                {
                                    if (claim.ClaimType != "IsGet")
                                    {
                                        if (!getlist.Any(c => c.MenuId == menu.MenuId && c.ClaimType == "IsGet"))
                                        {
                                            fund = menu.MenuId;

                                            ClaimsToRole class1 = new ClaimsToRole();
                                            class1.MenuId = claim.MenuId;
                                            class1.ClaimType = "IsGet";
                                            class1.ClaimValue = "IsGet";
                                            class1.IsChecked = true;
                                            getlists.Add(class1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    getlist = getlist.Concat(getlists).ToList();

                    if (getlist.Count > 0 && getlist != null)
                    {
                        string sBaseUrl = string.Empty;
                        GetBaseUrl();
                        if (vm.IMSRoleMaster.AddedBy == null)
                        {
                            sBaseUrl = sBU + "api/RoleMaster/RoleClaimsMstInsert";
                            iMSRole.Name = vm.IMSRoleMaster.Name;
                            iMSRole.NormalizedName = vm.IMSRoleMaster.NormalizedName;
                            iMSRole.LockPeriod = vm.IMSRoleMaster.LockPeriod;
                            iMSRole.LockPeriodAdd = vm.IMSRoleMaster.LockPeriodAdd;
                            iMSRole.ConcurrencyStamp = vm.IMSRoleMaster.ConcurrencyStamp;
                            iMSRole.Id = vm.IMSRoleMaster.Id;
                            iMSRole.AddedBy = sUserCode;
                            iMSRole.ActStatus = true;
                            iMSRole.AddedOn = DateTime.Now;
                            vm.IMSRoleMaster = iMSRole;
                        }
                        else
                        {
                            sBaseUrl = sBU + "api/RoleMaster/RoleClaimsMstUpdate";
                            iMSRole = vm.IMSRoleMaster;
                            iMSRole.AddedBy = sUserCode;
                            iMSRole.ActStatus = true;
                            vm.IMSRoleMaster = iMSRole;
                        }

                        vm.CheckClaims = getlist;
                        aPI.Result = vm;
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
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = response.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Claims Cannot be Blanks";

                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            vm.IMSClaims = await GetAllClaims();
            vm.TMenuMsts = await GetAllMenuMst();
            return View(vm);
        }

        //for AJX currently not in use
        public async Task<IActionResult> RoleMasterCreationpost(List<ClaimsToRole> SelectedClaims, string iRoleName, string NorRoleName, int iLockPrd, int iLockPrdAdd, string iCon)
        {
            try
            {
                IMSRoleMaster iMSRole = new IMSRoleMaster();

                iMSRole.Name = iRoleName;
                iMSRole.NormalizedName = NorRoleName;
                iMSRole.LockPeriod = iLockPrd;
                iMSRole.LockPeriodAdd = iLockPrdAdd;
                iMSRole.ConcurrencyStamp = iCon;
                iMSRole.AddedBy = sUserCode;
                iMSRole.ActStatus = true;
                iMSRole.AddedOn = DateTime.Now;

                List<ClaimsToRole> Selectedtrue = new List<ClaimsToRole>();
                Selectedtrue = SelectedClaims.Where(x => x.IsChecked == true).ToList();

                ViewRoleMaster_VM viewRole = new ViewRoleMaster_VM();
                viewRole.IMSRoleMaster = iMSRole;
                viewRole.CheckClaims = Selectedtrue;

                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = viewRole;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/RoleMaster/RoleClaimsMstInsert";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SuccessMsg = response.Message;
                }
                else
                {
                    ErrorMsg = response.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return Json(ErrorMsg);
        }
        public async Task<IActionResult> RoleMasterUpdate(List<ClaimsToRole> SelectedClaims, string iRoleName, string NorRoleName, int iLockPrd, int iLockPrdAdd, string iCon)
        {
            try
            {
                IMSRoleMaster iMSRole = new IMSRoleMaster();
                iMSRole.Name = iRoleName;
                iMSRole.Id = UpdateId;
                iMSRole.NormalizedName = NorRoleName;
                iMSRole.LockPeriod = iLockPrd;
                iMSRole.LockPeriodAdd = iLockPrdAdd;
                iMSRole.ConcurrencyStamp = iCon;
                iMSRole.AddedBy = sUserCode;
                iMSRole.ActStatus = true;
                iMSRole.AddedOn = DateTime.Now;

                List<ClaimsToRole> Selectedtrue = new List<ClaimsToRole>();
                Selectedtrue = SelectedClaims.Where(x => x.IsChecked == true).ToList();

                ViewRoleMaster_VM viewRole = new ViewRoleMaster_VM();
                viewRole.IMSRoleMaster = iMSRole;
                viewRole.CheckClaims = Selectedtrue;

                APIResponse_IO aPI = new APIResponse_IO();
                aPI.Result = viewRole;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/RoleMaster/RoleClaimsMstUpdate";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SuccessMsg = response.Message;
                }
                else
                {
                    ErrorMsg = response.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
            return Json(ErrorMsg);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                IMSRoleMaster master = new IMSRoleMaster();
                master = IMSRoleMasters.FirstOrDefault(x => x.Id == Id);
                if (master != null)
                {
                    master.AddedBy = sUserCode;
                    master.AddedOn = DateTime.Now;
                    master.ActStatus = false;

                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = master;

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/RoleMaster/RoleClaimsMstDelete";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Success"] = response.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
