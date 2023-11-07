using System.Collections.Generic;
using UnityEngine;
public class App : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Tile> tilePrefabs;
    [SerializeField] private List<Obstacle> obstaclePrefabs;
    private BiomeManager biomeManager;


    // Start is called before the first frame update
    void Start()
    {
        TileManager tileManager = new TileManager(tilePrefabs);
        ObstacleManager obstacleManager = new ObstacleManager(obstaclePrefabs);
        GridManager gridManager = new GridManager(Vector3.one);
        biomeManager = new BiomeManager(tileManager, obstacleManager, gridManager);

        biomeManager.Initialize();
        player.Initialize(gridManager, biomeManager);
    }
    private int playerCurrentRow = 1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCurrentRow++;
            biomeManager.UpdateBiomes(playerCurrentRow);
        }
    }
}