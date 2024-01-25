using Newtonsoft.Json;
using System.Net;

namespace IMS_Model.CommonModel
{
    public class APIResponse_IO
    {
        [JsonProperty("result")]
        public dynamic Result { get; set; }

        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        public string? status { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }

        public DateTime? Expiration { get; set; }

        public bool? IsSuccess { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
