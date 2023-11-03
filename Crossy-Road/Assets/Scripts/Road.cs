using UnityEngine;

public class Road : Tile
{
    [SerializeField] private TileType tileType;
    public override TileType TileType => tileType;
}
