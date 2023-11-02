using System.Collections.Generic;
using PoolSystem;

public class TileManager
{
    private readonly Dictionary<TileType, Pool<Tile>> multiPool = new();
    public TileManager(List<Tile> tilePrefabs)
    {
        foreach (Tile tilePrefab in tilePrefabs)
        {
            multiPool.Add(tilePrefab.TileType, new Pool<Tile>(new UnityObjectFactory<Tile>(tilePrefab), 50));
        }
    }


    public Tile GetTile(TileType tileType)
    {
        return multiPool[tileType].Spawn();
    }

    public bool ReturnTile(Tile tile)
    {
        return multiPool[tile.TileType].Despawn(tile);
    }
}
