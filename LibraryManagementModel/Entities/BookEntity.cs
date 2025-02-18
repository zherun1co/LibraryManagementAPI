namespace LibraryManagementModel.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Genere { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public AuthorEntity Author { get; set; }
        public List<CategoryEntity> Categories { get; set; }
    }
}