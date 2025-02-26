using System.Net;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;
using LibraryManagementModel.Responses;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementServices.BusinessServices.Interface;

namespace LibraryManagementAPI.Controllers
{

    [Authorize]
    [Route("api/categories")]
    public class CategoryController (ICategoryService iCategoryService) : Controller
    {
        private readonly ICategoryService categoryService = iCategoryService;

        [HttpGet]
        public IActionResult GetCategories([FromQuery] FilterGetCategoryDTO filter)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = categoryService.GetCategories(filter)
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCategory(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = categoryService.GetCategory(id)
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult PostCategory([FromBody] CategoryDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("The Name property is required");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = categoryService.AddCategory(model)
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [Route("{id}")]
        [HttpPatch]
        public IActionResult PatchCategory(string id, [FromBody] CategoryDTO model)
        {
            try
            {
                if (!model.IsDeleted.HasValue && string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("The Name property is required");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = categoryService.UpdateCategory(id, model)
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public IActionResult DeleteCategory(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = categoryService.DeleteCategory(id),
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success"
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new DefaultResponse {
                    Success = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message
                });
            }
        }
    }
}
