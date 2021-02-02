using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;
    private const float interactDistance = 3f;

    private List<RecipeSlot> recipeSlots;
    private Transform craftingUIParent, player;
    private Vector3 buildingPosition;

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
        if (isOpen && (Vector3.Distance(player.position, buildingPosition) > interactDistance
            || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
            craftingUIParent.gameObject.SetActive(false);
    }
    public void EnableCraftingUI(ProductionBuilding building)
    {
        UpdateRecipeSlots(building);
        buildingPosition = building.gameObject.transform.position;
        craftingUIParent.gameObject.SetActive(true);
        isOpen = true;
    }
    private void UpdateRecipeSlots(ProductionBuilding building)
    {
        foreach (RecipeSlot slot in recipeSlots)
            slot.ClearRecipeSlot();
        for (int i = 0; i < building.recipeIdList.Count; i++)
            recipeSlots[i].UpdateRecipeSlot(CraftingRecipeList.instance.recipes[building.recipeIdList[i]], building);
    }
    public void SetPopUpListActivity(int index)
    {
        for (int i = 0; i < recipeSlots.Count; i++)
        {
            if (index != i) recipeSlots[i].popUpList.gameObject.SetActive(false);
            else recipeSlots[i].popUpList.gameObject.SetActive(!recipeSlots[i].popUpList.gameObject.activeSelf);
        }
    }
    #region Initialization
    private void InitializeCraftingUI()
    {
        player = FindObjectOfType<PlayerController>().transform;
        craftingUIParent = transform.Find("CraftingUIParent");

        recipeSlots = new List<RecipeSlot>();
        for (int i = 0; i < craftingUIParent.childCount; i++)
        {
            RecipeSlot slot = craftingUIParent.GetChild(i).GetComponent<RecipeSlot>();
            recipeSlots.Add(slot);
            slot.InitializeRecipeSlot(i);
            slot.ClearRecipeSlot();
        }
    }
    #endregion
}
