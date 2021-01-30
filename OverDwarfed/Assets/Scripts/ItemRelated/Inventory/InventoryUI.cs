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
            if (i < inventory.InventorySlots.Count)
            {
                slots[i].AddItem(inventory.InventorySlots[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
