using Newtonsoft.Json;

namespace LibraryManagementModel.Filters
{
    public class FilterGetBookDTO
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("offset")]
        public int? Offset { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("isDeleted")]
        public Nullable<bool> IsDeleted { get; set; }
    }
}
