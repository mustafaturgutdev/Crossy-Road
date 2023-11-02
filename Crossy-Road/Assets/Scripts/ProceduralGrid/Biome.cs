using GridSystem.Square;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public enum TileType
{
    Road,
    RoadStart,
    RoadEnd,
    RoadMiddle,
    Grass
}

public enum ObstacleType
{
    Empty = 0,
    Tree,
    Rock,
}

public class BiomeManager
{
    private TileType lastBiome;

    private int startRow;
    private int endRow;
    private readonly TileManager tileManager;
    private readonly ObstacleManager obstacleManager;
    private readonly GridManager gridManager;

    public BiomeManager(TileManager tileManager, ObstacleManager obstacleManager, GridManager gridManager)
    {
        this.tileManager = tileManager;
        this.obstacleManager = obstacleManager;
        this.gridManager = gridManager;
    }

    private static List<ObstacleType> grassObstacles = new List<ObstacleType>()
    {
        ObstacleType.Tree,
        ObstacleType.Rock,
    };

    public void UpdateBiomes(int playerCurrentRow)
    {
        while (playerCurrentRow - 5 < startRow)
        {
            DestroyBiome();
        }


        while (playerCurrentRow + 5 > endRow)
        {
            CreateBiome();
        }
    }

    private void DestroyBiome()
    {
        for (int i = 0; i < 10; i++)
        {
            GridVector gridPos = new GridVector(startRow, i);

            Cell obstacleCell = gridManager.ObstacleGrid[gridPos];
            if (gridManager.ObstacleGrid.TryGetValue(obstacleCell, out Obstacle obstacle))
            {
                gridManager.ObstacleGrid.DisPlace(obstacle);
                obstacleManager.ReturnObstacle(obstacle);
            }

            Cell tileCell = gridManager.TileGrid[gridPos];
            if (gridManager.TileGrid.TryGetValue(tileCell, out Tile tile))
            {
                gridManager.TileGrid.DisPlace(tile);
                tileManager.ReturnTile(tile);
            }

            startRow +=1;
        }
    }

    private void CreateBiome()
    {

        TileType biomeType;
        do biomeType = Utils.GetRandomEnumValue<TileType>();
        while (biomeType == lastBiome);

        int biomeSize = Random.Range(1, 5);
        endRow += biomeSize;

        switch (biomeType)
        {
            case TileType.Road:
                CreateRoadBiome(new GridVector(biomeSize, 10), new GridVector(endRow, 0));
                break;

            case TileType.Grass:
                CreateGrassBiome(new GridVector(biomeSize, 10), new GridVector(endRow, 0));
                break;

            default:
                break;
        }
    }

    private void CreateGrassBiome(GridVector biomeSize, GridVector startPosition)
    {
        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);

                gridManager.TileGrid.Place(tileManager.GetTile(TileType.Grass), gridPosition);


                if (0.3f < Random.Range(0f, 1f))
                {
                    gridManager.ObstacleGrid.Place(obstacleManager.GetObstacle(grassObstacles[Random.Range(0, grassObstacles.Count)]), gridPosition);
                }
            }
        }
    }

    private void CreateRoadBiome(GridVector biomeSize, GridVector startPosition)
    {
        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);
                if (biomeSize.Row == 1)
                {
                    gridManager.TileGrid.Place(tileManager.GetTile(TileType.Road), gridPosition);
                }
                else
                {
                    if (i == 0)
                    {
                        gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadStart), gridPosition);
                    }
                    else if (i == biomeSize.Row - 1)
                    {
                        gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadEnd), gridPosition);
                    }
                    else
                    {
                        gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadMiddle), gridPosition);
                    }
                }
            }
        }
    }
}

public class RoadBiome : Biome
{
    public RoadBiome(GridVector biomeSize, GridVector startPosition, float frequency) : base(biomeSize, startPosition, frequency)
    {
    }
}

public class GrassBiome : Biome
{


    public GrassBiome(GridVector biomeSize, GridVector startPosition, float frequency) : base(biomeSize, startPosition, frequency)
    {
    }
}

public class Biome : IDisposable, IEnumerable<ObstacleType>
{
    protected readonly float frequency;
    public ObstacleType[,] Obstacles { get; }
    public GridVector BiomeSize { get; }
    public GridVector StartPosition { get; }

    public Biome(GridVector biomeSize, GridVector startPosition, float frequency)
    {
        Obstacles = new ObstacleType[biomeSize.Row, biomeSize.Column];
        BiomeSize = biomeSize;
        StartPosition = startPosition;
        this.frequency = frequency;
    }

    public void Dispose()
    {
    }

    public IEnumerator<ObstacleType> GetEnumerator()
    {
        foreach (ObstacleType obstacleType in Obstacles)
            yield return obstacleType;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}