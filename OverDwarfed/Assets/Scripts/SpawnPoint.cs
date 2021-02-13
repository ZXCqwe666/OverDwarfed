using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private SpawnPointData spData;
    public Vector3 position;

    private SpriteRenderer rend;
    private Animator anim;

    public void InitializeSpawnPoint(SpawnPointData _spData)
    {
        spData = _spData;
        position = transform.position;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        anim.runtimeAnimatorController = spData.animController;
        rend.sprite = spData.idleSprite;
    }
    public void SpawnBegan()
    {
        //anim.Play("idle / aктивный")
        //anim.Play(" short anim when mob spawns")
    }
    private void SpawnedMob()
    {
        //anim.Play(" short anim when mob spawns")
    }
    public void TurnOff()
    {
        // just sprite no idle anim
    }
}
