using Newtonsoft.Json;

namespace LibraryManagementModel.Models
{
    public class AuthorBookDTO
    {
        [JsonProperty("id")]
        public Nullable<Guid> Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publishedDate")]
        public Nullable<DateTime> PublishedDate { get; set; }

        [JsonProperty("genere")]
        public string Genere { get; set; }

        [JsonProperty("categories")]
        public List<BookCategoryDTO> Categories { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public Nullable<DateTime> ModifiedDate { get; set; }

        [JsonProperty("isDeleted")]
        public Nullable<bool> IsDeleted { get; set; }
    }
}