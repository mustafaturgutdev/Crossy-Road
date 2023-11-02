using UnityEngine;
using GridSystem.Square;
using PoolSystem;


public abstract class Tile : MonoBehaviour, IPoolable, IPlacable
{
    public abstract TileType TileType { get; }
    public Cell Cell 
    { 
        get => cell;
        set
        {
            cell = value;
            if(cell != null)
                transform.position=cell.GameObject.transform.position;
        }
    }

    private Cell cell;

    public void OnDespawned()
    {
        gameObject.SetActive(false);
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
    }
}
