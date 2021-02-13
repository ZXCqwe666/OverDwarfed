using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskMenu : MonoBehaviour
{
    public static TaskMenu instance;
    private List<Image> costImages;
    private List<Text> resNeededText;
    private Text timer;
    private float time;
    private bool running;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeTaskMenu();
    }
    private void Update()
    {
        if (running)
        {
            time -= Time.deltaTime;
            if (time < 0) 
            StopTimer();
            FormatTime();
        } 
    }
    #region TaskUpdating
    public void UpdateTaskMenu(Task currentTask)
    {
        foreach (Image image in costImages) image.color = Color.clear;
        for (int i = 0; i < currentTask.costList.Count; i++)
        {
            Item item = currentTask.costList[i].item;
            costImages[i].color = Color.white;
            costImages[i].sprite = ItemSpawner.instance.items[item].itemIcon;
        }
    }
    public void UpdateInfoText(List<Cost> currentCosts, Task currentTask)
    {
        foreach (Text text in resNeededText) text.text = "";
        for (int i = 0; i < currentTask.costList.Count; i++)
        resNeededText[i].text = currentCosts[i].amount.ToString() + " / " + currentTask.costList[i].amount.ToString();
    }
    #endregion
    #region TimerLogic
    public void StartTimer(float _time)
    {
        time = _time;
        running = true;
    }
    public void StopTimer()
    {
        running = false;
        time = 0f;
    }
    private void FormatTime()
    {
        string formattedTime;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time) - minutes * 60;
        formattedTime = minutes + ":" + (seconds / 10).ToString() + (seconds % 10).ToString();
        timer.text = formattedTime;
    }
    #endregion
    #region Initialization
    private void InitializeTaskMenu()
    {
        Transform taskList = transform.Find("TaskList");
        costImages = new List<Image>(); resNeededText = new List<Text>();
        timer = transform.Find("Timer").GetComponent<Text>();

        for (int i = 0; i < taskList.childCount; i++)
        {
            Transform cost = taskList.GetChild(i);
            costImages.Add(cost.transform.Find("CostImage").GetComponent<Image>());
            resNeededText.Add(cost.transform.Find("ResNeededText").GetComponent<Text>());
        }
    }
    #endregion
}
