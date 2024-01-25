using IMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using IMS.Controllers.Common;
using IMS_Model.CommonModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using IMS_Model.SC.SCCreation;

namespace IMS.Controllers.SC
{
    public class SCController : CommonController
    {

        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public static string? sUserCode;
        public static int iSiteId;
        public static string Ucode = string.Empty;
        public static string Site = string.Empty;
        public static string RoleId = string.Empty;
        public static string SiteId = string.Empty;
        public static string Username = string.Empty;
        public static int iMenuId;

        public static VmSC Vmscget = new VmSC();
        public SCMst SCMst = new SCMst();
        public static List<SCAddress> sCAddresses = new List<SCAddress>();
        public static List<SCMstReq> scmstreq = new List<SCMstReq>();
        public static List<SCMst> SCMstlist = new List<SCMst>();
        public static List<SCEquipDtl> equipmentlist = new List<SCEquipDtl>();
        public static List<SCAssComp> AssCompslist = new List<SCAssComp>();
        public static List<SCProfileWODtl> experiencelist = new List<SCProfileWODtl>();
        public static List<AttachDtl> _pdfFiles = new List<AttachDtl>();

        DataModel data = new DataModel
        {
            SCAddresses = sCAddresses,
            SCEquipDtl = equipmentlist,
            SCAssComp = AssCompslist,
            SCProfileWODtl = experiencelist,
            SCmstreq = scmstreq,
            AttachFile = _pdfFiles,
        };

        public SCController(IConfiguration configuration, IAPIService aPTService) : base(configuration, aPTService)
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

        public async Task<IActionResult> Index(string SCName, string ScReg,int MenuId)
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

                //testing
               // ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
              
                var sitelistcode = await siteMsts(sUserCode);
                ViewBag.Locations = sitelistcode;

                GetBaseUrl();
                string sBaseUrl = sBU + "api/SC/GetSCMstlistSrch?sName=" + SCName + "&sScRegNo=" + ScReg + "";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SCMstlist = JsonConvert.DeserializeObject<List<SCMst>>(response.Result);
                    var mess = response.Message;
                    return View(SCMstlist);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
        }


