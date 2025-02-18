using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementServices.BusinessServices.Interface
{
    public interface IBookService
    {
        BooksDTO GetBooks(FilterGetBookDTO filter);
        BookDTO GetBook(string id);
        BookDTO AddBook(BookDTO model);
        BookCategoryDTO AddCategoryBook(string id, BookCategoryDTO model);
        BookDTO UpdateBook(string id, BookDTO model);
        bool DeleteBook(string id);
        bool DeleteCategoryBook(string id, string categoryId);
    }
}