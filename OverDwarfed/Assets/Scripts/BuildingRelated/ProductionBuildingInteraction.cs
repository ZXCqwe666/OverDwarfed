using UnityEngine.UI;
using UnityEngine;

public class ProductionBuildingInteraction : MonoBehaviour
{
    private ProductionBuilding productionBuilding;
    private Transform buttonCanvas, player;
    private Button button;
    private bool isCanvasShown = false;

    void Start()
    {
        InitializeProductionBuildingInteraction();
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && isCanvasShown)
            CraftingUI.instance.ChangeOpenState(productionBuilding);
    }
    private void OnMouseOver()
    {
        ChangeButtonState(Vector3.Distance(transform.position, player.position) < 3f);
    }
    private void OnMouseExit()
    {
        ChangeButtonState(false);
    }
    private void ChangeButtonState(bool isActive)
    {
        if (isCanvasShown != isActive)
        {
            buttonCanvas.gameObject.SetActive(isActive);
            isCanvasShown = !isCanvasShown;
        }
    }
    private void InitializeProductionBuildingInteraction()
    {
        buttonCanvas = transform.Find("ButtonCanvas");
        button = buttonCanvas.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(() => { CraftingUI.instance.ChangeOpenState(productionBuilding); });
        player = FindObjectOfType<PlayerController>().transform;
        productionBuilding = GetComponent<ProductionBuilding>();
    }
}
