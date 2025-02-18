using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementServices.BusinessServices.Interface;

namespace LibraryManagementServices.BusinessServices.Service
{
    public class CategoryService (ICategoryRepository iCategoryRepository) : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository = iCategoryRepository;

        public List<CategoryDTO> GetCategories(FilterGetCategoryDTO filter)
        {
            return categoryRepository.GetCategories(filter);
        }

        public CategoryDTO GetCategory(string id)
        {
            if (!Guid.TryParse(id, out Guid categoryId))
                throw new InvalidOperationException("The id property path must be a valid GUID.");

            return categoryRepository.GetCategory(categoryId);
        }

        public CategoryDTO AddCategory(CategoryDTO model)
        {
            return categoryRepository.AddCategory(model);
        }

        public CategoryDTO UpdateCategory(string id, CategoryDTO model)
        {
            if (!Guid.TryParse(id, out Guid categoryId))
                throw new InvalidOperationException("The id property path must be a valid GUID.");

            model.Id = categoryId;

            return categoryRepository.UpdateCategory(model);
        }

        public bool DeleteCategory(string id)
        {
            if (!Guid.TryParse(id, out Guid categoryId))
                throw new InvalidOperationException("The id is not a Guid type");

            return categoryRepository.DeleteCategory(categoryId);
        }
    }
}
