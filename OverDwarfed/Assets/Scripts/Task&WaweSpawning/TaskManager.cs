using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class TaskManager : MonoBehaviour
{
    private List<Cost> taskCosts, currentCosts;

    private TaskDifficulty currentDifficulty;
    private int tasksElapsed, tasksBurned;
    private const int tasksToDifficulty2 = 2, tasksToDifficulty3 = 4, tasksToDifficulty4 = 6;
    private const float escapeTime = 600f;
    private float timeOnThisStage = 0f;

    private void Start()
    {
        InitializeTaskManager();
        AddTask();
    }
    private void FixedUpdate()
    {
        timeOnThisStage += Time.fixedDeltaTime;
    }
    private void AddTask() 
    {
        taskCosts.Clear();
        currentCosts.Clear();

        List<TaskTemplate> taskBuffer = new List<TaskTemplate>();
        taskBuffer = TaskList.instance.taskList.Where(task => task.difficulty == currentDifficulty).ToList();

        if (taskBuffer.Count == 0) return;
        TaskTemplate taskTemplate = taskBuffer[Random.Range(0, taskBuffer.Count)];

        for(int i = 0; i < taskTemplate.itemsNeeded.Count; i++)
        {
            int2 bounds = taskTemplate.amountBounds[i];
            int randomAmount = Random.Range(bounds.x, bounds.y);
            taskCosts.Add(new Cost(taskTemplate.itemsNeeded[i], randomAmount));
        }
        foreach (Cost cost in taskCosts)
            currentCosts.Add(new Cost(cost.item, 0));

        StopAllCoroutines();
        StartCoroutine(TaskLifecycle());

        TaskMenu.instance.UpdateTaskMenu(taskTemplate);
        TaskMenu.instance.UpdateInfoText(currentCosts, taskCosts);
    }
    private void CompleteTask()
    {
        tasksElapsed += 1;
        CalculateDifficulty();
        AddTask();
    }
    private void BurnTask()
    {
        tasksBurned++;
        AddTask();
        WaveSpawner.instance.StartSpawn(tasksBurned, timeOnThisStage/escapeTime); //Launch wave attacks 
    }
    private void CalculateDifficulty()
    {
        if (tasksElapsed >= tasksToDifficulty2 && tasksElapsed < tasksToDifficulty3) currentDifficulty = TaskDifficulty.dif_2;
        else if (tasksElapsed >= tasksToDifficulty3 && tasksElapsed < tasksToDifficulty4) currentDifficulty = TaskDifficulty.dif_3;
        else if (tasksElapsed >= tasksToDifficulty4) currentDifficulty = TaskDifficulty.dif_4;
    }
    private IEnumerator TaskLifecycle() 
    {
        float lifeTime = CalculateTaskDuration();
        TaskMenu.instance.StartTimer(lifeTime);
        yield return new WaitForSeconds(lifeTime);
        BurnTask();
    }
    private float CalculateTaskDuration()
    {
        float lifeTime = 0f;
        foreach (Cost cost in taskCosts)
            lifeTime += ItemSpawner.instance.items[cost.item].taskWeight * cost.amount;
        lifeTime *= 10 - (int)currentDifficulty;
        lifeTime /= 10;
        lifeTime = Mathf.FloorToInt(lifeTime);
        return lifeTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 9) return;

        ItemSpawned itemSpawned = collision.GetComponent<ItemSpawned>();
        List<Cost> matchingCost = new List<Cost>();
        matchingCost = currentCosts.Where(cost => cost.item == itemSpawned.item).ToList();

        if (matchingCost.Count != 0)
        {
            int costIndex = matchingCost.IndexOf(matchingCost[0]);
            int capacity = taskCosts[costIndex].amount - currentCosts[costIndex].amount;
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
                TaskMenu.instance.UpdateInfoText(currentCosts, taskCosts);
                CheckComplition();
            }
        }
    }
    private void CheckComplition()
    {
        for (int i = 0; i < currentCosts.Count; i++)
        if (currentCosts[i].amount != taskCosts[i].amount) return;
        CompleteTask();
    }
    private void InitializeTaskManager()
    {
        currentDifficulty = TaskDifficulty.dif_1;
        tasksElapsed = 0;
        tasksBurned = 0;
        taskCosts = new List<Cost>();
        currentCosts = new List<Cost>();
        PlayersPositions.instance.TaskDropOffPosition = transform.position;
    }
}
