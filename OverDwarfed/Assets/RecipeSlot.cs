using UnityEngine.UI;
using UnityEngine;

public class RecipeSlot : MonoBehaviour
{
    public Transform popUpList;
    private Button infoButton;
    private Text recipeName;
    public Image resultIcon;
    public int id;

    public void ClearRecipeSlot()
    {
        resultIcon.sprite = null;
        recipeName.text = "";
        popUpList.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void UpdateRecipeSlot(Recipe recipe)
    {
        ItemData resultItem = ItemSpawner.instance.items[recipe.resultItemId];
        resultIcon.sprite = resultItem.itemIcon;
        recipeName.text = resultItem.itemName;
        gameObject.SetActive(true);
    }
    #region Initialization
    public void InitializeRecipeSlot(int _id)
    {
        id = _id; 
        popUpList = transform.Find("PopUpList");
        infoButton = GetComponent<Button>();
        recipeName = transform.Find("RecipeName").GetComponent<Text>();
        resultIcon = transform.Find("ResultIcon").GetComponent<Image>(); 
        infoButton.onClick.AddListener(() => { CraftingUI.instance.ClosePopUpSlot(id); });
        infoButton.onClick.AddListener(() => { popUpList.gameObject.SetActive(!popUpList.gameObject.activeSelf); });
    }
    #endregion 
}
