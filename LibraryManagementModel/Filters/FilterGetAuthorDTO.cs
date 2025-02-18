using Newtonsoft.Json;

namespace LibraryManagementModel.Filters
{
    public class FilterGetAuthorDTO
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("isDeleted")]
        public Nullable<bool> IsDeleted { get; set; }

        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }
    }
}