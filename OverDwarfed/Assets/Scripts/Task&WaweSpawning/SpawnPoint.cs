using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpawnPointData spData;
    private Vector3 position;

    private SpriteRenderer rend;
    //private Animator anim;

    public void InitializeSpawnPoint(SpawnPointData _spData)
    {
        spData = _spData;
        position = transform.position;
        //anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        //anim.runtimeAnimatorController = spData.animController;
        rend.sprite = spData.idleSprite;
    }
    public void BeginSpawn(float difficulty)
    {
        StartCoroutine(Spawning(difficulty, spData.maxSpawnDelay));
        //anim.Play("idle / aктивный")
        //anim.Play(" short anim when mob spawns")
    }
    private IEnumerator Spawning(float difficulty, float spawningDelay)
    {
        yield return new WaitForSeconds(Random.Range(0, spawningDelay));

        List<int> spawnAmounts = CalculateSpawnAmount(difficulty);
        List<Enemy> enemiesToSpawn = GenerateEnemySpawnQueue(ref spawnAmounts);

        foreach (Enemy enemy in enemiesToSpawn)
        {
            if (WaveSpawner.instance.enemyCount >= WaveSpawner.maxEnemyCount) 
            yield break;

            SpawnEnemy(enemy);

            float delay = Random.Range(spData.spawnDelayInterval.x, spData.spawnDelayInterval.y);
            yield return new WaitForSeconds(delay);
        }
    }
    private void SpawnEnemy(Enemy enemyType)
    {
        GameObject enemySpawned = Instantiate(WaveSpawner.instance.enemyPrefabs[enemyType], position, Quaternion.identity, transform);
        EnemyAI enemyAI = enemySpawned.GetComponent<EnemyAI>();
        enemyAI.Initialize();
        enemyAI.SetState(States.GlobalChase);
        WaveSpawner.instance.enemyCount += 1;
    }
    private List<int> CalculateSpawnAmount(float difficulty)
    {
        List<int> spawnAmounts = new List<int>();
        for (int i = 0; i < spData.enemiesToSpawn.Count; i++)
            spawnAmounts.Add(Mathf.FloorToInt(spData.minSpawningAmount[i] * difficulty));
        return spawnAmounts;
    }
    private List<Enemy> GenerateEnemySpawnQueue(ref List<int> spawnAmounts)
    {
        List<Enemy> enemiesToSpawn = new List<Enemy>();
        for (int i = 0; i < spawnAmounts.Count; i++)
            for (int k = 0; k <= spawnAmounts[i]; k++)
                enemiesToSpawn.Add(spData.enemiesToSpawn[i]);
        enemiesToSpawn.Shuffle();
        return enemiesToSpawn;
    }
}
