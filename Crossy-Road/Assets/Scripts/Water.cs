using UnityEngine;

public class Water : Tile
{
    [SerializeField] private TileType tileType;
    public override TileType TileType => tileType;
}
