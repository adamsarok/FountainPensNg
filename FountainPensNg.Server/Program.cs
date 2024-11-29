using Carter;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Repos;
using FountainPensNg.Server.Helpers;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.CustomSchemaIds(type => type.ToString());
});
builder.Services.AddCarter();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

string? conn;
if (builder.Environment.IsDevelopment()) conn = builder.Configuration.GetConnectionString("DefaultConnection");
else {
    conn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    if (string.IsNullOrWhiteSpace(conn)) conn = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrWhiteSpace(conn)) throw new Exception("Connection string is empty"); //bad ide to throw here?

builder.Services.AddDbContextFactory<DataContext>(opt =>
    opt.UseNpgsql(conn));

builder.Services.RegisterMapsterConfiguration();

builder.Services.AddTransient<FinderRepo>();
builder.Services.AddTransient<FountainPensRepo>();
builder.Services.AddTransient<InkedUpsRepo>();
builder.Services.AddTransient<InksRepo>();
builder.Services.AddTransient<PapersRepo>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapCarter();

#warning todo!!!
//app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost", "http://192.168.1.79:4200", "https://192.168.1.79:4200", "https://localhost:4200", "http://localhost:4200", "http://localhost:8080"));

app.UseDefaultFiles();
app.UseStaticFiles();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
//app.MapFallbackToController("Index", "Fallback"); //???

app.Run();

public partial class Program { } //for integration tests