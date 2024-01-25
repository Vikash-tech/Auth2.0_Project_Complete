using IMS.Models;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonModel;
using IMS_Model.LoginModel;
using IMS_Model.SC.BOQ;
using IMS_Model.SettingModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;

namespace IMS.Controllers.Common
{
    public class CommonController : Controller
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public static string? sUserCode;
        public static int? iSiteId;

        //List<GSTRateMst> GSTRateMsts = new List<GSTRateMst>();
        //public static List<SiteMst> sitemstdata = new List<SiteMst>();

        public CommonController(IConfiguration configuration, IAPIService aPTService)
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

        // For Getting the Site Dropdown based on User Code 
        public async Task<List<IMSRoleMaster>> RoleMasterList()
        {
            List<IMSRoleMaster> RoleList = new List<IMSRoleMaster>();
            GetBaseUrl();
            string sBaseUrl = sBU + "GetRoleMstList";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                RoleList = JsonConvert.DeserializeObject<List<IMSRoleMaster>>(response.Result);
            }
            return RoleList.ToList();
        }

        // For Getting the Site Dropdown 
        public async Task<List<SiteMst>> AllSiteMstList()
        {
            List<SiteMst> sitelist = new List<SiteMst>();
            GetBaseUrl();
            string sBaseUrl = sBU + "GetSiteCode?sUsrCode=" + "" + "";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                sitelist = JsonConvert.DeserializeObject<List<SiteMst>>(response.Result);
                var mess = response.Message;
            }
            return sitelist.ToList();
        }


        public GetValuesForButtons GetValues(int iMenuId)
        {
            GetValuesForButtons values = new GetValuesForButtons()
            {
                 sUserCode = HttpContext.Session.GetString("UserCode"),
                iSiteId = (int)HttpContext.Session.GetInt32("SiteId"),
                iMenuId = iMenuId
            };
            return values;
        }


        public async Task<string> ProjLogException(Exception ex)
        {
            string mess = string.Empty;
            try
            {
                ExceptionLogModel logModel = new ExceptionLogModel(ex);
                APIResponse_IO api = new APIResponse_IO();
                api.Result = logModel;
                GetBaseUrl();
                string sBaseUrl = sBU + "InsertLog?";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, api);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    api = JsonConvert.DeserializeObject<APIResponse_IO>(response.Result);
                    if (api.Result == 1)
                    {
                        mess = api.Message;
                    }
                }
                else
                {
                    mess = api.Message;
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error logging exception: {logEx.Message}");
            }
            return mess;
        }


        //main menu
        public async Task<IActionResult> _MenuList()
        {
            sUserCode = HttpContext.Session.GetString("UserCode");
            iSiteId = HttpContext.Session.GetInt32("SiteId");
            GetBaseUrl();
            string sBaseUrl = sBU + "api/MenuManagement/GetMenu?UserCode=" + sUserCode + "&SiteId=" + iSiteId + "";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            List<MenuItem> menuItem = new List<MenuItem>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                menuItem = JsonConvert.DeserializeObject<List<MenuItem>>(response.Result);
                if (menuItem.Count > 0)
                {
                    menuItem = menuItem.Where(x => x.MenuTxt != "Setting").ToList();
                }
            }
            return PartialView("_MenuList", menuItem);
        }


        //Setting menu
        public async Task<IActionResult> _SettingMenuList()
        {
            sUserCode = HttpContext.Session.GetString("UserCode");
            GetBaseUrl();
            string sBaseUrl = sBU + "api/MenuManagement/GetMenu?UserCode=" + sUserCode + "&SiteId=16";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            List<MenuItem> SeettingmenuItem = new List<MenuItem>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                SeettingmenuItem = JsonConvert.DeserializeObject<List<MenuItem>>(response.Result);
                if (SeettingmenuItem.Count > 0)
                {
                    SeettingmenuItem = SeettingmenuItem.Where(x => x.MenuTxt == "Setting").ToList();
                }
            }
            return PartialView("_SettingMenuList", SeettingmenuItem);
        }


        //For Getting User Specific Claims for BUTTON HANDLING 
        public async Task<IActionResult> GetUserMenuAcce(string UserCode, int SiteId, int MenuId)
        {
            try
            {
                sUserCode = HttpContext.Session.GetString("UserCode");
                var siteid = HttpContext.Session.GetInt32("SiteId");
                if (string.IsNullOrEmpty(UserCode) && SiteId == 0 && MenuId == 0)
                {
                    MenuId = 200;
                    UserCode = sUserCode;
                    SiteId = 16;
                }

                GetBaseUrl();

                // string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/UserAuth/GetUserMenuAccess?UserCode=" + UserCode + "&SiteId=" + SiteId + "&MenuId=" + MenuId + "";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                UserAccessInMenu userAccessInMenu = new UserAccessInMenu();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    userAccessInMenu = JsonConvert.DeserializeObject<UserAccessInMenu>(response.Result);
                    if (userAccessInMenu != null)
                    {
                        if (userAccessInMenu.IsGet == true)
                        {
                            return Json(userAccessInMenu);
                        }
                    }
                }
                return Json(userAccessInMenu);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // For Getting the Site Dropdown based on User Code 
        public async Task<List<SiteMst>> siteMsts(string sUsrCode)
        {
            List<SiteMst> sitelist = new List<SiteMst>();
            GetBaseUrl();
            string sBaseUrl = sBU + "GetSiteCode?sUsrCode=" + sUsrCode + "";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                sitelist = JsonConvert.DeserializeObject<List<SiteMst>>(response.Result);
                var mess = response.Message;
            }
            return sitelist.ToList();
        }

        //Common Method for Fetching the Paramaster Table Value for Dropdown
        public async Task<List<ParameterVal>> param(string sParamCd)
        {
            List<ParameterVal> ParameterVals = new List<ParameterVal>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetCommonDDLBind?sParamCode=" + sParamCd;
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ParameterVals = JsonConvert.DeserializeObject<List<ParameterVal>>(response.Result);
            }
            return ParameterVals.ToList();

        }

        //For Getting the Company type dropdown
        public async Task<List<CompTypMst>> GetCompTypeMst()
        {
            List<CompTypMst> CompTypMst = new List<CompTypMst>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetCompTypeMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CompTypMst = JsonConvert.DeserializeObject<List<CompTypMst>>(response.Result);
            }
            return CompTypMst.ToList();

        }

        //For Getting the Address type dropdown
        public async Task<List<AddressType>> GetAddressTypeMst()
        {
            List<AddressType> AddressTypes = new List<AddressType>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetAddressTypeMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AddressTypes = JsonConvert.DeserializeObject<List<AddressType>>(response.Result);
            }
            return AddressTypes.ToList();

        }

        //For Getting the State Master dropdown
        public async Task<List<StateMst>> GetStateMst()
        {
            List<StateMst> statelist = new List<StateMst>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetStateMstlist";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                statelist = JsonConvert.DeserializeObject<List<StateMst>>(response.Result);
            }
            return statelist.ToList();

        }

        //For Getting the Country Master dropdown

        public async Task<List<CountryMst>> GetCountryMst()
        {
            List<CountryMst> CountryMsts = new List<CountryMst>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetCountryMstlist";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CountryMsts = JsonConvert.DeserializeObject<List<CountryMst>>(response.Result);
            }
            return CountryMsts.ToList();

        }



        public async Task<List<CompMst>> GetCompMst()
        {
            List<CompMst> CompMsts = new List<CompMst>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetCompMstDDl";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CompMsts = JsonConvert.DeserializeObject<List<CompMst>>(response.Result);
            }
            return CompMsts.ToList();

        }


        public async Task<List<IMSClaimMaster>> GetAllClaims()
        {
            List<IMSClaimMaster> ClaimMaster = new List<IMSClaimMaster>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetAllClaimsMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ClaimMaster = JsonConvert.DeserializeObject<List<IMSClaimMaster>>(response.Result);
            }
            return ClaimMaster.ToList();

        }

        public async Task<List<TMenuMst>> GetAllMenuMst()
        {
            List<TMenuMst> TMenuMst = new List<TMenuMst>();
            GetBaseUrl();

            //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetMenuMst";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TMenuMst = JsonConvert.DeserializeObject<List<TMenuMst>>(response.Result);
            }
            return TMenuMst.ToList();

        }




        public async Task<List<EmailTyp>> GetEmailTyp()
        {
            List<EmailTyp> emaillist = new List<EmailTyp>();
            GetBaseUrl();
            string sBaseUrl = sBU + "GetEmailTyp?";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                emaillist = JsonConvert.DeserializeObject<List<EmailTyp>>(response.Result);
            }
            return emaillist.ToList();
        }


        public async Task<List<CityMst>> GetCityMst()
        {
            List<CityMst> CityMstList = new List<CityMst>();
            GetBaseUrl();
            string sBaseUrl = sBU + "GetCityMst?";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CityMstList = JsonConvert.DeserializeObject<List<CityMst>>(response.Result);
            }
            return CityMstList.ToList();
        }

















        //public ActionResult GETSITEData(int iSiteID)
        //{
        //    var sitedata = sitemstdata.Find(a => a.SiteId == iSiteID);

        //    HttpContext.Session.SetString("SiteCode", sitedata.SiteCode.ToString().Trim());
        //   // HttpContext.Session.SetString("SiteId", sitedata.SiteId.ToString().Trim());

        //    return Json(sitedata);
        //}

        //public async Task<List<GSTRateMst>> GST()
        //{
        //    GetBaseUrl();
        //    sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
        //    iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetGSTDetails";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        GSTRateMsts = JsonConvert.DeserializeObject<List<GSTRateMst>>(response.Result);
        //    }
        //    return GSTRateMsts.ToList();

        //}
        //public async Task<List<Store>> Store(string sSiteCode)
        //{
        //    List<Store> Store = new List<Store>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetStoreCode" + "?sSiteCode=" + sSiteCode;
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        Store = JsonConvert.DeserializeObject<List<Store>>(response.Result);
        //    }
        //    return Store.ToList();

        //}

        //

        //public async Task<List<StoreLoc>> StoreLoc(int iStoreId)
        //{
        //    List<StoreLoc> StoreLoc = new List<StoreLoc>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetStoreLoc?iStoreId=" + iStoreId + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        StoreLoc = JsonConvert.DeserializeObject<List<StoreLoc>>(response.Result);
        //    }
        //    return StoreLoc.ToList();

        //}
        //public async Task<List<ItemMst>> OthItemMst()
        //{
        //    List<ItemMst> itemMsts = new List<ItemMst>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetOtherItem";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        itemMsts = JsonConvert.DeserializeObject<List<ItemMst>>(response.Result);
        //    }
        //    // return itemMsts.Where(x => x.MtrlGrp == 4).ToList();
        //    return itemMsts.ToList();

        //}
        //public async Task<List<ItemMst>> ItemMst(int iItemId)
        //{
        //    List<ItemMst> itemMsts = new List<ItemMst>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetItem?iItemId=" + iItemId + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        itemMsts = JsonConvert.DeserializeObject<List<ItemMst>>(response.Result);
        //    }
        //    return itemMsts.ToList();

        //}
        public async Task<List<CompTypMst>> DDcompTyps()
        {
            List<CompTypMst> compty = new List<CompTypMst>();
            GetBaseUrl();
            // sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
            iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));

            string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "api/SC/DDcompTyps";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                compty = JsonConvert.DeserializeObject<List<CompTypMst>>(response.Result);
            }
            return compty.ToList();
        }


        public async Task<List<StateMst>> GetStateMstlist()
        {
            List<StateMst> states = new List<StateMst>();
            GetBaseUrl();
            //sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
            iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));

            string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "api/SC/GetStateMstlist";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                states = JsonConvert.DeserializeObject<List<StateMst>>(response.Result);
            }
            return states.ToList();

        }
        public async Task<List<Bankmst>> GetBankmsts()
        {
            List<Bankmst> bank = new List<Bankmst>();

            GetBaseUrl();
            // sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
            iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));

            string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "GetBankmsts";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                bank = JsonConvert.DeserializeObject<List<Bankmst>>(response.Result);
            }
            return bank.ToList();
        }

        public async Task<List<TallyLedger>> GetTallyLedgers()
        {
            List<TallyLedger> TallyLedgerList = new List<TallyLedger>();
            GetBaseUrl();
            //sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
            iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));
            if (string.IsNullOrEmpty(sUserCode))
            {
                sUserCode = "10698";
                iSiteId = 16;
            }

            string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
            string sBaseUrl = sBU + "api/SCBOQ/TallyLedgerList?";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TallyLedgerList = JsonConvert.DeserializeObject<List<TallyLedger>>(response.Result);
            }
            return TallyLedgerList.ToList();
        }

        //public async Task<List<BOQGrp>> BOQgrpList()
        //{
        //    List<BOQGrp> BOQGrpsList = new List<BOQGrp>();
        //    GetBaseUrl();
        //    sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
        //    iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }

        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "api/SCBOQ/BOQGrpsList?";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        BOQGrpsList = JsonConvert.DeserializeObject<List<BOQGrp>>(response.Result);
        //    }
        //    return BOQGrpsList.ToList();
        //}

        //public async Task<List<TDS>> TDS()
        //{
        //    List<TDS> TDS = new List<TDS>();
        //    GetBaseUrl();
        //    sUserCode = Convert.ToString(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("UId"));
        //    iSiteId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("SiteId"));
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetTDS";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        TDS = JsonConvert.DeserializeObject<List<TDS>>(response.Result);
        //    }
        //    return TDS.ToList();

        //}

        ////to fetch data of selected gst scheme from dropdown        
        //public async Task<IActionResult> GSTCal(int GSTRate, int iBenefGSTStatus, int sTransStCode, int sBenefStCode, double TaxableAmt)
        //{
        //    GetBaseUrl();
        //    var igsttptal = 0;
        //    List<GSTVal> gstval = new List<GSTVal>();
        //    List<GSTRateMst> itemMsts = new List<GSTRateMst>();
        //    GSTVal val = new GSTVal();
        //    GSTCal cal = new GSTCal();

        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetGSTDetails";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        itemMsts = JsonConvert.DeserializeObject<List<GSTRateMst>>(response.Result);

        //        var valu = itemMsts.FirstOrDefault(x => x.RateId == GSTRate);
        //        if (valu != null)
        //        {
        //            igsttptal = (int)(valu.CGSTRate + valu.SGSTRate);
        //            val = cal.GSTCalaculation(igsttptal, iBenefGSTStatus, sTransStCode, sBenefStCode, TaxableAmt);

        //            // var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(val);
        //            // gstval = JsonConvert.DeserializeObject<List<GSTVal>>(jsonString);
        //        }
        //    }
        //    return Json(val);
        //}

        ////SO
        //public async Task<List<UnitMst>> Unit()
        //{
        //    List<UnitMst> units = new List<UnitMst>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetUnits";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        units = JsonConvert.DeserializeObject<List<UnitMst>>(response.Result);
        //    }
        //    return units.ToList();
        //}
        //public static List<VendAddress> VendAddressList = new List<VendAddress>();
        //public async Task<IActionResult> GetVendAddress(string iVendCode)
        //{
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetVendAddrById?iVendCode=" + iVendCode;
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        VendAddressList = JsonConvert.DeserializeObject<List<VendAddress>>(response.Result);
        //        ViewBag.Addlist = VendAddressList;
        //    }
        //    // return VendAddressList.ToList();
        //    return PartialView("GetVendAddress", ViewBag.Addlist);
        //}
        //public IActionResult GetAddbySelVend(int iSelVendId)
        //{
        //    var data = VendAddressList.FirstOrDefault(x => x.VendAddId == iSelVendId);
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    return Json(data);
        //}
        //public async Task<List<Service_Head>> ServiceHead(string Scode)
        //{
        //    List<Service_Head> ServiceHead = new List<Service_Head>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetServicehead?Scode=" + Scode + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        ServiceHead = JsonConvert.DeserializeObject<List<Service_Head>>(response.Result);
        //    }
        //    return ServiceHead.ToList();
        //}
        //public async Task<List<EquipDescrp>> EquipDesc(string Scode)
        //{
        //    List<EquipDescrp> equipDescrps = new List<EquipDescrp>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetEquipDesc?Scode=" + Scode + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        equipDescrps = JsonConvert.DeserializeObject<List<EquipDescrp>>(response.Result);
        //    }
        //    return equipDescrps.ToList();
        //}


        ////Mail//

        //public async Task<int> fnEmail(string sMsg, string sSub, string sFromEmail, int iEmailTyp, int iSiteId)
        //{
        //    string sToEmail = string.Empty;
        //    int iLoop;
        //    DataTable dtToDtl = await getEmail(iEmailTyp, iSiteId);
        //    sToEmail = "";
        //    for (iLoop = 0; iLoop < dtToDtl.Rows.Count; iLoop++)
        //    {
        //        if (Convert.ToInt32(sToEmail.Length.ToString()) <= 0 || sToEmail == string.Empty)
        //        {
        //            sToEmail = dtToDtl.Rows[iLoop]["EmailId"].ToString();
        //        }
        //        else
        //        {
        //            sToEmail = sToEmail + "," + dtToDtl.Rows[iLoop]["EmailId"].ToString();
        //        }
        //    }
        //    // Email Code 
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient();
        //        MailMessage mail = new MailMessage();
        //        SmtpServer.EnableSsl = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("ims_system@balajeegroup.com", "@Bicpl123@");
        //        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        SmtpServer.Port = 587;
        //        SmtpServer.Host = "smtp3.netcore.co.in";
        //        mail.From = new MailAddress("ims_system@balajeegroup.com");
        //        mail.To.Add("satendray019@gmail.com");
        //        //mail.To.Add(sToEmail); //Replace  the txtemail.Text with the user email id by fetching it from user
        //        if (iEmailTyp != 1)
        //        {
        //            //mail.CC.Add("coreteam@balajeegroup.com");
        //            mail.CC.Add("syadav@balajeegroup.com");
        //        }
        //        mail.Subject = sSub;
        //        mail.IsBodyHtml = true;
        //        mail.Body = sMsg;
        //        SmtpServer.Send(mail);
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        //public async Task<DataTable> getEmail(int iEmailTyp, int iSiteId)
        //{
        //    DataTable dt = new DataTable();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetEmail?iEmailTyp=" + iEmailTyp + "&iSiteId=" + iSiteId + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        dt = JsonConvert.DeserializeObject<DataTable>(response.Result);
        //    }
        //    return dt;

        //}

        //public async Task<int> fnEmailAndCC(string sMsg, string sSub, string sFromEmail, int iEmailTyp, int iCCEmailType, int iSiteId)
        //{
        //    string sToEmail = string.Empty;
        //    int iLoop;
        //    DataTable dtToDtl = await getMulipleEmail(iEmailTyp, iCCEmailType, iSiteId);
        //    sToEmail = "";
        //    for (iLoop = 0; iLoop < dtToDtl.Rows.Count; iLoop++)
        //    {
        //        if (Convert.ToInt32(sToEmail.Length.ToString()) <= 0 || sToEmail == string.Empty)
        //        {
        //            sToEmail = dtToDtl.Rows[iLoop]["EmailId"].ToString();
        //        }
        //        else
        //        {
        //            sToEmail = sToEmail + "," + dtToDtl.Rows[iLoop]["EmailId"].ToString();
        //        }
        //    }
        //    // Email Code 
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient();
        //        MailMessage mail = new MailMessage();
        //        SmtpServer.EnableSsl = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("ims_system@balajeegroup.com", "@Bicpl123@");
        //        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        SmtpServer.Port = 587;
        //        SmtpServer.Host = "smtp3.netcore.co.in";
        //        mail.From = new MailAddress("ims_system@balajeegroup.com");
        //        // mail.To.Add(sToEmail); //Replace  the txtemail.Text with the user email id by fetching it from user
        //        mail.To.Add("satendray019@gmail.com");
        //        if (iEmailTyp != 1)
        //        {
        //            mail.CC.Add("syadav@balajeegroup.com");
        //            //   mail.CC.Add("coreteam@balajeegroup.com");
        //        }
        //        mail.Subject = sSub;
        //        mail.IsBodyHtml = true;
        //        mail.Body = sMsg;
        //        SmtpServer.Send(mail);
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        //public async Task<DataTable> getMulipleEmail(int iEmailTyp, int iCCEmailType, int iSiteId)
        //{
        //    DataTable dt = new DataTable();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetMulipleEmail?iEmailTyp=" + iEmailTyp + "&iCCEmailType=" + iCCEmailType + "&iSiteId=" + iSiteId + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        dt = JsonConvert.DeserializeObject<DataTable>(response.Result);
        //    }
        //    return dt;
        //}

        //public async Task<List<WOTypMst>> GetWOTypMst()
        //{
        //    List<WOTypMst> wotypmst = new List<WOTypMst>();
        //    GetBaseUrl();
        //    if (string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = "10698";
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetWOTypMstDTO";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        wotypmst = JsonConvert.DeserializeObject<List<WOTypMst>>(response.Result);
        //    }
        //    return wotypmst.ToList();

        //}

        //public async Task<List<Client_Bind>> GetClient(string Site)
        //{
        //    GetBaseUrl();
        //    if (!string.IsNullOrEmpty(sUserCode))
        //    {
        //        sUserCode = sUserCode;
        //        iSiteId = 16;
        //    }
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "api/SCLoI/GetClient?Site=" + Site;
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    List<Client_Bind> clientlist = new List<Client_Bind>();
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        clientlist = JsonConvert.DeserializeObject<List<Client_Bind>>(response.Result);
        //    }
        //    return clientlist.ToList();
        //}

        ////

        //public async Task<List<Store>> InwardStore(string sSiteCode, string sAction)
        //{
        //    List<Store> storeinward = new List<Store>();
        //    GetBaseUrl();
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetStoreCode?sSiteCode=" + sSiteCode + "&sAction=" + sAction + "";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        storeinward = JsonConvert.DeserializeObject<List<Store>>(response.Result);
        //    }
        //    return storeinward.ToList();

        //}

        ////grnbill

        //public async Task<List<TCSApplMst>> TCSApplMst()
        //{
        //    List<TCSApplMst> TCSApplMstlist = new List<TCSApplMst>();
        //    GetBaseUrl();
        //    string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
        //    string sBaseUrl = sBU + "GetTCSApplicable";
        //    APIResponse response = await _apiservice.CalAPI(sBaseUrl, sTokenUrl);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        TCSApplMstlist = JsonConvert.DeserializeObject<List<TCSApplMst>>(response.Result);
        //    }
        //    return TCSApplMstlist.ToList();

        //}


    }

}

