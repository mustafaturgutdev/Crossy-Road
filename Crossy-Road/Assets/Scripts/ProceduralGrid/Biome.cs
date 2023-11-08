using DG.Tweening;
using GridSystem.Square;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
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
    Grass2,
    RailLight
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
    Wood1,
    Wood2,
    Wood3,
    Train,
    Truck,
}

public class BiomeManager
{
    private int spawnLenght = 10;
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
    private static List<ObstacleType> roadObstacles = new List<ObstacleType>()
    {
        ObstacleType.Car,
        ObstacleType.Truck,
    };

    private static List<ObstacleType> grassObstacles = new List<ObstacleType>()
    {
        ObstacleType.Tree1,
        ObstacleType.Tree2,
        ObstacleType.Tree3,
        ObstacleType.Rock1,
        ObstacleType.Rock2,
        ObstacleType.Rock3,
    };

    private static List<ObstacleType> waterObstacles = new List<ObstacleType>()
    {
        ObstacleType.Wood1,
        ObstacleType.Wood2,
        ObstacleType.Wood3,
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

        for (int i = 0; i < 6; i++)
        {
            CreateBiome();
        }

    }
    public async void UpdateBiomes(int playerCurrentRow)
    {
        this.playerCurrentRow = playerCurrentRow;
        while (playerCurrentRow - 10 > startRow)
        {
            DestroyBiome();
            await Task.Yield();
        }

        while (playerCurrentRow + 10 > endRow)
        {
            CreateBiome();
            await Task.Yield();
        }
    }

    private void DestroyBiome()
    {
        for (int i = -5; i <= 5; i++)
        {
            GridVector gridPosition = new GridVector(startRow, i);

            if (gridManager.ObstacleGrid.DisPlace(gridPosition, out Obstacle obstacle))
                obstacleManager.ReturnObstacle(obstacle);

            if (gridManager.TileGrid.DisPlace(gridPosition, out Tile tile))
                tileManager.ReturnTile(tile);
        }
        startRow += 1;
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

                if (j != 5 && 0.83f < Random.Range(0f, 1f))
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
            Transform light = default;
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);
                if (j + startPosition.Column == 0)
                {
                    Tile tile = tileManager.GetTile(TileType.RailLight);
                    light = tile.transform.GetChild(0);
                    gridManager.TileGrid.Place(tile, gridPosition);
                }
                else gridManager.TileGrid.Place(tileManager.GetTile(TileType.Rail), gridPosition);
            }

            int value = 0;
            if (i % 2 == 0)
                value = -20;
            else
                value = 20;
            _ = SpawnTrainUntil(light, gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, value)), gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, -value)), value == 20);
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

            int value = 0;
            if (i % 2 == 0)
                value = -spawnLenght;
            else
                value = spawnLenght;
            _ = SpawnWoodUntil(gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, value)), gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, -value)));
        }
    }
    private async Task SpawnTrainUntil(Transform light, Vector3 spawnPosition, Vector3 endPosition, bool positive)
    {
        float duration = 1f;
        float frequency = Random.Range(8f, 12f);
        float elapsedTime = Random.Range(frequency / 2, frequency);
        int playerRow = playerCurrentRow;

        bool lightOn = true;

        while (playerRow + 30 > endRow)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > frequency)
            {
                Obstacle obstacle = obstacleManager.GetObstacle(ObstacleType.Train);
                if (positive)
                    obstacle.transform.eulerAngles = new Vector3(0, 90, 0);
                else
                    obstacle.transform.eulerAngles = new Vector3(0, -90, 0);
                obstacle.transform.position = spawnPosition;
                obstacle.transform.DOMove(endPosition, duration).SetEase(Ease.Linear).OnComplete(() => obstacleManager.ReturnObstacle(obstacle));
                elapsedTime = 0f;
                light.localScale = new Vector3(0.1f, 1f, 0.1f);
                lightOn = true;
            }
            else if (lightOn && frequency - elapsedTime < 1.5f)
            {
                light.localScale = new Vector3(0.2f, 2, 0.2f);
                lightOn = false;
            }
            await Task.Yield();
        }
    }
    private async Task SpawnWoodUntil(Vector3 spawnPosition, Vector3 endPosition)
    {
        float velocity = Random.Range(2f, 6f);
        float frequency = Random.Range(2f, 4f);
        float elapsedTime = Random.Range(frequency / 2, frequency);
        int playerRow = playerCurrentRow;

        while (playerRow + 30 > endRow)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > frequency)
            {
                Wood obstacle = (Wood)obstacleManager.GetObstacle(waterObstacles[Random.Range(0, waterObstacles.Count)]);
                obstacle.Move(velocity, spawnPosition, endPosition, gridManager.ObstacleGrid, () => obstacleManager.ReturnObstacle(obstacle));
                elapsedTime = 0f;
            }
            await Task.Yield();
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
            int value = 0;
            if (Random.Range(0f, 1f) > 0.5f)
                value = -spawnLenght;
            else
                value = spawnLenght;
            _ = SpawnCarUntil(gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, value)), gridManager.ObstacleGrid.GetWorldPosition(new GridVector(startPosition.Row + i, -value)), value == spawnLenght);

        }
    }

    private async Task SpawnCarUntil(Vector3 spawnPosition, Vector3 endPosition, bool positive)
    {
        float duration = Random.Range(4f, 8f);
        float frequency = Random.Range(3f, 6f);
        float elapsedTime = Random.Range(frequency / 2, frequency);
        int playerRow = playerCurrentRow;



        while (playerRow + 30 > endRow)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > frequency)
            {
                Obstacle obstacle = obstacleManager.GetObstacle(roadObstacles[Random.Range(0,roadObstacles.Count)]);

                if (positive)
                    obstacle.transform.eulerAngles = new Vector3(0, -90, 0);
                else
                    obstacle.transform.eulerAngles = new Vector3(0, 90, 0);
                obstacle.transform.position = spawnPosition;
                obstacle.transform.DOMove(endPosition, duration).SetEase(Ease.Linear).OnComplete(() => obstacleManager.ReturnObstacle(obstacle));
                elapsedTime = 0f;
            }
            await Task.Yield();
        }
    }
}