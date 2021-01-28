using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{
    private List<Recipe> recipeList = new List<Recipe>() { new Recipe(new List<Cost>() { new Cost(0, 1) }, 1, 5) };
    private int itemsToProduce, currentRecipeId;
    private bool isProducing, isInfinite;

    private void Start()
    {
        StartProduction(0, 0, true);
    }
    public void StartProduction(int recipeId, int amount, bool _isInfinite) // This method is perfect
    {
        if(currentRecipeId != recipeId)
            CancelProduction();

        itemsToProduce += amount;
        currentRecipeId = recipeId;
        isInfinite = _isInfinite;

        if (isProducing == false)
        {
            isProducing = true;
            StartCoroutine(Produce(recipeList[recipeId]));
        }
    }
    public IEnumerator Produce(Recipe recipe)
    {
        while ((itemsToProduce > 0 || isInfinite)) // removed canBuy to allow for waiting for res to come
        {
            yield return new WaitForSeconds(recipe.creationTime);

            if (PlayerInventory.instance.Buy(recipe)) // if you can buy item now spawnIt  else wait for res another time;
            {
                ItemSpawner.instance.SpawnItem(transform.position + Vector3.down, recipe.resultItemId);
                if(isInfinite == false) itemsToProduce--;
            }
        }
        CancelProduction();
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
