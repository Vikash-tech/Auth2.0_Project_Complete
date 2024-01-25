using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonDropDown;
using IMS_Model.CommonDropDown.ViewModel;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing;

namespace IMS.Controllers.Setting
{
    public class CompanyMstController : CommonController
    {
        private IConfiguration _configuration;
        private readonly IAPIService _apiservice;

        public static string? sBU;
        public static string? sUserCode;
        public static int iMenuId;
        public CompanyMstController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
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

        public static List<CompMst> CompMsts = new List<CompMst>();

        public static CompanyMst_VM vm = new CompanyMst_VM();

        public static List<CompTypMst> CompTypMsts = new List<CompTypMst>();

        public static List<AddressType> AddressTypes = new List<AddressType>();

        public static List<StateMst> StateMsts = new List<StateMst>();

        public static List<CountryMst> CountryMsts = new List<CountryMst>();

        public static List<CompanyAddress> CompanyAddresslist = new List<CompanyAddress>();

        public static byte[] logo;
        public static byte[] letterheaderlogo;
        public async Task<IActionResult> Index(int MenuId)
        {
            if (MenuId == 0)
            {
                MenuId = iMenuId;
            }
            var data = GetValues(MenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            GetBaseUrl();
            string sBaseUrl = sBU + "api/CompanyMaster/GetAllCompMstList";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CompMsts = JsonConvert.DeserializeObject<List<CompMst>>(response.Result);
            }

            return View(CompMsts);
        }

