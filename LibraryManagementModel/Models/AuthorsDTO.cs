using Newtonsoft.Json;
using LibraryManagementModel.Commons;

namespace LibraryManagementModel.Models
{
    public class AuthorsDTO
    {
        [JsonProperty("authors")]
        public List<AuthorDTO> Authors { get; set; }

        [JsonProperty("paging")]
        public PagingDTO Paging { get; set; }
    }
}