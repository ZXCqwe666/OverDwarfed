using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Transform itemParent;
    private PlayerInventory inventory;
    private InventorySlot[] slots;

    private void Start()
    {
        InitializeInventoryUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            itemParent.gameObject.SetActive(!itemParent.gameObject.activeSelf);
    }
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].ClearSlot();

        int k = 0;
        foreach (int key in inventory.InventorySlots.Keys)
        {
            slots[k].AddItem(key, inventory.InventorySlots[key], ItemSpawner.instance.items[key].itemIcon);
            k++;
        }
    }
    private void InitializeInventoryUI()
    {
        inventory = PlayerInventory.instance;
        inventory.onInventoryChangedCallback += UpdateUI;
        itemParent = transform.Find("ItemsParent");
        itemParent.gameObject.SetActive(false);
        slots = itemParent.GetComponentsInChildren<InventorySlot>();
    }
}
