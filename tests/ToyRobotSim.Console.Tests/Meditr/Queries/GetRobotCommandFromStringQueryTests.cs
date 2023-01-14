namespace ToyRobotSim.Console.Tests.Meditr.Queries;

public class GetRobotCommandFromStringQueryTests
{
    private readonly GetRobotCommandFromStringQueryHandler _hut;

    public GetRobotCommandFromStringQueryTests()
    {
        _hut = new GetRobotCommandFromStringQueryHandler();
    }

    [Theory]
    [InlineData("MOVE", RobotCommand.Move)]
    [InlineData("LEFT", RobotCommand.Left)]
    [InlineData("RIGHT", RobotCommand.Right)]
    [InlineData("REPORT", RobotCommand.Report)]
    [InlineData("asdfas", RobotCommand.Unknown)]
    [InlineData("", RobotCommand.Unknown)]
    [InlineData("  ", RobotCommand.Unknown)]
    public async Task Handle_WhenCommandStringIsRecognised_ReturnsCommand(string commandString, RobotCommand expectedCommand)
    {
        var query = new GetRobotCommandFromStringQuery(commandString);

        var res = await _hut.Handle(query, CancellationToken.None);

        Assert.Equal(expectedCommand, res);
    }

    [Theory]
    [InlineData("PLACE 1,1")]
    [InlineData("PLACE 1,1,NORTH")]
    [InlineData("PLACE 1,1,NORTH,1")]
    public async Task Handle_WhenCommandStringIsPlace_ReturnsPlaceCommand(string commandString)
    {
        var query = new GetRobotCommandFromStringQuery(commandString);

        var res = await _hut.Handle(query, CancellationToken.None);

        Assert.Equal(RobotCommand.Place, res);
    }
}
