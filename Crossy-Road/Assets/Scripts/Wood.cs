using UnityEngine;

public class Wood : Obstacle
{
    [SerializeField] private ObstacleType obstacleType;
    public override ObstacleType ObstacleType => obstacleType;
}