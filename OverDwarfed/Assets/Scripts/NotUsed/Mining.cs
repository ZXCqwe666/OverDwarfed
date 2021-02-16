using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;

public class Mining : MonoBehaviour
{
    private const float colliderPenetration = 0.05f, miningDistance = 2f;

    public LayerMask destuctableBlocksLayer;
    private Camera mainCam; 
    private float lastHit = 0f;

    public GameObject dynamite;
    private void Start()
    {
        mainCam = Camera.main;
    }
    void Update() 
    {
        if (Input.GetMouseButton(0)) //check if current slot contains pickaxe 
            MineBlock();
        if (Input.GetMouseButton(1) && lastHit + 0.45f < Time.time) // rofl bombing
            BoomMining();
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Apocal());
    }
    private void MineBlock()
    {
        ItemData selectedItem = ItemSpawner.instance.items[InventoryUI.instance.slots[PlayerHotbar.instance.currentSlot].item];
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (selectedItem.isPickaxe)
        {
            Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer);
            if (raycast && lastHit + selectedItem.attackInterval < Time.time)
            {
                MiningManager.instance.Mine(raycast.point + direction * colliderPenetration, Random.Range(selectedItem.damageRange.x, selectedItem.damageRange.y + 1));
                lastHit = Time.time;
            }
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
