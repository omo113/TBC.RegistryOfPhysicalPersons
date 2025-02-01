using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TBC.ROPP.Infrastructure;
using TBC.ROPP.MigrationClient;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureAppConfiguration((context, configurationBuilder) =>
{
    configurationBuilder.AddEnvironmentVariables();
})
.ConfigureServices((hostContext, sc) =>
{
    sc.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    sc.AddDatabaseContext(hostContext.Configuration);
    sc.AddMigrations();
    sc.AddHostedService<MigrationService>();
});

await builder.Build()
    .RunAsync();