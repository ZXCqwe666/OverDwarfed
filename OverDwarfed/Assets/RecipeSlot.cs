using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RecipeSlot : MonoBehaviour
{
    public Transform popUpList;
    private Text recipeName;
    private Image resultIcon;
    private Button infoButton;

    private List<Image> costImages;
    private List<Text> resNeededText;

    private Recipe recipe;
    private Button produceOne, produceHalf, produceMax, produceInfinite;
    private ProductionBuilding currentBuilding;

    public void ClearRecipeSlot()
    {
        resultIcon.sprite = null;
        recipeName.text = "";
        popUpList.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void UpdateRecipeSlot(Recipe _recipe, ProductionBuilding _building)
    {
        gameObject.SetActive(true);
        currentBuilding = _building;
        recipe = _recipe;
        ItemData resultItem = ItemSpawner.instance.items[_recipe.resultItem];
        resultIcon.sprite = resultItem.itemSprite;
        recipeName.text = resultItem.itemName;
    }
    public void UpdatePopUpList()
    {
        foreach (Image image in costImages) image.color = Color.clear;
        foreach (Text text in resNeededText) text.text = "";
        for (int i = 0; i < recipe.CostList.Count; i++)
        {
            Item item = recipe.CostList[i].item;
            costImages[i].color = Color.white;
            costImages[i].sprite = ItemSpawner.instance.items[item].itemSprite;
            resNeededText[i].text = PlayerInventory.instance.GetResourceCount(item).ToString() + " / " + recipe.CostList[i].amount.ToString();
        }
    }
    #region Initialization
    public void InitializeRecipeSlot(int _id)
    {
        popUpList = transform.Find("PopUpList");
        recipeName = transform.Find("RecipeName").GetComponent<Text>();
        resultIcon = transform.Find("ResultIcon").GetComponent<Image>(); 
        infoButton = transform.Find("PopUpListButton").GetComponent<Button>();

        infoButton.onClick.AddListener(() => { CraftingUI.instance.SetPopUpListActivity(_id); });
        infoButton.onClick.AddListener(UpdatePopUpList);

        costImages = new List<Image>(); resNeededText = new List<Text>();
        Transform costList = popUpList.Find("CostList");
        Transform buttonList = popUpList.Find("ButtonList");

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

        produceOne.onClick.AddListener(() => { currentBuilding.StartProduction(recipe, true, false, false, false); });
        produceHalf.onClick.AddListener(() => { currentBuilding.StartProduction(recipe, false, true, false, false); });
        produceMax.onClick.AddListener(() => { currentBuilding.StartProduction(recipe, false, false, true, false); });
        produceInfinite.onClick.AddListener(() => { currentBuilding.StartProduction(recipe, false, false, false, true); });
    }
    #endregion 
}
