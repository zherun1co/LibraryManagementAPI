using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using LibraryManagementAPI;
using LibraryManagementModel.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using LibraryManagementModel.Responses;

namespace LibraryManagementUnitTest.Controllers
{
    public class AuthorControllerTests(WebApplicationFactory<UnitTestsConfiguration> factory) : IClassFixture<WebApplicationFactory<UnitTestsConfiguration>>
    {
        private readonly HttpClient client = factory.CreateClient();
        private const string authorPath = "/api/authors";

        [Fact]
        public async Task GetAuthors_WithValidParams_ShouldReturnSuccess()
        {
            // Arrange
            string queryParams = "?Offset=0&Limit=5";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAuthors_WithoutOffset_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Limit=5";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Offset parameter is required.");
        }

        [Fact]
        public async Task GetAuthors_WithoutLimit_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Offset=0";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Limit parameter is required.");
        }

        [Fact]
        public async Task GetAuthors_WithZeroOrNegativeLimit_ShouldReturnBadRequest()
        {
            // Arrange
            string queryParams = "?Offset=0&Limit=0";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}{queryParams}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Limit parameter must have a valid value greater than 0.");
        }

        [Fact]
        public async Task GetAuthor_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000001";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAuthor_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "invalid-guid";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}/{invalidId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task GetAuthor_WithNonExistingId_ShouldReturnSuccessWithNullData()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetAuthor_WithNonExistingId_ShouldReturnSuccess_WithNullData()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.GetAsync($"{authorPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task PostAuthor_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            AuthorDTO newAuthor = new() {
                Name = $"New-Author-{Guid.NewGuid()}",
                DateOfBirth = new DateTime(1970, 1, 1)
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(authorPath, newAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PostAuthor_WithoutName_ShouldReturnBadRequest()
        {
            // Arrange
            AuthorDTO newAuthor = new() {
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(authorPath, newAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The Name property is required.");
        }

        [Fact]
        public async Task PostAuthor_WithExistingName_ShouldReturnInternalServerError()
        {
            // Arrange
            AuthorDTO existingAuthor = new() {
                Name = "J.K. Rowling",
                DateOfBirth = new DateTime(1965, 7, 31)
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(authorPath, existingAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PatchAuthor_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "invalid-guid";

            AuthorDTO updatedAuthor = new() {
                Name = "Updated Author Name"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{invalidId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property must be a valid GUID.");
        }

        [Fact]
        public async Task PatchAuthor_WithNonExistingId_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            AuthorDTO updatedAuthor = new() {
                Name = "Updated Author Name"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{nonExistingId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PatchAuthor_WithExistingName_ShouldReturnInternalServerError()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000001";

            AuthorDTO updatedAuthor = new() {
                Name = "J.K. Rowling"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{existingId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task PatchAuthor_WithValidId_AndNameUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000004";

            AuthorDTO updatedAuthor = new() {
                Name = "Agatha Christi3"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{existingId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchAuthor_WithValidId_AndDateOfBirthUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000005";

            AuthorDTO updatedAuthor = new() {
                DateOfBirth = new DateTime(1920, 1, 3)
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{existingId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PatchAuthor_WithValidId_AndIsDeletedUpdate_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000017";

            AuthorDTO updatedAuthor = new() {
                IsDeleted = true
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{authorPath}/{existingId}", updatedAuthor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAuthor_WithNonExistingId_ShouldReturnInternalServerError()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{authorPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Message.Should().Be("Error");
        }

        [Fact]
        public async Task DeleteAuthor_WithExistingId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "C4D01A85-2B47-4B4F-A2E8-000000000019";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{authorPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            DefaultResponse result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Message.Should().Be("Success");
        }

        [Fact]
        public async Task DeleteAuthor_WithInvalidIdFormat_ShouldReturnBadRequest()
        {
            // Arrange
            string invalidId = "invalid-guid";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{authorPath}/{invalidId}");

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
