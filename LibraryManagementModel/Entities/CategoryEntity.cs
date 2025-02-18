namespace LibraryManagementModel.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<BookEntity> Books { get; set; }
    }
}