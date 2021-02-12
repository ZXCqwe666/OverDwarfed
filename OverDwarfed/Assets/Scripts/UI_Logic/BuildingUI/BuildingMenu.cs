using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    public static BuildingMenu instance;
    private Button spawnBuilding;
    private List<Image> costImages;
    private List<Text> resNeededText;
    private Text description;

    private Recipe selectedRecipe;
    private bool isOpen;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeBuildingMenu();
    }
    public void SetActive(bool activeState)
    {
        gameObject.SetActive(activeState);
        isOpen = activeState;
    }
    public void UpdateBuildingMenu(int _buildingId)
    {
        selectedRecipe = CraftingRecipeList.instance.recipes[BuildingSystem.instance.buildings[_buildingId].recipeId];

        foreach (Image image in costImages) image.color = Color.clear;
        for (int i = 0; i < selectedRecipe.CostList.Count; i++)
        {
            Item item = selectedRecipe.CostList[i].item;
            costImages[i].color = Color.white;
            costImages[i].sprite = ItemSpawner.instance.items[item].itemIcon;
        }
        description.text = BuildingSystem.instance.buildings[_buildingId].description;

        spawnBuilding.onClick.RemoveAllListeners();
        spawnBuilding.onClick.AddListener(() => { BuildingSystem.instance.UpdateBlueprint(_buildingId); }) ;
        spawnBuilding.onClick.AddListener(BuildingUI.instance.DisableBuildingUI);

        SetActive(true);
        UpdateInfoText(Item.log, 0);
    }
    public void UpdateInfoText(Item _item, int _amount)
    {
        if (isOpen)
        {
            foreach (Text text in resNeededText) text.text = "";
            for (int i = 0; i < selectedRecipe.CostList.Count; i++)
            {
                Item item = selectedRecipe.CostList[i].item;
                resNeededText[i].text = PlayerInventory.instance.GetResourceCount(item).ToString() + " / " + selectedRecipe.CostList[i].amount.ToString();
            }
        }
    }
    #region Initialization
    public void InitializeBuildingMenu()
    {
        PlayerInventory.instance.onInventoryChanged += UpdateInfoText;

        costImages = new List<Image>(); resNeededText = new List<Text>();
        Transform costList = transform.Find("CostList");

        for (int i = 0; i < costList.childCount; i++)
        {
            Transform cost = costList.GetChild(i);
            costImages.Add(cost.transform.Find("CostImage").GetComponent<Image>());
            resNeededText.Add(cost.transform.Find("ResNeededText").GetComponent<Text>());
        }
        spawnBuilding = transform.Find("SpawnBuilding").GetComponent<Button>();
        description = transform.Find("Description").GetComponent<Text>();

        SetActive(false);
    }
    #endregion
}
