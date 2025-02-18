using Serilog;

namespace LibraryManagementAPI.Middleware
{
    public class LoggingMiddleware (RequestDelegate next)
    {
        private readonly RequestDelegate requestDelegate = next;

        public async Task Invoke(HttpContext context)
        {
            // Capture Request
            context.Request.EnableBuffering();

            using StreamReader requestReader = new(context.Request.Body);
            string requestBody = await requestReader.ReadToEndAsync();
            
            context.Request.Body.Position = 0; // Reset body position for the next middleware

            Log.Information("HTTP Request: {Method} {Url} - Body: {RequestBody}",
                context.Request.Method, context.Request.Path, requestBody);

            // Capture Response
            Stream originalBodyStream = context.Response.Body;

            using MemoryStream responseBodyStream = new();
            context.Response.Body = responseBodyStream;

            await requestDelegate(context); // Continue to next middleware

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            Log.Information("HTTP Response: {StatusCode} - Body: {ResponseBody}",
                context.Response.StatusCode, responseBody);

            await responseBodyStream.CopyToAsync(originalBodyStream); // Send response to the client
        }
    }
}