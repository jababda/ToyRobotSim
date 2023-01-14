using ToyRobotSim.Domain;

namespace ToyRobotSim.Services;

internal interface IMapService
{
    bool IsPositionValid(SimulationMap map, int x, int y);
}

internal class MapService : IMapService
{
    public bool IsPositionValid(SimulationMap map, int x, int y)
    {
        if (map.Width <= 0 || map.Height <= 0)
            throw new ArgumentException("Map must have a positive width and height");

        return x >= 0 && x < map.Width && y >= 0 && y < map.Height;
    }
}