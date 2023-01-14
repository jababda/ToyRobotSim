
namespace ToyRobotSim.Console.Tests.Meditr.Commands;

public class ProcessPlaceStringCommandTests
{
    private readonly ISimulationService _subSimService;
    private readonly ProcessPlaceStringCommandHandler _hut;

    public ProcessPlaceStringCommandTests()
    {
        _subSimService = Substitute.For<ISimulationService>();
        _hut = new ProcessPlaceStringCommandHandler(_subSimService);
    }

    [Fact]
    public void Handle_WhenCommandStringDoesNotStartWithPlace_ThrowsArgumentException()
    {
        var cmd = new ProcessPlaceStringCommand("MOVE", new SimulationData());

        Assert.ThrowsAsync<ArgumentException>(() => _hut.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WhenCommandStringStartsWithPlace_ReturnsSimulationActionResult()
    {
        var expectedRes = new SimulationActionResult(
            Result: true,
            SimData: new SimulationData
            {
                RobotPosition = (2, 2),
                RobotPlaced = true
            }
        );

        _subSimService.Place(Arg.Any<SimulationData>(), Arg.Any<PlaceCommandData>())
            .Returns(expectedRes);

        var initialSimData = new SimulationData();
        var expectedPlaceCommandData = new PlaceCommandData(1, 1);

        var cmd = new ProcessPlaceStringCommand("PLACE 1,1", initialSimData);

        var res = await _hut.Handle(cmd, CancellationToken.None);

        Assert.Equal(expectedRes, res);

        _subSimService.Received().Place(
            Arg.Is<SimulationData>(_ => _ == initialSimData),
            Arg.Is<PlaceCommandData>(_ => _ == expectedPlaceCommandData));
    }

    [Fact]
    public void ParsePlaceCommand_WhenCommandStringDoesNotStartWithPlace_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _hut.ParsePlaceCommand("MOVE"));
    }

    [Theory]
    [InlineData("PLACE 1,1,NORTH", 1, 1, RobotOrintation.North)]
    [InlineData("PLACE 3,4,EAST", 3, 4, RobotOrintation.East)]
    [InlineData("PLACE 1,1,SOUTH", 1, 1, RobotOrintation.South)]
    [InlineData("PLACE 0,0,WEST", 0, 0, RobotOrintation.West)]
    [InlineData("PLACE 1,1", 1, 1, null)]
    public void ParsePlaceCommand_ReturnsCorrectPlaceCommandData(string input, int expectedX, int expectedY, RobotOrintation? expectedOrintation )
    {
        var expectedRes = new PlaceCommandData(expectedX, expectedY, expectedOrintation);

        var res = _hut.ParsePlaceCommand(input);

        Assert.Equal(expectedRes, res);
    }

    [Theory]
    [InlineData("PLACE ,,,,,,")]
    [InlineData("PLACE 3,a,EAST")]
    [InlineData("PLACE b,1,NotAThing")]
    [InlineData("PLACE")]
    [InlineData("PLACE 1,1,1,1")]
    [InlineData("PLACE ,1,1")]
    [InlineData("PLACE ,1")]
    [InlineData("PLACE ,1,")]
    public void ParsePlaceCommand_WithInvalidString_ThrowsArugementExcpetion(string input)
    {
        Assert.Throws<ArgumentException>(() => _hut.ParsePlaceCommand(input));
    }
}
