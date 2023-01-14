using ToyRobotSim;
using Microsoft.Extensions.DependencyInjection;
using ToyRobotSim.Console.Meditr.Commands;
using ToyRobotSim.Console.Meditr.Queries;

var mapLength = 6;
var mapWidth = 6;

var serviceProvider = new ServiceCollection()
    .AddToyRobotSim()
    .AddMediatR(typeof(Program).Assembly)
    .BuildServiceProvider();

var mediatr = serviceProvider.GetRequiredService<IMediator>();

var simData = await mediatr.Send(new GetSimulationQuery(mapLength, mapWidth));

var intro =
$@"Welcome to the Toy Robot on a Table Simulator!
First you need to place the robot on the table.
Then you can move the robot around the table.
You can also change the direction of the robot by turning it left or right.
The robot will only move forward, so make sure you remember where its facing!
The table is {mapLength} by {mapWidth} units wide.
The valid coordinate ranges are (0,0) which is the SOUTH WEST to ({mapLength - 1}{mapWidth - 1}) which is the NORTH EAST.
Dont worry though the robot will not fall off the table.
If you don't place the robot on the table first, you can't move it.
When your done moving the robot around the table ask it to report its position, after which it will fly off the table so you can go again!";

Console.WriteLine(intro);

var inputPrompt =
@"Please enter your commands, starting with PLACE command.
PLACE X,Y,DIRECTION (DIRECTION is optional after the first place)
MOVE
LEFT
RIGHT
REPORT";

Console.WriteLine(inputPrompt);

while(true)
{
    var placeCommandString = Console.ReadLine();
    var robotCommand = await mediatr.Send(new GetRobotCommandFromStringQuery(placeCommandString));

    if (robotCommand != RobotCommand.Place)
    {
        Console.WriteLine("You must place the robot on the table first!");
        continue;
    }

    var placeRes = await mediatr.Send(new ProcessPlaceStringCommand(placeCommandString, simData));

    if (!placeRes.Result)
    {
        Console.WriteLine("That doesn't look like a valid place command, please try again");
        continue;

    }

    simData = placeRes.SimData;
    break;
}

while(true)
{
    var robotCommandString = Console.ReadLine();
    var robotCommand = await mediatr.Send(new GetRobotCommandFromStringQuery(robotCommandString));

    if (RobotCommand.Report == robotCommand)
    {
        Console.WriteLine(await mediatr.Send(new ReportRobotPositionQuery(simData)));
        break;
    }

    simData = robotCommand switch
    {
        RobotCommand.Move => (await mediatr.Send(new MoveRobotCommand(simData))).SimData,
        RobotCommand.Left => (await mediatr.Send(new TurnRobotLeftCommand(simData))).SimData,
        RobotCommand.Right => (await mediatr.Send(new TurnRobotRightCommand(simData))).SimData,
        _ => simData
    };
}





