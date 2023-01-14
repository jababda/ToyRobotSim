using Microsoft.Extensions.DependencyInjection;
using ToyRobotSim.Services;

namespace ToyRobotSim;

public static class ServiceCollectionExtensions
{
    public static ServiceCollection AddToyRobotSim(this ServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<ISimulationService, SimulationService>()
            .AddScoped<IMapService, MapService>();

        return serviceCollection;
    }
}
