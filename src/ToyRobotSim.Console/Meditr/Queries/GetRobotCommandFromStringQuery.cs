namespace ToyRobotSim.Console.Meditr.Queries;

internal record GetRobotCommandFromStringQuery(string Command) : IRequest<RobotCommand>;

internal class GetRobotCommandFromStringQueryHandler : IRequestHandler<GetRobotCommandFromStringQuery, RobotCommand>
{
    public Task<RobotCommand> Handle(GetRobotCommandFromStringQuery request, CancellationToken cancellationToken)
    {
        var command = request.Command.Trim().ToLowerInvariant();

        if (command.StartsWith("place"))
        {
            return Task.FromResult(RobotCommand.Place);
        }

        return Task.FromResult(command switch
        {
            "move" => RobotCommand.Move,
            "left" => RobotCommand.Left,
            "right" => RobotCommand.Right,
            "report" => RobotCommand.Report,
            _ => RobotCommand.Unknown
        });
    }
}
