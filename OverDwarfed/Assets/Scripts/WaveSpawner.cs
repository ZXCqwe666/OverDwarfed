using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;
    private Dictionary<Enemy, GameObject> enemyPrefabs;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeWaweSpawner()
    }
    private void InitializeWaweSpawner()
    {
        enemyPrefabs = new Dictionary<Enemy, GameObject>
        {
            {Enemy.goldMaggot, Resources.Load<GameObject>("Enemies/goldMaggot") },
        };
    }
}
public enum Enemy
{
    stoneMaggot,
    ironMaggot,
    goldMaggot
}
