using Newtonsoft.Json;

namespace LibraryManagementModel.Filters
{
    public class FilterGetCategoryDTO
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("isDeleted")]
        public Nullable<bool> IsDeleted { get; set; }

        [JsonProperty("limit")]
        public Nullable<int> Limit { get; set; }
    }
}