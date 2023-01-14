namespace ToyRobotSim.Console.Meditr.Commands;

internal record MoveRobotCommand(SimulationData SimData) : IRequest<SimulationActionResult>;

internal class MoveRobotCommandHandler : IRequestHandler<MoveRobotCommand, SimulationActionResult>
{
    private readonly ISimulationService _simulationService;

    public MoveRobotCommandHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<SimulationActionResult> Handle(MoveRobotCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(_simulationService.Move(request.SimData));
}