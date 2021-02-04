using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    private Dictionary<Item, int> InventorySlots;
    public delegate void InventoryChanged(object sender, ChangeArgs inventoryChange);
    public InventoryChanged onInventoryChanged;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InventorySlots = new Dictionary<Item, int>();
    }
    public int CanAddCapacity(Item item)
    {
        List<InventorySlot> emptySlots = new List<InventorySlot>(), slotsWithThisId = new List<InventorySlot>();
        InventoryUI.instance.FillLists(ref emptySlots, ref slotsWithThisId, item);
        int stackSize = ItemSpawner.instance.items[item].stackSize;

        int capacity = emptySlots.Count * stackSize;
        foreach (InventorySlot slot in slotsWithThisId)
            capacity += stackSize - slot.amount;
        return capacity;
    }
    public void AddItem(Item item, int amount)
    {
        if (InventorySlots.ContainsKey(item)) 
             InventorySlots[item] += amount;
        else InventorySlots.Add(item, amount);
        onInventoryChanged?.Invoke(this, new ChangeArgs(item, amount));
    }
    public void RemoveItem(Item item, int amount, bool slotDropping)
    {
        if (InventorySlots.ContainsKey(item))
        {
            InventorySlots[item] -= amount;
            if (slotDropping) ItemSpawner.instance.SpawnItem(transform.position + Vector3.down * 1.5f, item, amount); // temp position
            else onInventoryChanged?.Invoke(this, new ChangeArgs(item, -amount));
            if (InventorySlots[item] <= 0) InventorySlots.Remove(item);
        }
    }
    public bool Buy(Recipe recipe)
    {
        if (CanBuy(recipe))
        {
            SpendResources(recipe);
            return true;
        }
        else return false;
    }
    public bool CanBuy(Recipe recipe)
    {
        foreach (Cost cost in recipe.CostList)
        {
            if (InventorySlots.ContainsKey(cost.item) && InventorySlots[cost.item] >= cost.amount) continue;
            return false;
        }
        return true;
    }
    private void SpendResources(Recipe recipe)
    {
        foreach (Cost cost in recipe.CostList)
            RemoveItem(cost.item, cost.amount, false);
    }
    public int CanBuyAmount(Recipe recipe)
    {
        List<int> buyAmounts = new List<int>();

        foreach (Cost cost in recipe.CostList)
        {
            if (InventorySlots.ContainsKey(cost.item) && InventorySlots[cost.item] >= cost.amount)
            {
                buyAmounts.Add(InventorySlots[cost.item] / cost.amount);
            }
            else return 0;
        }
        return buyAmounts.Min();
    }
    public int GetResourceCount(Item item)
    {
        if (InventorySlots.ContainsKey(item))
            return InventorySlots[item];
        else return 0;
    }
}
public class ChangeArgs : EventArgs
{
    public Item item;
    public int amount;
    public ChangeArgs(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
