using UnityEngine;

public class Grass : Tile
{
    [SerializeField] private TileType tileType;
    public override TileType TileType => tileType;
}
