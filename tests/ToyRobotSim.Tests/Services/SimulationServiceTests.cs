namespace ToyRobotSim.Tests.Services;

public class SimulationServiceTests
{
    private readonly IMapService _subMapService;
    private readonly SimulationService _sut;


    public SimulationServiceTests()
    {
        _subMapService = Substitute.For<IMapService>();
        _sut = new SimulationService(_subMapService);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(-1, -1)]
    [InlineData(5, -1)]
    [InlineData(-1, 5)]
    public void CreateFreshSimulation_WhenMapWidthOrLengthIs0OrLess_ThrowsArgumentException(int width, int height)
    {
        Assert.Throws<ArgumentException>(() => _sut.CreateFreshSimulation(new SimulationMap(width, height)));
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(6, 6)]
    public void CreateFreshSimulation_WithPositiveValues_ReturnsNewObject(int width, int height)
    {
        var res = _sut.CreateFreshSimulation(new SimulationMap(width, height));
        Assert.Equal(width, res.Map.Width);
        Assert.Equal(height, res.Map.Height);
    }

    [Fact]
    public void Place_WhenPositionIsNotValid_Fails()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(false);

        var res = _sut.Place(new SimulationData(), new PlaceCommandData(1, 1, RobotOrintation.North));

        Assert.False(res.Result);
    }

    [Fact]
    public void Place_WhenRobotIsNotPlacedAndOrintationIsNotProvided_Fails()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(true);

        var simData = new SimulationData
        {
            RobotPlaced = false
        };

        var res = _sut.Place(simData, new PlaceCommandData(1, 1));

        Assert.False(res.Result);
    }

    [Fact]
    public void Place_WhenRobotIsNotPlacedAndOrintationIsProvided_Succeds()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(true);

        var simData = new SimulationData
        {
            RobotPlaced = false
        };

        var res = _sut.Place(simData, new PlaceCommandData(1, 1, RobotOrintation.North));

        Assert.True(res.Result);
    }

    [Fact]
    public void Place_WhenRobotIsPlacedAndOrintationIsNotProvided_Succeds()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(true);

        var simData = new SimulationData
        {
            RobotPlaced = true,
            Orintation = RobotOrintation.South
        };

        var res = _sut.Place(simData, new PlaceCommandData(1, 1));

        Assert.True(res.Result);
        Assert.Equal(simData.Orintation, res.SimData.Orintation);
    }

    [Fact]
    public void Place_WhenRobotIsPlacedAndOrintationIsProvided_Succeds()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(true);

        var simData = new SimulationData
        {
            RobotPlaced = false,
            Orintation = RobotOrintation.North
        };

        var expectedOrintation = RobotOrintation.South;

        var res = _sut.Place(simData, new PlaceCommandData(1, 1, expectedOrintation));

        Assert.True(res.Result);
        Assert.Equal(expectedOrintation, res.SimData.Orintation);
    }

    [Fact]
    public void Move_IfRobotNotPlace_Fails()
    {
        var simData = new SimulationData
        {
            RobotPlaced = false
        };

        var res = _sut.Move(simData);
        Assert.False(res.Result);
    }

    [Fact]
    public void Move_IfNewPositionIsNotValid_Fails()
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(false);

        var simData = new SimulationData
        {
            RobotPlaced = true
        };

        var res = _sut.Move(simData);
        Assert.False(res.Result);
    }

    [Theory]
    [InlineData(RobotOrintation.North, 0, 1)]
    [InlineData(RobotOrintation.East, 1, 0)]
    [InlineData(RobotOrintation.South, 0, -1)]
    [InlineData(RobotOrintation.West, -1, 0)]
    public void Move_HasTheCorrectPostionForAnOrintation(RobotOrintation orintation, int changeX, int changeY)
    {
        _subMapService
            .IsPositionValid(Arg.Any<SimulationMap>(), Arg.Any<int>(), Arg.Any<int>())
            .ReturnsForAnyArgs(true);

        var position = (X: 2, Y: 2);

        var simData = new SimulationData
        {
            RobotPlaced = true,
            Orintation = orintation,
            RobotPosition = position
        };

        var res = _sut.Move(simData);

        Assert.True(res.Result);
        Assert.Equal(position.X + changeX, res.SimData.RobotPosition.X);
        Assert.Equal(position.Y + changeY, res.SimData.RobotPosition.Y);
    }

    [Fact]
    public void Left_IfRobotNotPlace_Fails()
    {
        var simData = new SimulationData
        {
            RobotPlaced = false
        };

        var res = _sut.Left(simData);
        Assert.False(res.Result);
    }


    [Theory]
    [InlineData(RobotOrintation.North, RobotOrintation.West)]
    [InlineData(RobotOrintation.West, RobotOrintation.South)]
    [InlineData(RobotOrintation.South, RobotOrintation.East)]
    [InlineData(RobotOrintation.East, RobotOrintation.North)]
    public void Left_WithCorrectOrintation_Succeds(RobotOrintation original, RobotOrintation expected)
    {
        var simData = new SimulationData
        {
            RobotPlaced = true,
            Orintation = original
        };

        var res = _sut.Left(simData);
        Assert.True(res.Result);
        Assert.Equal(expected, res.SimData.Orintation);
    }

    [Fact]
    public void Right_IfRobotNotPlace_Fails()
    {
        var simData = new SimulationData
        {
            RobotPlaced = false
        };

        var res = _sut.Right(simData);
        Assert.False(res.Result);
    }


    [Theory]
    [InlineData(RobotOrintation.North, RobotOrintation.East)]
    [InlineData(RobotOrintation.East, RobotOrintation.South)]
    [InlineData(RobotOrintation.South, RobotOrintation.West)]
    [InlineData(RobotOrintation.West, RobotOrintation.North)]
    public void Right_WithCorrectOrintation_Succeds(RobotOrintation original, RobotOrintation expected)
    {
        var simData = new SimulationData
        {
            RobotPlaced = true,
            Orintation = original
        };

        var res = _sut.Right(simData);
        Assert.True(res.Result);
        Assert.Equal(expected, res.SimData.Orintation);
    }

    [Fact]
    public void Report_WhenRobotNotPlaced_ReturnsCorrectMessage()
    {
        var simData = new SimulationData
        {
            RobotPlaced = false,
        };

        var res = _sut.Report(simData);
        Assert.Equal("Robot not placed", res);
    }

    [Theory]
    [InlineData(0, 0, RobotOrintation.North)]
    [InlineData(5, 7, RobotOrintation.South)]
    [InlineData(9, 12, RobotOrintation.East)]
    [InlineData(3, 1, RobotOrintation.West)]
    public void Report_WhenRobotPlaced_ReturnsCorrectMessage(int x, int y, RobotOrintation orintation)
    {
        var simData = new SimulationData
        {
            RobotPlaced = true,
            RobotPosition = (x, y),
            Orintation = orintation
        };

        var res = _sut.Report(simData);
        Assert.Equal($"{x},{y},{orintation}".ToUpper(), res);
    }

}