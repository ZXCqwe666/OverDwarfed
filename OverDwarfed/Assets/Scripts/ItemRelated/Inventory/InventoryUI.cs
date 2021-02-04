using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public List<InventorySlot> slots;
    private Transform itemParent, hotbarSlotsHolder;
    public bool isOpen = false, startedDrag = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeInventoryUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            StartCoroutine(ChangeOpenState()); 
    }
    private IEnumerator ChangeOpenState()
    {
        isOpen = !itemParent.gameObject.activeSelf;
        yield return new WaitForSeconds(Time.fixedDeltaTime * 2f);
        itemParent.gameObject.SetActive(isOpen);
    }
    #region SlotsUpdating
    private void UpdateSlot(object sender, ChangeArgs inventoryChange)
    {
        List<InventorySlot> emptySlots = new List<InventorySlot>(), slotsWithThisId = new List<InventorySlot>();
        FillLists(ref emptySlots, ref slotsWithThisId, inventoryChange.item);

        int stackSize = ItemSpawner.instance.items[inventoryChange.item].stackSize;
        int amountChange = inventoryChange.amount;

        if(amountChange > 0) // adding
        {
            foreach (InventorySlot slot in slotsWithThisId)
            {
                if (slot.amount < stackSize && amountChange > 0)
                {
                    int fitAmount = stackSize - slot.amount;

                    if(fitAmount >= amountChange)
                    {
                        slot.SetItemAmount(slot.amount + amountChange);
                        amountChange = 0;
                    }
                    else
                    {
                        slot.SetItemAmount(stackSize);
                        amountChange -= fitAmount;
                    }
                }
            }
            foreach(InventorySlot slot in emptySlots)
            {
                if(amountChange > 0)
                {
                    if(amountChange >= stackSize)
                    {
                        slot.AddItem(inventoryChange.item, stackSize, ItemSpawner.instance.items[inventoryChange.item].itemSprite);
                        amountChange -= stackSize;
                    }
                    else
                    {
                        slot.AddItem(inventoryChange.item, amountChange, ItemSpawner.instance.items[inventoryChange.item].itemSprite);
                        amountChange = 0;
                    }
                }
            }
        }
        else // removing
        {
            for (int i = slotsWithThisId.Count - 1; i >= 0; i--)
            {
                if(amountChange < 0)
                {
                    int canRemove = slotsWithThisId[i].amount;

                    if (amountChange < -canRemove)
                    {
                        slotsWithThisId[i].SetItemAmount(0);
                        amountChange += canRemove;
                    }
                    else
                    {
                        slotsWithThisId[i].SetItemAmount(slotsWithThisId[i].amount + amountChange);
                        return;
                    }
                }
            }
        }
    }
    public void FillLists(ref List<InventorySlot> emptySlots, ref List<InventorySlot> slotsWithThisId, Item changedItem)
    {
        emptySlots = slots.Where(slot => slot.isEmpty).ToList();
        slotsWithThisId = slots.Where(slot => slot.isEmpty == false && slot.item == changedItem).ToList();
    }
    #endregion
    #region Initialization
    private void InitializeInventoryUI()
    {
        PlayerInventory.instance.onInventoryChanged += UpdateSlot;

        itemParent = transform.Find("ItemsParent");
        itemParent.gameObject.SetActive(isOpen);
        hotbarSlotsHolder = transform.Find("SlotsHolder");

        slots = new List<InventorySlot>();
        for (int i = 0; i < hotbarSlotsHolder.childCount; i++)
        slots.Add(hotbarSlotsHolder.GetChild(i).GetComponent<InventorySlot>());
        for (int i = 0; i < itemParent.childCount; i++)
        slots.Add(itemParent.GetChild(i).GetComponent<InventorySlot>());
        foreach (InventorySlot slot in slots)
            slot.InitializeItemSlot();
    }
    #endregion
}
