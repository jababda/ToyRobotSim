namespace ToyRobotSim.Domain;

public record SimulationData
{
    public SimulationMap Map { get; set; } = null!;
    public bool RobotPlaced { get; set; } = false;
    public (int X, int Y) RobotPosition { get; set; } = (0, 0);
    public RobotOrintation Orintation { get; set; } = RobotOrintation.North;
}
