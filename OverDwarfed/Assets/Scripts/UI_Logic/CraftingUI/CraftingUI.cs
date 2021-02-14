using System.Collections;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;
    private const float interactDistance = 3f;
    private const int buttonHeight = 150, buttonWidht = 504;  

    private Transform craftingUIParent, player;
    private RectTransform recipeLayout;
    private Vector3 buildingPosition, craftingUIParentStartPosition, craftingUIParentEndPosition;

    private GameObject recipeSlotPrefab;
    private bool isOpen;

    private ProductionBuilding currentBuilding;

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
            DisableCraftingUI();
    }
    public void EnableCraftingUI(ProductionBuilding building)
    {
        if (isOpen) return;
        DisableOtherUIPanels();

        currentBuilding = building;

        UpdateRecipeSlots(currentBuilding);
        buildingPosition = building.gameObject.transform.position;
        craftingUIParent.gameObject.SetActive(true);
        StartCoroutine(DelayedIsOpen(true));
    }
    public void DisableCraftingUI()
    {
        if (isOpen == false) return;

        craftingUIParent.gameObject.SetActive(false);
        CraftingMenu.instance.SetActive(false);
        craftingUIParent.position = craftingUIParentStartPosition;
        isOpen = false;
    }
    private void DisableOtherUIPanels()
    {
        StartCoroutine(InventoryUI.instance.ChangeOpenState(false));
        BuildingUI.instance.DisableBuildingUI();
    }
    private IEnumerator DelayedIsOpen(bool value) // prevents immediate closing in update()
    {
        yield return new WaitForEndOfFrame();
        isOpen = value;
    }
    private void UpdateRecipeSlots(ProductionBuilding building)
    {
        for (int i = recipeLayout.childCount - 1; i >= 0; i--)
            Destroy(recipeLayout.GetChild(i).gameObject);
        foreach(int id in building.recipeIdList)
        {
            GameObject newSlot = Instantiate(recipeSlotPrefab, Vector3.zero, Quaternion.identity, recipeLayout);
            newSlot.GetComponent<RecipeSlot>().UpdateRecipeSlot(CraftingRecipeList.instance.recipes[id]);
        }
        recipeLayout.sizeDelta = new Vector2(buttonWidht, buttonHeight * building.recipeIdList.Count - 6); //-6 to avoid double outline
    }
    public void OpenCraftingMenu(Recipe recipe)
    {
        CraftingMenu.instance.UpdateCraftingMenu(currentBuilding, recipe);
        craftingUIParent.position = craftingUIParentEndPosition;
    }
    private void InitializeCraftingUI()
    {
        player = FindObjectOfType<PlayerController>().transform;
        craftingUIParent = transform.Find("CraftingUIParent");
        recipeLayout = craftingUIParent.Find("ScrollRect").Find("RecipeLayout").GetComponent<RectTransform>();
        recipeSlotPrefab = Resources.Load<GameObject>("UI/RecipeSlot");
        craftingUIParentStartPosition = craftingUIParent.position;
        craftingUIParentEndPosition = craftingUIParentStartPosition + Vector3.left * 270;
    }
}
