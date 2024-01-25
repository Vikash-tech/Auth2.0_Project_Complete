
using IMS_Model.CommonModel;
using IMS_Model.LoginModel;
using IMS_Model.SC.SCCreation;

namespace IMS.Service
{
    public interface IAPIService
	{
        //Login 
        public Task<APIResponse_IO> PostAPI(string sUrl, Login login);

        //GET Method For API
        public Task<APIResponse_IO> CalAPI(string sUrl);

        //POST Method For API
        public Task<APIResponse_IO> PostAPI(string sUrl, APIResponse_IO data);

        //SC
        public Task<APIResponse_IO> PostAPI(string sUrl, DataModel data);

   
    }
}
