using IMS.Controllers.Common;
using IMS.Service;
using IMS_Model.CommonModel;
using IMS_Model.UserMaster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;

namespace IMS.Controllers
{
    public class CompConfgController : CommonController
    {
        public readonly IConfiguration _configuration;
        private readonly IAPIService _apiservice;
        public static string? sBU;
        public CompConfgController(IConfiguration configuration, IAPIService apiservice) : base(configuration, apiservice)
        {
            _configuration = configuration;
            _apiservice = apiservice;
        }
        public static UserMst userMst = new UserMst();
        public static CompConfg master = new CompConfg();

        public string GetBaseUrl()
        {
            string sBaseUsrl = _configuration.GetSection("appSettings")["BaseUrl"].ToString();
            sBU = sBaseUsrl;
            return sBaseUsrl;
        }

        public static List<CompConfg> compconfglist = new List<CompConfg>();

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> _CompConfgList()
        {
            GetBaseUrl();
            string sBaseUrl = sBU + "api/CompConfg/GetCompConfg";
            APIResponse response = await _apiservice.CalAPI(sBaseUrl);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                compconfglist = JsonConvert.DeserializeObject<List<CompConfg>>(response.Result);
            }
            return PartialView("_CompConfgList", compconfglist);
        }


        public async Task<IActionResult> CompConfgCreation()
        {

            ViewBag.CompMstddl = await GetCompMst();
            if (master != null)
            {
                return View(master);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CompConfgCreation(CompConfg Obj)
        {

            ViewBag.CompMstddl = await GetCompMst();
            if (ModelState.IsValid)
            {
                try
                {

                    // Ensure the file is not null
                    if (Obj.LogoFile != null)
                    {
                        // Check image size (15 KB max)
                        if (Obj.LogoFile.Length > 150 * 1024)
                        {
                            ModelState.AddModelError("LogoFile", "The JPG file size should not exceed 150 KB.");
                            return View(Obj);
                        }

                        // Read the file into a byte array
                        using (var ms = new MemoryStream())
                        {
                            await Obj.LogoFile.CopyToAsync(ms);
                            Obj.Logo = ms.ToArray(); // Assuming PdfData is a byte[] property in your CompConfg model
                        }

                        // Check image dimensions (adjust as needed)
                        using (var imageStream = new MemoryStream(Obj.Logo))
                        {
                            var image = Image.FromStream(imageStream);
                            int maxWidth = 150; // Adjust as needed
                            int maxHeight = 150; // Adjust as needed

                            if (image.Width > maxWidth || image.Height > maxHeight)
                            {
                                ModelState.AddModelError("LogoFile", $"The JPG dimensions should not exceed {maxWidth}x{maxHeight} Percentage.");
                                return View(Obj);
                            }
                        }
                    }
                    Obj.AddedBy = sUserCode;
                    Obj.FileName = Obj.LogoFile.FileName;

                    APIResponse aPI = new APIResponse();
                    aPI.Result = Obj;
                    GetBaseUrl();

                    string sBaseUrl = sBU + "api/CompConfg/InsertCompConfg";
                    //APIResponse response = await _apiservice.PostAPI(sBaseUrl, new APIResponse { Result = Obj });
                    APIResponse response = await _apiservice.PostAPI(sBaseUrl, aPI);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Success"] = response.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View(Obj);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error uploading PDF: " + ex.Message;
                    return View(Obj);
                }
            }

            return View(Obj);
        }



        public async Task<IActionResult> Edit(int id)
        {
            master = compconfglist.FirstOrDefault(x => x.ConfgId == id);
            if (master != null)
            {
                return RedirectToAction("CompConfgCreation");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CompConfg Obj)
        {

            try
            {

                ViewBag.CompMstddl = await GetCompMst();
                if (ModelState.IsValid)
                {
                    // Additional validation if needed

                    // Ensure the file is not null
                    if (Obj.LogoFile != null)
                    {
                        // Check image size (15 KB max)
                        if (Obj.LogoFile.Length > 150 * 1024)
                        {
                            ModelState.AddModelError("LogoFile", "The JPG file size should not exceed 150 KB.");
                            return View("CompConfgCreation", Obj);
                        }

                        // Read the file into a byte array
                        using (var ms = new MemoryStream())
                        {
                            await Obj.LogoFile.CopyToAsync(ms);
                            Obj.Logo = ms.ToArray();
                        }

                        // Check image dimensions (adjust as needed)
                        using (var imageStream = new MemoryStream(Obj.Logo))
                        {
                            var image = Image.FromStream(imageStream);
                            int maxWidth = 130; // Adjust as needed
                            int maxHeight = 130; // Adjust as needed

                            if (image.Width > maxWidth || image.Height > maxHeight)
                            {
                                ModelState.AddModelError("LogoFile", $"The JPG dimensions should not exceed {maxWidth}x{maxHeight} pixels.");
                                return View("CompConfgCreation", Obj);
                            }
                        }
                    }

                    Obj.AddedBy = sUserCode;
                    Obj.FileName = Obj.LogoFile?.FileName;

                    APIResponse aPI = new APIResponse();
                    aPI.Result = Obj;

                    GetBaseUrl();

                    string sBaseUrl = sBU + "api/CompConfg/UpdateCompConfg";

                    APIResponse response = await _apiservice.PostAPI(sBaseUrl, aPI);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Success"] = response.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = response.Message;
                        return View("CompConfgCreation", Obj);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred during the update: " + ex.Message;
            }

            return View("CompConfgCreation", Obj);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CompConfg master = new CompConfg();
            master = compconfglist.FirstOrDefault(x => x.ConfgId == id);

            master.AddedBy = (sUserCode);
            master.AddedOn = DateTime.Now;
            master.ActStatus = false;
            APIResponse aPI = new APIResponse();
            aPI.Result = master;
            GetBaseUrl();
            string sBaseUrl = sBU + "api/CompConfg/DeleteCompConfg";
            APIResponse response = await _apiservice.PostAPI(sBaseUrl, aPI);
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
    }
}
