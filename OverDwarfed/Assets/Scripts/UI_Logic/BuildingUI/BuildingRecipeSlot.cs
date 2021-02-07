using UnityEngine.UI;
using UnityEngine;

public class BuildingRecipeSlot : MonoBehaviour
{
    private Text buildingName;
    private Image buildingIcon;
    private Button craftButton;

    public int buildingID;

    private void Start()
    {
        InitializeBuildingRecipeSlot();
    }
    public void InitializeBuildingRecipeSlot()
    {
        buildingName = transform.Find("RecipeName").GetComponent<Text>();
        buildingIcon = transform.Find("ResultIcon").GetComponent<Image>();
        craftButton = GetComponent<Button>();

        BuildingData building = BuildingSystem.instance.buildings[buildingID];
        buildingIcon.sprite = building.buildingIcon;
        buildingName.text = building.buildingName;
        craftButton.onClick.AddListener(() => { BuildingUI.instance.OpenBuildingMenu(buildingID); });
    }
}
