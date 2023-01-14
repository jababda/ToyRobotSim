namespace ToyRobotSim.Console.Meditr.Commands;

internal record ProcessPlaceStringCommand(string CommandString, SimulationData simData) : IRequest<SimulationActionResult>;

internal class ProcessPlaceStringCommandHandler : IRequestHandler<ProcessPlaceStringCommand, SimulationActionResult>
{
    private readonly ISimulationService _simulationService;

    public ProcessPlaceStringCommandHandler(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    public Task<SimulationActionResult> Handle(ProcessPlaceStringCommand request, CancellationToken cancellationToken)
    {
        var sanatisedCommand = request.CommandString.Trim().ToUpper();

        if (!sanatisedCommand.StartsWith("PLACE"))
            throw new ArgumentException("Command string must start with PLACE", request.CommandString);

        var placeCommandData = ParsePlaceCommand(sanatisedCommand);

        return Task.FromResult(_simulationService.Place(request.simData, placeCommandData));
    }

    internal PlaceCommandData ParsePlaceCommand(string sanatisedCommand)
    {
        sanatisedCommand = sanatisedCommand.Replace("PLACE", "").Trim();
        var placeCommandParts = sanatisedCommand.Split(',');

        try
        {
            return placeCommandParts.Length switch
            {
                2 => new PlaceCommandData(int.Parse(placeCommandParts[0]), int.Parse(placeCommandParts[1])),
                3 => new PlaceCommandData(int.Parse(placeCommandParts[0]), int.Parse(placeCommandParts[1]), (RobotOrintation)Enum.Parse(typeof(RobotOrintation), placeCommandParts[2], true)),
                _ => throw new ArgumentException($"{sanatisedCommand} has too many commas", sanatisedCommand)
            };
        }
        catch(Exception e)
        {
            throw new ArgumentException($"{sanatisedCommand} cannot be parsed to command string", sanatisedCommand, e);
        }
    }
}
