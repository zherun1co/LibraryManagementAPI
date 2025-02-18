using Serilog;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Middleware;
using LibraryManagementRepository.Repositories.Interface;
using LibraryManagementServices.BusinessServices.Service;
using LibraryManagementRepository.Repositories.Repository;
using LibraryManagementServices.BusinessServices.Interface;
using LibraryManagementRepository.Repositories.DatabaseContext;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
ConfigurationManager configuration = builder.Configuration;

// Configure Serilog using the loaded configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Seq(configuration.GetValue<string>("Serilog:WriteTo:2:Args:serverUrl") ?? string.Empty) // Uses SEQ URL from appsettings.json
    .CreateLogger();

// Replace the default logging system with Serilog
builder.Host.UseSerilog();

// Add the database context using the connection string from appsettings.json
builder.Services.AddDbContext<LibraryManagementContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Dependency Injection for Repositories
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Dependency Injection for Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Configure the ports on which the API will listen (Using Kestrel)
builder.WebHost.ConfigureKestrel(options => {
    options.ListenAnyIP(configuration.GetValue<int>("Kestrel:Endpoints:Http:Port", 5100)); // Default to 5100 if not specified
});

// Add Controllers with Newtonsoft.Json
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Avoid infinite loops in references
    options.SerializerSettings.Formatting = Formatting.Indented; // Indenting JSON for readability
});

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configures CORS to allow requests from the Angular frontend
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngularClient", policy => {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Applies the CORS policy named "AllowAngularClient" to enable cross-origin 
app.UseCors("AllowAngularClient");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Register the Logging Middleware for Serilog
app.UseMiddleware<LoggingMiddleware>();

// Enable HTTP request logging with Serilog
app.UseSerilogRequestLogging();

app.Run();