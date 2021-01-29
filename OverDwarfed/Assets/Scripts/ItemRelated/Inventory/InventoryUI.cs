using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    PlayerInventory inventory;

    private void Start()
    {
        inventory = PlayerInventory.instance;
        inventory.onInventoryChangedCallback += UpdateUI;
    }

    private void UpdateUI()
    {
    }
}
