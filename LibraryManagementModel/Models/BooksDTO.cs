using Newtonsoft.Json;
using LibraryManagementModel.Commons;

namespace LibraryManagementModel.Models
{
    public class BooksDTO
    {
        [JsonProperty("books")]
        public List<BookDTO> Books { get; set; }

        [JsonProperty("paging")]
        public PagingDTO Paging { get; set; }
    }
}