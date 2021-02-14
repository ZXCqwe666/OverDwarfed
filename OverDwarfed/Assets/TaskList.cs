using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    public static TaskList instance;
    public List<TaskTemplate> taskList;

    private void Awake()
    {
        instance = this;
        taskList = new List<TaskTemplate>()
        {
            new TaskTemplate(TaskDifficulty.dif_1,
            new List<Item>() { Item.stone_block }, 
            new List<int2>{ new int2(20, 30)}),

            new TaskTemplate(TaskDifficulty.dif_2,
            new List<Item>() { Item.crystal_ore },
            new List<int2>{ new int2(30, 40)}),

            new TaskTemplate(TaskDifficulty.dif_3,
            new List<Item>() { Item.gold_coin },
            new List<int2>{ new int2(400, 500)}),

            new TaskTemplate(TaskDifficulty.dif_4,
            new List<Item>() { Item.forbidden_crown },
            new List<int2>{ new int2(4, 8)}),
        };
    }
}

public struct Task
{
    public float lifeTime;
    public TaskDifficulty difficulty;
    public List<Cost> costList;

    public Task(float _lifetime, TaskDifficulty _difficulty, List<Cost> _costList)
    {
        lifeTime = _lifetime;
        difficulty = _difficulty;
        costList = _costList;
    }
}
public struct TaskTemplate
{
    public TaskDifficulty difficulty;
    public List<Item> itemsNeeded;
    public List<int2> amountBounds;

    public TaskTemplate(TaskDifficulty _difficulty, List<Item> _itemsNeeded, List<int2> _amountBounds)
    {
        difficulty = _difficulty;
        itemsNeeded = _itemsNeeded;
        amountBounds = _amountBounds;
    }
}
public enum TaskDifficulty
{
    dif_1,
    dif_2,
    dif_3,
    dif_4,
}