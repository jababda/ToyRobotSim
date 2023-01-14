using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace ToyRobotSim.Console.Tests;


public class IntegrationTests
{
    private readonly IMediator _mediatr;

    public IntegrationTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddToyRobotSim()
            .AddMediatR(typeof(Program).Assembly)
            .BuildServiceProvider();

        _mediatr = serviceProvider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task ExampleA()
    {
        //PLACE 0,0,NORTH
        //MOVE
        //REPORT
        //Output: 0,1,NORTH

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 0,0,NORTH", simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("0,1,NORTH", report);
    }

    [Fact]
    public async Task ExampleB()
    {
        //PLACE 0,0,NORTH
        //LEFT
        //REPORT
        //Output: 0,0,WEST

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 0,0,NORTH", simData))).SimData;

        simData = (await _mediatr.Send(new TurnRobotLeftCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("0,0,WEST", report);
    }

    [Fact]
    public async Task ExampleC()
    {
        //PLACE 1,2,EAST
        //MOVE
        //MOVE
        //LEFT
        //MOVE
        //REPORT
        //Output: 3,3,NORTH

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 1,2,EAST", simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new TurnRobotLeftCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("3,3,NORTH", report);
    }

    [Fact]
    public async Task ExampleD()
    {
        //PLACE 1,2,EAST
        //MOVE
        //LEFT
        //MOVE
        //PLACE 3,1
        //MOVE
        //REPORT
        //Output: 3,2,NORTH

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 1,2,EAST", simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new TurnRobotLeftCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 3,1", simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("3,2,NORTH", report);
    }

    [Fact]
    public async Task TestRightTurns()
    {
        //PLACE 1,2,EAST
        //MOVE
        //MOVE
        //RIGHT
        //MOVE
        //RIGHT
        //REPORT
        //Output: 3,3,NORTH

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 1,2,EAST", simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new TurnRobotRightCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new TurnRobotRightCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("3,1,WEST", report);
    }

    [Fact]
    public async Task SouthWestToNorthEast()
    {
        //PLACE 0,0,SOUTH
        //MOVE
        //RIGHT
        //MOVE
        //RIGHT
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //RIGHT
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //MOVE
        //REPORT
        //Output: 5,5,EAST

        var mapLength = 6;
        var mapWidth = 6;

        var simData = await _mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

        simData = (await _mediatr.Send(new ProcessPlaceStringCommand("PLACE 0,0,SOUTH", simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new TurnRobotRightCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        simData = (await _mediatr.Send(new TurnRobotRightCommand(simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        simData = (await _mediatr.Send(new TurnRobotRightCommand(simData))).SimData;

        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;
        simData = (await _mediatr.Send(new MoveRobotCommand(simData))).SimData;

        var report = await _mediatr.Send(new ReportRobotPositionQuery(simData));

        Assert.Equal("5,5,EAST", report);
    }
}
