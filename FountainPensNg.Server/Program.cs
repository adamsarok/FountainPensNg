using Carter;
using FountainPensNg.Server.API;
using FountainPensNg.Server.Config;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Repos;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Services;
using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.CustomSchemaIds(type => type.ToString());
});
builder.Services.AddCarter();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

string? conn = builder.Configuration.GetConnectionString("FountainPens");
if (string.IsNullOrWhiteSpace(conn)) throw new Exception("Connection string is empty");
 
builder.Services.AddDbContextFactory<FountainPensContext>(opt => {
    opt.UseNpgsql(conn);
    opt.AddInterceptors(new EntityInterceptor());
});
builder.Services.AddHealthChecks()
	.AddNpgSql(conn);

builder.Services.AddHttpContextAccessor();
builder.Services.RegisterMapsterConfiguration();

builder.Services.AddHttpClient<R2UploadService>();

builder.Services.AddScoped<FinderRepo>();
builder.Services.AddScoped<FountainPensRepo>();
builder.Services.AddScoped<InkedUpsRepo>();
builder.Services.AddScoped<InksRepo>();
builder.Services.AddScoped<PapersRepo>();
builder.Services.AddScoped<IR2UploadService, R2UploadService>();
builder.Services.AddScoped<IPresignedUrlService, PresignedUrlService>();
builder.Services.AddSingleton<R2Configuration>(sp =>
	new R2Configuration(sp.GetRequiredService<IConfiguration>()));

var app = builder.Build();

app.UseExceptionHandler();
app.MapCarter();

using (var scope = app.Services.CreateScope()) {
	var dbContext = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
	await dbContext.Database.MigrateAsync();
}

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost", "http://192.168.1.79:4200", "https://192.168.1.79:4200", "https://localhost:4200", "http://localhost:4200", "http://localhost:8080"));

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseHealthChecks("/api/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions {
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program { } //for integration tests