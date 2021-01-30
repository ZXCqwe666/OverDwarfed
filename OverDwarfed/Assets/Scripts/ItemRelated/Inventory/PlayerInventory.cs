using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public Dictionary<int, int> InventorySlots; //need dictionary for every player 
    private const int capacity = 32, stackSize = 64; //inventory cells 

    public delegate void InventoryChanged(object sender, InventoryChangeArgs inventoryChange);
    public InventoryChanged onInventoryChanged;

    private void Awake()
    {
        instance = this; //remove instance later // change ITEM drop position Later!!!!
    }
    private void Start()
    {
        InventorySlots = new Dictionary<int, int>();
    }
    public bool CanAdd(int id)
    {
        if (InventorySlots.ContainsKey(id) == false && InventorySlots.Count >= capacity ||
            InventorySlots.ContainsKey(id) && InventorySlots[id] >= stackSize)
            return false;
        return true;
    }
    public void AddItem(int id, int amount) 
    {
        if (InventorySlots.ContainsKey(id))
        {
            InventorySlots[id] += amount;
            Mathf.Clamp(InventorySlots[id], 1, stackSize);
        }
        else InventorySlots.Add(id, amount);

        onInventoryChanged?.Invoke(this, new InventoryChangeArgs(id, amount, false));
    }
    public void RemoveItem(int id, int amount, bool shouldDrop)
    {
        if (InventorySlots.ContainsKey(id))
        {
            InventorySlots[id] -= amount;

            if (InventorySlots[id] <= 0)
            {
                InventorySlots.Remove(id);
                onInventoryChanged?.Invoke(this, new InventoryChangeArgs(id, 0, true));
            }
            else 
                onInventoryChanged?.Invoke(this,new InventoryChangeArgs(id, -amount, false));

            if (shouldDrop) ItemSpawner.instance.SpawnItem(transform.position + Vector3.down * 1.5f, id); // change drop position Later
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
        foreach(Cost cost in recipe.CostList) 
        {
            if (InventorySlots.ContainsKey(cost.itemCostId) && InventorySlots[cost.itemCostId] >= cost.itemCostAmount)
                continue;
            return false;
        }
        return true;
    }
    public void SpendResources(Recipe recipe) 
    {
        foreach (Cost cost in recipe.CostList)
        RemoveItem(cost.itemCostId, cost.itemCostAmount, false);
    }
}
public class InventoryChangeArgs : EventArgs
{
    public int id, amount;
    public bool removeItem;
    public InventoryChangeArgs(int _id, int _amount, bool _removeItem)
    {
        id = _id;
        amount = _amount;
        removeItem = _removeItem;
    }
}
