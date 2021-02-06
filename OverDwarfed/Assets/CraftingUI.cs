using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI instance;
    private const float interactDistance = 3f;

    private Transform craftingUIParent, recipeLayout, player;
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
        currentBuilding = building;

        UpdateRecipeSlots(currentBuilding);
        buildingPosition = building.gameObject.transform.position;
        craftingUIParent.gameObject.SetActive(true);
        isOpen = true;
    }
    public void DisableCraftingUI()
    {
        craftingUIParent.gameObject.SetActive(false);
        CraftingMenu.instance.SetActive(false);
        craftingUIParent.position = craftingUIParentStartPosition;
        isOpen = false;
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
        recipeLayout = craftingUIParent.Find("ScrollRect").Find("RecipeLayout");
        recipeSlotPrefab = Resources.Load<GameObject>("UI/RecipeSlot");
        craftingUIParentStartPosition = craftingUIParent.position;
        craftingUIParentEndPosition = craftingUIParentStartPosition + Vector3.left * 270;
    }
}
