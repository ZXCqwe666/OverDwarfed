using UnityEngine;

public class Mining : MonoBehaviour
{
    public LayerMask destuctableBlocksLayer;
    private Camera mainCam;
    private readonly float miningDistance = 2f;
    private readonly int miningDamage = 1;

    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update() 
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer);
            Debug.Log(hit.point);
            MiningManager.instance.Mine(hit.point, miningDamage);
        }
    }
}
