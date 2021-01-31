using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class InventorySlot : MonoBehaviour , IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Image icon;
    private Text text;

    public int itemId, amount;
    public bool isEmpty;

    private void Start()
    {
        InitializeItemSlot();
    }
    public void AddItem(int _itemId, int _amount, Sprite _itemIcon)
    {
        isEmpty = false;
        icon.sprite = _itemIcon;
        icon.color = Color.white;
        itemId = _itemId;
        SetItemAmount(_amount);
    }
    public void SetItemAmount(int _amount)
    {
        amount = _amount;
        text.text = amount.ToString();
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
    private void InitializeItemSlot()
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
        while (Input.GetMouseButton(1) && isEmpty == false && InventoryUI.instance.isOpen)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                PlayerInventory.instance.RemoveItem(itemId, amount, true);
                SetItemAmount(0);
            }
            else
            {
                PlayerInventory.instance.RemoveItem(itemId, 1, true);
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
        if(Input.GetMouseButton(0) && Input.GetMouseButton(1) == false) // LMB == true RMB == false
        {
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
        CancelDrag();
    }
    private IEnumerator CheckInventoryOpen()
    {
        while (_lastPointerData != null)
        {
            if (InventoryUI.instance.isOpen == false)
                CancelDrag();
            yield return new WaitForEndOfFrame();
        }
    }
    private void CancelDrag()
    {
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        _lastPointerData = null;      
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            InventorySlot slotWePutting = eventData.pointerDrag.GetComponent<InventorySlot>();
            if(slotWePutting.isEmpty == false)
            {
                int thisId = itemId, thisAmount = amount;
                AddItem(slotWePutting.itemId, slotWePutting.amount, ItemSpawner.instance.items[slotWePutting.itemId].itemIcon);
                slotWePutting.AddItem(thisId, thisAmount, ItemSpawner.instance.items[thisId].itemIcon);
            }
        }
    }
    #endregion
}
