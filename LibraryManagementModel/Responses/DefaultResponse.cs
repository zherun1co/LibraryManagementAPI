using Newtonsoft.Json;

namespace LibraryManagementModel.Responses
{
    public class DefaultResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}