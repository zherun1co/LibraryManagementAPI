using Serilog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LibraryManagementAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    .WriteTo.Seq(configuration.GetValue<string>("Serilog:WriteTo:2:Args:serverUrl"))
    .CreateLogger();

// Replace the default logging system with Serilog
builder.Host.UseSerilog();

// Add the database context using the connection string from appsettings.json
builder.Services.AddDbContext<LibraryManagementContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Setting up authentication with JwtBearer
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.Authority = configuration.GetSection("Keycloak")["Authority"];
    options.Audience = configuration.GetSection("Keycloak")["ClientId"];
    options.RequireHttpsMetadata = bool.Parse(configuration.GetSection("Keycloak")["RequireHttpsMetadata"]);
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuers = configuration.GetSection("Keycloak:ValidIssuers").Get<string[]>(),
        ValidateAudience = true,
        ValidAudiences = configuration.GetSection("Keycloak:ValidAudiences").Get<string[]>(),
        ValidateIssuerSigningKey = true
    };
});

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
    options.ListenAnyIP(configuration.GetValue<int>("Kestrel:Endpoints:Http:Port"));
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
    options.AddPolicy(configuration.GetValue<string>("Cors:Clients:0:ClientName"), policy => {
        policy.WithOrigins(configuration.GetValue<string>("Cors:Clients:0:ClientOrigin"))
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Applies the CORS policy Angular Client to enable cross-origin 
app.UseCors(configuration.GetValue<string>("Cors:Clients:0:ClientName"));

// Register the Logging Middleware for Serilog
app.UseMiddleware<LoggingMiddleware>();

// Add Middleware to handle 401 Unauthorized
app.UseMiddleware<UnauthorizedMiddleware>();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// Enable HTTP request logging with Serilog
app.UseSerilogRequestLogging();

app.Run();