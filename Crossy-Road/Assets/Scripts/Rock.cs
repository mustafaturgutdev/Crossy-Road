using UnityEngine;

public class Rock : Obstacle
{
    [SerializeField] private ObstacleType obstacleType;
    public override ObstacleType ObstacleType => obstacleType;
}
