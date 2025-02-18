using System.Net;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementModel.Models;
using LibraryManagementModel.Filters;
using LibraryManagementModel.Responses;
using LibraryManagementServices.BusinessServices.Interface;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/books")]
    public class BookController(IBookService iBookService) : Controller
    {
        private readonly IBookService bookService = iBookService;

        [HttpGet]
        public IActionResult GetBooks([FromQuery] FilterGetBookDTO filter)
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
                    Data = bookService.GetBooks(filter)
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
        public IActionResult GetBook(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = bookService.GetBook(id)
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
        public IActionResult PostBook([FromBody] BookDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Title))
                    throw new InvalidOperationException("The Title property is required.");

                if (!model.AuthorId.HasValue)
                    throw new InvalidOperationException("The AuthorId property is required.");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = bookService.AddBook(model)
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
        [Route("{id}/categories")]
        public IActionResult PostCategoryBook(string id, [FromBody] BookCategoryDTO model)
        {
            try
            {
                if (!model.Id.HasValue)
                    throw new InvalidOperationException("The id property is required.");

                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = bookService.AddCategoryBook(id, model)
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
        public IActionResult PatchBook(string id, [FromBody] BookDTO model)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = bookService.UpdateBook(id, model)
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
        public IActionResult DeleteBook(string id)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = bookService.DeleteBook(id),
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

        [HttpDelete]
        [Route("{id}/categories/{categoryId}")]
        public IActionResult DeleteCategortBook(string id, string categoryId)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.OK, new DefaultResponse {
                    Success = bookService.DeleteCategoryBook(id, categoryId),
                    Code = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = null
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