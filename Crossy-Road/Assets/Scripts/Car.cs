using UnityEngine;

public class Car : Obstacle
{
    [SerializeField] private ObstacleType obstacleType;
    public override ObstacleType ObstacleType => obstacleType;
}
