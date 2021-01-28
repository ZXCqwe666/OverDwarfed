using UnityEngine;

public class Mining : MonoBehaviour
{
    public LayerMask destuctableBlocksLayer;
    private Camera mainCam;
    private readonly float miningDistance = 2f;
    private readonly int miningDamage = 1;

    private float reloadTime = 0.4f;
    private float lastHit = 0;

    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update() 
    { 
        if (Input.GetMouseButton(0) && lastHit + reloadTime < Time.time) 
        {
            MineBlock();
        }
    }
    private void MineBlock()
    {
        Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        if (Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer);
            MiningManager.instance.Mine(hit.point + direction * 0.05f, miningDamage);

            lastHit = Time.time;
        }
    }
}
