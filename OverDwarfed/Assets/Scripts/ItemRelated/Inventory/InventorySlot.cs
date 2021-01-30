using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour , IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{ 

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 startPosition;

    private const float holdDuration = 0.1f;
    private Image icon;
    private Text text;

    public int itemId, amount;
    public bool isEmpty;

    private void Start()
    {
        InitializeItemSlot();
    }
    private void InitializeItemSlot()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        startPosition = rectTransform.anchoredPosition;

        icon = GetComponent<Image>();
        text = transform.Find("Amount").GetComponent<Text>();
        ClearSlot();
    }
    public void AddItem(int _itemId, int _amount, Sprite _itemIcon)
    {
        isEmpty = false;

        itemId = _itemId;
        UpdateItemAmount(_amount);

        icon.sprite = _itemIcon;
        icon.color = Color.white;
    }
    public void UpdateItemAmount(int _amount)
    {
        amount += _amount;
        text.text = amount.ToString();
    }
    public void ClearSlot()
    {
        icon.color = Color.clear;
        text.text = "";
        isEmpty = true;
        amount = 0;
    }
    private IEnumerator HoldDrop()
    {
        while (Input.GetMouseButton(1))
        {
            DropItem();
            yield return new WaitForSeconds(holdDuration);
        }
    }
    public void DropItem()
    {
        if (isEmpty == false)
            PlayerInventory.instance.RemoveItem(itemId, 1, true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1))
        {
            StopCoroutine(HoldDrop());
            StartCoroutine(HoldDrop());
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;    
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("on drop");

        if (eventData.pointerDrag != null) // я пустой
        {
            InventorySlot slotWePutting = eventData.pointerDrag.GetComponent<InventorySlot>();
            if (slotWePutting == this) Debug.LogError("qweqe");

            if (slotWePutting.isEmpty == false)
            {
                if (isEmpty == true) // putting slot in empty slot
                {
                    AddItem(slotWePutting.itemId, slotWePutting.amount, ItemSpawner.instance.items[slotWePutting.itemId].itemIcon); // can drag empty tile 
                    slotWePutting.ClearSlot();
                }
                else // swapping slots
                {
                    int thisId = itemId;
                    int thisAmount = amount;
                    ClearSlot();
                    AddItem(slotWePutting.itemId, slotWePutting.amount, ItemSpawner.instance.items[slotWePutting.itemId].itemIcon);
                    slotWePutting.ClearSlot();
                    slotWePutting.AddItem(thisId, thisAmount, ItemSpawner.instance.items[thisId].itemIcon);
                }
            }
        }
    }
}