        public async Task<IActionResult> SCIndex(int Data, int Tempscid, int SCPANAttach, int SCGSTAttach, int SCPFAttach, int SCESIAttach,
                                                 int SCCHQAttac, int MandateAttach, int MSMEAttach, int SCAttch)
        {
            ClearList(); Vmscget.scmst = null;

            if (Data == 1 && Tempscid > 0)
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetReqVmDatabyID?SCPANAttach=" + SCPANAttach + "&SCGSTAttach=" + SCGSTAttach + "&SCPFAttach=" + SCPFAttach + "&SCESIAttach=" + SCESIAttach + "&SCCHQAttac=" + SCCHQAttac + "&MandateAttach=" + MandateAttach + "&MSMEAttach=" + MSMEAttach + "&SCAttch=" + SCAttch + "&Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Vmscget = JsonConvert.DeserializeObject<VmSC>(response.Result);
                    //sCAddresses = Vmscget.SCAddresses;
                    //equipmentlist = Vmscget.SCEquipDtl;
                    //AssCompslist = Vmscget.SCAssComp;
                    //experiencelist = Vmscget.SCProfileWODtl;
                    //_pdfFiles = Vmscget.AttachFile;

                    Vmscget = JsonConvert.DeserializeObject<VmSC>(response.Result);
                    var SCAddresses = await PartialAddress(Tempscid);
                    var equipment = await EquipmentMst(Tempscid);
                    var Assocompany = await AssociateMst(Tempscid);
                    var Experiment = await ExperienceMst(Tempscid);
                    var Attachment = await Attach(SCPANAttach, SCGSTAttach, SCPFAttach, SCESIAttach, SCCHQAttac, MandateAttach, MSMEAttach, SCAttch);
                }
            }
            else
            {
                var SCAddresses = await AddressMst(Tempscid);
                var equipment = await EquipmentMst(Tempscid);
                var Assocompany = await AssociateMst(Tempscid);
                var Experiment = await ExperienceMst(Tempscid);
                var Attachment = await Attach(SCPANAttach, SCGSTAttach, SCPFAttach, SCESIAttach, SCCHQAttac, MandateAttach, MSMEAttach, SCAttch);
                Vmscget.scmst = SCMstlist.Find(x => x.TmpScId == Tempscid);
            }

            var data = GetValues(iMenuId);
            Ucode = data.sUserCode;
            SiteId= data.iSiteId.ToString();
            ViewBag.btnAccControl = data;


            Username = HttpContext.Session.GetString("Username");
            iSiteId = Convert.ToInt32(SiteId);
            ViewBag.Locations = await siteMsts(Ucode);

            var Msme = await param("MSMEStatus");
            Vmscget.Msme = Msme.Select(x => new SelectListItem() { Value = x.ParamValueTxt.ToString(), Text = x.ParamValDispTxt }).ToList();

            var gststatus = await param("GST Status");
            Vmscget.GST = gststatus.Select(x => new SelectListItem() { Value = x.ParamValueTxt.ToString(), Text = x.ParamValDispTxt }).ToList();

            var Site = await param("Site");
            Vmscget.Site = Site.Select(x => new SelectListItem() { Value = x.ParamValueTxt.ToString(), Text = x.ParamValDispTxt }).ToList();

            var Enterprise = await param("Enterprise");
            Vmscget.Enterprise = Enterprise.Select(x => new SelectListItem() { Value = x.ParamValueTxt.ToString(), Text = x.ParamValDispTxt }).ToList();

            var file = await param("AttachFile");
            Vmscget.Attachfilepara = file.Select(x => new SelectListItem() { Value = x.ParamValueTxt.ToString(), Text = x.ParamValDispTxt }).ToList();

            var Compty = await DDcompTyps();
            Vmscget.Compty = Compty.Select(x => new SelectListItem() { Value = x.CtmId.ToString(), Text = x.CtmDesc }).ToList();

            var state = await GetStateMstlist();
            Vmscget.Statelist = state.Select(x => new SelectListItem() { Value = x.StCode.ToString(), Text = x.StName }).ToList();

            var banklist = await GetBankmsts();
            Vmscget.Banklists = banklist.Select(x => new SelectListItem() { Value = x.BankName.ToString(), Text = x.BankName }).ToList();

            return View(Vmscget);
        }

        public async Task<IActionResult> PartialScmst(string SCName, string ScReg)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCMstlistSrch?sName=" + SCName + "&sScRegNo=" + ScReg + "";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SCMstlist = JsonConvert.DeserializeObject<List<SCMst>>(response.Result);
                    return PartialView(SCMstlist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView(SCMstlist);
        }
        public async Task<IActionResult> PendingSC(string pName)
        {
            try
            {
                ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCpendinglistSrch?pName=" + pName + "";
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    scmstreq = JsonConvert.DeserializeObject<List<SCMstReq>>(response.Result);
                    return PartialView(scmstreq);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView(scmstreq);
        }


        public ActionResult PartialAddressmst()
        {
            return View();
        }
        public async Task<IActionResult> AddressMst(int Tempscid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCAddressMst?Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    sCAddresses = JsonConvert.DeserializeObject<List<SCAddress>>(response.Result);
                    return View("PartialAddressmst", sCAddresses);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return View("PartialAddressmst", sCAddresses);
        }
        public async Task<IActionResult> PartialAddress(int Tempscid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCAddress?Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    sCAddresses = JsonConvert.DeserializeObject<List<SCAddress>>(response.Result);
                    return PartialView("PartialAddressmst", sCAddresses);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialAddressmst", sCAddresses);
        }
        public async Task<PartialViewResult> AddScAdd(int SRNO, string Address1, string Address2, string Address3, string City
                                            , int Country, string? Pincode, string Phone, string Fax, string namecntpern, string design
                                             , string Mobile, string Emails)
        {
            if (SRNO > 0)
            {
                sCAddresses.Add(new SCAddress()
                {
                    SRNO = SRNO,
                    SCAdd1 = Address1,
                    SCAdd2 = Address2,
                    SCAdd3 = Address3,
                    City = City,
                    Country = Country,
                    Pincode = Pincode,
                    PhoneNo = Phone,
                    FaxNo = Fax,
                    CntPerson = namecntpern,
                    Desiganation = design,
                    MobileNo = Mobile,
                    EmailId = Emails,
                });
            }

            return PartialView("PartialAddressmst", sCAddresses);
        }
        public async Task<PartialViewResult> AddressUpdate(int Id, int AddID, string? address, string Address2, string Address3, string City
                                                 , int Country, string? Pincode, string Phone, string Fax, string namecntpern, string design
                                                , string Mobile, string Emails)
        {
            if (Id > 0)
            {
                SCAddress tmpSC = sCAddresses.FirstOrDefault(i => i.SRNO == Id);

                if (tmpSC != null)
                {
                    tmpSC.SCAdd1 = address;
                    tmpSC.SCAdd2 = Address2;
                    tmpSC.SCAdd3 = Address3;
                    tmpSC.City = City;
                    tmpSC.Country = Country;
                    tmpSC.Pincode = Pincode;
                    tmpSC.PhoneNo = Phone;
                    tmpSC.FaxNo = Fax;
                    tmpSC.CntPerson = namecntpern;
                    tmpSC.Desiganation = design;
                    tmpSC.MobileNo = Mobile;
                    tmpSC.EmailId = Emails;
                }
            }
            else
            {
                SCAddress tmpSC = sCAddresses.FirstOrDefault(i => i.SCAddressId == AddID);

                if (tmpSC != null)
                {
                    tmpSC.SCAdd1 = address;
                    tmpSC.SCAdd2 = Address2;
                    tmpSC.SCAdd3 = Address3;
                    tmpSC.City = City;
                    tmpSC.Country = Country;
                    tmpSC.Pincode = Pincode;
                    tmpSC.PhoneNo = Phone;
                    tmpSC.FaxNo = Fax;
                    tmpSC.CntPerson = namecntpern;
                    tmpSC.Desiganation = design;
                    tmpSC.MobileNo = Mobile;
                    tmpSC.EmailId = Emails;
                }

            }

            return PartialView("PartialAddressmst", sCAddresses);
        }
        public IActionResult AddressDelete(int SRNO, int SCAddressId)
        {
            if (sCAddresses.Count != 1)
            {
                if (SRNO == 0)
                {
                    SCAddress address = sCAddresses.FirstOrDefault(i => i.SCAddressId == SCAddressId);
                    sCAddresses.Remove(address);
                    return PartialView("PartialAddressmst", sCAddresses);
                }
                else
                {
                    SCAddress address = sCAddresses.FirstOrDefault(i => i.SRNO == SRNO);
                    sCAddresses.Remove(address);
                }
            }
            return PartialView("PartialAddressmst", sCAddresses);
        }



        public async Task<IActionResult> PartialEquipment(string Tempscid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCEquipDtl?Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    equipmentlist = JsonConvert.DeserializeObject<List<SCEquipDtl>>(response.Result);
                    return PartialView("PartialEquipment", equipmentlist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialEquipment", equipmentlist);
        }
        public async Task<IActionResult> EquipmentMst(int eqipid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCEquipDtl?Tempscid=" + eqipid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    equipmentlist = JsonConvert.DeserializeObject<List<SCEquipDtl>>(response.Result);
                    return PartialView("PartialEquipment", equipmentlist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialEquipment", equipmentlist);
        }
        public async Task<PartialViewResult> Equipsc(string ScEquipDes, string ScEquipMake, string ScEquipModel, string ScEquipNo, int Srno, int Equipid)
        {
            if (Srno > 0)
            {
                equipmentlist.Add(new SCEquipDtl()
                {
                    SrNo = Srno,
                    SCEquipId = Equipid,
                    ScEquipDesc = ScEquipDes,
                    ScEquipMake = ScEquipMake,
                    ScEquipModel = ScEquipModel,
                    ScEquipNo = ScEquipNo,

                });
            }
            return PartialView("PartialEquipment", equipmentlist);

        }
        public async Task<PartialViewResult> EquipmentUpdate(string ScEquipDes, string ScEquipMake, string ScEquipModel, string ScEquipNo, int SrNo, int Equipid)
        {
            if (SrNo > 0)
            {
                SCEquipDtl sCEquipDtl = equipmentlist.FirstOrDefault(i => i.SrNo == SrNo);
                if (sCEquipDtl != null)
                {
                    {
                        sCEquipDtl.ScEquipDesc = ScEquipDes;
                        sCEquipDtl.ScEquipMake = ScEquipMake;
                        sCEquipDtl.ScEquipModel = ScEquipModel;
                        sCEquipDtl.ScEquipNo = ScEquipNo;

                    };
                };
            }
            else
            {
                SCEquipDtl sCEquipDtl = equipmentlist.FirstOrDefault(i => i.SCEquipId == Equipid);
                if (sCEquipDtl != null)
                {
                    {
                        sCEquipDtl.ScEquipDesc = ScEquipDes;
                        sCEquipDtl.ScEquipMake = ScEquipMake;
                        sCEquipDtl.ScEquipModel = ScEquipModel;
                        sCEquipDtl.ScEquipNo = ScEquipNo;

                    };
                };
            }
            return PartialView("PartialEquipment", equipmentlist);

        }
        public IActionResult EquipDelete(int SRNO, int SCEquipId)
        {
            if (SRNO == 0)
            {
                SCEquipDtl equipDtl = equipmentlist.FirstOrDefault(i => i.SCEquipId == SCEquipId);
                equipmentlist.Remove(equipDtl);
                return PartialView("PartialEquipment", equipmentlist);
            }
            else
            {
                SCEquipDtl equipDtl = equipmentlist.FirstOrDefault(i => i.SrNo == SRNO);
                equipmentlist.Remove(equipDtl);
                return PartialView("PartialEquipment", equipmentlist);
            }
        }



        public async Task<IActionResult> PartialAssociate(int Tempscid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCAssCompDtl?Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    AssCompslist = JsonConvert.DeserializeObject<List<SCAssComp>>(response.Result);
                    return PartialView("PartialAssociate", AssCompslist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialAssociate", AssCompslist);
        }
        public async Task<IActionResult> AssociateMst(int eqipid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCAssCompDtl?Tempscid=" + eqipid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    AssCompslist = JsonConvert.DeserializeObject<List<SCAssComp>>(response.Result);
                    return PartialView("PartialAssociate", AssCompslist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialAssociate", AssCompslist);
        }
        public async Task<PartialViewResult> Assocompy(string Asscompany, int SrNo, int SCAssCmpId)
        {
            if (SrNo > 0)
            {
                AssCompslist.Add(new SCAssComp()
                {
                    AssCompName = Asscompany,
                    SCAssCmpId = SCAssCmpId,
                    SrNo = SrNo,
                });
            }
            return PartialView("PartialAssociate", AssCompslist);
        }
        public async Task<PartialViewResult> AsscocompyUpdate(string Asscompany, int SrNo, int SCAssCmpId)
        {
            if (SrNo > 0)
            {
                SCAssComp sCAssComp = AssCompslist.FirstOrDefault(i => i.SrNo == SrNo);

                if (sCAssComp != null)
                {
                    {
                        sCAssComp.AssCompName = Asscompany;
                    }

                };
            }
            else
            {
                SCAssComp sCAssComp = AssCompslist.FirstOrDefault(i => i.SCAssCmpId == SCAssCmpId);

                if (sCAssComp != null)
                {
                    {
                        sCAssComp.AssCompName = Asscompany;
                    }

                };
            }
            return PartialView("PartialAssociate", AssCompslist);
        }
        public IActionResult AssoDelete(int SRNO, int SCAssoId)
        {
            if (SRNO == 0)
            {
                SCAssComp comp = AssCompslist.FirstOrDefault(i => i.SCAssCmpId == SCAssoId);
                AssCompslist.Remove(comp);
                return PartialView("PartialAssociate", AssCompslist);
            }
            else
            {
                SCAssComp comlist = AssCompslist.FirstOrDefault(i => i.SrNo == SRNO);
                AssCompslist.Remove(comlist);
                return PartialView("PartialAssociate", AssCompslist);
            }
        }



        public async Task<IActionResult> PartialExperience(int Tempscid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCProfileWODtl?Tempscid=" + Tempscid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    experiencelist = JsonConvert.DeserializeObject<List<SCProfileWODtl>>(response.Result);
                    return PartialView("PartialExperience", experiencelist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialExperience", experiencelist);
        }
        public async Task<IActionResult> ExperienceMst(int eqipid)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/GetSCProfileWODtl?Tempscid=" + eqipid;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    experiencelist = JsonConvert.DeserializeObject<List<SCProfileWODtl>>(response.Result);
                    return PartialView("PartialExperience", experiencelist);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("PartialExperience", experiencelist);
        }
        public async Task<PartialViewResult> experiencesc(string Site, string Projname, int Value, string Client, string State, string Wodes, int SrNo, int SCWOId)
        {
            if (SrNo > 0)
            {
                experiencelist.Add(new SCProfileWODtl()
                {
                    SrNo = SrNo,
                    SCProfWOId = SCWOId,
                    ProjSite = Site,
                    ProjName = Projname,
                    WOValue = Value,
                    ProjClient = Client,
                    States = State,
                    WODesc = Wodes,
                });
            }
            return PartialView("PartialExperience", experiencelist);
        }
        public async Task<PartialViewResult> ExperienceUpdate(int SrNo, string Site, string Projname, int Value, string Client, string State, string Wodes, int SCWOId)
        {
            if (SrNo > 0)
            {
                SCProfileWODtl sCProfileWODtl = experiencelist.FirstOrDefault(i => i.SrNo == SrNo);

                if (sCProfileWODtl != null)
                {
                    {
                        sCProfileWODtl.ProjSite = Site;
                        sCProfileWODtl.ProjName = Projname;
                        sCProfileWODtl.WOValue = Value;
                        sCProfileWODtl.ProjClient = Client;
                        sCProfileWODtl.States = State;
                        sCProfileWODtl.WODesc = Wodes;
                    }

                };
            }
            else
            {
                SCProfileWODtl sCProfileWODtl = experiencelist.FirstOrDefault(i => i.SCProfWOId == SCWOId);

                if (sCProfileWODtl != null)
                {
                    {
                        sCProfileWODtl.ProjSite = Site;
                        sCProfileWODtl.ProjName = Projname;
                        sCProfileWODtl.WOValue = Value;
                        sCProfileWODtl.ProjClient = Client;
                        sCProfileWODtl.States = State;
                        sCProfileWODtl.WODesc = Wodes;
                    }

                };

            }
            return PartialView("PartialExperience", experiencelist);
        }
        public IActionResult ExperiDelete(int SRNO, int SCExperiId)
        {
            if (SRNO == 0)
            {
                SCProfileWODtl sCProfile = experiencelist.FirstOrDefault(i => i.SCProfWOId == SCExperiId);
                experiencelist.Remove(sCProfile);
                return PartialView("PartialExperience", experiencelist);
            }
            else
            {
                SCProfileWODtl sCProfile = experiencelist.FirstOrDefault(i => i.SrNo == SRNO);
                experiencelist.Remove(sCProfile);
                return PartialView("PartialExperience", experiencelist);
            }
        }



        // FOR Viewing Pdf And Upload
        public IActionResult View(int SRNO, int Attachid)
        {
            if (SRNO == 0)
            {
                AttachDtl attach = _pdfFiles.FirstOrDefault(f => f.AttId == Attachid);
                if (attach != null)
                {
                    return new FileContentResult(attach.AttachDoc, "application/pdf");
                }
            }
            var file = _pdfFiles.FirstOrDefault(f => f.SrNo == SRNO);
            byte[] fileBytes = System.IO.File.ReadAllBytes(file.Path);

            return new FileContentResult(fileBytes, "application/pdf");
        }
        public IActionResult PageAttachPartial()
        {
            return PartialView("AttachPartial", _pdfFiles);
        }

        [HttpPost]
        public IActionResult UpdateAttachPartial(int AttachSrNo, int AttachId, IFormFile file, string CustomName)
        {
            try
            {
                AttachDtl existingFile;

                if (AttachSrNo>0)
                {
                    existingFile = _pdfFiles.FirstOrDefault(f => f.SrNo == AttachSrNo);
                }
                else if (AttachId > 0)
                {
                    existingFile = _pdfFiles.FirstOrDefault(f => f.AttId == AttachId);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid update parameters.");
                    return PartialView("AttachPartial", _pdfFiles);
                }

                if (existingFile == null)
                {
                    ModelState.AddModelError("", "File not found for update.");
                    return PartialView("AttachPartial", _pdfFiles);
                }

                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Path.GetTempPath(), fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    byte[] fileContent;
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        fileContent = memoryStream.ToArray();
                    }

                    // Update existing file properties
                    existingFile.FileName = fileName;
                    existingFile.AttachDoc = fileContent;
                    existingFile.AttachDocName = CustomName;
                    existingFile.Path = filePath;

                    // Validate the model
                    var validationContext = new ValidationContext(existingFile);
                    var validationResults = new List<ValidationResult>();
                    bool isValid = Validator.TryValidateObject(existingFile, validationContext, validationResults, true);

                    if (ModelState.IsValid)
                    {
                        // Additional validation for PDF file extension and size
                        if (!string.Equals(Path.GetExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            ModelState.AddModelError("", "Only PDF files are allowed.");
                            return PartialView("AttachPartial", _pdfFiles);
                        }

                        const int maxFileSize = 1 * 1024 * 1024; // 1MB

                        if (file.Length > maxFileSize)
                        {
                            ModelState.AddModelError("", "File size should not exceed 1MB.");
                            return PartialView("AttachPartial", _pdfFiles);
                        }
                    }
                    else
                    {
                        // Model validation failed, add error messages to ModelState
                        foreach (var validationResult in validationResults)
                        {
                            ModelState.AddModelError("", validationResult.ErrorMessage);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please select a file for update.");
                }

                return PartialView("AttachPartial", _pdfFiles);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an appropriate response
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpPost]
        public IActionResult AttachPartial(IFormFile file, string CustomName, int SrNo)
        {

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Path.GetTempPath(), fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                byte[] fileContent;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                var pdfFile = new AttachDtl
                {
                    SrNo = SrNo,
                    FileName = fileName,
                    AttachDoc = fileContent,
                    AttachDocName = CustomName,
                    Path = filePath,
                };

                // Perform server-side validation
                var validationContext = new ValidationContext(pdfFile);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(pdfFile, validationContext, validationResults, true);

                if (ModelState.IsValid)
                {
                    // Additional validation for PDF file extension and size
                    if (!string.Equals(Path.GetExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError("", "Only PDF files are allowed.");
                        return PartialView("AttachPartial", _pdfFiles);
                    }

                    // Specify the maximum allowed file size (in bytes)
                    const int maxFileSize = 1 * 1024 * 1024; // 5MB

                    if (file.Length > maxFileSize)
                    {
                        ModelState.AddModelError("", "File size should not exceed 1MB.");
                        return PartialView("AttachPartial", _pdfFiles);
                    }

                    _pdfFiles.Add(pdfFile);
                }
                else
                {
                    // Model validation failed, add error messages to ModelState
                    foreach (var validationResult in validationResults)
                    {
                        ModelState.AddModelError("", validationResult.ErrorMessage);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select a file.");
            }

            return PartialView("AttachPartial", _pdfFiles);
        }

        public IActionResult AttachDelete(int SRNO, int Attachid)
        {
            if (SRNO == 0)
            {
                AttachDtl attach = _pdfFiles.FirstOrDefault(f => f.AttId == Attachid);
                _pdfFiles.Remove(attach);
                return PartialView("AttachPartial", _pdfFiles);
            }
            else
            {
                AttachDtl attach = _pdfFiles.FirstOrDefault(f => f.SrNo == SRNO);
                _pdfFiles.Remove(attach);
                return PartialView("AttachPartial", _pdfFiles);
            }
        }
        public async Task<IActionResult> Attach(int SCPANAttach, int SCGSTAttach, int SCPFAttach, int SCESIAttach,
                                                 int SCCHQAttac, int MandateAttach, int MSMEAttach, int SCAttch)
        {
            try
            {
                GetBaseUrl();
                //string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/AttachFile?SCPANAttach=" + SCPANAttach + "&SCGSTAttach=" + SCGSTAttach + "&SCPFAttach=" + SCPFAttach + "&SCESIAttach=" + SCESIAttach + "&SCCHQAttac=" + SCCHQAttac + "&MandateAttach=" + MandateAttach + "&MSMEAttach=" + MSMEAttach + "&SCAttch=" + SCAttch;
                APIResponse_IO response = await _apiservice.CalAPI(sBaseUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _pdfFiles = JsonConvert.DeserializeObject<List<AttachDtl>>(response.Result);
                    return PartialView("AttachPartial", _pdfFiles);
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
            return PartialView("AttachPartial", _pdfFiles);
        }

        //Insert SC data While Preparation
        [HttpPost]
        public async Task<IActionResult> Insert(int Tempid, bool Activest, int SCstatus, string Site, string SCName, int Compy, string CIINo, string Pan, int Gststatus, string State, string Gstno, int MeseStatus, int EnterPrise
            , string Regno, string Pfno, string Esic, string SIte, string Bank, string AC, string Ifsc, string Industry, string Reffered, string GstText, string Note
             , int Year1, double Turn1, int Year2, double Turn2, int Year3, double Turn3)
        {
            try
            {
                List<string> missingAttachments = new List<string>();

                if (!IsAttachmentValid("PAN Card")) missingAttachments.Add("PAN Card");
                if (!IsAttachmentValid("SC Reg Form ")) missingAttachments.Add("SC Reg Form ");
                if (!IsAttachmentValid("GSTIN Ack")) missingAttachments.Add("GSTIN Ack");
                if (!IsAttachmentValid("MSME")) missingAttachments.Add("MSME");
                if (!IsAttachmentValid("Cheque Copy*")) missingAttachments.Add("Cheque Copy*");
                if (!IsAttachmentValid("Mandate Form")) missingAttachments.Add("Mandate Form");
                if (!string.IsNullOrEmpty(Pfno))
                {
                    if (!IsAttachmentValid("PF Regn.Certificate")) missingAttachments.Add("PF Regn.Certificate");
                }

                if (!string.IsNullOrEmpty(Esic))
                {
                    if (!IsAttachmentValid("ESIC Regn.Certificate")) missingAttachments.Add("ESIC Regn.Certificate");
                }

                if (missingAttachments.Count > 0)
                {
                    var dataAsString = string.Join(", ", missingAttachments);
                    return Json(new { success = false, message = "Attachments are missing: " + dataAsString });
                }

                scmstreq.Add(new SCMstReq()
                {
                    ReqSite = "NMIA",
                    SCName = SCName,
                    SCCompType = Compy,
                    SCCIN = CIINo,
                    SCPAN = Pan,
                    SCGSTStatus = Gststatus,
                    SCRegdState = State,
                    SCGST = Gstno,
                    MSMEStatus = MeseStatus,
                    RegisterNo = Regno,
                    SCPF = Pfno,
                    SCESI = Esic,
                    BankSite = "NMIA",
                    SCBANK = Bank,
                    SCBANKAcNo = AC,
                    SCBANKIFSC = Ifsc,
                    SCExpConstInd = Industry,
                    Remarks = Note,
                    ReferredBy = Reffered,
                    ActStatus = Activest,
                    SCStatus = SCstatus,
                    GSTStatus = GstText,
                    TmpScId = Tempid,
                    Category = EnterPrise,
                    Year1 = Year1,
                    Year2 = Year2,
                    Year3 = Year3,
                    Turnover1 = Turn1,
                    Turnover2 = Turn2,
                    Turnover3 = Turn3,
                    AddedBy = Ucode,

                });
                SCMstlist.Clear();
                data.scmstlist = SCMstlist;
                GetBaseUrl();
                string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/PendingInsert";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = response.Result;
                    if (result == "1")
                    {
                        string sMsg = "<html> <body> <p> Dear Sir, </p> ";
                        sMsg = sMsg + "<p> <br> SC Creation details are as under <br>";
                        sMsg = sMsg + "<br> SC Name: " + SCName;
                        sMsg = sMsg + "<br> Site : " + Site;
                        sMsg = sMsg + "<br> GST Status: " + GstText;
                        sMsg = sMsg + "<br> Requested On: " + DateTime.Now.ToShortDateString();
                        sMsg = sMsg + "<br> ";
                        sMsg = sMsg + "<br> <br> Regards, <br> " + Username;
                        sMsg = sMsg + "<br> Balajee Infratech & Constructions Pvt. Ltd. ";
                        sMsg = sMsg + "<br> " + Site;
                        sMsg = sMsg + "</p> </body> </html>";
                        int iMail = SCEmail.fnSCEmail(sMsg, "SC Request For " + SCName, Username, 8, 2);
                        if (iMail == 1)
                        {
                            TempData["Success"] = "SC Creation Mail Sent Successfully.";
                            ClearList();
                            return Json(new { success = true, message = " " });
                        }
                        else
                        {
                            TempData["Error"] = "But Mail Could not be sent due to Technical Error. ";
                            ClearList();
                            return Json(new { success = true, message = " " });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "But Mail Could not be sent due to Technical Error. ";
                        return Json(new { success = true, message = " " });
                    }
                }
                else
                {
                    TempData["Error"] = ("APIrespone Failed");
                    return Json(new { success = true, message = " " });
                }

            }
            catch (Exception ex)
            {
                var error = "Please Report Error Message. " + ex.Message;
                return Json(new { success = true, message = error });
            }
        }

        //Verification for SC by SC Team And Update Data
        [HttpPost]
        public async Task<IActionResult> Verification(VmSC vm)
        {
            try
            {
                if (vm.scmst.SCStatus == 1)
                {
                    vm.scmst.SCStatus = 2;
                }
                List<SCMst> Sclist = new List<SCMst>();
                SCMst = vm.scmst;
                SCMst.VerifiedBy = Ucode;
                Sclist.Add(SCMst);
                data.scmstlist = Sclist;
                GetBaseUrl();
                string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/Verification";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string result = response.Result;
                    if (result == "2")
                    {
                        string sMsg = "<html> <body> <p> Dear Sir, </p> ";
                        sMsg = sMsg + "<p> <br> Please be informed that following SC has been Verified. The details are as under; <br>";
                        sMsg = sMsg + "<br> SC  Name: " + data.scmstlist[0].SCName;
                        int gststatus = data.scmstlist[0].SCGSTStatus;
                        sMsg = sMsg + "<br> GST Status: " + Vmscget.GST.FirstOrDefault(x => x.Value == gststatus.ToString());
                        sMsg = sMsg + "<br> Site : " + data.scmstlist[0].ReqSite;
                        sMsg = sMsg + "<br> ";
                        sMsg = sMsg + "<br> <br> Regards, <br> " + Username;
                        sMsg = sMsg + "<br> Balajee Infratech & Constructions Pvt. Ltd. ";
                        sMsg = sMsg + "<br> " + Site;
                        sMsg = sMsg + "</p> </body> </html>";

                        int iMail = SCEmail.fnSCEmail(sMsg, "SC Request Verified For " + data.scmstlist[0].SCName, Username, 9, 2);
                        if (iMail == 1)
                        {
                            TempData["Success"] = "SC Verification Mail Sent Successfully.";
                            ClearList();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = " SC Verification Successfully .But Mail Could not be sent due to Technical Error. ";
                            ClearList();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "But Mail Could not be sent due to Technical Error. ";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return Json("APIrespone Failed");
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
        }

        //Final Approval for SC only SC REG nO. Allocated
        [HttpPost]
        public async Task<IActionResult> Approval(VmSC vm)
        {
            try
            {
                if (vm.scmst.SCStatus == 2)
                {
                    vm.scmst.SCStatus = 3;
                }
                List<SCMst> Sclist = new List<SCMst>();
                SCMst = vm.scmst;
                SCMst.AddedBy = Ucode;
                Sclist.Add(SCMst);
                data.scmstlist = Sclist;
                GetBaseUrl();
                string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/SCMstInsert";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, data);
                string SCRegNo = string.Empty;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string ResponeData = response.Result;
                    string result = ResponeData.Substring(0, 1);
                    SCRegNo = ResponeData.Substring(3);
                    if (result == "3")
                    {
                        string sMsg = "<html> <body> <p> Dear Sir, </p> ";
                        sMsg = sMsg + "<p> <br> Please be informed that SC Reg No  of following SC has been Approved. The details are as under; <br>";
                        sMsg = sMsg + "<br> SC  Name: " + data.scmstlist[0].SCName;
                        sMsg = sMsg + "<br> SC Reg. No. : " + SCRegNo;
                        sMsg = sMsg + "<br> Site : " + data.scmstlist[0].ReqSite;
                        sMsg = sMsg + "<br> ";
                        sMsg = sMsg + "<br> <br> Regards, <br> " + Username;
                        sMsg = sMsg + "<br> Balajee Infratech & Constructions Pvt. Ltd. ";
                        sMsg = sMsg + "<br> " + Site;
                        sMsg = sMsg + "</p> </body> </html>";
                        int iMail = SCEmail.fnSCEmail(sMsg, "SC Request Approved For " + data.scmstlist[0].SCName, Username, 9, 2);
                        if (iMail == 1)
                        {
                            TempData["Success"] = "SC Approved Mail Sent Successfully. SC Reg. No is -" + SCRegNo;
                            ClearList();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = " SC Approved Successfully. But Mail Could not be sent due to Technical Error. ";
                            ClearList();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "But Mail Could not be sent due to Technical Error. ";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return Json("APIrespone Failed");
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
        }

        //Both Reject Implemented (SC Team And SC Manager)
        [HttpPost]
        public async Task<IActionResult> RejectSC(VmSC vm)
        {
            try
            {
                if (vm.scmst.SCStatus == 1)
                {
                    vm.scmst.SCStatus = 4;
                }
                else
                {
                    vm.scmst.SCStatus = 5;
                }
                List<SCMst> Sclist = new List<SCMst>();
                SCMst = vm.scmst;
                SCMst.AddedBy = Ucode;
                Sclist.Add(SCMst);
                data.scmstlist = Sclist;
                GetBaseUrl();
                string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/RejectSC";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl, data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = 1;
                    var SCstatus = response.Result;
                    var check = "4";
                    if (result == 1)
                    {
                        string sMsg = "<html> <body> <p> Dear Sir, </p> ";
                        if (SCstatus == check)
                        {
                            sMsg = sMsg + "<p> <br> Please be informed that following SC has been Rejected by Verification Authority. The details are as under; <br>";
                        }
                        else
                        {
                            sMsg = sMsg + "<p> <br> Please be informed that following SC has been Rejected by Approval Authority. The details are as under; <br>";
                        }
                        sMsg = sMsg + "<br> SC  Name: " + data.scmstlist[0].SCName;
                        sMsg = sMsg + "<br> Site : " + data.scmstlist[0].ReqSite;
                        sMsg = sMsg + "<br> ";
                        sMsg = sMsg + "<br> <br> Regards, <br> " + Username;
                        sMsg = sMsg + "<br> Balajee Infratech & Constructions Pvt. Ltd. ";
                        sMsg = sMsg + "<br> " + Site;
                        sMsg = sMsg + "</p> </body> </html>";
                        int iMail = SCEmail.fnSCEmail(sMsg, "SC Request Rejected For " + data.scmstlist[0].SCName, Username, 9, 2);
                        if (iMail == 1)
                        {
                            ClearList();
                            TempData["Success"] = "SC Rejected Mail Sent Successfully.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ClearList();
                            TempData["Error"] = " SC Rejected Successfully. But Mail Could not be sent due to Technical Error. ";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "But Mail Could not be sent due to Technical Error. ";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return Json("APIrespone Failed");
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
        }

        //Update ALL Data for SC Creation USER
        [HttpPost]
        public async Task<IActionResult> UpdateSC(VmSC vm)
        {
            try
            {
                List<SCMst> sclist = new List<SCMst>();
                SCMst = vm.scmst;
                SCMst.AddedBy = Ucode;
                sclist.Add(SCMst);
                data.scmstlist = sclist;
                GetBaseUrl();
                string sTokenUrl = sBU + "GenerateToken" + "?sUsrCode=" + sUserCode + "&iSiteID=" + iSiteId;
                string sBaseUrl = sBU + "api/SC/UpdateData";
                APIResponse_IO response = await _apiservice.PostAPI(sBaseUrl,data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ClearList();
                    TempData["Success"] = response.Result;

                    return RedirectToAction("Index");
                }
                else
                {
                    return Json("APIrespone Failed");
                }
            }
            catch (Exception ex)
            {
                return Json("Please Report Error Message. " + ex.Message);
            }
        }

        private void ClearList()
        {
            scmstreq.Clear(); sCAddresses.Clear(); AssCompslist.Clear(); equipmentlist.Clear(); experiencelist.Clear(); _pdfFiles.Clear();
        }
        public IActionResult Refresh()
        {
            ClearList();
            return Ok();
        }
        private bool IsAttachmentValid(string attachmentName)
        {
            foreach (var subitem in data.AttachFile)
            {
                if (subitem.AttachDocName == attachmentName)
                {
                    return true;
                }
            }
            return false;
        }


    }
}