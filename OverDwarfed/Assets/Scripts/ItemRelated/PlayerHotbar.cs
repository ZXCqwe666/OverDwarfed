using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHotbar : MonoBehaviour
{
    public static PlayerHotbar instance;
    private const int hotbarSize = 5;

    private Transform panel;
    private List<Image> slots;
    private List<int> itemIdInSlot;
    private int currentSlot;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializePlayerHotbar();
    }
    private void Update()
    {
        CheckForWheelInput();
        CheckForNumberInput();
    }
    private void CheckForWheelInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            currentSlot += (int)Input.mouseScrollDelta.y;
            if (currentSlot > hotbarSize - 1) currentSlot = 0;
            else if (currentSlot < 0) currentSlot = hotbarSize - 1;
            ChangeSelectedSlot();
        }
    }
    private void CheckForNumberInput()
    {
        if (Input.anyKeyDown)
        for (int i = 1; i < 6; ++i)
        if (Input.GetKeyDown("" + i))
        {
        currentSlot = i - 1;
        ChangeSelectedSlot();
        } 
    }
    private void ChangeSelectedSlot()
    {
        for (int i = 0; i < hotbarSize; i++)
            slots[i].color = (i == currentSlot) ? Color.red : Color.white;
    }
    private void InitializePlayerHotbar()
    {
        panel = GameObject.Find("Canvas/HotBar/Panel").transform;
        itemIdInSlot = new List<int>();

        slots = new List<Image>();
        for (int i = 0; i < hotbarSize; i++)
            slots.Add(panel.Find(i.ToString()).GetComponent<Image>());

        currentSlot = 0;
        ChangeSelectedSlot();
    }
}
