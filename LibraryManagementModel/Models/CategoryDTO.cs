using Newtonsoft.Json;

namespace LibraryManagementModel.Models
{
    public class CategoryDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isDeleted", NullValueHandling = NullValueHandling.Ignore)]
        public Nullable<bool> IsDeleted { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public Nullable<DateTime> ModifiedDate { get; set; }

        [JsonProperty("books", NullValueHandling = NullValueHandling.Ignore)]
        public List<BookDTO> Books { get; set; }
    }
}
