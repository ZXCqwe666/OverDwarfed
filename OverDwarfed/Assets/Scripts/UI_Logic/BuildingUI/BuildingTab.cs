using UnityEngine.UI;
using UnityEngine;

public class BuildingTab : MonoBehaviour
{
    public GameObject[] tabs;
    private Button openTabsButton;
    private bool isOpen;

    private void Start()
    {
        openTabsButton = GetComponent<Button>();
        openTabsButton.onClick.AddListener(SwitchTabsState);
    }

    private void SwitchTabsState()
    {
        isOpen = !isOpen;
        foreach (GameObject tab in tabs)
            tab.gameObject.SetActive(isOpen);

        if(isOpen == false)
            BuildingUI.instance.CloseBuildingMenu();

        int change = isOpen ? tabs.Length : -tabs.Length;
        BuildingUI.instance.ChangeLayoutSize(change);
    }
}
