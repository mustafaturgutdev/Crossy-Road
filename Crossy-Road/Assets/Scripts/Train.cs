using UnityEngine;

public class Train : Obstacle
{
    [SerializeField] private ObstacleType obstacleType;
    public override ObstacleType ObstacleType => obstacleType;
}
