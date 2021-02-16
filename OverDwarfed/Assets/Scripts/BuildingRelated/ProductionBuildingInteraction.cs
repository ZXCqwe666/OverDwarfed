using UnityEngine.UI;
using UnityEngine;

public class ProductionBuildingInteraction : MonoBehaviour
{
    private const float interactDistance = 3f;
    private const KeyCode interactKey = KeyCode.E;

    private ProductionBuilding productionBuilding;
    private Transform player, buttonCanvas;
    public Button interactButton, cancelButton;

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
        interactButton.gameObject.SetActive(isActive);
        if (productionBuilding.isProducing)
        {
            cancelButton.gameObject.SetActive(isActive);
            interactButton.transform.position = transform.position + new Vector3(-0.3f, -0.25f, 0f);
        }
        else
        {
            cancelButton.gameObject.SetActive(false);
            interactButton.transform.position = transform.position + new Vector3(0, -0.25f, 0f);
        }
    }
    private void InitializeProductionBuildingInteraction()
    {
        productionBuilding = GetComponent<ProductionBuilding>();
        player = FindObjectOfType<PlayerController>().transform;
        buttonCanvas = transform.Find("ButtonCanvas");
        interactButton = buttonCanvas.Find("InteractButton").GetComponent<Button>();
        cancelButton = buttonCanvas.Find("CancelButton").GetComponent<Button>();
        interactButton.onClick.AddListener(() => { CraftingUI.instance.EnableCraftingUI(productionBuilding); });
        cancelButton.onClick.AddListener(() => { productionBuilding.CancelProduction(); });
    }
}
