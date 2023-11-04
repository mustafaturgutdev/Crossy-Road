using UnityEngine;
using GridSystem.Square;
using PoolSystem;
using System.Threading.Tasks;
using System;
using DG.Tweening;

public class Car : Obstacle
{
    [SerializeField] private ObstacleType obstacleType;
    public override ObstacleType ObstacleType => obstacleType;


}

public abstract class Obstacle : MonoBehaviour, IPoolable, IPlacable
{
    private Cell cell;
    public abstract ObstacleType ObstacleType { get; }
    public Cell Cell
    {
        get => cell;
        set
        {
            cell = value;
            if (cell != null)
                transform.position = cell.GameObject.transform.position;
        }
    }

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
    }
}
