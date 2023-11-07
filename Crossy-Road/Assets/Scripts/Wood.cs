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
    public override Cell Cell { get; set; }
    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private List<Transform> transforms;
    public Dictionary<Transform, Cell> Slots { get; private set; } = new();
    private void Awake()
    {
        foreach (Transform transform in transforms)
        {
            Slots.Add(transform, null);
        }
    }


    public override ObstacleType ObstacleType => obstacleType;

    public async void Move(float velocity, Vector3 startPosition, Vector3 endPosition, InfinityGrid<Obstacle> grid, Action onComplete)
    {
        transform.position = startPosition;
        Vector3 dir = (endPosition - startPosition).normalized;
        while (Vector3.Distance(transform.position, endPosition) > 0.1f)
        {

            transform.position += velocity * dir * Time.deltaTime;// Vector3.MoveTowards(transform.position, endPosition, velocity * Time.deltaTime);
            foreach (Transform tf in transforms)
            {
                GridVector gridPosition = grid.GetGridPosition(tf.position);
                if (Slots[tf] != null)
                    grid.DisPlace(Slots[tf].GridPosition, out _);
                grid.Place(this, gridPosition);
                Cell cell = grid[gridPosition];
                Slots[tf] = cell;
            }

            await Task.Yield( /*TimeSpan.FromSeconds(Time.deltaTime)*/ );
        }
        transform.position = endPosition;
        onComplete?.Invoke();
    }
}