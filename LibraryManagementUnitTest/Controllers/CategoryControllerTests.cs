using System.Net;
using FluentAssertions;
using System.Net.Http.Json;
using LibraryManagementAPI;
using LibraryManagementModel.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using LibraryManagementModel.Responses;

namespace LibraryManagementUnitTest.Controllers
{
    public class CategoryControllerTests(WebApplicationFactory<UnitTestsConfiguration> factory) : IClassFixture<WebApplicationFactory<UnitTestsConfiguration>>
    {
        private readonly HttpClient client = factory.CreateClient();
        private const string categoryPath = "/api/categories";

        [Fact]
        public async Task GetCategories_ShouldReturnSuccess()
        {
            // Act
            HttpResponseMessage response = await client.GetAsync(categoryPath);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCategory_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "D3B01A85-3C47-4B4F-A3E8-000000000001";

            // Act
            HttpResponseMessage response = await client.GetAsync($"{categoryPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCategory_WithInvalidId_ShouldReturnBadRequest()
        {
            // Act
            HttpResponseMessage response = await client.GetAsync($"{categoryPath}/abc123");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property path must be a valid GUID.");
        }

        [Fact]
        public async Task GetCategory_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            string existingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.GetAsync($"{categoryPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task PostCategory_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            CategoryDTO newCategory = new() {
                Name = $"New-Category-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(categoryPath, newCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PostCategory_WithExistingData_ShouldReturnBadRequest()
        {
            // Arrange
            CategoryDTO invalidCategory = new() {
                Name = "Horror"
            };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(categoryPath, invalidCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PostCategory_WithEmptyData_ShouldReturnBadRequest()
        {
            // Arrange
            CategoryDTO invalidCategory = new() { };

            // Act
            HttpResponseMessage response = await client.PostAsJsonAsync(categoryPath, invalidCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutCategory_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "47B1CBB3-403B-4396-AD3C-27C7F3EC077D";

            CategoryDTO updatedCategory = new() {
                Name = $"New-Category-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{existingId}", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task PutCategory_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            CategoryDTO updatedCategory = new() {
                Name = $"New-Category-{nonExistingId}"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{nonExistingId}", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PutCategory_WithInvalidId_ShouldReturnBadRequest()
        {
            // Arrange
            CategoryDTO updatedCategory = new() {
                Name = $"New-Category-{Guid.NewGuid()}"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/abc123", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id property path must be a valid GUID.");
        }

        [Fact]
        public async Task PutCategory_WithEmptyData_ShouldReturnBadRequest()
        {
            // Arrange
            string existingId = "47B1CBB3-403B-4396-AD3C-27C7F3EC077D";

            CategoryDTO invalidCategory = new() { };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{existingId}", invalidCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutCategory_WithSameName_ShouldNotModifyCategory()
        {
            // Arrange
            string existingId = "D3B01A85-3C47-4B4F-A3E8-000000000001";

            CategoryDTO updatedCategory = new () {
                Name = "Horror"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{existingId}", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task PutCategory_WithExistingName_ShouldNotModifyCategory()
        {
            // Arrange
            string existingId = "D3B01A85-3C47-4B4F-A3E8-000000000001";

            CategoryDTO updatedCategory = new() {
                Name = "Fantasy"
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{existingId}", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PutCategory_ActiveStatus_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "47B1CBB3-403B-4396-AD3C-27C7F3EC077D";

            CategoryDTO updatedCategory = new() {
                IsDeleted = false
            };

            // Act
            HttpResponseMessage response = await client.PatchAsJsonAsync($"{categoryPath}/{existingId}", updatedCategory);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteCategory_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            string existingId = "47B1CBB3-403B-4396-AD3C-27C7F3EC077D";

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{categoryPath}/{existingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Code.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteCategory_WithInvalidId_ShouldReturnBadRequest()
        {
            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{categoryPath}/abc123");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<DefaultResponse>();

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Message.Should().Be("The id is not a Guid type");
        }

        [Fact]
        public async Task DeleteCategory_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            string nonExistingId = Guid.NewGuid().ToString();

            // Act
            HttpResponseMessage response = await client.DeleteAsync($"{categoryPath}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
