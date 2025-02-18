using Newtonsoft.Json;

namespace LibraryManagementModel.Models
{
    public class BookCategoryDTO
    {
        [JsonProperty("id")]
        public Nullable<Guid> Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}