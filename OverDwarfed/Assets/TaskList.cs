using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    public static TaskList instance;
    public List<Task> taskList;

    private void Awake()
    {
        instance = this;
        taskList = new List<Task>()
        {
            new Task(100f, TaskDifficulty.difficulty_1, new List<Cost>(){new Cost(Item.log, 8)}), //8 log test task
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
public enum TaskDifficulty
{
    difficulty_1,
    difficulty_2,
    difficulty_3,
    difficulty_4,
}