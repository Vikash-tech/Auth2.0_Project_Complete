using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model;
using IMS_Model.CommonDropDown.ViewModel;
using IMS_Model.CommonModel;
using IMS_Model.SettingModel;
using IMS_Model.UserMaster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace IMS.Controllers
{
    public class UsersManagmentController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public UsersManagmentController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
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


        public static List<TMenuMst> MenuItems = new List<TMenuMst>();
        public static List<UserDetail> Userlist = new List<UserDetail>();
        public static UserVm uservm = new UserVm();
        public static PostUserVm postuservm = new PostUserVm();

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<PartialViewResult> _UserDetailsList()
        {
            try
            {
                GetBaseUrl();
                string sBaseUrl = sBU + "api/UserAuth/GetAllUserDetail";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Userlist = JsonConvert.DeserializeObject<List<UserDetail>>(response.Result);
                    Userlist = Userlist.Where(x => x.ActStatus == true).ToList();
                    return PartialView("_UserDetailsList", Userlist);
                }
            }
            catch (Exception ex)
            {
                ProjLogException(ex);
            }
            return PartialView("_UserDetailsList", Userlist);
        }


        public static Guid UpdateId;
        [HttpGet]
        public async Task<IActionResult> UserCreation(Guid guid, string EditId)
        {
            uservm.UserMst = null;
            if (string.IsNullOrEmpty(EditId))
            {
                if (uservm.imsUsrRoles!=null)
                {
                    uservm.imsUsrRoles.Clear();
                    uservm.imsUserClaims.Clear();
                }
            }


            sUserCode = HttpContext.Session.GetString("UserCode");
            if (!string.IsNullOrEmpty(EditId))
            {
                UpdateId = guid;
                GetBaseUrl();
                string sBaseUrl = sBU + "api/UserAuth/GetUserDtlById?Id=" + UpdateId;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    postuservm = JsonConvert.DeserializeObject<PostUserVm>(response.Result);
                    uservm.UserMst = postuservm.UserMst;
                    uservm.imsUsrRoles = postuservm.imsUsrRoles;
                    uservm.imsUserClaims = postuservm.imsUserClaims;
                }
            }

            uservm.roleMasters = await RoleMasterList();
            uservm.siteMasters = await AllSiteMstList();
            
            uservm.IMSClaims = await GetAllClaims();
            MenuItems = await GetAllMenuMst();
            uservm.TMenuMsts = MenuItems;



            uservm.AllSite = uservm.siteMasters.Select(x => new SelectListItem()
            {
                Value = x.SiteId.ToString(),
                Text = x.StName
            }).ToList();

            var yesno = await param("Yes/No");
            uservm.AddYesNo = yesno.Select(x => new SelectListItem()
            {
                Value = x.ParamValueTxt.ToString(),
                Text = x.ParamValDispTxt
            }).ToList();

            return View(uservm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreation(UserVm userVm, IFormCollection fc)
        {
            try
            {
                List<ClaimsToRole> userRoleList = new List<ClaimsToRole>();
                List<ClaimToUser> userClaimList = new List<ClaimToUser>();

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
                            ClaimsToRole claimRole = new ClaimsToRole();
                            claimRole.MenuId = Convert.ToInt32(parts[0]);
                            claimRole.ClaimType = parts[1];
                            claimRole.ClaimValue = parts[2];
                            claimRole.IsChecked = Convert.ToBoolean(value);
                            userRoleList.Add(claimRole);
                        }
                        else if (parts.Length == 7)
                        {
                            ClaimToUser userClaim = new ClaimToUser();
                            userClaim.SiteId = Convert.ToInt32(parts[0]);
                            userClaim.RoleId = parts[1]+"-"+parts[2]+"-"+parts[3]+"-"+parts[4]+"-"+parts[5];
                            userClaim.IsChecked = Convert.ToBoolean(parts[6]);
                            userClaimList.Add(userClaim);
                        }
                    }
                }

                List<ClaimsToRole> claimroles = new List<ClaimsToRole>();

                int fund = -1;

                foreach (var menu in MenuItems)
                {
                    foreach (var claim in userRoleList)
                    {
                        if (menu.MenuId == claim.MenuId)
                        {
                            if (fund != menu.MenuId)
                            {

                                if (claim.ClaimType != "IsGet")
                                {

                                    if (!userRoleList.Any(c => c.MenuId == menu.MenuId && c.ClaimType == "IsGet"))
                                    {
                                        fund = menu.MenuId;

                                        ClaimsToRole class1 = new ClaimsToRole();
                                        class1.MenuId = claim.MenuId;
                                        class1.ClaimType = "IsGet";
                                        class1.ClaimValue = "IsGet";
                                        class1.IsChecked = true;
                                        claimroles.Add(class1);
                                    }
                                }
                            }
                        }
                    }
                }
                userRoleList = userRoleList.Concat(claimroles).ToList();


                postuservm.UserMst = userVm.UserMst;
                postuservm.userRoleList = userRoleList;
                postuservm.userClaimList = userClaimList;

                if (userClaimList.Count > 0 && userClaimList != null)
                {
                    string sBaseUrl = string.Empty;
                    GetBaseUrl();
                    if (postuservm.UserMst.AddedBy == null)
                    {
                        sBaseUrl = sBU + "api/UserAuth/IMSUserRegister";
                        postuservm.UserMst.AddedBy = sUserCode;
                        postuservm.UserMst.ActStatus = true;
                        postuservm.UserMst.NormalizedUserName = userVm.UserMst.UsrCode;
                        var Fullname = userVm.UserMst.UsrFN +"_"+ userVm.UserMst.UsrLN;
                        postuservm.UserMst.UserName = Fullname;
                        postuservm.UserMst.AddedOn = DateTime.Now;
                        string Password = GeneratePassword();
                        postuservm.UserMst.Password = Password;
                    }
                    else
                    {
                        sBaseUrl = sBU + "api/UserAuth/IMSUserUpdate";
                        postuservm.UserMst.AddedBy = sUserCode;
                        postuservm.UserMst.ActStatus = true;
                        postuservm.UserMst.Id = userVm.UserMst.Id;
                        postuservm.UserMst.NormalizedUserName = userVm.UserMst.UsrCode;
                        var Fullname = userVm.UserMst.UsrFN +"_"+ userVm.UserMst.UsrLN;
                        postuservm.UserMst.UserName = Fullname;
                        postuservm.UserMst.AddedOn = DateTime.Now;
                    }

                    APIResponse_IO API = new APIResponse_IO();
                    API.Result = postuservm;

                    APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, API);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (response.Result == 1)
                        {
                            string newUserMsg = "<html> <body> <p> Dear Sir, </p> ";
                            newUserMsg += "<p> <br> Please be informed that a new user has been created in Balajee Infratech & Constructions Pvt. Ltd. The details are as follows; <br>";
                            newUserMsg += "<br> User Code: " + postuservm.UserMst.UsrCode;  // Include additional user details as needed
                            newUserMsg += "<br> Password: " + postuservm.UserMst.Password; // Include the new password
                            newUserMsg += "<br> ";
                            newUserMsg += "<br> <br> Regards, <br> Balajee Infratech & Constructions Pvt. Ltd. </p> </body> </html>";

                            string sysmtMail = postuservm.UserMst.Email;
                            // Send the email with the new password
                            int iMail = SCEmail.fnSCEmail(newUserMsg,"New User Created", sysmtMail,0,0);

                            if (iMail == 1)
                            {
                                TempData["Success"] = "New user created successfully. Password sent to user by email.";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Success"] = "New user created successfully. But Mail Could not be sent due to Technical Error. ";
                                return RedirectToAction("Index");
                            }
                        }
                        else if (response.Result == 2)
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
                    TempData["Error"] = "Site Assigment Cannot be Blanks";
                    return RedirectToAction("UserCreation", userVm);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
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


        [HttpGet]
        public async Task<IActionResult> Delete(Guid guid)
        {
            try
            {
                UserMst master = new UserMst();
                var data = Userlist.FirstOrDefault(x => x.Id == guid.ToString());
                if (data != null)
                {
                    master.Id = data.Id;
                    master.AddedBy = sUserCode;
                    master.AddedOn = DateTime.Now;
                    master.ActStatus = false;

                    APIResponse_IO aPI = new APIResponse_IO();
                    aPI.Result = master;

                    GetBaseUrl();
                    string sBaseUrl = sBU + "api/UserAuth/IMSUserDelete";
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
