using Microsoft.EntityFrameworkCore;
using LibraryManagementModel.Entities;

namespace LibraryManagementRepository.Repositories.DatabaseContext
{
    public class LibraryManagementContext(DbContextOptions<LibraryManagementContext> options) : DbContext(options)
    {
        public DbSet<AuthorEntity> AuthorEntity { get; set; }
        public DbSet<BookEntity> BookEntity { get; set; }
        public DbSet<CategoryEntity> CategoryEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure AuthorEntity (One-to-Many with BookEntity)
            modelBuilder.Entity<AuthorEntity>(entity =>
            {
                entity.ToTable("Author");
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(a => a.DateOfBirth)
                      .IsRequired(false);

                entity.Property(a => a.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(a => a.CreatedDate)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(a => a.ModifiedDate)
                      .IsRequired(false);
            });

            // Configure BookEntity (Many-to-Many with CategoryEntity)
            modelBuilder.Entity<BookEntity>(entity =>
            {
                entity.ToTable("Book");
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Title)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(b => b.AuthorId)
                      .IsRequired();

                entity.Property(b => b.PublishedDate)
                      .IsRequired(false);

                entity.Property(b => b.Genere)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(b => b.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(b => b.CreatedDate)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(b => b.ModifiedDate)
                      .IsRequired(false);

                entity.HasOne(b => b.Author)
                      .WithMany(a => a.Books)
                      .HasForeignKey(b => b.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(b => b.Categories)
                      .WithMany(c => c.Books)
                      .UsingEntity(j => j.ToTable("BookCategory"));
            });

            // Configure CategoryEntity
            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(c => c.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(c => c.CreatedDate)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(c => c.ModifiedDate)
                      .IsRequired(false);
            });
        }
    }
}
