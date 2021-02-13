using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    private const float taskBurnMultiplier = 0.25f, timePassedMultiplier = 3f;

    public Dictionary<Enemy, GameObject> enemyPrefabs;
    public List<SpawnPoint> spawnPoints;

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
    public void StartSpawn(float taskBurned, float timePassedPercent) 
    {
        float difficulty = taskBurned * taskBurnMultiplier + timePassedPercent * timePassedMultiplier;// Сложность должна открывать новых мобов + количество спавнов
        foreach (SpawnPoint point in spawnPoints)
            point.BeginSpawn(difficulty);
    }
}
public enum Enemy
{
    stoneMaggot,
    ironMaggot,
    goldMaggot
}
