using UnityEngine;
using GridSystem.Square;
using PoolSystem;

public abstract class Obstacle : MonoBehaviour, IPoolable, IPlacable
{
    private Cell cell;
    public abstract ObstacleType ObstacleType { get; }
    public virtual Cell Cell
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
