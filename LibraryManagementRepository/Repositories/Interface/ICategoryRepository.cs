using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementRepository.Repositories.Interface
{
    public interface ICategoryRepository
    {
        List<CategoryDTO> GetCategories(FilterGetCategoryDTO filter);
        CategoryDTO GetCategory(Guid id);
        CategoryDTO AddCategory(CategoryDTO model);
        CategoryDTO UpdateCategory(CategoryDTO model);
        bool DeleteCategory(Guid id);
    }
}
