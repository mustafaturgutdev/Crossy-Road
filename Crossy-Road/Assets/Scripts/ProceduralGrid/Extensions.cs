using System;

public static class Extensions
{
    private readonly static Random random = new Random();


    public static ObstacleType GetRandomObstacleType() => (ObstacleType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(ObstacleType)).Length);
    public static ObstacleType GetRandomObstacleType(int min, int max) => (ObstacleType)UnityEngine.Random.Range(min, max);
}
