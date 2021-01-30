using UnityEngine.UI;
using UnityEngine;
using System;

public class InventorySlot : MonoBehaviour
{
    public int itemId;
    public int amount;
    public bool isEmpty;


    private Image icon;
    private Text text;

    private void Start()
    {
        icon = transform.Find("ItemButton/Icon").GetComponent<Image>();
        text = transform.Find("ItemButton/Amount").GetComponent<Text>();
        ClearSlot();
    }

    public void AddItem(int _itemId, int _amount , Sprite _itemIcon)
    {
        itemId = _itemId;
        icon.sprite = _itemIcon;
        icon.color = Color.white;
        text.text = _amount.ToString();
        isEmpty = false;
        amount = _amount;
        Debug.Log("AddItem");
    }
    public void UpdateItemAmount(int _amount)
    {
        amount += _amount;
        text.text = amount.ToString();
        Debug.Log("UpdateItem");
    }
    public void ClearSlot()
    {
        icon.color = Color.clear;
        isEmpty = true;
    }
    public void DropItem()
    {
        if (isEmpty == false)
        {
            amount -= 1;
            PlayerInventory.instance.RemoveItem(itemId, 1);
        }
    }
}
