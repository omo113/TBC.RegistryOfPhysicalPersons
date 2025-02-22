using System.Text.Json;
using System.Text.Json.Serialization;
using TBC.ROPP.Api;
using TBC.ROPP.Api.Middlewares;
using TBC.ROPP.Application;
using TBC.ROPP.Infrastructure;
using TBC.ROPP.MigrationClient;
using TBC.ROPP.Shared.Settings;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(ApplicationSettings.Section));
builder.Services.Configure<AWSSettings>(builder.Configuration.GetSection(AWSSettings.Section));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.Section));
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services
    .AddDatabaseContext(builder.Configuration)
    .AddFileInfrastructure()
    .AddApplication(builder.Configuration);

builder.Services.AddMigrations();
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase; })
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwagger();
builder.Services.AddLocalizationService();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.AddRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program;//todo for testing
