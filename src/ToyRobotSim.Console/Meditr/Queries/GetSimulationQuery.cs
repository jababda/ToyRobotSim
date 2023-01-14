namespace ToyRobotSim.Console.Meditr.Queries;

record GetSimulationQuery(int MapWidth, int MapHeight) : IRequest<SimulationData>;

class GetSimulationQueryHandler : IRequestHandler<GetSimulationQuery, SimulationData>
{
    private readonly ISimulationService _simulationService;

    public GetSimulationQueryHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<SimulationData> Handle(GetSimulationQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            _simulationService.CreateFreshSimulation(new SimulationMap(request.MapWidth, request.MapHeight))
            );
}
