using System.Collections.Generic;
using UnityEngine;
using GridSystem.Square;
using PoolSystem;

public class ObstacleManager
{
    private static Dictionary<ObstacleType,int> obstacleInitialSize = new Dictionary<ObstacleType,int>()
    {
       { ObstacleType.Tree1,10},
       { ObstacleType.Tree2,10},
       { ObstacleType.Tree3,10},
       { ObstacleType.Rock1,10},
       { ObstacleType.Rock2,10},
       { ObstacleType.Rock3,10},

              { ObstacleType.Car,20},
                     { ObstacleType.Wood1,10},
                     { ObstacleType.Wood2,10},
                     { ObstacleType.Wood3,10},
    };

    private readonly Dictionary<ObstacleType, Pool<Obstacle>> multiPool = new();
    public ObstacleManager(List<Obstacle> obstaclePrefabs)
    {
        foreach (Obstacle obstaclePrefab in obstaclePrefabs)
            multiPool.Add(obstaclePrefab.ObstacleType, new Pool<Obstacle>(new UnityObjectFactory<Obstacle>(obstaclePrefab), obstacleInitialSize[obstaclePrefab.ObstacleType]));
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
    public Cell GetCell(Vector3 pos)
    {
        return TileGrid[TileGrid.GetGridPosition(pos)];
    }
}
public class App : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Tile> tilePrefabs;
    [SerializeField] private List<Obstacle> obstaclePrefabs;
    private BiomeManager biomeManager;


    // Start is called before the first frame update
    void Start()
    {

        TileManager tileManager = new TileManager(tilePrefabs);
        ObstacleManager obstacleManager = new ObstacleManager(obstaclePrefabs);
        GridManager gridManager = new GridManager(Vector3.one);
         biomeManager = new BiomeManager(tileManager, obstacleManager, gridManager);


        biomeManager.Initialize();
        player.Initialize(gridManager, biomeManager);
    }
    private int playerCurrentRow=1;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerCurrentRow++;
            biomeManager.UpdateBiomes(playerCurrentRow);
        }
    }
}