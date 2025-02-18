using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementServices.BusinessServices.Interface
{
    public interface IAuthorService
    {
        AuthorsDTO GetAuthors(FilterGetAuthorDTO filter);
        AuthorDTO GetAuthor(string id);
        List<AuthorBookDTO> GetAuthorBooks(string id);
        AuthorDTO AddAuthor(AuthorDTO model);
        AuthorDTO UpdateAuthor(string id, AuthorDTO model);
        bool DeleteAuthor(string id);
    }
}
