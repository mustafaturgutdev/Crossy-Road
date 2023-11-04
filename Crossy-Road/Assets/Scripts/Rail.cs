using UnityEngine;

public class Rail : Tile
{
    [SerializeField] private TileType tileType;
    public override TileType TileType => tileType;
}
