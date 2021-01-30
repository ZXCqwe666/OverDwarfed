using UnityEngine.EventSystems;
using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour
{
    private const float colliderPenetration = 2.05f;

    private Camera mainCam; 
    public LayerMask destuctableBlocksLayer;
    private int2 miningDamage = new int2(3, 5);
    private float miningDistance = 2f, reloadTime = 0.45f, lastHit = 0f;

    public GameObject dynamite;
    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update() 
    { 
        if (Input.GetMouseButton(0) && lastHit + reloadTime < Time.time) //check if current slot contains pickaxe 
            MineBlock();
        if (Input.GetMouseButton(1) && lastHit + reloadTime < Time.time) // rofl bombing
            BoomMining();
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Apocal());
    }
    private void MineBlock()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer);
        if (raycast)
        {
            MiningManager.instance.Mine(raycast.point + direction * colliderPenetration, UnityEngine.Random.Range(miningDamage.x, miningDamage.y + 1));
            lastHit = Time.time;
        }
    }
    private void BoomMining()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Instantiate(dynamite, transform.position + new Vector3(direction.x, direction.y, 0f) * 3f, Quaternion.identity);
        lastHit = Time.time;
    }
    private IEnumerator Apocal()
    {
        for(int x = 0; x < 300; x+=10)
        {
            for (int y = 0; y < 100; y += 10)
            {
                Instantiate(dynamite, new Vector3(x,y, 0f), Quaternion.identity);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
