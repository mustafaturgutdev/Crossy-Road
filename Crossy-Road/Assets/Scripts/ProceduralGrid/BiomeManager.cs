using DG.Tweening;
using GridSystem.Square;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BiomeManager
{
    private GridVector biomeLenght = new GridVector(-11, 11);
    private int spawnLenght = 10;
    private BiomeType lastBiome = BiomeType.Grass;
    private int playerCurrentRow;
    private int startRow = -14;
    private int endRow = 5;
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
        CreateGrassBiome(new GridVector(19, 23), new GridVector(-14, biomeLenght.Row));

        for (int i = 0; i < 6; i++)
        {
            CreateBiome();
        }

    }
    public async void UpdateBiomes(int playerCurrentRow)
    {
        this.playerCurrentRow = playerCurrentRow;
        while (playerCurrentRow - 20 > startRow)
        {
            DestroyBiome();
            await Task.Yield();
        }

        while (playerCurrentRow + 20 > endRow)
        {
            CreateBiome();
            await Task.Yield();
        }
    }

    public async Task Restart()
    {
        while (startRow <= endRow)
        {
            DestroyBiome();
            await Task.Yield();
        }
        endRow = 5;
        startRow = -14;
        playerCurrentRow = 0;
        await Task.Delay(300);
        Initialize();
    }

    private void DestroyBiome()
    {
        for (int i = biomeLenght.Row; i <= biomeLenght.Column; i++)
        {
            GridVector gridPosition = new GridVector(startRow, i);

            if (gridManager.ObstacleGrid.DisPlace(gridPosition, out Obstacle obstacle))
            {
                obstacleManager.ReturnObstacle(obstacle);
            }


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

        GridVector biomeSize = new GridVector(Random.Range(1, 5), 23);
        GridVector startPosition = new GridVector(endRow, biomeLenght.Row);
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

                if (j != 11 &&
                    0.83f < Random.Range(0f, 1f) &&
                    (!gridManager.ObstacleGrid.TryGetValue(new GridVector(gridPosition.Row - 1, gridPosition.Column), out Obstacle obstacle) ||
                    obstacle.ObstacleType != ObstacleType.Leaf))
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

        for (int i = 0; i < 2; i++)
        {
            GridVector rand = new GridVector(startPosition.Row, Random.Range(-5, 5));
            if (gridManager.ObstacleGrid.TryGetValue(rand, out _) || gridManager.ObstacleGrid.TryGetValue(new GridVector(rand.Row - 1, rand.Column), out _)) i--;
            else gridManager.ObstacleGrid.Place(obstacleManager.GetObstacle(ObstacleType.Leaf), rand);
        }

        for (int i = 0; i < biomeSize.Row; i++)
        {
            for (int j = 0; j < biomeSize.Column; j++)
            {
                GridVector gridPosition = startPosition + new GridVector(i, j);
                gridManager.TileGrid.Place(tileManager.GetTile(TileType.Water), gridPosition);

                if (gridManager.ObstacleGrid.TryGetValue(new GridVector(gridPosition.Row - 1, gridPosition.Column), out Obstacle obstacle) &&
                    obstacle.ObstacleType == ObstacleType.Leaf)
                {
                    int rand = Random.Range(-2, 2);
                    for (int k = 0; k <= Mathf.Abs(rand); k++)
                    {
                        GridVector gridPos = gridPosition + new GridVector(0, rand < 0 ? -k : k);
                        if (!gridManager.ObstacleGrid.TryGetValue(gridPos, out _))
                            gridManager.ObstacleGrid.Place(obstacleManager.GetObstacle(ObstacleType.Leaf), gridPos);
                    }
                }
            }
        }
    }
    private async Task SpawnTrainUntil(Transform light, Vector3 spawnPosition, Vector3 endPosition, bool positive)
    {
        float duration = 1f;
        float frequency = Random.Range(8f, 12f);
        float elapsedTime = Random.Range(frequency / 2, frequency);
        int playerRow = playerCurrentRow;

        bool lightOn = true;

        while (playerRow + 50 > endRow && endRow != 5)
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
            else if (lightOn && frequency - elapsedTime < 1.2f)
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



        while (playerRow + 50 > endRow && endRow != 5)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > frequency)
            {
                Obstacle obstacle = obstacleManager.GetObstacle(roadObstacles[Random.Range(0, roadObstacles.Count)]);

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