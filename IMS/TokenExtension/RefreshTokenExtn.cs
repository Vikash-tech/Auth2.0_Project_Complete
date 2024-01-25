using IMS.Models;
using IMS_Model.CommonModel;
using Newtonsoft.Json;
using System.Net;

namespace IMS.TokenExtension
{
    public class RefreshTokenExtn
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private static readonly string LogFilePath = "path/to/logfile.txt";

        public RefreshTokenExtn(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<APIResponse_IO> CalAPITokenAsync()
        {
            APIResponse_IO getapiresponse = new APIResponse_IO();

            try
            {
                string tokenExpiry = _httpContextAccessor.HttpContext.Session.GetString("TokenExpiry");
                string currentDate = DateTime.Now.ToString();
                string refresherTokenExpiry = _httpContextAccessor.HttpContext.Session.GetString("RefresherTokenExpiry");
                string refresherToken = _httpContextAccessor.HttpContext.Session.GetString("RefresherToken");
                string userName = _httpContextAccessor.HttpContext.Session.GetString("UserCode");

                if (Convert.ToDateTime(tokenExpiry) < Convert.ToDateTime(currentDate))
                {
                    string baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value;
                    string apiUrl = $"{baseUrl}api/UserLogin/GetTokenFromRefresher?UserName={userName}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            getapiresponse = JsonConvert.DeserializeObject<APIResponse_IO>(apiResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                getapiresponse.Result = ex.Message.ToString();
                getapiresponse.StatusCode = HttpStatusCode.NotFound;
            }

            return getapiresponse;
        }

        public void LogException(Exception ex)
        {
            try
            {
                ExceptionLogModel logModel = new ExceptionLogModel(ex);
                Console.WriteLine(logModel.ToString());
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error logging exception: {logEx.Message}");
            }
        }
    }


}
