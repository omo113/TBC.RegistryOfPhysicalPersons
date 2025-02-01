using System.Text.Json;
using System.Text.Json.Serialization;
using TBC.ROPP.Api;
using TBC.ROPP.Application;
using TBC.ROPP.Application.Middlewares;
using TBC.ROPP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddEnvironmentVariables();
builder.Services.AddCors(c =>
    {
        c.AddPolicy("AllowOrigin", options =>
        options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
builder.Services
    .AddDatabaseContext(builder.Configuration)
    .AddFileInfrastructure()
    .AddApplication(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase; })
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddOpenApi();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.AddRequestLocalization();
app.Run();
