using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public static CraftingMenu instance;
    private Button produceOne, produceHalf, produceMax, produceInfinite;
    private List<Image> costImages;
    private List<Text> resNeededText;
    private Text description, ownedAmount;

    private Recipe selectedRecipe;
    private bool isOpen;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeCraftingMenu();
    }
    public void SetActive(bool activeState)
    {
        gameObject.SetActive(activeState);
        isOpen = activeState;
    }
    public void UpdateCraftingMenu(ProductionBuilding building, Recipe recipe)
    {
        selectedRecipe = recipe;

        foreach (Image image in costImages) image.color = Color.clear;
        for (int i = 0; i < selectedRecipe.CostList.Count; i++)
        {
            Item item = selectedRecipe.CostList[i].item;
            costImages[i].color = Color.white;
            costImages[i].sprite = ItemSpawner.instance.items[item].itemIcon;
        }
        description.text = ItemSpawner.instance.items[selectedRecipe.resultItem].description;

        produceOne.onClick.RemoveAllListeners();
        produceHalf.onClick.RemoveAllListeners();
        produceMax.onClick.RemoveAllListeners();
        produceInfinite.onClick.RemoveAllListeners();

        produceOne.onClick.AddListener(() => { building.StartProduction(selectedRecipe, true, false, false, false); });
        produceHalf.onClick.AddListener(() => { building.StartProduction(selectedRecipe, false, true, false, false); });
        produceMax.onClick.AddListener(() => { building.StartProduction(selectedRecipe, false, false, true, false); });
        produceInfinite.onClick.AddListener(() => { building.StartProduction(selectedRecipe, false, false, false, true); });

        produceHalf.onClick.AddListener(CraftingUI.instance.DisableCraftingUI);
        produceMax.onClick.AddListener(CraftingUI.instance.DisableCraftingUI);
        produceInfinite.onClick.AddListener(CraftingUI.instance.DisableCraftingUI);

        SetActive(true);
        UpdateInfoText(Item.log, 0);
    }
    public void UpdateInfoText(Item _item, int amount)
    {
        if(isOpen)
        {
            ownedAmount.text = PlayerInventory.instance.GetResourceCount(selectedRecipe.resultItem).ToString();

            foreach (Text text in resNeededText) text.text = "";
            for (int i = 0; i < selectedRecipe.CostList.Count; i++)
            {
                Item item = selectedRecipe.CostList[i].item;
                resNeededText[i].text = PlayerInventory.instance.GetResourceCount(item).ToString() + " / " + selectedRecipe.CostList[i].amount.ToString();
            }
        }
    }
    #region Initialization
    public void InitializeCraftingMenu()
    {
        PlayerInventory.instance.onInventoryChanged += UpdateInfoText;

        costImages = new List<Image>(); resNeededText = new List<Text>();
        Transform buttonList = transform.Find("ButtonList");
        Transform costList = transform.Find("CostList");

        for (int i = 0; i < costList.childCount; i++)
        {
            Transform cost = costList.GetChild(i);
            costImages.Add(cost.transform.Find("CostImage").GetComponent<Image>());
            resNeededText.Add(cost.transform.Find("ResNeededText").GetComponent<Text>());
        }
        produceOne = buttonList.Find("One").GetComponent<Button>();
        produceHalf = buttonList.Find("Half").GetComponent<Button>();
        produceMax = buttonList.Find("Max").GetComponent<Button>();
        produceInfinite = buttonList.Find("Infinite").GetComponent<Button>();
        description = transform.Find("Description").GetComponent<Text>();
        ownedAmount = transform.Find("OwnedAmount").GetComponent<Text>();

        SetActive(false);
    }
    #endregion
}
