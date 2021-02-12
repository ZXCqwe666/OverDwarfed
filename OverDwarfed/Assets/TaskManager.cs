using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class TaskManager : MonoBehaviour
{   
    private List<Cost> currentCosts;
    private Task currentTask;

    private TaskDifficulty currentDifficulty;
    private int tasksElapsed;
    private const int tasksToDifficulty2 = 2, tasksToDifficulty3 = 4, tasksToDifficulty4 = 6;

    void Start()
    {
        InitializeTaskManager();
        AddTask();
    }
    private void AddTask() 
    {
        List<Task> taskBuffer = new List<Task>();
        taskBuffer = TaskList.instance.taskList.Where(task => task.difficulty == currentDifficulty).ToList();
        if (taskBuffer.Count == 0) return;
        currentTask = taskBuffer[Random.Range(0, taskBuffer.Count)];

        foreach (Cost cost in currentTask.costList)
            currentCosts.Add(new Cost(cost.item, 0));

        tasksElapsed += 1;
        StopAllCoroutines();
        StartCoroutine(TaskLifecycle());
    }
    private void CompleteTask()
    {
        CalculateDifficulty();
        AddTask();
        Debug.Log("Task Completed");
    }
    private void BurnTask()
    {
        //Launch wave attacks 
        AddTask(); 
    }
    private void CalculateDifficulty()
    {
        if (tasksElapsed >= tasksToDifficulty2 && tasksElapsed < tasksToDifficulty3) currentDifficulty = TaskDifficulty.difficulty_2;
        else if (tasksElapsed >= tasksToDifficulty3 && tasksElapsed < tasksToDifficulty4) currentDifficulty = TaskDifficulty.difficulty_3;
        else if (tasksElapsed >= tasksToDifficulty4) currentDifficulty = TaskDifficulty.difficulty_4;
    }
    private IEnumerator TaskLifecycle() 
    {
        yield return new WaitForSeconds(currentTask.lifeTime);
        BurnTask();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (collision.gameObject.layer != 9) return;

        ItemSpawned itemSpawned = collision.GetComponent<ItemSpawned>();
        List<Cost> matchingCost = new List<Cost>();
        matchingCost = currentCosts.Where(cost => cost.item == itemSpawned.item).ToList();

        if (matchingCost.Count != 0)
        {
            int costIndex = matchingCost.IndexOf(matchingCost[0]);
            int capacity = currentTask.costList[costIndex].amount - currentCosts[costIndex].amount;
            if (capacity > 0)
            {
                if (capacity >= itemSpawned.amount)
                {
                    currentCosts[costIndex] = new Cost(currentCosts[costIndex].item, currentCosts[costIndex].amount + itemSpawned.amount);
                    Destroy(collision.gameObject);
                }
                else
                {
                    currentCosts[costIndex] = new Cost(currentCosts[costIndex].item, currentCosts[costIndex].amount + capacity);
                    itemSpawned.amount -= capacity;
                    itemSpawned.UpdateAmountDisplay();
                }
                CheckComplition();
            }
        }
    }
    private void CheckComplition()
    {
        for (int i = 0; i < currentCosts.Count; i++)
        if (currentCosts[i].amount != currentTask.costList[i].amount) return;
        CompleteTask();
    }
    private void InitializeTaskManager()
    {
        currentDifficulty = TaskDifficulty.difficulty_1;
        tasksElapsed = -1;
        currentCosts = new List<Cost>();
    }
}
