using IMS.TokenExtension;
using IMS_Model.CommonModel;
using IMS_Model.LoginModel;
using IMS_Model.SC.SCCreation;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace IMS.Service
{
    public class APIService : IAPIService
    {
        private readonly RefreshTokenExtn _refreshTokenExtn;

        public APIService(RefreshTokenExtn refreshTokenExtn)
        {
            _refreshTokenExtn = refreshTokenExtn;
        }

        //Login 
        public async Task<APIResponse_IO> PostAPI(string sUrl, Login login)
        {
            APIResponse_IO getapiresponse = new APIResponse_IO();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (HttpClient client = new HttpClient(clientHandler))
                {

                    HttpResponseMessage response = await client.PostAsJsonAsync(sUrl, login);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        getapiresponse = JsonConvert.DeserializeObject<APIResponse_IO>(apiResponse);
                    }

                }
            }
            catch (Exception ex)
            {
                getapiresponse.Result = ex.Message.ToString();
                getapiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            return getapiresponse;
        }

        //Common GET API For All Method
        public async Task<APIResponse_IO> CalAPI(string sUrl)
        {
            APIResponse_IO getapiresponse = new APIResponse_IO();
            try
            {
                var newtoken = await _refreshTokenExtn.CalAPITokenAsync();
                if (newtoken != null)
                {
                    if (newtoken.Token != null)
                    {
                        string sToken = newtoken.Token.ToString();

                        HttpClientHandler clientHandler = new HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                        using (HttpClient client = new HttpClient(clientHandler))
                        {

                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sToken);
                            HttpResponseMessage response = await client.GetAsync(sUrl);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                getapiresponse = JsonConvert.DeserializeObject<APIResponse_IO>(apiResponse);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                getapiresponse.Result = ex.Message.ToString();
                getapiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            return getapiresponse;
        }

        //Common POST API For All Method
        public async Task<APIResponse_IO> PostAPI(string sUrl, APIResponse_IO data)
        {
            APIResponse_IO getapiresponse = new APIResponse_IO();
            try
            {
                var newtoken = await _refreshTokenExtn.CalAPITokenAsync();
                if (newtoken != null)
                {
                    if (newtoken.Token != null)
                    {
                        string sToken = newtoken.Token.ToString();

                        HttpClientHandler clientHandler = new HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                        using (HttpClient client = new HttpClient(clientHandler))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sToken);
                            HttpResponseMessage response = await client.PostAsJsonAsync(sUrl, data);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                getapiresponse = JsonConvert.DeserializeObject<APIResponse_IO>(apiResponse);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                getapiresponse.Result = ex.Message.ToString();
                getapiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            return getapiresponse;
        }

       

        //sc
        public async Task<APIResponse_IO> PostAPI(string sUrl, DataModel data)
        {
            APIResponse_IO getapiresponse = new APIResponse_IO();
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(sUrl, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        getapiresponse = JsonConvert.DeserializeObject<APIResponse_IO>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                getapiresponse.Result = ex.Message.ToString();
                getapiresponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            return getapiresponse;
        }


    }
}


