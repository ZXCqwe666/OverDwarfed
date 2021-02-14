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
        List<int> spawnAmount = new List<int>();
        for (int i = 0; i < spData.enemiesToSpawn.Count; i++)
            spawnAmount.Add(Mathf.FloorToInt(spData.minSpawningAmount[i] * difficulty));

        List<Enemy> enemiesToSpawn = new List<Enemy>();
        for (int i = 0; i < spawnAmount.Count; i++)
        for (int k = 0; k <= spawnAmount[i]; k++)
        enemiesToSpawn.Add(spData.enemiesToSpawn[i]);
        enemiesToSpawn.Shuffle();

        yield return new WaitForSeconds(Random.Range(0, spawningDelay));

        foreach (Enemy enemy in enemiesToSpawn)
        {
            Instantiate(WaveSpawner.instance.enemyPrefabs[enemy], position, Quaternion.identity ).GetComponent<EnemyAI>().SetState(States.chase, 3f);
            float delay = Random.Range(spData.spawnDelayInterval.x, spData.spawnDelayInterval.y);

            yield return new WaitForSeconds(delay);
        }
    }
}
