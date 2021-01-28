using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public Dictionary<int, int> InventorySlots; //need dictionary for every player 

    private void Awake()
    {
        instance = this; //remove instance later
    }
    private void Start()
    {
        InventorySlots = new Dictionary<int, int>();
    }
    public void AddItem(int id, int amount) 
    {
        if (InventorySlots.ContainsKey(id)) 
        {
            InventorySlots[id] += amount;
            Mathf.Clamp(amount, 0, 999);
        }
        else 
        {
            InventorySlots.Add(id, amount);
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
            if (InventorySlots.ContainsKey(cost.itemCostId)) 
            { 
                if(InventorySlots[cost.itemCostId] >= cost.itemCostAmount)
                {
                    continue;
                }
            }
            return false;
        }
        return true;
    }
    public void SpendResources(Recipe recipe) 
    {
        foreach (Cost cost in recipe.CostList)
        {
            InventorySlots[cost.itemCostId] -= cost.itemCostAmount;
            if (InventorySlots[cost.itemCostId] == 0)
            {
                InventorySlots.Remove(cost.itemCostId);
            }
        }
    }
}