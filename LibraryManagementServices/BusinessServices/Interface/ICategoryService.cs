using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;

namespace LibraryManagementServices.BusinessServices.Interface
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetCategories(FilterGetCategoryDTO filter);
        CategoryDTO GetCategory(string id);
        CategoryDTO AddCategory(CategoryDTO model);
        CategoryDTO UpdateCategory(string id, CategoryDTO model);
        bool DeleteCategory(string id);
    }
}
