using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnPointData : ScriptableObject
{
    public List<Enemy> enemiesToSpawn;
    public List<float> minSpawningAmount;

    public float maxSpawnDelay;
    public Vector2 spawnDelayInterval;

    public Sprite idleSprite;
    public RuntimeAnimatorController animController;
}
