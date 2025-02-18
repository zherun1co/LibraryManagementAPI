using Dapper;
using Serilog;
using System.Data;
using static Dapper.SqlMapper;
using Microsoft.Data.SqlClient;
using LibraryManagementModel.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementModel.Filters;
using LibraryManagementModel.Entities;
using Microsoft.Extensions.Configuration;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementRepository.Repositories.DatabaseContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryManagementRepository.Repositories.Repository
{
    public class CategoryRepository(LibraryManagementContext context, IConfiguration configuration) : ICategoryRepository
    {
        private readonly LibraryManagementContext context = context;
        private readonly Serilog.ILogger logger = Log.ForContext<CategoryRepository>();
        private readonly string connectionString = configuration.GetConnectionString("DefaultConnection");

        public List<CategoryDTO> GetCategories(FilterGetCategoryDTO filter)
        {
            try
            {
                IOrderedQueryable<CategoryEntity> query = context.CategoryEntity
                    .AsNoTracking()
                    .Where(c => (string.IsNullOrEmpty(filter.Category) || c.Name.Contains(filter.Category))
                                && (!filter.IsDeleted.HasValue || c.IsDeleted == filter.IsDeleted.Value))
                    .OrderBy(c => c.Name);

                if (filter.Limit.HasValue)
                    query = (IOrderedQueryable<CategoryEntity>)query.Take(filter.Limit.Value);

                return [.. query.Select(c => new CategoryDTO {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedDate = c.CreatedDate,
                    ModifiedDate = c.ModifiedDate,
                    IsDeleted = c.IsDeleted,
                    Books = null
                })];
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetCategories)} method of the CategoryRepository class");
                throw;
            }
        }

        public CategoryDTO GetCategory(Guid id)
        {
            CategoryDTO category = null;
            SqlConnection connection = null;

            try
            {
                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using GridReader multi = connection.QueryMultiple(
                    "usp_get_category_books",
                    new { CategoryId = id },
                    commandType: CommandType.StoredProcedure
                );

                category = multi.Read<CategoryDTO>().FirstOrDefault();

                if (category == null)
                    return null;

                if (!multi.IsConsumed)
                    category.Books = multi.Read<BookDTO>().ToList();

                return category;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetCategory)} method of the CategoryRepository class");

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

        public CategoryDTO AddCategory(CategoryDTO model)
        {
            try
            {
                CategoryEntity categoryEntityExists = context.CategoryEntity.SingleOrDefault(o =>
                    o.Name.ToLower().Equals(model.Name.Trim().ToLower())
                );

                if (categoryEntityExists != null)
                    throw new ApplicationException($"The category '{categoryEntityExists.Name}' already exists with the id {categoryEntityExists.Id}.");

                CategoryEntity categoryEntity = new() {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    CreatedDate = DateTime.UtcNow
                };

                context.CategoryEntity.Add(categoryEntity);
                context.SaveChanges();

                model.Id = categoryEntity.Id;
                model.CreatedDate = categoryEntity.CreatedDate;

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(AddCategory)} method of the CategoryRepository class");

                throw;
            }
        }

        public CategoryDTO UpdateCategory(CategoryDTO model)
        {
            try
            {
                CategoryEntity categoryEntityExists = context.CategoryEntity.Find(model.Id)
                    ?? throw new ApplicationException($"Attempted to update non-existing category {model.Id}");

                if (!string.IsNullOrEmpty(model.Name)) {
                    CategoryEntity categoryEntityNameExists = context.CategoryEntity.SingleOrDefault(o =>
                        o.Id != model.Id
                        && o.Name.ToLower().Equals(model.Name.Trim().ToLower())
                    );

                    if (categoryEntityNameExists != null)
                        throw new ApplicationException($"The category name already exists with the id {categoryEntityNameExists.Id}.");

                    categoryEntityExists.Name = model.Name.Trim();
                }
                
                if (model.IsDeleted.HasValue)
                    categoryEntityExists.IsDeleted = model.IsDeleted.Value;

                if (!string.IsNullOrEmpty(model.Name) || model.IsDeleted.HasValue) {
                    categoryEntityExists.ModifiedDate = DateTime.UtcNow;

                    context.CategoryEntity.Update(categoryEntityExists);
                    context.SaveChanges();

                    model.ModifiedDate = categoryEntityExists.ModifiedDate;
                }

                return new CategoryDTO() {
                    Id = categoryEntityExists.Id,
                    Name = categoryEntityExists.Name,
                    IsDeleted = categoryEntityExists.IsDeleted,
                    CreatedDate = categoryEntityExists.CreatedDate,
                    ModifiedDate = categoryEntityExists.ModifiedDate,
                    Books = null
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(UpdateCategory)} method of the CategoryRepository class");

                throw;
            }
        }

        public bool DeleteCategory(Guid id)
        {
            try
            {
                CategoryEntity categoryEntityExists = context.CategoryEntity.Find(id)
                    ?? throw new ApplicationException($"Attempted to delete non-existing category {id}");

                categoryEntityExists.IsDeleted = true;
                categoryEntityExists.ModifiedDate = DateTime.UtcNow;

                context.CategoryEntity.Update(categoryEntityExists);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(DeleteCategory)} method of the CategoryRepository class");

                throw;
            }
        }
    }
}