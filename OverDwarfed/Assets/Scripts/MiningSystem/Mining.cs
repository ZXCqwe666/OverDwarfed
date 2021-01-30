using UnityEngine.EventSystems;
using Unity.Mathematics;
using UnityEngine;

public class Mining : MonoBehaviour
{
    private const float colliderPenetration = 2.05f;

    private Camera mainCam; 
    public LayerMask destuctableBlocksLayer;
    private int2 miningDamage = new int2(3, 5);
    private float miningDistance = 2f, reloadTime = 0.45f, lastHit = 0f;

    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update() 
    { 
        if (Input.GetMouseButton(0) && lastHit + reloadTime < Time.time) //check if current slot contains pickaxe 
            MineBlock();
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
}