        public async Task<IActionResult> CompanyMstCreation(int iCompId, int iCtmId)
        {
    
            var data = GetValues(iMenuId);
            iMenuId = data.iMenuId;
            sUserCode = data.sUserCode;
            ViewBag.btnAccControl = data;

            try
            {

                CompTypMsts = await GetCompTypeMst();

                AddressTypes = await GetAddressTypeMst();

                StateMsts = await GetStateMst();

                CountryMsts = await GetCountryMst();

                if (iCompId > 0)
                {
                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/CompanyMaster/GetCompDtlById?iCompId=" + iCompId + "";
                    APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        vm = JsonConvert.DeserializeObject<CompanyMst_VM>(response.Result);
                        CompanyAddresslist = vm.CompanyAddresslist;
                        logo = vm.CompConfg.Logo;
                        letterheaderlogo = vm.CompConfg.LetterHeadLogo;
                        if (vm.CompMst == null)
                        {
                            TempData["Error"] = response.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return RedirectToAction("Index");
                    }
                    if (iCtmId > 0)
                    {
                        TempData["buttons"] = "Details";
                    }
                }
                else
                {
                    vm.CompMst = null;
                    vm.CompanyAddress = null;
                    CompanyAddresslist.Clear();
                    vm.CompConfg = null;
                }

                vm.CompTypeMst = CompTypMsts.Select(x => new SelectListItem()
                {
                    Value = x.CtmId.ToString(),
                    Text = x.CtmDesc
                }).ToList();

                vm.AddressType = AddressTypes.Select(x => new SelectListItem()
                {
                    Value = x.AtId.ToString(),
                    Text = x.AtDesc
                }).ToList();

                vm.StateMst = StateMsts.Select(x => new SelectListItem()
                {
                    Value = x.StId.ToString(),
                    Text = x.StName
                }).ToList();

                vm.CountryMst = CountryMsts.Select(x => new SelectListItem()
                {
                    Value = x.CtId.ToString(),
                    Text = x.CtName
                }).ToList();

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(vm);
        }

        public async Task<PartialViewResult> _CompanyAddress(int SRNO, int Aid, string Address1, string Address2, string Address3, string City, int PinCode, int StateId, int CountryId, string Phone)
        {
            try
            {
                var addty = AddressTypes.FirstOrDefault(x => x.AtId == Aid);
                var state = StateMsts.FirstOrDefault(x => x.StId == StateId);
                var country = CountryMsts.FirstOrDefault(x => x.CtId == CountryId);

                if (SRNO > 0)
                {
                    CompanyAddresslist.Add(new CompanyAddress()
                    {
                        SRNO = SRNO,
                        AtId = Aid,
                        Add1 = Address1,
                        Add2 = Address2,
                        Add3 = Address3,
                        City = City,
                        CaPin = PinCode,
                        StId = StateId,
                        CtId = CountryId,
                        Phone = Phone,
                        AddedBy = sUserCode,
                        AddedOn = DateTime.Now,
                        ActStatus = true,
                        AtDesc = addty.AtDesc,
                        StName = state.StName,
                        CtName = country.CtName

                    });
                    vm.CompanyAddresslist = CompanyAddresslist;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return PartialView("_CompanyAddress", vm);

        }

        public async Task<PartialViewResult> AddressUpdate(int AddrId, int SRNO, int CompId, int Aid, string Address1, string Address2, string Address3, string City, int PinCode, int StateId, int CountryId, string Phone)
        {
            var addty = AddressTypes.FirstOrDefault(x => x.AtId == Aid);
            var state = StateMsts.FirstOrDefault(x => x.StId == StateId);
            var country = CountryMsts.FirstOrDefault(x => x.CtId == CountryId);
            if (SRNO > 0)
            {
                CompanyAddress compadd = CompanyAddresslist.FirstOrDefault(i => i.SRNO == SRNO);

                if (compadd != null)
                {
                    compadd.CompId = CompId;
                    compadd.SRNO = SRNO;
                    compadd.AtId = Aid;
                    compadd.Add1 = Address1;
                    compadd.Add2 = Address2;
                    compadd.Add3 = Address3;
                    compadd.City = City;
                    compadd.CaPin = PinCode;
                    compadd.StId = StateId;
                    compadd.CtId = CountryId;
                    compadd.Phone = Phone;
                    compadd.AtDesc = addty.AtDesc;
                    compadd.StName = state.StName;
                    compadd.CtName = country.CtName;
                    compadd.AddedBy = sUserCode;
                    compadd.AddedOn = DateTime.Now;
                    compadd.ActStatus = true;
                }
            }
            else
            {
                CompanyAddress compadd = CompanyAddresslist.FirstOrDefault(i => i.CAddId == AddrId);

                if (compadd != null)
                {
                    compadd.CompId = CompId;
                    compadd.SRNO = SRNO;
                    compadd.AtId = Aid;
                    compadd.Add1 = Address1;
                    compadd.Add2 = Address2;
                    compadd.Add3 = Address3;
                    compadd.City = City;
                    compadd.CaPin = PinCode;
                    compadd.StId = StateId;
                    compadd.CtId = CountryId;
                    compadd.Phone = Phone;
                    compadd.AtDesc = addty.AtDesc;
                    compadd.StName = state.StName;
                    compadd.CtName = country.CtName;
                    compadd.AddedBy = sUserCode;
                    compadd.AddedOn = DateTime.Now;
                    compadd.ActStatus = true;
                }

            }
            vm.CompanyAddresslist = CompanyAddresslist;
            return PartialView("_CompanyAddress", vm);
        }

        public IActionResult _CompanyAddressDel(int SRNO, int AddressId)
        {

            if (SRNO == 0)
            {
                CompanyAddress address = CompanyAddresslist.FirstOrDefault(i => i.CAddId == AddressId);
                CompanyAddresslist.Remove(address);
                vm.CompanyAddresslist = CompanyAddresslist;
                return PartialView("_CompanyAddress", vm);
            }
            else
            {
                CompanyAddress address = CompanyAddresslist.FirstOrDefault(i => i.SRNO == SRNO);
                CompanyAddresslist.Remove(address);
                vm.CompanyAddresslist = CompanyAddresslist;
                return PartialView("_CompanyAddress", vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompanyMstCreation(CompanyMst_VM mst_VM)
        {
            try
            {
                

                if (ModelState.IsValid)
                {
                    APIResponse_IO aPI = new APIResponse_IO();
                    string sBaseUrl = string.Empty;

                    CompMst CompMsts = new CompMst();
                    CompMsts = mst_VM.CompMst;
                    CompMsts.AddedBy = sUserCode;
                    CompMsts.AddedOn = DateTime.Now;
                    CompMsts.ActStatus = true;

                    mst_VM.CompanyAddresslist = CompanyAddresslist;
                    mst_VM.CompMst = CompMsts;

                    CompConfg confg = new CompConfg();
                    confg = mst_VM.CompConfg;

                    // Ensure the file is not null
                    if (mst_VM.CompConfg.LogoFile != null)
                    {
                        // Check image size (15 KB max)
                        if (mst_VM.CompConfg.LogoFile.Length > 150 * 1024)
                        {
                            ModelState.AddModelError("LogoFile", "The JPG file size should not exceed 150 KB.");
                            return View(mst_VM);
                        }

                        // Read the file into a byte array
                        using (var ms = new MemoryStream())
                        {
                            await mst_VM.CompConfg.LogoFile.CopyToAsync(ms);
                            mst_VM.CompConfg.Logo = ms.ToArray(); // Assuming PdfData is a byte[] property in your CompConfg model
                        }

                        // Check image dimensions (adjust as needed)
                        using (var imageStream = new MemoryStream(mst_VM.CompConfg.Logo))
                        {
                            var image = Image.FromStream(imageStream);
                            int maxWidth = 150; // Adjust as needed
                            int maxHeight = 150; // Adjust as needed

                            if (image.Width > maxWidth || image.Height > maxHeight)
                            {
                                ModelState.AddModelError("LogoFile", $"The JPG dimensions should not exceed {maxWidth}x{maxHeight} Percentage.");
                                return View(mst_VM);
                            }
                        }
                        confg.Logo = mst_VM.CompConfg.Logo;
                    }
                    else
                    {
                        confg.Logo = logo;
                    }
                    if (mst_VM.CompConfg.LetterHeadLogoFile != null)
                    {
                        //if (mst_VM.CompConfg.LogoFile.Length > 150 * 1024)
                        //{
                        //    ModelState.AddModelError("LogoFile", "The JPG file size should not exceed 150 KB.");
                        //    return View(mst_VM);
                        //}
                        using (var ms = new MemoryStream())
                        {
                            await mst_VM.CompConfg.LogoFile.CopyToAsync(ms);
                            mst_VM.CompConfg.Logo = ms.ToArray();
                        }

                        using (var imageStream = new MemoryStream(mst_VM.CompConfg.Logo))
                        {
                            var image = Image.FromStream(imageStream);
                            int maxWidth = 150;
                            int maxHeight = 150;
                            if (image.Width > maxWidth || image.Height > maxHeight)
                            {
                                ModelState.AddModelError("LogoFile", $"The JPG dimensions should not exceed {maxWidth}x{maxHeight} Percentage.");
                                return View(mst_VM);
                            }
                        }
                        confg.LetterHeadLogo = mst_VM.CompConfg.LetterHeadLogo;
                    }
                    else
                    {
                        confg.LetterHeadLogo = letterheaderlogo;
                    }
                    confg.AddedBy = sUserCode;
                    confg.AddedOn = DateTime.Now;
                    confg.ActStatus = true;

                    mst_VM.CompConfg = confg;
                    aPI.Result = mst_VM;

                    GetBaseUrl();

                    if (CompMsts.CompId > 0)
                    {
                        sBaseUrl = sBU + "api/CompanyMaster/CompMstUpdate";
                    }
                    else
                    {
                        sBaseUrl = sBU + "api/CompanyMaster/CompMstInsert";
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return View(mst_VM);
        }
    }
}
