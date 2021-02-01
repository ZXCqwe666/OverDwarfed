using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    private Transform buildingUIParent, buttonHolder;

    private void Start()
    {
        InitializeBuildingUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
            ChangeButtonHolderState();
    }
    private void InitializeBuildingUI()
    {
        buildingUIParent = transform.Find("BuildingUIParent");
        buttonHolder = buildingUIParent.Find("ButtonHolder");
        Button buildingUIParentButton = buildingUIParent.GetComponent<Button>();
        buildingUIParentButton.onClick.AddListener(ChangeButtonHolderState);

        for (int i = 0; i < buttonHolder.childCount - 1; i++)
            buttonHolder.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(0); });
    }
    private void ChangeUIParentState()
    {
        buildingUIParent.gameObject.SetActive(!buildingUIParent.gameObject.activeSelf);
    }
    private void ChangeButtonHolderState()
    {
        buttonHolder.gameObject.SetActive(!buttonHolder.gameObject.activeSelf);
        if (buttonHolder.gameObject.activeSelf)
            BuildingSystem.instance.SetSpriteAndActivity(null, false);
    }
}
