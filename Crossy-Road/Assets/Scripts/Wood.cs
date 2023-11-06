using GridSystem.Square;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IMovable
{
    void Move(float velocity, Vector3 startPosition, Vector3 endPosition, InfinityGrid<Obstacle> grid, Action onComplete);
}
public class Wood : Obstacle, IMovable
{
    public override Cell Cell { get ; set ; }
    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private List<Transform> transforms;

    public override ObstacleType ObstacleType => obstacleType;

    public async void Move(float velocity,Vector3 startPosition, Vector3 endPosition, InfinityGrid<Obstacle> grid, Action onComplete)
    {
        transform.position = startPosition;
        Vector3 dir = (endPosition- startPosition).normalized;
        while (Vector3.Distance(transform.position, endPosition) > 0.1f)
        {
            transform.position += velocity * dir*Time.deltaTime;// Vector3.MoveTowards(transform.position, endPosition, velocity * Time.deltaTime);
            foreach (Transform transform in transforms)
            {
                grid.DisPlace(this);
                grid.Place(this, grid.GetGridPosition(transform.position));
            }

            await Task.Yield(/*TimeSpan.FromSeconds(Time.deltaTime)*/);
        }
        transform.position = endPosition;
        onComplete?.Invoke();
    }
}