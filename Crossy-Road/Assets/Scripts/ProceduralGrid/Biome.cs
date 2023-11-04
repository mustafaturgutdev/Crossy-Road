using DG.Tweening;
using GridSystem.Square;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BiomeType
{
    Road,
    Grass,
    Rail,
    Water
}
public enum TileType
{
    Rail,
    Water,
    Road,
    RoadStart,
    RoadEnd,
    RoadMiddle,
    Grass1,
    Grass2
}

public enum ObstacleType
{
    Empty = 0,
    Tree1,
    Tree2,
    Tree3,
    Rock1,
    Rock2,
    Rock3,
    Car,
}

public class BiomeManager
{
    private BiomeType lastBiome = BiomeType.Grass;
    private int playerCurrentRow;
    private int startRow;
    private int endRow = 4;
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
        ObstacleType.Tree1,
        ObstacleType.Tree2,
        ObstacleType.Tree3,
        ObstacleType.Rock1,
        ObstacleType.Rock2,
        ObstacleType.Rock3,
    };

    private static List<TileType> grassTiles = new List<TileType>
    {
        TileType.Grass1,
        TileType.Grass2,
    };

    private static List<BiomeType> biomes = new List<BiomeType>
    {
        BiomeType.Grass,
        BiomeType.Grass,
        BiomeType.Grass,
        BiomeType.Road,
        BiomeType.Road,
        BiomeType.Rail,
        BiomeType.Water,
    };
    public void Initialize()
    {
        CreateGrassBiome(new GridVector(4, 11), new GridVector(0, -5));

        for (int i = 0; i < 20; i++)
        {
            CreateBiome();
        }

    }
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

            startRow += 1;
        }
    }

    private void CreateBiome()
    {



        BiomeType biomeType;
        do biomeType = biomes[Random.Range(0, biomes.Count)];
        while (biomeType == lastBiome /*|| (biomeType == BiomeType.Rail && lastBiome == BiomeType.Road)*/);

        lastBiome = biomeType;

        GridVector biomeSize = new GridVector(Random.Range(1, 5), 11);
        GridVector startPosition = new GridVector(endRow, -5);
        switch (biomeType)
        {
            case BiomeType.Road:
                CreateRoadBiome(biomeSize, startPosition);
                break;

            case BiomeType.Grass:
                CreateGrassBiome(biomeSize, startPosition);
                break;

            case BiomeType.Water:
                CreateWaterBiome(biomeSize, startPosition);
                break;

            case BiomeType.Rail:
                CreateRailBiome(biomeSize, startPosition);
                break;

            default:
                break;
        }
        endRow += biomeSize.Row;
    }

    private void CreateGrassBiome(GridVector biomeSize, GridVector startPosition)
    {
        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);

                if (i % 2 == 0)
                    gridManager.TileGrid.Place(tileManager.GetTile(TileType.Grass1), gridPosition);
                else
                    gridManager.TileGrid.Place(tileManager.GetTile(TileType.Grass2), gridPosition);
                if (j != 5 && 0.9f < Random.Range(0f, 1f))
                {
                    gridManager.ObstacleGrid.Place(obstacleManager.GetObstacle(grassObstacles[Random.Range(0, grassObstacles.Count)]), gridPosition);
                }
            }
        }
    }

    private void CreateRailBiome(GridVector biomeSize, GridVector startPosition)
    {
        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);
                gridManager.TileGrid.Place(tileManager.GetTile(TileType.Rail), gridPosition);
            }
        }
    }
    private void CreateWaterBiome(GridVector biomeSize, GridVector startPosition)
    {
        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);
                gridManager.TileGrid.Place(tileManager.GetTile(TileType.Water), gridPosition);
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
                    if (i == 0) gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadStart), gridPosition);
                    else if (i == biomeSize.Row - 1) gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadEnd), gridPosition);
                    else gridManager.TileGrid.Place(tileManager.GetTile(TileType.RoadMiddle), gridPosition);
                }
            }

            _ = SpawnUntil(gridManager.ObstacleGrid.GetWorldPosition(new GridVector(i, -6)), gridManager.ObstacleGrid.GetWorldPosition(new GridVector(i, 6)));

        }
    }

    private async Task SpawnUntil(Vector3 spawnPosition, Vector3 endPosition)
    {
        float duration = Random.Range(4, 12);
        float frequency = Random.Range(1, 3);
        int playerRow = playerCurrentRow;



        //while (playerRow+5+ < playerCurrentRow)
        //{

        //}


        Obstacle obstacle = obstacleManager.GetObstacle(ObstacleType.Car);
        obstacle.transform.position = spawnPosition;
        obstacle.transform.DOMove(endPosition, duration).OnComplete(() => obstacleManager.ReturnObstacle(obstacle));
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