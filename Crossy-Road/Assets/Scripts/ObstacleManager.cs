using System.Collections.Generic;
using PoolSystem;

public class ObstacleManager
{
    private static Dictionary<ObstacleType, int> obstacleInitialSize = new Dictionary<ObstacleType, int>()
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
       { ObstacleType.Train,10},
       { ObstacleType.Truck,10},
       { ObstacleType.Leaf,20},
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
