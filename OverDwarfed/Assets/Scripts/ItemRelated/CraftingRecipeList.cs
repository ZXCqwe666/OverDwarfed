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
            new Recipe(new List<Cost>(){new Cost(0,1)}, 1, 5f), //ingot
            new Recipe(new List<Cost>(){new Cost(0,5)}, 0, 0f), //furnace
        };
    }
}
public struct Cost
{
    public int itemCostId, itemCostAmount;
    public Cost(int _id, int _amount)
    {
        itemCostId = _id;
        itemCostAmount = _amount;
    }
}
public struct Recipe
{
    public List<Cost> CostList;
    public int resultItemId;
    public float creationTime;
    public Recipe(List<Cost> _CostList, int _resultItemId, float _creationTime)
    {
        CostList = _CostList;
        resultItemId = _resultItemId;
        creationTime = _creationTime;
    }
}
