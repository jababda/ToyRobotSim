namespace ToyRobotSim.Console.Meditr.Commands;

internal record TurnRobotLeftCommand(SimulationData SimData) : IRequest<SimulationActionResult>;

internal class TurnRobotLeftCommandHandler : IRequestHandler<TurnRobotLeftCommand, SimulationActionResult>
{
    private readonly ISimulationService _simulationService;

    public TurnRobotLeftCommandHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<SimulationActionResult> Handle(TurnRobotLeftCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(_simulationService.Left(request.SimData));
}