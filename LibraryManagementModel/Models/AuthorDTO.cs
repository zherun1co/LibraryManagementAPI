using Newtonsoft.Json;

namespace LibraryManagementModel.Models
{
    public class AuthorDTO
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dateOfBirth")]
        public Nullable<DateTime> DateOfBirth { get; set; }
        
        [JsonProperty("isDeleted")]
        public Nullable<bool> IsDeleted { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modifiedDate")]
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}