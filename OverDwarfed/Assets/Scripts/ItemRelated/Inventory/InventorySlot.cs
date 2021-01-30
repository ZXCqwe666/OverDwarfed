using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private int itemId;
    private Image icon;
    private Text text;

    private void Start()
    {
        icon = transform.Find("ItemButton/Icon").GetComponent<Image>();
        text = transform.Find("ItemButton/Amount").GetComponent<Text>();
    }

    public void AddItem(int _itemId, int _amount , Sprite _itemIcon)
    {
        itemId = _itemId;
        icon.sprite = _itemIcon;
        icon.color = Color.white;
        text.text = _amount.ToString();
    }
    public void ClearSlot()
    {
        icon.color = Color.clear;
    }
}
