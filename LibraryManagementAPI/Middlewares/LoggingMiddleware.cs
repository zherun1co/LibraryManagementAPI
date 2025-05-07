using Serilog;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LibraryManagementAPI.Middlewares
{
    public class LoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate requestDelegate = next;

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            context.Request.Body.Position = 0;
            using StreamReader requestReader = new(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
            string requestBody = await requestReader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            Stream originalBodyStream = context.Response.Body;
            using MemoryStream responseBodyStream = new();
            context.Response.Body = responseBodyStream;

            await requestDelegate(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            int statusCode = context.Response.StatusCode;
            string statusDescription = Enum.IsDefined(typeof(HttpStatusCode), statusCode)
                ? ((HttpStatusCode)statusCode).ToString()
                : string.Empty;

            Log.ForContext("Method", context.Request.Method)
               .ForContext("Path", context.Request.Path)
               .ForContext("RequestId", context.TraceIdentifier)
               .ForContext("RequestPath", context.Request.Path)
               .ForContext("RequestBody", requestBody)
               .ForContext("StatusCode", statusCode)
               .ForContext("StatusDescription", statusDescription)
               .ForContext("ResponseBody", responseBody)
               .Information("HTTP {Method} {Path} → Status Code: {StatusCode} - {StatusDescription}");

            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }
}