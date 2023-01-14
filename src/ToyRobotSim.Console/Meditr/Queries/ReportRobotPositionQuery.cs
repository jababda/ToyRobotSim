namespace ToyRobotSim.Console.Meditr.Queries;

internal record ReportRobotPositionQuery(SimulationData SimData) : IRequest<string>;

internal class ReportRobotPositionQueryHandler : IRequestHandler<ReportRobotPositionQuery, string>
{
    private readonly ISimulationService _simulationService;

    public ReportRobotPositionQueryHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<string> Handle(ReportRobotPositionQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(_simulationService.Report(request.SimData));
}