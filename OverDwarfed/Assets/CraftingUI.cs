using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;

    private List<RecipeSlot> recipeSlots;
    private Transform craftingUIParent;
    private bool isOpen;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeCraftingUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
            craftingUIParent.gameObject.SetActive(false);
    }
    public void ChangeOpenState(ProductionBuilding building)
    {
        isOpen = !craftingUIParent.gameObject.activeSelf;
        craftingUIParent.gameObject.SetActive(isOpen);
        if (isOpen) UpdateRecipeSlots(building);
    }
    private void InitializeCraftingUI()
    {
        craftingUIParent = transform.Find("CraftingUIParent");
        recipeSlots = new List<RecipeSlot>();
        for (int i = 0; i < craftingUIParent.childCount ; i++)
        {
            RecipeSlot slot = craftingUIParent.GetChild(i).GetComponent<RecipeSlot>();
            recipeSlots.Add(slot);
            slot.InitializeRecipeSlot(i);
            slot.ClearRecipeSlot();
        }
    }
    private void UpdateRecipeSlots(ProductionBuilding building)
    {
        foreach (RecipeSlot slot in recipeSlots)
            slot.ClearRecipeSlot();
        for (int i = 0; i < building.recipeIdList.Count; i++)
            recipeSlots[i].UpdateRecipeSlot(CraftingRecipeList.instance.recipes[building.recipeIdList[i]]);
    }
    public void ClosePopUpSlot(int index)
    {
        for (int i = 0; i < recipeSlots.Count; i++)
            if (index != i) recipeSlots[i].popUpList.gameObject.SetActive(false);
    }
}
