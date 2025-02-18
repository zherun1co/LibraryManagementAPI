using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementServices.BusinessServices.Interface;

namespace LibraryManagementServices.BusinessServices.Service
{
    public class AuthorService(IAuthorRepository iAuthorRepository, IBookRepository iBookRepository) : IAuthorService
    {
        private readonly IAuthorRepository authorRepository = iAuthorRepository;
        private readonly IBookRepository iBookRepository = iBookRepository;

        public AuthorsDTO GetAuthors(FilterGetAuthorDTO filter)
        {
            if (filter.Offset < 0)
                throw new InvalidOperationException("The Offset parameter must have a valid value greater than or equal to 0.");

            if (filter.Limit <= 0)
                throw new InvalidOperationException("The Limit parameter must have a valid value greater than 0.");

            return authorRepository.GetAuthors(filter);
        }

        public AuthorDTO GetAuthor(string id)
        {
            if (!Guid.TryParse(id, out Guid authorId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            return authorRepository.GetAuthor(authorId);
        }

        public List<AuthorBookDTO> GetAuthorBooks(string id)
        {
            if (!Guid.TryParse(id, out Guid authorId))
                throw new InvalidOperationException("The id property path must be a valid GUID.");

            return iBookRepository.GetBooksByAuthor(authorId);
        }

        public AuthorDTO AddAuthor(AuthorDTO model)
        {
            return authorRepository.AddAuthor(model);
        }

        public AuthorDTO UpdateAuthor(string id, AuthorDTO model)
        {
            if (!Guid.TryParse(id, out Guid authorId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            model.Id = authorId;

            return authorRepository.UpdateAuthor(model);
        }

        public bool DeleteAuthor(string id)
        {
            if (!Guid.TryParse(id, out Guid authorId))
                throw new InvalidOperationException("The id property must be a valid GUID.");

            return authorRepository.DeleteAuthor(authorId);
        }
    }
}