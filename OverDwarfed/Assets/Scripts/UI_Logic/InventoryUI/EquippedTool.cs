using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class EquippedTool : MonoBehaviour
{
    public static EquippedTool instance;

    private const float miningDistance = 1.5f, colliderPenetration = 0.02f, weaponSwapDelay = 0.25f;

    private SpriteRenderer toolSprite, playerSprite;
    private Transform tool, toolHolder;
    private Camera mainCam;
    private ItemData equippedItem;
    private bool itemValid = false;

    public LayerMask destuctableBlocksLayer, enemyLayer;
    private float lastHit;

    public Action OnSlotChanged;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeEquippedTool();
    }
    private void Update()
    {
        UpdatePlayerAndGunRotation();
        ClickDetector();
    }
    private void ClickDetector()
    {
        if (Input.GetMouseButton(0) && itemValid && equippedItem.isWeapon)
            Attack();
    }
    private void Attack()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (lastHit + equippedItem.attackInterval > Time.time) return;
        lastHit = Time.time;

        Vector2 direction = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector3 attackCenter = tool.position + (Vector3)direction * equippedItem.attackDistance;
        int damageRoll = Random.Range(equippedItem.damageRange.x, equippedItem.damageRange.y + 1);

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackCenter, equippedItem.attackRadius, enemyLayer);
        foreach (Collider2D collider in enemiesHit)
        {
            if (collider.TryGetComponent(out Health health))
                health.TakeDamage(damageRoll);
        }
        if (equippedItem.isPickaxe)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction, miningDistance, destuctableBlocksLayer);
            Vector3 miningPoint = raycast.point + direction * colliderPenetration;
            if (raycast) MiningManager.instance.Mine(miningPoint, damageRoll);
        }
        if (equippedItem.isHammer)
        {
            // hammer logic (building repair and anvil craft speed up)
        }
    }
    private void UpdatePlayerAndGunRotation()
    {
        Vector3 mouseViewportPos = mainCam.ScreenToViewportPoint(Input.mousePosition) * 2f - Vector3.one;
        playerSprite.flipX = (mouseViewportPos.x > 0f) ? false : true;
        playerSprite.flipY = (mouseViewportPos.x > 0f) ? false : true;

        Vector2 aimDirection = (mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        toolHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
    private void GetEquippedItem()
    {
        Debug.Log("event fired");
        if (InventoryUI.instance.slots[PlayerHotbar.instance.currentSlot].isEmpty == false)
        {
            ItemData previosItem = equippedItem;
            equippedItem = ItemSpawner.instance.items[InventoryUI.instance.slots[PlayerHotbar.instance.currentSlot].item];
            itemValid = true;

            if (equippedItem.isWeapon && previosItem != equippedItem)
            {
                toolSprite.sprite = equippedItem.itemIcon;
                lastHit = Time.time - equippedItem.attackInterval + weaponSwapDelay;
            }
            else toolSprite.sprite = null;
        }
        else
        {
            toolSprite.sprite = null;
            itemValid = false;
        }
    }
    private void InitializeEquippedTool()
    {
        mainCam = Camera.main;
        toolHolder = transform.Find("ToolHolder");
        tool = toolHolder.Find("Tool");
        toolSprite = tool.GetComponent<SpriteRenderer>();
        playerSprite = GetComponent<SpriteRenderer>();

        PlayerHotbar.instance.OnCurrentSlotChanged += GetEquippedItem;
        OnSlotChanged += GetEquippedItem;
    }
}
