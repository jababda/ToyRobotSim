namespace ToyRobotSim.Tests.Services;

public class MapServiceTests
{
    private readonly MapService _sut;

    public MapServiceTests()
    {
        _sut = new MapService();
    }

    [Theory]
    [InlineData(6, 6, 0, 0)]
    [InlineData(6, 6, 1, 3)]
    [InlineData(6, 6, 5, 5)]
    public void IsPositionValid_WhenPositionIsWithinMapBounds_ReturnsTrue(int width, int height, int x, int y)
    {
        var res = _sut.IsPositionValid(new SimulationMap(width, height), x, y);
        Assert.True(res);
    }

    [Theory]
    [InlineData(6, 6, -1, 0)]
    [InlineData(6, 6, 0, -1)]
    [InlineData(6, 6, 6, 5)]
    [InlineData(6, 6, 5, 6)]
    [InlineData(6, 6, 6, 6)]
    public void IsPositionValid_WhenPositionIsOutsideMapBounds_ReturnsFalse(int width, int height, int x, int y)
    {
        var res = _sut.IsPositionValid(new SimulationMap(width, height), x, y);
        Assert.False(res);
    }
}
