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
    [Route("api/authors")]
    public class AuthorController(IAuthorService iAuthorService) : Controller
    {
        private readonly IAuthorService authorService = iAuthorService;

        [HttpGet]
        public IActionResult GetAuthors([FromQuery] FilterGetAuthorDTO filter)
        {
            try
            {
                if (!filter.Offset.HasValue)
                    throw new InvalidOperationException("The Offset parameter is required.");

                if (!filter.Limit.HasValue)
                    throw new InvalidOperationException("The Limit parameter is required.");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = authorService.GetAuthors(filter)
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

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAuthor(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = authorService.GetAuthor(id)
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

        [HttpGet]
        [Route("{id}/books")]
        public IActionResult GetAuthorBooks(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = authorService.GetAuthorBooks(id)
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
        public IActionResult PostAuthor([FromBody] AuthorDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("The Name property is required.");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = authorService.AddAuthor(model)
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

        [HttpPatch]
        [Route("{id}")]
        public IActionResult PatchAuthor(string id, [FromBody] AuthorDTO model)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = authorService.UpdateAuthor(id, model)
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

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAuthor(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = authorService.DeleteAuthor(id),
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
