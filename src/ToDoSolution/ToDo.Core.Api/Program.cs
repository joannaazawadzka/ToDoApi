using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ToDo.Core.Api.Middleware;
using ToDo.Core.Application;
using ToDo.Core.Application.DatabaseContext;
using ToDo.Core.Application.RepositoryInterfaces;
using ToDo.Core.Infrastructure.DatabaseContext;
using ToDo.Core.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

ConfigureDatabase(builder);
ConfigureMediatR(builder);
ConfigureRepositories(builder);
ConfigureSwagger(builder);

var app = builder.Build();

// Add the global exception handler middleware to the pipeline
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Enable Swagger in the HTTP pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
    options.RoutePrefix = string.Empty; // Optional: Makes Swagger UI accessible at the root URL.
});

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<CoreDatabaseContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("ToDoCoreDatabase");
        options.UseSqlServer(connectionString);
    }).AddScoped<ICoreDatabaseContext, CoreDatabaseContext>();
}

void ConfigureMediatR(WebApplicationBuilder builder)
{
    builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(CoreModuleClassForMediator).Assembly));
}

void ConfigureRepositories(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ToDo API",
            Version = "v1",
            Description = "An API for managing ToDo tasks",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "your-email@example.com",
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    });
}