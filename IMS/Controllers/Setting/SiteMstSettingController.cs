using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonDropDown.ViewModel;
using IMS_Model.CommonModel;
using IMS_Model.UserMaster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace IMS.Controllers.Setting
{
    public class SiteMstSettingController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int? iSiteId;
        public static int iMenuId;
        public SiteMstSettingController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
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


        public static SiteMaster master = new SiteMaster();
        public static SiteMst_vm vmsite = new SiteMst_vm();

        public static List<SiteMaster> SiteMasters = new List<SiteMaster>();
        public static List<SiteAddressMst> sAddressList = new List<SiteAddressMst>();
        public static List<SiteEmailMst> sEmailList = new List<SiteEmailMst>();
        public static List<SiteStoreMst> sStoreList = new List<SiteStoreMst>();
        public static List<SiteStatusMst> sStatusList = new List<SiteStatusMst>();

        public IActionResult Index(int Menuid)
        {
            if (Menuid == 0)
            {
                Menuid = iMenuId;
            }
            var data = GetValues(Menuid);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            return View();
        }

        public async Task<PartialViewResult> _SiteMstList()
        {
            var data = GetValues(iMenuId);
            ViewBag.btnAccControl = data;
            GetBaseUrl();
            string sBaseUrl = sBU + "api/SiteMaster/ReadAllSiteMaster";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                SiteMasters = JsonConvert.DeserializeObject<List<SiteMaster>>(response.Result);
            }
            return PartialView("_SiteMstList", SiteMasters);
        }

        public void Clear()
        {
            sAddressList.Clear();
            sEmailList.Clear();
            sStoreList.Clear();
        }

        public async Task<IActionResult> SiteCreation(int SiteId)
        {
            var data = GetValues(iMenuId);
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            vmsite.SiteMaster = null;
            Clear();
            if (SiteId > 0)
            {
                GetBaseUrl();

                string sBaseUrl = sBU + "api/SiteMaster/GetDataById?SiteId=" + SiteId;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    vmsite = JsonConvert.DeserializeObject<SiteMst_vm>(response.Result);
                    sAddressList = vmsite.siteAddressMst;
                    sEmailList = vmsite.siteEmailMst;
                    sStoreList = vmsite.siteStoreMst;
                    sStatusList = vmsite.sStatusList;
                    master = vmsite.SiteMaster;
                }
            }

            var CompMstList = await GetCompMst();
            vmsite.CompMstList = CompMstList.Select(x => new SelectListItem()
            {
                Value = x.CompId.ToString(),
                Text = x.CompDisp
            }).ToList();

            var SiteStatus = await param("SiteStatus");
            vmsite.SiteStatus = SiteStatus.Select(x => new SelectListItem()
            {
                Value = x.ParamValueTxt.ToString(),
                Text = x.ParamValDispTxt
            }).ToList();

            var yesno = await param("Yes/No");
            vmsite.AddYesNo = yesno.Select(x => new SelectListItem()
            {
                Value = x.ParamValueTxt.ToString(),
                Text = x.ParamValDispTxt
            }).ToList();

            var AddTypeList = await GetAddressTypeMst();
            vmsite.AddTypeList = AddTypeList.Select(x => new SelectListItem()
            {
                Value = x.AtId.ToString(),
                Text = x.AtDesc
            }).ToList();

            var Statelist = await GetStateMst();
            vmsite.Statelist = Statelist.Select(x => new SelectListItem()
            {
                Value = x.StId.ToString(),
                Text = x.StName
            }).ToList();

            var CountryList = await GetCountryMst();
            vmsite.CountryList = CountryList.Select(x => new SelectListItem()
            {
                Value = x.CtId.ToString(),
                Text = x.CtName,
            }).ToList();

            var EmailTypList = await GetEmailTyp();
            vmsite.EmailTypList = EmailTypList.Select(x => new SelectListItem()
            {
                Value = x.EID.ToString(),
                Text = x.EDesc,
            }).ToList();

            var CityMstList = await GetCityMst();
            vmsite.CityMstList = CityMstList.Select(x => new SelectListItem()
            {
                Value = x.CityId.ToString(),
                Text = x.CityDispName,
            }).ToList();

            return View(vmsite);
        }
        public async Task<IActionResult> ddlCityBind(int iStCode)
        {
            var CityMstList = await GetCityMst();
            var Selectedcity = CityMstList.Where(x => x.StCode == iStCode).ToList();
            return Json(Selectedcity);
        }

        public IActionResult _SiteAddressMst(int srno, int addressType, string addressLine1, string addressLine2, string addressLine3, int country, int state, string city, string pincode, string phone)
        {
            if (srno > 0)
            {
                sAddressList.Add(new SiteAddressMst()
                {
                    AddSrNo = srno,
                    SiteAddType = addressType,
                    Add1 = addressLine1,
                    Add2 = addressLine2,
                    Add3 = addressLine3,
                    CtId = country,
                    StId = state,
                    City = city,
                    SitePin = pincode,
                    PhoneNo = phone,
                    ActStatus = true,
                });
            }
            vmsite.siteAddressMst = sAddressList;

            return PartialView("_SiteAddressMst", vmsite);
        }
        public async Task<PartialViewResult> AddressUpdate(int AddSrNo, int SiteId, int addressType, string addressLine1, string addressLine2, string addressLine3, int country, int state, string city, string pincode, string phone)
        {
            if (AddSrNo > 0)
            {
                SiteAddressMst tmpSC = sAddressList.FirstOrDefault(i => i.AddSrNo == AddSrNo);

                if (tmpSC != null)
                {
                    tmpSC.AddSrNo = AddSrNo;
                    tmpSC.SiteId = SiteId;
                    tmpSC.SiteAddType = addressType;
                    tmpSC.Add1 = addressLine1;
                    tmpSC.Add2 = addressLine2;
                    tmpSC.Add3 = addressLine3;
                    tmpSC.CtId = country;
                    tmpSC.StId = state;
                    tmpSC.City = city;
                    tmpSC.SitePin = pincode;
                    tmpSC.PhoneNo = phone;
                    tmpSC.ActStatus = true;
                }
            }
            else
            {
                SiteAddressMst tmpSC = sAddressList.FirstOrDefault(i => i.SiteAddId == SiteId);

                if (tmpSC != null)
                {
                    tmpSC.AddSrNo = AddSrNo;
                    tmpSC.SiteId = SiteId;
                    tmpSC.SiteAddType = addressType;
                    tmpSC.Add1 = addressLine1;
                    tmpSC.Add2 = addressLine2;
                    tmpSC.Add3 = addressLine3;
                    tmpSC.CtId = country;
                    tmpSC.StId = state;
                    tmpSC.City = city;
                    tmpSC.SitePin = pincode;
                    tmpSC.PhoneNo = phone;
                    tmpSC.ActStatus = true;
                }
            }
            vmsite.siteAddressMst = sAddressList;
            return PartialView("_SiteAddressMst", vmsite);
        }
        public IActionResult SiteAddDelete(int AddSrNo, int SCAddressId)
        {

            if (AddSrNo > 0)
            {
                SiteAddressMst address = sAddressList.FirstOrDefault(i => i.AddSrNo == AddSrNo);
                if (address != null)
                {
                    sAddressList.Remove(address);
                    return PartialView("_SiteAddressMst", vmsite);
                }
            }
            else
            {
                SiteAddressMst address = sAddressList.FirstOrDefault(i => i.SiteAddId == SCAddressId);
                if (address != null)
                {
                    address.ActStatus = false;
                }
            }
            return PartialView("_SiteAddressMst", vmsite);
        }
        public IActionResult _SiteEmailMst(int emailsrno, int emailId, int emailType, string emailAddress, string personName, string mobileNumber)
        {
            if (emailsrno > 0)
            {
                sEmailList.Add(new SiteEmailMst()
                {
                    Srno = emailsrno,
                    SeId = emailId,
                    EmailId = emailAddress,
                    EmailType = emailType,
                    PersonName = personName,
                    MobileNo = mobileNumber,
                    ActStatus = true,
                });
            }
            vmsite.siteEmailMst = sEmailList;

            return PartialView("_SiteEmailMst", vmsite);
        }
        public IActionResult EmailUpdate(int emailsrno, int emailId, int emailType, string emailAddress, string personName, string mobileNumber)
        {
            if (emailsrno > 0)
            {
                SiteEmailMst tmpSC = sEmailList.FirstOrDefault(i => i.Srno == emailsrno);

                if (tmpSC != null)
                {
                    tmpSC.Srno = emailsrno;
                    tmpSC.SeId = emailId;
                    tmpSC.EmailId = emailAddress;
                    tmpSC.EmailType = emailType;
                    tmpSC.PersonName = personName;
                    tmpSC.MobileNo = mobileNumber;
                    tmpSC.ActStatus = true;
                }
            }
            else
            {
                SiteEmailMst tmpSC = sEmailList.FirstOrDefault(i => i.SeId == emailId);

                if (tmpSC != null)
                {
                    tmpSC.Srno = emailsrno;
                    tmpSC.SeId = emailId;
                    tmpSC.EmailId = emailAddress;
                    tmpSC.EmailType = emailType;
                    tmpSC.PersonName = personName;
                    tmpSC.MobileNo = mobileNumber;
                    tmpSC.ActStatus = true;
                }
            }
            vmsite.siteEmailMst = sEmailList;
            return PartialView("_SiteEmailMst", vmsite);
        }
        public IActionResult SiteEmailDelete(int AddSrNo, int EmailId)
        {

            if (AddSrNo > 0)
            {
                SiteEmailMst email = sEmailList.FirstOrDefault(i => i.Srno == AddSrNo);
                if (email != null)
                {
                    sEmailList.Remove(email);
                    return PartialView("_SiteEmailMst", vmsite);
                }
            }
            else
            {
                SiteEmailMst email = sEmailList.FirstOrDefault(i => i.SeId == EmailId);
                if (email != null)
                {
                    email.ActStatus = false;
                }
            }
            return PartialView("_SiteEmailMst", vmsite);
        }
        public IActionResult _SiteStoreMst(int StoreSrNO, string Storesite, int BulkStore, string Storecode, string StoreDes)
        {
            if (StoreSrNO > 0)
            {
                sStoreList.Add(new SiteStoreMst()
                {
                    SrNo = StoreSrNO,
                    SiteCode = Storesite,
                    StoreCode = Storecode,
                    StoreDesc = StoreDes,
                    iBulkSt = BulkStore,
                    ActStatus = 1,
                });
            }
            vmsite.siteStoreMst = sStoreList;

            return PartialView("_SiteStoreMst", vmsite);
        }
        public IActionResult StoreUpdate(int StoreSrNO, int StoreId, string Storesite, int BulkStore, string Storecode, string StoreDes)
        {
            if (StoreSrNO > 0)
            {
                SiteStoreMst tmpSC = sStoreList.FirstOrDefault(i => i.SrNo == StoreSrNO);

                if (tmpSC != null)
                {
                    tmpSC.SrNo = StoreSrNO;
                    tmpSC.SiteCode = Storesite;
                    tmpSC.StoreCode = Storecode;
                    tmpSC.StoreDesc = StoreDes;
                    tmpSC.iBulkSt = BulkStore;
                    tmpSC.ActStatus = 1;
                }
            }
            else
            {
                SiteStoreMst tmpSC = sStoreList.FirstOrDefault(i => i.StoreId == StoreId);

                if (tmpSC != null)
                {
                    tmpSC.StoreId = StoreId;
                    tmpSC.SiteCode = Storesite;
                    tmpSC.StoreCode = Storecode;
                    tmpSC.StoreDesc = StoreDes;
                    tmpSC.iBulkSt = BulkStore;
                    tmpSC.ActStatus = 1;

                }
            }
            vmsite.siteStoreMst = sStoreList;
            return PartialView("_SiteStoreMst", vmsite);
        }
        public IActionResult SiteStoreDelete(int AddSrNo, int StoreId)
        {

            if (AddSrNo > 0)
            {
                SiteStoreMst Store = sStoreList.FirstOrDefault(i => i.SrNo == AddSrNo);
                if (Store != null)
                {
                    sStoreList.Remove(Store);
                    return PartialView("_SiteStoreMst", vmsite);
                }
            }
            else
            {
                SiteStoreMst Store = sStoreList.FirstOrDefault(i => i.StoreId == StoreId);
                if (Store != null)
                {
                    Store.ActStatus = 0;
                }
            }
            return PartialView("_SiteStoreMst", vmsite);
        }
        public IActionResult _SiteStatusMst(int SRNo, string SiteCode, int Statuscode, DateTime Effecdt, string Remark)
        {
            if (SRNo > 0)
            {
                SiteStatusMst SiteStatus = new SiteStatusMst()
                {
                    SrNo = SRNo,
                    SiteCode = SiteCode,
                    StatusCode = Statuscode,
                    EffectiveDate = Effecdt,
                    Remarks = Remark,
                    ActStatus = true,
                };
                sStatusList.Add(SiteStatus);
            }
            vmsite.sStatusList = sStatusList;
            return PartialView("_SiteStatusMst", vmsite);
        }
        public IActionResult StatusUpdate(int SRNo, int StatusId, string SiteCode, int Statuscode, DateTime Effecdt, string Remark)
        {
            if (SRNo > 0)
            {
                SiteStatusMst tmpSC = sStatusList.FirstOrDefault(i => i.SrNo == SRNo);

                if (tmpSC != null)
                {
                    tmpSC.SrNo = SRNo;
                    tmpSC.SiteCode = SiteCode;
                    tmpSC.StatusCode = Statuscode;
                    tmpSC.EffectiveDate = Effecdt;
                    tmpSC.Remarks = Remark;
                    tmpSC.ActStatus = true;
                }
            }
            else
            {
                SiteStatusMst tmpSC = sStatusList.FirstOrDefault(i => i.SiteStatusId == StatusId);

                if (tmpSC != null)
                {
                    tmpSC.SrNo = SRNo;
                    tmpSC.SiteCode = SiteCode;
                    tmpSC.StatusCode = Statuscode;
                    tmpSC.EffectiveDate = Effecdt;
                    tmpSC.Remarks = Remark;
                    tmpSC.ActStatus = true;
                }

            }
            vmsite.sStatusList = sStatusList;
            return PartialView("_SiteStatusMst", vmsite);
        }
        public IActionResult SiteStatusDelete(int AddSrNo, int StatusId)
        {

            if (AddSrNo > 0)
            {
                SiteStatusMst Status = sStatusList.FirstOrDefault(i => i.SrNo == AddSrNo);
                if (Status != null)
                {
                    sStatusList.Remove(Status);
                    return PartialView("_SiteStatusMst", vmsite);
                }
            }
            else
            {
                SiteStatusMst Status = sStatusList.FirstOrDefault(i => i.SiteStatusId == StatusId);
                if (Status != null)
                {
                    Status.ActStatus = false;
                }
            }
            return PartialView("_SiteStatusMst", vmsite);
        }

        [HttpPost]
        public async Task<IActionResult> SiteCreation(SiteMst_vm vm)
        {
            try
            {

                if (vm != null)
                {
                    vm.SiteMaster.AddedBy = (sUserCode);

                    vm.siteAddressMst = sAddressList;
                    vm.siteEmailMst = sEmailList;
                    vm.siteStoreMst = sStoreList;
                    vm.sStatusList = sStatusList;
                }

                GetBaseUrl();
                APIResponse_IO APIsite = new APIResponse_IO();
                APIsite.Result = vm;

                string sBaseUrl = sBU + "api/SiteMaster/SiteMstInsert";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, APIsite);
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSiteCreation(SiteMst_vm vm)
        {
            try
            {
                if (vm != null)
                {
                    master.AddedBy = (sUserCode);
                    vm.SiteMaster = master;
                    vm.siteAddressMst = sAddressList;
                    vm.siteEmailMst = sEmailList;
                    vm.siteStoreMst = sStoreList;
                    vm.sStatusList = sStatusList;
                }

                GetBaseUrl();
                APIResponse_IO APIsite = new APIResponse_IO();
                APIsite.Result = vm;


                string sBaseUrl = sBU + "api/SiteMaster/UpdateSiteMaster";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, APIsite);
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
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
