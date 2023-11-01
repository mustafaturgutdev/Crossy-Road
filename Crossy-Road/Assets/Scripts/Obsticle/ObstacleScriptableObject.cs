using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ObsticleScriptableObject", menuName = "ScriptableObjects/ObsticleScriptableObject", order = 1)]
public class ObstacleScriptableObject : ScriptableObject
{
    public enum ObsticleType
    {
        tree,
        stone
    }
    public ObsticleType obsticleType;
}

//public class BiomeManager
//{
//    public BiomeManager()
//    {

//    }
//}
//public class GameManager
//{
//    public GameManager(BiomeManager biomManager, Grid gridManager)
//    {

//    }
//}
//public class App : MonoBehaviour
//{
//    private void Awake()
//    {
//        for(int i = 0; i < 20; i++)
//        {
//            //biomeManager.AddChild(new Biome());
//        }
//        for (int i = 0; i < 20; i++)
//        {
//            //Biome.AddChild(new Tile(random.range()))
//        }
//    }
//}
