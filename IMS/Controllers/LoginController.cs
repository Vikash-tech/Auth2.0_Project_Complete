
using IMS.Service;
using IMS_Model.CommonModel;
using IMS_Model.LoginModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using IMS_Model.UserMaster;
using IMS.Controllers.Common;
using IMS_Model.CommonDropDown;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using IMS_Model.CommonDropDown.ViewModel;

namespace IMS.Controllers
{
    public class LoginController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public LoginController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;
        }
        public static UserMst userMst = new UserMst();



        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }


        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/UserLogin/login";
                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, login);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        UserDetail userDetail = new UserDetail();

                        HttpContext.Session.SetString("TokenExpiry", response.Expiration.ToString());
                        HttpContext.Session.SetString("RefresherToken", response.RefreshToken);
                        HttpContext.Session.SetString("RefresherTokenExpiry", response.RefreshTokenExpiryTime.ToString());
                        HttpContext.Session.SetString("UserCode", login.UserName);

                        userDetail = await GetUserDetails(login.UserName);
                        if (userDetail != null)
                        {
                            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userDetail.UserName) },
                                      CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                            HttpContext.Session.SetString("UserEmail", userDetail.UserEmail);
                            if (userDetail.HOUser == 1)
                            {
                                HttpContext.Session.SetInt32("SiteId", 16);
                                //HttpContext.Session.SetInt32("RoleId", 16);
                            }
                            else
                            {
                                HttpContext.Session.SetInt32("SiteId", 51);
                                //HttpContext.Session.SetInt32("RoleId", 13);
                            }
                            byte[] Logo = await GetCompanyLogo();
                            HttpContext.Session.Set("Logo", (Logo));
                        }
                        return RedirectToAction("DashBoard");
                    }
                    else
                    {
                        TempData["Error"] = response.Result;
                        return View("Error");
                    }
                }
                else
                {
                    TempData["error"] = "error";
                    return View(login);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(login);
        }


        public static string SiteDropdownid { get; set; }
        public async Task<IActionResult> DashBoard()
        {

            sUserCode = HttpContext.Session.GetString("UserCode");

            if (SiteDropdownid == null)
            {
                SiteDropdownid = HttpContext.Session.GetInt32("SiteId").ToString();
            }
            else
            {
                HttpContext.Session.SetInt32("SiteId", Convert.ToInt32(SiteDropdownid));
            }

            ViewBag.sUserCode = sUserCode;
            ViewBag.iSiteId = SiteDropdownid;
            ViewBag.MenuId = 200;
            ViewBag.Success="Login SuccessFully..";
            return View();
        }
        public async Task<PartialViewResult> dropdown()
        {

            List<SiteMst> sitelist = new List<SiteMst>();

            sitelist = await siteMsts(sUserCode);

            var ddlist = (from p in sitelist
                          select new
                          {
                              Text = p.StName,
                              Value = p.SiteId.ToString(),
                          }).ToList();
            ViewBag.ddl = new SelectList(ddlist, "Value", "Text");
            ViewBag.SelectedId = SiteDropdownid;
            return PartialView("_SiteDropdown", ddlist);
        }
        public IActionResult SetSiteDropdownId(int? isiteid)
        {
            if (isiteid != null)
            {
                SiteDropdownid = isiteid.ToString();

            }
            return Json(SiteDropdownid);
        }
        public async Task<IActionResult> Logout()
        {
            // Clear session
            var session = HttpContext.Session;
            session.Clear();
            // Clear cookies
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookie in storedCookies)
            {
                Response.Cookies.Delete(cookie);
            }

            // Sign out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> IMSSetting(string MenuId)
        {
            return View();
        }
        public async Task<UserDetail> GetUserDetails(string UserCode)
        {
            try
            {

                GetBaseUrl();
                // string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/UserAuth/GetUserDetail?UserCode=" + UserCode + "";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                UserDetail user = new UserDetail();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    user = JsonConvert.DeserializeObject<UserDetail>(response.Result);
                }
                return user;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<byte[]> GetCompanyLogo()
        {
            byte[] Logodata = null;
            List<CompConfg> compConfgs = new List<CompConfg>();
            GetBaseUrl();
            string sBaseUrl = sBU + "api/CompanyMaster/GetCompConfg";
            APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                compConfgs = JsonConvert.DeserializeObject<List<CompConfg>>(response.Result);
                Logodata = compConfgs[0].Logo;
            }
            return Logodata;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(IFormCollection fc)
        {
            string EmailId = fc["email"].ToString();
            if (EmailId != null)
            {
                string NewPassword = GeneratePassword();
                string Token = HttpContext.Session.GetString("RefresherToken");
                string sBaseUrl = sBU + "api/UserLogin/IMSUserResetPassWord?Email=" +EmailId+ "&Token=" +Token+ "&NewPassWord=" +NewPassword+ "";
                APIResponse_IO API = new APIResponse_IO();
                API.Result = EmailId;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Result == 1)
                    {
                        string newUserMsg = "<html> <body> <p> Dear Sir, </p> ";
                        newUserMsg += "<p> <br> Please be informed that a new user has been created in Balajee Infratech & Constructions Pvt. Ltd. The details are as follows; <br>";
                        newUserMsg += "<br> User Code: ";
                        newUserMsg += "<br> Password: ";
                        newUserMsg += "<br> ";
                        newUserMsg += "<br> <br> Regards, <br> Balajee Infratech & Constructions Pvt. Ltd. </p> </body> </html>";

                        string sysmtMail = EmailId;
                        // Send the email with the new password
                        int iMail = SCEmail.fnSCEmail(newUserMsg, "New User Created", EmailId, 0, 0);
                        if (iMail == 1)
                        {
                            TempData["Success"] = "New PassWord Created And Send to Register Mail successfully . Password sent to user by email.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Success"] = "New PassWord Created successfully. But Mail Could not be sent due to Technical Error. ";
                            return RedirectToAction("Index");
                        }
                    }
                    
                }
            }
            return View();
        }

        private string GeneratePassword()
        {
            string AllowedCharacter = _configuration.GetSection("AppSettings")["Character"].ToString();
            const int PasswordLength = 8;

            Random rand = new Random();
            char[] password = new char[PasswordLength];

            for (int i = 0; i < PasswordLength; i++)
            {
                password[i] = AllowedCharacter[rand.Next(0, AllowedCharacter.Length)];
            }

            return new string(password);
        }
    }
}
