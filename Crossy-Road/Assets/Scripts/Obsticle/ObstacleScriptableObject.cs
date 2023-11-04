using System.Collections;
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
