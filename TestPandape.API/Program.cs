using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestPandape.API;
using TestPandape.Entity.UriServices;
using TestPandape.Repository.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string title = "Api PANDAPE";
string version = "1.0";
string description = $"{title} {version} - Knowledge Test.";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IUriservice>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext?.Request;
    var uri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent());
    return new Uriservice(uri);
});

//add Dependency Injection
builder.Services.AddDependency();
//add DataBase in Memory
builder.Services.AddDbContext<DatabaseContext>();
////builder.Services.AddDbContext<DatabaseContext>(opt =>
////{
////    opt.UseInMemoryDatabase(databaseName: "ApplicantDB");
////});

var app = builder.Build();

////using (var scope = app.Services.CreateScope())
////{
////    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
////    context.Database.IsInMemory();
////}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", description);
        });
    }

app.UseCors(option =>
{
    option.WithOrigins("*");
    option.AllowAnyMethod();
    option.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
