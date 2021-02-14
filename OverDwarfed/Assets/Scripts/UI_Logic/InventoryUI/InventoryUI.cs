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
        if (isOpen && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape)))
            StartCoroutine(ChangeOpenState(false));
        else if (isOpen == false && (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)))
            StartCoroutine(ChangeOpenState(true));
    }
    public IEnumerator ChangeOpenState(bool activity)
    {
        if (activity) DisableOtherUIPanels();
        isOpen = activity;
        yield return new WaitForSeconds(Time.fixedDeltaTime); // allows to cancel DragOnSlots (they check isOpen variable)
        itemParent.gameObject.SetActive(isOpen);
    }
    private void DisableOtherUIPanels()
    {
        CraftingUI.instance.DisableCraftingUI();
        BuildingUI.instance.DisableBuildingUI();
    }
    #region SlotsUpdating
    private void UpdateSlot(Item changedItem, int changeAmount)
    {
        List<InventorySlot> emptySlots = new List<InventorySlot>(), slotsWithThisId = new List<InventorySlot>();
        FillLists(ref emptySlots, ref slotsWithThisId, changedItem);

        int stackSize = ItemSpawner.instance.items[changedItem].stackSize;
        int amountChange = changeAmount;

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
                        slot.AddItem(changedItem, stackSize, ItemSpawner.instance.items[changedItem].itemIcon);
                        amountChange -= stackSize;
                    }
                    else
                    {
                        slot.AddItem(changedItem, amountChange, ItemSpawner.instance.items[changedItem].itemIcon);
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
        for (int i = hotbarSlotsHolder.childCount - 1; i >= 0; i--)
        slots.Add(hotbarSlotsHolder.GetChild(i).Find("ItemIcon").GetComponent<InventorySlot>());
        for (int i = 0; i < itemParent.childCount; i++)
        slots.Add(itemParent.GetChild(i).Find("ItemIcon").GetComponent<InventorySlot>());
        foreach (InventorySlot slot in slots)
            slot.InitializeItemSlot();
    }
    #endregion
}
