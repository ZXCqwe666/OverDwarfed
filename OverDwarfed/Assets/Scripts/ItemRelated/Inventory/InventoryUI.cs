using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemParent;

    InventorySlot[] slots;

    PlayerInventory inventory;

    private void Start()
    {
        inventory = PlayerInventory.instance;
        inventory.onInventoryChangedCallback += UpdateUI;

        slots = itemParent.GetComponentsInChildren<InventorySlot>();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {

            slots[i].ClearSlot();
            Debug.Log("Cleared");
        }

        int k = 0;
        foreach (int key in inventory.InventorySlots.Keys)
        {
            slots[k].AddItem(key, inventory.InventorySlots[key], ItemSpawner.instance.items[key].itemIcon);
            k++;
            Debug.Log("ItemAdded");
        }
        Debug.Log("Updated");
    }
}
