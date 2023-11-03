using System.Collections.Generic;
using UnityEngine;
using GridSystem.Square;
using PoolSystem;


public class Rock : Obstacle
{
    public override ObstacleType ObstacleType => ObstacleType.Rock;
}

public abstract class Obstacle : MonoBehaviour, IPoolable, IPlacable
{
    public abstract ObstacleType ObstacleType { get; }
    public Cell Cell { get; set; }

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
    }
}

public class ObstacleManager
{

    private readonly Dictionary<ObstacleType, Pool<Obstacle>> multiPool = new();
    public ObstacleManager(List<Obstacle> obstaclePrefabs)
    {
        foreach (Obstacle obstaclePrefab in obstaclePrefabs)
            multiPool.Add(obstaclePrefab.ObstacleType, new Pool<Obstacle>(new UnityObjectFactory<Obstacle>(obstaclePrefab), 50));
    }


    public Obstacle GetObstacle(ObstacleType obstacleType)
    {
        return multiPool[obstacleType].Spawn();
    }

    public bool ReturnObstacle(Obstacle obstacle)
    {
        return multiPool[obstacle.ObstacleType].Despawn(obstacle);
    }

}
public class GridManager
{
    public InfinityGrid<Obstacle> ObstacleGrid { get; }
    public InfinityGrid<Tile> TileGrid { get; }
    public GridManager(Vector3 cellSize)
    {
        ObstacleGrid = new InfinityGrid<Obstacle>(cellSize);
        TileGrid = new InfinityGrid<Tile>(cellSize);
    }
}
public class App : MonoBehaviour
{
    [SerializeField] private List<Tile> tilePrefabs;
    [SerializeField] private List<Obstacle> obstaclePrefabs;


    // Start is called before the first frame update
    void Start()
    {

        TileManager tileManager = new TileManager(tilePrefabs);
        ObstacleManager obstacleManager = new ObstacleManager(obstaclePrefabs);
        GridManager gridManager = new GridManager(Vector3.one);
        BiomeManager biomeManager = new BiomeManager(tileManager, obstacleManager, gridManager);

        //biomeManager.Initialize();
    }
}