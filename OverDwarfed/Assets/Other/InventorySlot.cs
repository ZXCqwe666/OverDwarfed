using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private int itemId;
    private Image icon;

    private void Start()
    {
        icon = transform.Find("ItemButton/Icon").GetComponent<Image>();
    }

    public void AddItem(int _itemId)
    {
        itemId = _itemId;
        icon.sprite = ItemSpawner.instance.items[_itemId].itemIcon;
        icon.color = Color.white;
    }
    public void ClearSlot()
    {
        icon.color = Color.clear;
    }
}
