using System.Net;
using Newtonsoft.Json;
using LibraryManagementModel.Responses;

namespace LibraryManagementAPI.Middlewares
{
    public class UnauthorizedMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate requestDelegate = next;

        public async Task InvokeAsync(HttpContext context)
        {
            await requestDelegate(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized) {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(
                    new DefaultResponse() {
                        Success = false,
                        Code = (int)HttpStatusCode.Unauthorized,
                        Message = "Unauthorized"
                    }, Formatting.None)
                );
            }
        }
    }
}