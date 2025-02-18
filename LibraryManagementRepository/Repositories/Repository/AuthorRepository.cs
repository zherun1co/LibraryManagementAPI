using Serilog;
using System.Data;
using static Dapper.SqlMapper;
using Microsoft.Data.SqlClient;
using LibraryManagementModel.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementModel.Commons;
using LibraryManagementModel.Filters;
using LibraryManagementModel.Entities;
using Microsoft.Extensions.Configuration;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementRepository.Repositories.DatabaseContext;

namespace LibraryManagementRepository.Repositories.Repository
{
    public class AuthorRepository(LibraryManagementContext context, IConfiguration configuration) : IAuthorRepository
    {
        private readonly LibraryManagementContext context = context;
        private readonly Serilog.ILogger logger = Log.ForContext<AuthorRepository>();
        private readonly string connectionString = configuration.GetConnectionString("DefaultConnection");

        public AuthorsDTO GetAuthors(FilterGetAuthorDTO filter)
        {
            SqlConnection connection = null;

            AuthorsDTO authors = new();

            try
            {
                connection = new(connectionString);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using GridReader multi = connection.QueryMultiple(
                    "usp_get_authors",
                    new {
                        Name = filter.Author,
                        filter.IsDeleted,
                        filter.Offset,
                        filter.Limit
                    },
                    commandType: CommandType.StoredProcedure
                );

                authors.Authors = multi.Read<AuthorDTO>().ToList();

                authors.Paging = multi.Read<PagingDTO>().SingleOrDefault();

                return authors;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetAuthors)} method of the AuthorRepository class");

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

        public AuthorDTO GetAuthor(Guid idAuthor)
        {
            try
            {
                AuthorEntity authorEntity = context.AuthorEntity
                    .AsNoTracking()
                    .SingleOrDefault(a => a.Id == idAuthor);

                if (authorEntity == null)
                    return null;

                return new AuthorDTO {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name,
                    DateOfBirth = authorEntity.DateOfBirth,
                    CreatedDate = authorEntity.CreatedDate,
                    ModifiedDate = authorEntity.ModifiedDate,
                    IsDeleted = authorEntity.IsDeleted
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(GetAuthor)} method of the AuthorRepository class");

                throw;
            }
        }

        public AuthorDTO AddAuthor(AuthorDTO model)
        {
            try
            {
                AuthorEntity authorEntityExists = context.AuthorEntity
                    .SingleOrDefault(o => o.Name.ToLower().Equals(model.Name.Trim().ToLower()));

                if (authorEntityExists != null)
                    throw new ApplicationException($"The author '{authorEntityExists.Name}' already exists with the id {authorEntityExists.Id}.");

                AuthorEntity authorEntity = new() {
                    Id = Guid.NewGuid(),
                    Name = model.Name.Trim(),
                    DateOfBirth= model.DateOfBirth,
                    CreatedDate = DateTime.UtcNow
                };

                context.AuthorEntity.Add(authorEntity);
                context.SaveChanges();

                model.Id = authorEntity.Id;
                model.IsDeleted = false;
                model.CreatedDate = authorEntity.CreatedDate;

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(AddAuthor)} method of the AuthorRepository class");

                throw;
            }
        }

        public AuthorDTO UpdateAuthor(AuthorDTO model)
        {
            try
            {
                AuthorEntity authorEntityExists = context.AuthorEntity.Find(model.Id)
                    ?? throw new ApplicationException($"Attempted to update non-existing author {model.Id}");

                if (!string.IsNullOrEmpty(model.Name)) {
                    AuthorEntity authorEntityNameExists = context.AuthorEntity.SingleOrDefault(o =>
                        o.Id != model.Id
                        && o.Name.ToLower().Equals(model.Name.Trim().ToLower()));

                    if (authorEntityNameExists != null)
                        throw new ApplicationException($"The author name '{model.Name}' already exists with the id {authorEntityNameExists.Id}.");

                    authorEntityExists.Name = model.Name.Trim();
                }

                if (model.DateOfBirth.HasValue)
                    authorEntityExists.DateOfBirth = model.DateOfBirth.Value;

                if (model.IsDeleted.HasValue)
                    authorEntityExists.IsDeleted = model.IsDeleted.Value;

                if (!string.IsNullOrEmpty(model.Name) || model.DateOfBirth.HasValue || model.IsDeleted.HasValue) {
                    authorEntityExists.ModifiedDate = DateTime.UtcNow;

                    context.AuthorEntity.Update(authorEntityExists);
                    context.SaveChanges();

                    model.ModifiedDate = authorEntityExists.ModifiedDate;
                }

                model = context.AuthorEntity
                    .AsNoTracking()
                    .Where(w => w.Id == model.Id).Select(o => new AuthorDTO {
                        Id = o.Id,
                        Name = o.Name,
                        DateOfBirth = o.DateOfBirth,
                        IsDeleted = o.IsDeleted,
                        CreatedDate = o.CreatedDate,
                        ModifiedDate = o.ModifiedDate
                    }).Single();

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(UpdateAuthor)} method of the AuthorRepository class");

                throw;
            }
        }

        public bool DeleteAuthor(Guid id)
        {
            try
            {
                AuthorEntity authorEntityExists = context.AuthorEntity.Find(id)
                    ?? throw new ApplicationException($"Attempted to delete non-existing author {id}");

                authorEntityExists.IsDeleted = true;
                authorEntityExists.ModifiedDate = DateTime.UtcNow;

                context.AuthorEntity.Update(authorEntityExists);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"An error occurred in the {nameof(DeleteAuthor)} method of the AuthorRepository class");

                throw;
            }
        }
    }
}
