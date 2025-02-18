using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementRepository.Repositories.Interface
{
    public interface IBookRepository
    {
        BooksDTO GetBooks(FilterGetBookDTO filter);
        List<AuthorBookDTO> GetBooksByAuthor(Guid idAuthor);
        BookDTO GetBook(Guid idBook);
        BookDTO AddBook(BookDTO model);
        BookCategoryDTO AddCategoryBook(Guid bookId, BookCategoryDTO model);
        BookDTO UpdateBook(BookDTO model);
        bool DeleteBook(Guid idBook);
        bool DeleteCategoryBook(Guid id, Guid categoryId);
    }
}
