using Newtonsoft.Json;

namespace LibraryManagementModel.Mappings
{
    public class BookCategoryMappingDTO
    {
        [JsonProperty("bookId")]
        public Guid BookId { get; set; }

        [JsonProperty("categoryId")]
        public Guid CategoryId { get; set; }
        
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
    }
}
