using UnityEngine.UI;
using UnityEngine;

public class RecipeSlot : MonoBehaviour
{
    private Text recipeName, craftingAmount;
    private Image resultIcon;
    private Button craftButton;

    public void UpdateRecipeSlot(Recipe _recipe)
    {
        InitializeRecipeSlot();

        ItemData resultItem = ItemSpawner.instance.items[_recipe.resultItem];
        resultIcon.sprite = resultItem.itemIcon;
        recipeName.text = resultItem.itemName;
        craftingAmount.text = (_recipe.resultAmount == 1) ? "" : _recipe.resultAmount.ToString();
        craftButton.onClick.AddListener(() => { CraftingUI.instance.OpenCraftingMenu(_recipe); });
    }
    public void InitializeRecipeSlot()
    {
        recipeName = transform.Find("RecipeName").GetComponent<Text>();
        craftingAmount = transform.Find("CraftingAmount").GetComponent<Text>();
        resultIcon = transform.Find("ResultIcon").GetComponent<Image>();
        craftButton = GetComponent<Button>();
    }
}
