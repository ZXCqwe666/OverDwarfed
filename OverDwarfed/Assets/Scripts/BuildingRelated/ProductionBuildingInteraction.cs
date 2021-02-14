using UnityEngine.UI;
using UnityEngine;

public class ProductionBuildingInteraction : MonoBehaviour
{
    private const float interactDistance = 3f;
    private const KeyCode interactKey = KeyCode.E;

    private ProductionBuilding productionBuilding;
    private Transform player, buttonCanvas;
    private Button button;

    void Start()
    {
        InitializeProductionBuildingInteraction();
    }
    private void OnMouseOver()
    {
        bool isCloseEnough = Vector3.Distance(transform.position, player.position) <= interactDistance;
        ChangeButtonState(isCloseEnough);

        if (isCloseEnough && Input.GetKeyDown(interactKey))
            CraftingUI.instance.EnableCraftingUI(productionBuilding);
    }
    private void OnMouseExit()
    {
        ChangeButtonState(false);
    }
    private void ChangeButtonState(bool isActive)
    {
        buttonCanvas.gameObject.SetActive(isActive);
    }
    private void InitializeProductionBuildingInteraction()
    {
        productionBuilding = GetComponent<ProductionBuilding>();
        player = FindObjectOfType<PlayerController>().transform;
        buttonCanvas = transform.Find("ButtonCanvas");
        button = buttonCanvas.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(() => { CraftingUI.instance.EnableCraftingUI(productionBuilding); });
    }
}
