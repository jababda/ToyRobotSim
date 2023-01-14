namespace ToyRobotSim.Console.Meditr.Commands;

internal record TurnRobotRightCommand(SimulationData SimData) : IRequest<SimulationActionResult>;

internal class TurnRobotRightCommandHandler : IRequestHandler<TurnRobotRightCommand, SimulationActionResult>
{
    private readonly ISimulationService _simulationService;

    public TurnRobotRightCommandHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<SimulationActionResult> Handle(TurnRobotRightCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(_simulationService.Right(request.SimData));
}