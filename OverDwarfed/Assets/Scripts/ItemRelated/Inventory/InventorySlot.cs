using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour , IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Image icon;
    private Text text;

    public Item item;
    public int amount;
    public bool isEmpty, isHotbarSlot;

    public void AddItem(Item _item, int _amount, Sprite _itemIcon)
    {
        isEmpty = false;
        icon.sprite = _itemIcon;
        icon.color = Color.white;
        item = _item;
        SetItemAmount(_amount);
    }
    public void SetItemAmount(int _amount)
    {
        amount = _amount;
        text.text = _amount.ToString();
        if (amount == 1) text.text = "";
        if (amount <= 0) ClearSlot();
    }
    public void ClearSlot()
    {
        isEmpty = true;
        icon.color = Color.clear;
        text.text = "";
        amount = 0;
    }

    #region Initialization
    public void InitializeItemSlot()
    {
        icon = GetComponent<Image>();
        text = transform.Find("Amount").GetComponent<Text>();
        ClearSlot();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        startPosition = rectTransform.anchoredPosition;
    }
    #endregion
    #region HoldDropping

    private const float holdDuration = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0) == false) // LMB == false RMB == true
        {
            StopCoroutine(HoldDrop());
            StartCoroutine(HoldDrop());
        }
    }
    private IEnumerator HoldDrop()
    {
        while (Input.GetMouseButton(1) && isEmpty == false && (InventoryUI.instance.isOpen || isHotbarSlot))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerInventory.instance.RemoveItem(item, amount, true);
                SetItemAmount(0);
            }
            else
            {
                PlayerInventory.instance.RemoveItem(item, 1, true);
                SetItemAmount(amount - 1);
            }
            yield return new WaitForSeconds(holdDuration);
        }
    }
    #endregion
    #region DragAndDrop

    private PointerEventData _lastPointerData;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1) == false) // LMB == true RMB == false
        {
            InventoryUI.instance.startedDrag = true;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.6f;
            _lastPointerData = eventData;
            StartCoroutine(CheckInventoryOpen());
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(_lastPointerData != null)
            rectTransform.anchoredPosition += _lastPointerData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastForEdgeDrop();
        CancelDrag();
    }
    void RaycastForEdgeDrop()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, results);
        if (results.Count == 0 && InventoryUI.instance.startedDrag)
        {
            PlayerInventory.instance.RemoveItem(item, amount, true);
            SetItemAmount(0);
        }
    }
    private IEnumerator CheckInventoryOpen()
    {
        while (_lastPointerData != null)
        {
            if (InventoryUI.instance.isOpen == false && isHotbarSlot == false ) 
                CancelDrag();
            yield return new WaitForEndOfFrame();
        }
    }
    private void CancelDrag()
    {
        InventoryUI.instance.startedDrag = false;
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        _lastPointerData = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && InventoryUI.instance.startedDrag)
        {
            InventorySlot slotWePutting = eventData.pointerDrag.GetComponent<InventorySlot>();
            if(slotWePutting.isEmpty == false)
            {
                Item thisItem = item; int thisAmount = amount;
                AddItem(slotWePutting.item, slotWePutting.amount, ItemSpawner.instance.items[slotWePutting.item].itemSprite);
                slotWePutting.AddItem(thisItem, thisAmount, ItemSpawner.instance.items[thisItem].itemSprite);
            }
        }
    }
    #endregion
}
