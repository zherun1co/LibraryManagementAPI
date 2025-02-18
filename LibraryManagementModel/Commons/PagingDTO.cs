using Newtonsoft.Json;

namespace LibraryManagementModel.Commons
{
    public class PagingDTO
    {
        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }
    }
}