using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    private Camera mainCam;
    private GameObject buildingPrefab;
    private Transform buildingBlueprint;
    private SpriteRenderer blueprintRenderer;

    private List<BuildingData> buildings;
    private BuildingData selectedData;
    private Vector2 cellPosition, previousCellPosition, cellOffset;
    private bool blueprintActive, canPlaceBuilding;

    public LayerMask blockerLayer;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeBuildingSystem();
    }
    private void Update()
    {
        if (blueprintActive) UpdateBlueprintPosition();
        if (Input.GetMouseButtonDown(0)) SpawnBuilding(cellPosition);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) SetSpriteAndActivity(null, false);
    }
    private void SpawnBuilding(Vector2 position)
    {
        if (blueprintActive && canPlaceBuilding)
        {
            if (PlayerInventory.instance.Buy(CraftingRecipeList.instance.recipes[selectedData.recipeId])) // add call to static list from Recipe collection;
            {
                GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity, transform);
                building.GetComponent<SpriteRenderer>().sprite = selectedData.buildingSprite;

                BoxCollider2D collider = building.GetComponent<BoxCollider2D>();
                collider.size = selectedData.colliderSize;
                collider.offset = selectedData.colliderOffset;

                if (selectedData.isProductionBuilding)
                {
                    building.AddComponent<ProductionBuilding>();
                    building.GetComponent<ProductionBuilding>().InitializeProductionBuilding(selectedData.recipeIds);
                } 

                if (Input.GetKey(KeyCode.LeftShift) == false) // TEMPORARY CONTROLS
                    SetSpriteAndActivity(null, false);
            }
            else SetSpriteAndActivity(null, false);
        }
    }
    private void UpdateBlueprintPosition()
    {
        Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        cellPosition = new Vector2(Mathf.FloorToInt(mousePosition.x / 1f), Mathf.FloorToInt(mousePosition.y / 1f));
        cellPosition += cellOffset;
        buildingBlueprint.position = cellPosition;

        if (cellPosition != previousCellPosition)
            CheckPlacing();
        previousCellPosition = cellPosition;
    }
    private void CheckPlacing()
    {
        canPlaceBuilding = !Physics2D.OverlapArea(cellPosition - cellOffset + Vector2.one * 0.1f, cellPosition + cellOffset - Vector2.one * 0.1f, blockerLayer);
        blueprintRenderer.color = canPlaceBuilding ? new Color(0f, 0.5f, 0f) : new Color(0.5f, 0f, 0f);
    }
    public void UpdateBlueprint(int index)
    {
        selectedData = buildings[index];
        if (PlayerInventory.instance.CanBuy(CraftingRecipeList.instance.recipes[selectedData.recipeId]))
        {
            cellOffset = new Vector2(selectedData.size.x / 2, selectedData.size.y / 2);
            SetSpriteAndActivity(selectedData.buildingSprite, true);
        }
    }
    public void SetSpriteAndActivity(Sprite blueprintSprite, bool isActive)
    {
        blueprintRenderer.sprite = blueprintSprite;
        blueprintActive = isActive;
    }
    private void InitializeBuildingSystem()
    {
        mainCam = Camera.main;
        buildingPrefab = Resources.Load<GameObject>("Buildings/building");
        buildingBlueprint = transform.Find("buildingBlueprint");
        blueprintRenderer = buildingBlueprint.GetComponent<SpriteRenderer>();

        buildings = new List<BuildingData>();
        for (int i = 0; i < 100; i++)
        {
            BuildingData loadedBuilding = Resources.Load<BuildingData>("Buildings/" + i.ToString());
            if (loadedBuilding != null)
                buildings.Add(loadedBuilding);
            else break;
        }
    }
}
