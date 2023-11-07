using UnityEngine;
using GridSystem.Square;

public class GridManager
{
    public InfinityGrid<Obstacle> ObstacleGrid { get; }
    public InfinityGrid<Tile> TileGrid { get; }
    public GridManager(Vector3 cellSize)
    {
        ObstacleGrid = new InfinityGrid<Obstacle>(cellSize);
        TileGrid = new InfinityGrid<Tile>(cellSize);
    }
    public bool IsObstacle(Vector3 position, Vector3 direction)
    {
        GridVector gridVector = ObstacleGrid.GetGridPosition(position + direction);
        return ObstacleGrid.TryGetValue(gridVector, out Obstacle obstacle);
    }
    public bool HasTile(Vector3 position, Vector3 direction,out Tile tile)
    {
        GridVector gridVector = TileGrid.GetGridPosition(position + direction);
        return TileGrid.TryGetValue(gridVector, out tile);
    }
}
