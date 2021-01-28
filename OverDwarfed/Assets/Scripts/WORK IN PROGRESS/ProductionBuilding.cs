using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using NUnit.Framework;

public class ProductionBuilding : MonoBehaviour
{
    public int[] ids, itemCost;
    private Recipe goldIngotRecipe = new Recipe(new List<Cost>() { new Cost(0, 1) }, 1, 5);

    private int itemsToProduce = 0;
    private bool isProducing;
    private int currentRecipeId;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Produce(goldIngotRecipe, false));
        }    
    }
    public void StartProduction(int recipeId, Recipe recipe, int amount, bool isInfinite)
    {
        if(isProducing == false && currentRecipeId != recipeId)
            CancelProduction();

        currentRecipeId = recipeId;
        itemsToProduce += amount;

        if (isProducing == false)
            StartCoroutine(Produce(goldIngotRecipe, false));
    }
    public IEnumerator Produce(Recipe recipe, bool isInfinite)
    {
        while (itemsToProduce > 0 || isInfinite && PlayerInventory.instance.CanBuy(recipe) )
        {
            yield return new WaitForSeconds(recipe.creationTime);

            if (PlayerInventory.instance.Buy(recipe))
            {
                itemsToProduce--;
                ItemSpawner.instance.SpawnItem(transform.position + Vector3.down, recipe.resultItemId);
            }
            else CancelProduction();
        }
    }
    private void CancelProduction()
    {
        isProducing = false;
        itemsToProduce = 0;
        StopAllCoroutines();
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
