using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public static BuildingUI instance;
    private const int buttonHeight = 174;

    private Transform buildingUIParent;
    private RectTransform recipeLayout;
    private Vector3 buildingUIParentStartPosition, buildingUIParentEndPosition;

    private bool isOpen;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeBuidingUI();
    }
    private void Update()
    {
        if (isOpen && (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape)))
            DisableBuildingUI();
        else if (isOpen == false && Input.GetKeyDown(KeyCode.B))
            EnableBuildingUI();
    }
    public void EnableBuildingUI()
    {
        DisableOtherUIPanels();

        buildingUIParent.gameObject.SetActive(true);
        isOpen = true;
    }
    public void DisableBuildingUI()
    {
        buildingUIParent.gameObject.SetActive(false);
        isOpen = false;
        CloseBuildingMenu();
    }
    public void OpenBuildingMenu(int buildingId)
    {
        BuildingMenu.instance.UpdateBuildingMenu(buildingId);
        buildingUIParent.position = buildingUIParentEndPosition;
    }
    public void CloseBuildingMenu()
    {
        BuildingMenu.instance.SetActive(false);
        buildingUIParent.position = buildingUIParentStartPosition;
    }
    public void ChangeLayoutSize(int slotsChange)
    {
        recipeLayout.sizeDelta += new Vector2(0, buttonHeight * slotsChange);
    }
    private void DisableOtherUIPanels()
    {
        StartCoroutine(InventoryUI.instance.ChangeOpenState(false));
        CraftingUI.instance.DisableCraftingUI();
    }
    private void InitializeBuidingUI()
    {
        buildingUIParent = transform.Find("BuildingUIParent");
        recipeLayout = buildingUIParent.Find("ScrollRect").Find("BuildingRecipeLayout").GetComponent<RectTransform>();
        buildingUIParentStartPosition = buildingUIParent.position;
        buildingUIParentEndPosition = buildingUIParentStartPosition + Vector3.left * 270;
    }
}
