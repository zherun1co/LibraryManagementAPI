using Dapper;
using Serilog;
using System.Data;
using static Dapper.SqlMapper;
using Microsoft.Data.SqlClient;
using LibraryManagementModel.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementModel.Filters;
using LibraryManagementModel.Commons;
using LibraryManagementModel.Mappings;
using LibraryManagementModel.Entities;
using Microsoft.Extensions.Configuration;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementRepository.Repositories.DatabaseContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryManagementRepository.Repositories.Repository
{
    public class BookRepository(LibraryManagementContext context, IConfiguration configuration) : IBookRepository
    {
        private readonly LibraryManagementContext context = context;
        private readonly Serilog.ILogger logger = Log.ForContext<BookRepository>();
        private readonly string connectionString = configuration.GetConnectionString("DefaultConnection");

        public BooksDTO GetBooks(FilterGetBookDTO filter)
        {
            SqlConnection connection = null;
            
            BooksDTO books = new();
            List<BookCategoryMappingDTO> listBookCategories = [];

            try
            {
                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using GridReader multi = connection.QueryMultiple(
                    "usp_get_books",
                    new {
                        AuthorName = filter.Author,
                        filter.Title,
                        CategoryName = filter.Category,
                        filter.IsDeleted,
                        filter.Offset,
                        filter.Limit
                    },
                    commandType: CommandType.StoredProcedure
                );

                books.Books = multi.Read<BookDTO>().ToList();

                listBookCategories = multi.Read<BookCategoryMappingDTO>().ToList();

                if (listBookCategories.Count > 0) {
                    foreach (BookDTO book in books.Books) {
                        book.Categories = listBookCategories.Where(o => o.BookId == book.Id).Select(i => new BookCategoryDTO {
                            Id = i.CategoryId,
                            Name = i.CategoryName
                        }).ToList();
                    }
                }

                books.Paging = multi.Read<PagingDTO>().SingleOrDefault();

                return books;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetBooks)} method of the BookRepository class");

                throw;
            }
            finally
            {
                if (connection != null) {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public List<AuthorBookDTO> GetBooksByAuthor(Guid idAuthor)
        {
            SqlConnection connection = null;

            List<AuthorBookDTO> books = [];
            List<BookCategoryMappingDTO> bookCategories = [];

            try
            {
                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using GridReader multi = connection.QueryMultiple(
                    "usp_get_books_by_author",
                    new { AuthorId = idAuthor },
                    commandType: CommandType.StoredProcedure
                );

                books = multi.Read<AuthorBookDTO>().ToList();

                if (books.Count > 0 && !multi.IsConsumed)
                    bookCategories = multi.Read<BookCategoryMappingDTO>().ToList();

                if (bookCategories.Count > 0) {
                    foreach (AuthorBookDTO book in books) {
                        book.Categories = bookCategories.Where(c => c.BookId == book.Id).Select(c => new BookCategoryDTO {
                            Id = c.CategoryId,
                            Name = c.CategoryName
                        }).ToList();
                    }
                }

                return books;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetBooksByAuthor)} method of the AuthorRepository class");

                throw;
            }
            finally
            {
                if (connection != null) {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public BookDTO GetBook(Guid idBook)
        {
            SqlConnection connection = null;
            BookDTO book = null;

            try
            {
                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var multi = connection.QueryMultiple(
                    "usp_get_book",
                    new { BookId = idBook },
                    commandType: CommandType.StoredProcedure
                );

                book = multi.Read<BookDTO>().FirstOrDefault();

                if (book != null && !multi.IsConsumed)
                    book.Categories = multi.Read<BookCategoryDTO>().ToList();

                return book;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetBook)} method of the BookRepository class");

                throw;
            }
            finally
            {
                if (connection != null) {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public BookDTO AddBook(BookDTO model)
        {
            try
            {
                BookEntity bookEntityExists = context.BookEntity.SingleOrDefault(o =>
                       o.Title.ToLower().Equals(model.Title.Trim().ToLower())
                    && o.AuthorId == model.AuthorId
                );

                if (bookEntityExists != null)
                    throw new ApplicationException($"The book '{bookEntityExists.Title}' already exists with the id {bookEntityExists.Id}.");

                model.Id = CreateBook(model);

                return GetBook(
                    model.Id.Value
                );
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(AddBook)} method of the BookRepository class");

                throw;
            }
        }

        public BookCategoryDTO AddCategoryBook(Guid bookId, BookCategoryDTO model)
        {
            SqlConnection connection = null;
            DynamicParameters parameters = new();

            try
            {
                bool bookExists = context.BookEntity.AsNoTracking().Any(b => b.Id == bookId && !b.IsDeleted);

                if (!bookExists)
                    throw new ApplicationException($"The book with ID {bookId} does not exist.");

                CategoryEntity categoryEntity = context.CategoryEntity
                    .AsNoTracking()
                    .SingleOrDefault(c => c.Id == model.Id)
                ?? throw new ApplicationException($"The category with ID {model.Id} does not exist.");

                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                parameters.Add("@BookId", bookId);
                parameters.Add("@CategoryId", model.Id);

                model.Name = connection.ExecuteScalar<string>(
                    "usp_post_category_book",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(AddCategoryBook)} method of the BookRepository class");

                throw;
            }
            finally
            {
                if (connection != null) {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        public BookDTO UpdateBook(BookDTO model)
        {
            try
            {
                BookEntity bookEntityExists = context.BookEntity.Find(model.Id)
                    ?? throw new ApplicationException($"Attempted to update non-existing book {model.Id}");

                if (!string.IsNullOrEmpty(model.Title)) {
                    BookEntity bookEntityTitleExists = context.BookEntity.SingleOrDefault(o =>
                            o.Id != model.Id
                        && o.Title.ToLower().Equals(model.Title.Trim().ToLower())
                        && o.AuthorId == model.AuthorId
                    );

                    if (bookEntityTitleExists != null)
                        throw new ApplicationException($"The book title already exists with the id {bookEntityTitleExists.Id}.");

                    bookEntityExists.Title = model.Title.Trim();
                }

                if (model.AuthorId.HasValue)
                    bookEntityExists.AuthorId = model.AuthorId.Value;

                if (model.PublishedDate.HasValue)
                    bookEntityExists.PublishedDate = model.PublishedDate.Value;

                if (!string.IsNullOrEmpty(model.Genere))
                    bookEntityExists.Genere = model.Genere.Trim();

                if (model.IsDeleted.HasValue)
                    bookEntityExists.IsDeleted = model.IsDeleted.Value;

                if (!string.IsNullOrEmpty(model.Title) || model.AuthorId.HasValue ||
                    model.PublishedDate.HasValue || !string.IsNullOrEmpty(model.Genere) ||
                    model.IsDeleted.HasValue) {

                    bookEntityExists.ModifiedDate = DateTime.UtcNow;

                    context.BookEntity.Update(bookEntityExists);
                    context.SaveChanges();

                    model.ModifiedDate = bookEntityExists.ModifiedDate;
                }

                model = context.BookEntity
                   .AsNoTracking()
                   .Where(w => w.Id == model.Id).Select(o => new BookDTO {
                       Id = o.Id,
                       Title = o.Title,
                       AuthorId = o.AuthorId,
                       Genere = o.Genere,
                       PublishedDate = DateTime.UtcNow,
                       IsDeleted = o.IsDeleted,
                       CreatedDate = o.CreatedDate,
                       ModifiedDate = o.ModifiedDate
                   }).Single();

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(UpdateBook)} method of the BookRepository class");

                throw;
            }
        }

        public bool DeleteBook(Guid idBook)
        {
            try
            {
                BookEntity bookEntityExists = context.BookEntity.Find(idBook)
                    ?? throw new ApplicationException($"Attempted to delete non-existing book {idBook}");

                bookEntityExists.IsDeleted = true;
                bookEntityExists.ModifiedDate = DateTime.UtcNow;

                context.BookEntity.Update(bookEntityExists);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(DeleteBook)} method of the BookRepository class");

                throw;
            }
        }

        public bool DeleteCategoryBook(Guid id, Guid categoryId)
        {
            SqlConnection connection = null;
            DynamicParameters parameters = new();

            try
            {
                bool bookExists = context.BookEntity
                    .AsNoTracking()
                    .Any(b => b.Id == id && !b.IsDeleted);

                if (!bookExists)
                    throw new ApplicationException($"The book with ID {id} does not exist.");

                CategoryEntity categoryEntity = context.CategoryEntity
                    .AsNoTracking()
                    .SingleOrDefault(c => c.Id == categoryId)
                ?? throw new ApplicationException($"The category with ID {categoryId} does not exist.");

                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                parameters.Add("@BookId", id);
                parameters.Add("@CategoryId", categoryId);

                connection.Execute(
                    "usp_delete_category_book",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(DeleteCategoryBook)} method of the BookRepository class");

                throw;
            }
            finally
            {
                if (connection != null) {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                    connection.Dispose();
                    connection = null;
                }
            }
        }

        private Guid CreateBook(BookDTO model)
        {
            using DataTable categoryTable = new();
            DynamicParameters parameters = new();

            Guid bookId = Guid.NewGuid();

            categoryTable.Columns.Add("CategoryId", typeof(Guid));

            if (model.Categories != null && model.Categories.Count > 0)
                model.Categories.ForEach(category => categoryTable.Rows.Add(category.Id));

            using SqlConnection connection = new(connectionString);
            
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            parameters.Add("@Id", bookId);
            parameters.Add("@Title", model.Title.Trim());
            parameters.Add("@AuthorId", model.AuthorId);
            parameters.Add("@PublishedDate", model.PublishedDate);
            parameters.Add("@Genere", model.Genere);
            parameters.Add("@Categories", categoryTable.AsTableValuedParameter("udt_category"));

            bookId = connection.ExecuteScalar<Guid>("usp_post_book", parameters, commandType: CommandType.StoredProcedure);

            if (connection.State == ConnectionState.Open)
                connection.Close();

            return bookId;
        }
    }
}