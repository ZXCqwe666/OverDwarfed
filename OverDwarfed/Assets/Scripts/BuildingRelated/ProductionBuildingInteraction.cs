using UnityEngine.UI;
using UnityEngine;

public class ProductionBuildingInteraction : MonoBehaviour
{
    private const float interactDistance = 3f;

    private ProductionBuilding productionBuilding;
    private Transform player, buttonCanvas;
    private Button button;
    private bool isButtonActive;

    void Start()
    {
        InitializeProductionBuildingInteraction();
    }
    private void OnMouseOver()
    {
        ChangeButtonState(Vector3.Distance(transform.position, player.position) < interactDistance);
    }
    private void OnMouseExit()
    {
        ChangeButtonState(false);
    }
    private void ChangeButtonState(bool isActive)
    {
        if (isButtonActive != isActive)
        {
            buttonCanvas.gameObject.SetActive(isActive);
            isButtonActive = isActive;
        }
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
