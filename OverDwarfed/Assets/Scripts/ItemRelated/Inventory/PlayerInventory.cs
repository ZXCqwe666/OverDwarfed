using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    private Dictionary<int, int> InventorySlots;
    public delegate void InventoryChanged(object sender, ChangeArgs inventoryChange);
    public InventoryChanged onInventoryChanged;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InventorySlots = new Dictionary<int, int>();
    }
    public bool CanAdd(int id)
    {
        bool hasEmptySlot = (InventoryUI.instance.slots.Where(w => w.isEmpty).ToList().Count > 0) ? true : false;

        return (InventorySlots.ContainsKey(id) == false && hasEmptySlot || // есть место для новой вещи
            InventorySlots.ContainsKey(id) && InventorySlots[id] % ItemSpawner.instance.items[id].stackSize != 0 || // cтак не забит
            InventorySlots.ContainsKey(id) && InventorySlots[id] % ItemSpawner.instance.items[id].stackSize == 0 && hasEmptySlot); // стак забит но есть слот
    }
    public void AddItem(int id, int amount)
    {
        if (InventorySlots.ContainsKey(id)) 
             InventorySlots[id] += amount;
        else InventorySlots.Add(id, amount);
        onInventoryChanged?.Invoke(this, new ChangeArgs(id, amount));
    }
    public void RemoveItem(int id, int amount, bool slotDropping)
    {
        if (InventorySlots.ContainsKey(id))
        {
            InventorySlots[id] -= amount;
            if (slotDropping) ItemSpawner.instance.SpawnItem(transform.position + Vector3.down * 1.5f, id, amount); // temp position
            else onInventoryChanged?.Invoke(this, new ChangeArgs(id, -amount));
            if (InventorySlots[id] <= 0) InventorySlots.Remove(id);
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
            if (InventorySlots.ContainsKey(cost.id) && InventorySlots[cost.id] >= cost.amount) continue;
            return false;
        }
        return true;
    }
    private void SpendResources(Recipe recipe)
    {
        foreach (Cost cost in recipe.CostList)
            RemoveItem(cost.id, cost.amount, false);
    }
}
public class ChangeArgs : EventArgs
{
    public int id, amount;
    public ChangeArgs(int _id, int _amount)
    {
        id = _id;
        amount = _amount;
    }
}
