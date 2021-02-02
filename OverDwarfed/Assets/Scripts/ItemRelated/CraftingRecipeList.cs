using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipeList : MonoBehaviour
{
    public static CraftingRecipeList instance;
    public List<Recipe> recipes;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        recipes = new List<Recipe>()
        {
            new Recipe(new List<Cost>(){new Cost(4, 2)}, 1, 1, 5f), //ingot
            new Recipe(new List<Cost>(){new Cost(2, 1)}, 0, 0, 0f), //furnace
            new Recipe(new List<Cost>(){new Cost(2, 2)}, 5, 1, 2.5f), //stoneBlock
        };
    }
}
public struct Cost
{
    public int id, amount;
    public Cost(int _id, int _amount)
    {
        id = _id;
        amount = _amount;
    }
}
public struct Recipe 
{
    public List<Cost> CostList;
    public int resultItemId, resultItemAmount;
    public float creationTime;
    public Recipe(List<Cost> _CostList, int _resultItemId, int _resultsItemAmount, float _creationTime)
    {
        CostList = _CostList;
        resultItemId = _resultItemId;
        resultItemAmount = _resultsItemAmount;
        creationTime = _creationTime;
    }
}
