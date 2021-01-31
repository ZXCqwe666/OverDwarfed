using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Transform itemParent;
    private PlayerInventory inventory;
    private List<InventorySlot> slots;

    private void Start()
    {
        InitializeInventoryUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            itemParent.gameObject.SetActive(!itemParent.gameObject.activeSelf);
    }
    private void UpdateSlot(object sender, InventoryChangeArgs inventoryChange)
    {
        List<InventorySlot> slot = new List<InventorySlot>();
        slot = slots.Where(wslot => wslot.isEmpty == false && wslot.itemId == inventoryChange.id).ToList();
        if (slot.Count > 0)
        {
            if (inventoryChange.removeItem) slot[0].ClearSlot();
            else slot[0].UpdateItemAmount(inventoryChange.amount); 
        }
        else
        {
            foreach (InventorySlot _slot in slots) 
                if (_slot.isEmpty)
                {
                    _slot.AddItem(inventoryChange.id, inventoryChange.amount, ItemSpawner.instance.items[inventoryChange.id].itemIcon);
                    break;
                }
        }
    }
    private void InitializeInventoryUI()
    {
        inventory = PlayerInventory.instance;
        inventory.onInventoryChanged += UpdateSlot;

        itemParent = transform.Find("ItemsParent");
        itemParent.gameObject.SetActive(false);

        slots = new List<InventorySlot>();
        //SPAGETTI??????????????????????????????
        Transform hotbar = GameObject.Find("Canvas/Hotbar").transform;
        for (int i = 0; i < hotbar.childCount; i++)
            slots.Add(hotbar.Find(i.ToString() + "/InventorySlot." + (i + 32).ToString()).GetComponent<InventorySlot>());

        for (int i = 0; i < itemParent.childCount; i++)
            slots.Add(itemParent.GetChild(i).GetComponent<InventorySlot>());

    }
}
  