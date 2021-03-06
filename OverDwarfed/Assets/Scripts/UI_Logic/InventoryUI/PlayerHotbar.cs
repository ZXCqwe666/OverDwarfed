﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerHotbar : MonoBehaviour
{
    public static PlayerHotbar instance;
    public Action OnCurrentSlotChanged;

    private const int hotbarSize = 5;
    private List<Image> slots;
    public int currentSlot;

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
        slots[i].color = (i == currentSlot) ? Color.white : Color.clear;
        OnCurrentSlotChanged?.Invoke();
    }
    private void InitializePlayerHotbar()
    {
        slots = new List<Image>();
        for (int i = 0; i < hotbarSize; i++)
            slots.Add(transform.Find(i.ToString()).GetComponent<Image>());
        currentSlot = 0;
    }
}
