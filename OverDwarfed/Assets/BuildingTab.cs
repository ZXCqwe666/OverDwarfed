using UnityEngine.UI;
using UnityEngine;

public class BuildingTab : MonoBehaviour
{
    public GameObject[] tabs;
    private Button openTabsButton;
    private int unlockedTabs;
    private bool isOpen;

    private void Start()
    {
        openTabsButton = GetComponent<Button>();
        openTabsButton.onClick.AddListener(openTabs);
    }

    private void openTabs()
    {
        isOpen = !isOpen;
        foreach (GameObject tab in tabs)
            tab.gameObject.SetActive(isOpen);
    }
    private void InitializeBuildingTab()
    {
        foreach (GameObject tab in tabs)
            tab.gameObject.SetActive(false);
    }
}
