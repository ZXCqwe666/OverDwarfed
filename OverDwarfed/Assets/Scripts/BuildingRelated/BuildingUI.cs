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
    private void InitializeBuildingUI()
    {
        buildingUIParent = transform.Find("BuildingUIParent");
        buttonHolder = buildingUIParent.Find("ButtonHolder");
        Button buildingUIParentButton = buildingUIParent.GetComponent<Button>();
        buildingUIParentButton.onClick.AddListener(ChangeButtonHolderState);

        buttonHolder.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(0); });
        buttonHolder.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(1); });
        buttonHolder.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(2); });
        buttonHolder.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(3); });
        buttonHolder.GetChild(4).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(4); });
        buttonHolder.GetChild(5).GetComponent<Button>().onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(5); });
    }
}
