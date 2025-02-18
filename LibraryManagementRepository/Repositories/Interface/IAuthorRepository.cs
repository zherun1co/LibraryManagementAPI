using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementRepository.Repositories.Interface
{
    public interface IAuthorRepository
    {
        AuthorsDTO GetAuthors(FilterGetAuthorDTO filter);
        AuthorDTO GetAuthor(Guid id);
        AuthorDTO AddAuthor(AuthorDTO model);
        AuthorDTO UpdateAuthor(AuthorDTO model);
        bool DeleteAuthor(Guid id);
    }
}