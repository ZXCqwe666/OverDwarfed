using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    public Dictionary<int, int> InventorySlots;

    private void Awake()
    {
        instance = this;
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
        Debug.Log(InventorySlots[id]);
    }
    public bool CanBuy(int[] _itemCostId, int[] _itemCostAmount)
    {
        bool isAbleToBuy = true;

        foreach(int element in _itemCostId) 
        {
            if (InventorySlots.ContainsKey(_itemCostId[element])) 
            { 
                if(InventorySlots[element] >= _itemCostAmount[element])
                {
                    continue;
                }
            }
            isAbleToBuy = false;
        }
        return isAbleToBuy;
    }
    public void SpendResources(int[] _itemCostId, int[] _itemCostAmount) 
    {
        foreach (int element in _itemCostId)
        {
            InventorySlots[element] -= _itemCostAmount[element];
            if (InventorySlots[element] == 0)
            {
                InventorySlots.Remove(element);
            }
        }
    }
}