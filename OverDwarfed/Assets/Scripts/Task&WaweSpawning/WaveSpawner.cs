using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    private const float taskBurnMultiplier = 0.25f, timePassedMultiplier = 3f;
    public const int maxEnemyCount = 300;

    public Dictionary<Enemy, GameObject> enemyPrefabs;
    public List<SpawnPoint> spawnPoints;

    public int enemyCount = 0;

    public void StartSpawn(float taskBurned, float timePassedPercent) 
    {
        if (enemyCount >= maxEnemyCount) return;

        float difficulty = taskBurned * taskBurnMultiplier + timePassedPercent * timePassedMultiplier;
        foreach (SpawnPoint point in spawnPoints)
            point.BeginSpawn(difficulty);
    }
    #region Initialization
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeWaweSpawner();
    }
    private void InitializeWaweSpawner()
    {
        spawnPoints = new List<SpawnPoint>();
        enemyPrefabs = new Dictionary<Enemy, GameObject>
        {
            {Enemy.goldMaggot, Resources.Load<GameObject>("Enemies/goldMaggot") },
        };
    }
    #endregion
}
public enum Enemy
{
    stoneMaggot,
    ironMaggot,
    goldMaggot
}
