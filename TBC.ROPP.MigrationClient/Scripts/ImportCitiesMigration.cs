using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.MigrationClient.Abstractions;
using TBC.ROPP.Shared;

namespace TBC.ROPP.MigrationClient.Scripts;

internal record Cities(string NameKa, string Name);

public class ImportCitiesMigration(IServiceScopeFactory scopeFactory) : PostMigrationClientScript
{
    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetAssembly(typeof(SystemDate));
        var resourceName = $"{assembly!.GetName().Name}.ImportedFiles.subdivisions.json";

        await using var stream = assembly.GetManifestResourceStream(resourceName);

        var cities = await JsonSerializer.DeserializeAsync<ICollection<Cities>>(stream ?? throw new InvalidOperationException(),
                                                                                  new JsonSerializerOptions
                                                                                  {
                                                                                      PropertyNameCaseInsensitive = true
                                                                                  },
                                                                                  cancellationToken: cancellationToken);

        await using var scope = scopeFactory.CreateAsyncScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var cityRepository = scope.ServiceProvider.GetRequiredService<IRepository<City>>();

        foreach (var city in cities!)
        {

            await cityRepository.Store(
                City.Create(city.Name, city.NameKa));
        }

        await unitOfWork.SaveAsync(cancellationToken);
    }
}