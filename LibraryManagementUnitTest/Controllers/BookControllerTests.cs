using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using LibraryManagementAPI;
using LibraryManagementModel.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using LibraryManagementModel.Responses;

namespace LibraryManagementUnitTest.Controllers
{
    public class BookControllerTests(WebApplicationFactory<UnitTestsConfiguration> factory) : IClassFixture<WebApplicationFactory<UnitTestsConfiguration>>
    {
        private readonly HttpClient client = factory.CreateClient();
        private const string bookPath = "/api/books";

        [Fact]
        public async Task GetBooks_WithValidParams_ShouldReturnSuccess()
        {
            // Arrange
            string queryParams = "?Offset=0&Limit=5";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBooks_WithoutOffset_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Limit=5";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Offset parameter is required.");
        }

        [Fact]
        public async Task GetBooks_WithZeroOrNegativeLimit_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Offset=0&Limit=0";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Limit parameter must have a valid value greater than 0.");
        }

        [Fact]
        public async Task GetBooks_WithoutLimit_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Offset=0";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Limit parameter is required.");
        }

        [Fact]
        public async Task GetBooks_WithNegativeOffset_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Offset=-1&Limit=5";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Offset parameter must have a valid value greater than or equal to 0.");
        }

        [Fact]
        public async Task GetBook_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000001";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBook_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "1234-invalid-guid";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property path must be a valid GUID.");
        }

        [Fact]
        public async Task GetBook_WithNonExistingId_ShouldReturnBadRequest()
        {
            // Arrange
            string nonExistingId = "3c31f9e6-003f-4d58-9e68-8ee5aa19a72d";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{bookPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task PostBook_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            BookDTO newBook = new() {
                Title = $"New Book - {Guid.NewGuid()}",
                AuthorId = Guid.Parse("C4D01A85-2B47-4B4F-A2E8-000000000001"),
                PublishedDate = DateTime.UtcNow,
                Genere = "Fiction",
                Categories = [new BookCategoryDTO() {
                    Id = Guid.Parse("D3B01A85-3C47-4B4F-A3E8-000000000001")
                }]
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(bookPath, newBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PostBook_WithoutTitle_ShouldReturnBadRequest()
        {
            // Arrange
            BookDTO newBook = new() {
                AuthorId = Guid.NewGuid(),
                PublishedDate = DateTime.UtcNow,
                Genere = "Fiction",
                Categories = [new BookCategoryDTO() {
                    Id = Guid.NewGuid(),
                    Name = "Adventure"
                }]
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(bookPath, newBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Title property is required.");
        }

        [Fact]
        public async Task PostBook_WithEmptyAuthorId_ShouldReturnBadRequest()
        {
            // Arrange
            BookDTO newBook = new() {
                Title = "Book without Author",
                AuthorId = Guid.Empty,
                PublishedDate = DateTime.UtcNow,
                Genere = "Fiction",
                Categories = [new BookCategoryDTO() {
                    Id = Guid.NewGuid(),
                    Name = "Adventure" 
                }]
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(bookPath, newBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The AuthorId property cannot be empty.");
        }

        [Fact]
        public async Task PostBook_WithExistingTitleAndAuthor_ShouldReturnInternalServerError()
        {
            // Arrange
            BookDTO existingBook = new() {
                Title = "Harry Potter and the Sorcerer's Stone",
                AuthorId = Guid.Parse("C4D01A85-2B47-4B4F-A2E8-000000000002"),
                PublishedDate = DateTime.UtcNow,
                Genere = "Fiction",
                Categories = [new BookCategoryDTO() {
                    Id = Guid.Parse("D3B01A85-3C47-4B4F-A3E8-000000000007")
                }]
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(bookPath, existingBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PostBook_WithoutAuthorId_ShouldReturnBadRequest()
        {
            // Arrange
            BookDTO newBook = new() {
                Title = "Book Without AuthorId",
                PublishedDate = DateTime.UtcNow,
                Genere = "Fiction",
                Categories = [new BookCategoryDTO() {
                    Id = Guid.NewGuid(),
                    Name = "Adventure"
                }]
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(bookPath, newBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The AuthorId property is required.");
        }

        [Fact]
        public async Task PatchBook_WithNonExistingId_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            BookDTO updatedBook = new() {
                Title = "Updated Book Title"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{nonExistingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PatchBook_WithExistingTitleAndAuthor_ShouldReturnInternalServerError()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000001";

            BookDTO updatedBook = new() {
                Title = "A Game of Thrones",
                AuthorId = Guid.Parse("C4D01A85-2B47-4B4F-A2E8-000000000003")
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PatchBook_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "invalid-guid";

            BookDTO updatedBook = new() {
                Title = "Updated Book Title"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{invalidId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task PatchBook_WithValidId_AndTitleUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000013";

            BookDTO updatedBook = new() {
                Title = "A Tale of Two Citi3s"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchBook_WithValidId_AndAuthorIdUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000013";

            BookDTO updatedBook = new() {
                AuthorId = Guid.Parse("C4D01A85-2B47-4B4F-A2E8-000000000009")
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchBook_WithValidId_AndPublishedDateUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000014";

            BookDTO updatedBook = new() {
                PublishedDate = new DateTime(1877, 1, 2)
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchBook_WithValidId_AndGenereUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000001";

            BookDTO updatedBook = new() {
                Genere = "Dram@"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchBook_WithValidId_AndIsDeleted_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000014";

            BookDTO updatedBook = new() {
                IsDeleted = false
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{bookPath}/{existingId}", updatedBook);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteBook_WithNonExistingId_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task DeleteBook_WithExistingId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "E2C01A85-4D47-4B4F-A4E8-000000000014";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Message.Should().Be("Success");
        }

        [Fact]
        public async Task DeleteBook_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "invalid-guid";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task PostCategoryBook_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            string existingBookId = "E2C01A85-4D47-4B4F-A4E8-000000000007";

            BookCategoryDTO category = new() {
                Id = Guid.Parse("D3B01A85-3C47-4B4F-A3E8-000000000007")
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync($"{bookPath}/{existingBookId}/categories", category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PostCategoryBook_WithoutCategoryId_ShouldReturnBadRequest()
        {
            // Arrange
            string existingBookId = "E2C01A85-4D47-4B4F-A4E8-000000000007";
            BookCategoryDTO category = new();

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync($"{bookPath}/{existingBookId}/categories", category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property is required.");
        }

        [Fact]
        public async Task PostCategoryBook_WithNonExistingBook_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingBookId = Guid.NewGuid().ToString();

            BookCategoryDTO category = new() {
                Id = Guid.Parse("D3B01A85-3C47-4B4F-A3E8-000000000001")
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync($"{bookPath}/{nonExistingBookId}/categories", category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PostCategoryBook_WithInvalidBookIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidBookId = "invalid-guid";

            BookCategoryDTO category = new() {
                Id = Guid.Parse("D3B01A85-3C47-4B4F-A3E8-000000000001")
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync($"{bookPath}/{invalidBookId}/categories", category);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task DeleteCategoryBook_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            string existingBookId = "E2C01A85-4D47-4B4F-A4E8-000000000010";
            string existingCategoryId = "D3B01A85-3C47-4B4F-A3E8-000000000004";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{existingBookId}/categories/{existingCategoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Message.Should().Be("Success");
        }

        [Fact]
        public async Task DeleteCategoryBook_WithNonExistingBook_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingBookId = Guid.NewGuid().ToString();
            string existingCategoryId = "D3B01A85-3C47-4B4F-A3E8-000000000001";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{nonExistingBookId}/categories/{existingCategoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task DeleteCategoryBook_WithNonExistingCategory_ShouldReturnInternalServerError()
        {
            // Arrange
            string existingBookId = "E2C01A85-4D47-4B4F-A4E8-000000000005";
            string nonExistingCategoryId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{existingBookId}/categories/{nonExistingCategoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task DeleteCategoryBook_WithInvalidBookIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidBookId = "invalid-guid";
            string existingCategoryId = "D3B01A85-3C47-4B4F-A3E8-000000000001";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{invalidBookId}/categories/{existingCategoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task DeleteCategoryBook_WithInvalidCategoryIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string existingBookId = "E2C01A85-4D47-4B4F-A4E8-000000000001";
            string invalidCategoryId = "invalid-guid";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{bookPath}/{existingBookId}/categories/{invalidCategoryId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }
    }
}
