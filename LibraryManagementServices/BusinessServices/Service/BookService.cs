using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementServices.BusinessServices.Interface;

namespace LibraryManagementServices.BusinessServices.Service
{
    public class BookService (IBookRepository iBookRepository) : IBookService
    {
        private readonly IBookRepository bookRepository = iBookRepository;

        public BooksDTO GetBooks(FilterGetBookDTO filter)
        {
            if (filter.Offset < 0)
                throw new InvalidOperationException("The Offset parameter must have a valid value greater than or equal to 0.");

            if (filter.Limit <= 0)
                throw new InvalidOperationException("The Limit parameter must have a valid value greater than 0.");

            return bookRepository.GetBooks(filter);
        }

        public BookDTO GetBook(string id)
        {
            if (!Guid.TryParse(id, out Guid bookId))
                throw new InvalidOperationException("The id property path must be a valid GUID.");

            return bookRepository.GetBook(bookId);
        }

        public BookDTO AddBook(BookDTO model)
        {
            if (model.AuthorId == Guid.Empty)
                throw new InvalidOperationException("The AuthorId property cannot be empty.");

            return bookRepository.AddBook(model);
        }

        public BookCategoryDTO AddCategoryBook(string id, BookCategoryDTO model)
        {
            if (!Guid.TryParse(id, out Guid authorId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            return bookRepository.AddCategoryBook(authorId, model); ;
        }

        public BookDTO UpdateBook(string id, BookDTO model)
        {
            if (!Guid.TryParse(id, out Guid bookId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            model.Id = bookId;

            return bookRepository.UpdateBook(model);
        }

        public bool DeleteBook(string id)
        {
            if (!Guid.TryParse(id, out Guid bookId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            return bookRepository.DeleteBook(bookId);
        }

        public bool DeleteCategoryBook(string id, string categoryId)
        {
            if (!Guid.TryParse(id, out Guid converterBookId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            if (!Guid.TryParse(categoryId, out Guid converterCategoryId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            return bookRepository.DeleteCategoryBook(converterBookId, converterCategoryId);
        }
    }
}
