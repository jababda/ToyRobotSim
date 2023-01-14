using ToyRobotSim.Domain;

namespace ToyRobotSim.Services;

public interface ISimulationService
{
    SimulationData CreateFreshSimulation(SimulationMap map);
    SimulationActionResult Place(SimulationData simData, PlaceCommandData placeCommandData);
    SimulationActionResult Move(SimulationData simData);
    SimulationActionResult Left(SimulationData simData);
    SimulationActionResult Right(SimulationData simData);
    string Report(SimulationData simData);
}

internal class SimulationService : ISimulationService
{
    private readonly IMapService _mapService;

    public SimulationService(IMapService mapService)
    {
        _mapService = mapService;
    }

    public SimulationData CreateFreshSimulation(SimulationMap map)
    {
        if (map.Width <= 0 || map.Height <= 0)
            throw new ArgumentException("Map must have a positive width and height");

        return new SimulationData
        {
            Map = map
        };
    }

    public SimulationActionResult Place(SimulationData simData, PlaceCommandData placeCommandData)
    {
        if (!_mapService.IsPositionValid(simData.Map, placeCommandData.X, placeCommandData.Y))
        {
            return new (false, simData);
        }

        if (!simData.RobotPlaced && placeCommandData.Orintation == null)
        {
            return new (false, simData);
        }

        return new (true, simData with
        {
            RobotPosition = (placeCommandData.X, placeCommandData.Y),
            RobotPlaced = true,
            Orintation = placeCommandData.Orintation ?? simData.Orintation
        });

    }

    public SimulationActionResult Move(SimulationData simData)
    {
        if (!simData.RobotPlaced)
        {
            return new (false, simData);
        }

        var (newX, newY) = simData.Orintation switch
        {
            RobotOrintation.North => (simData.RobotPosition.X, simData.RobotPosition.Y + 1),
            RobotOrintation.East => (simData.RobotPosition.X + 1, simData.RobotPosition.Y),
            RobotOrintation.South => (simData.RobotPosition.X, simData.RobotPosition.Y - 1),
            RobotOrintation.West => (simData.RobotPosition.X - 1, simData.RobotPosition.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (!_mapService.IsPositionValid(simData.Map, newX, newY))
        {
            return new (false, simData);
        }

        return new (true, simData with { RobotPosition = (newX, newY) });
    }

    public SimulationActionResult Left(SimulationData simData)
    {
        if (!simData.RobotPlaced)
        {
            return new (false, simData);
        }

        var newOrintation = simData.Orintation switch
        {
            RobotOrintation.North => RobotOrintation.West,
            RobotOrintation.East => RobotOrintation.North,
            RobotOrintation.South => RobotOrintation.East,
            RobotOrintation.West => RobotOrintation.South,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new (true, simData with { Orintation = newOrintation });
    }

    public SimulationActionResult Right(SimulationData simData)
    {
        if (!simData.RobotPlaced)
        {
            return new (false, simData);
        }

        var newOrintation = simData.Orintation switch
        {
            RobotOrintation.North => RobotOrintation.East,
            RobotOrintation.East => RobotOrintation.South,
            RobotOrintation.South => RobotOrintation.West,
            RobotOrintation.West => RobotOrintation.North,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new (true, simData with { Orintation = newOrintation });

    }

    public string Report(SimulationData simData)
    {
        if (!simData.RobotPlaced)
        {
            return "Robot not placed";
        }

        return $"{simData.RobotPosition.X},{simData.RobotPosition.Y},{simData.Orintation}".ToUpper();
    }


}
