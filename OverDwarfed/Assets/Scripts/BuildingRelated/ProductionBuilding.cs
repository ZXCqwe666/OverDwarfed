using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{
    private List<int> recipeIdList;
    private int itemsToProduce, currentRecipeId;
    private bool isProducing, isInfinite;

    private void Start()
    {
        StartProduction(0, 1, true);
    }
    public void StartProduction(int recipeId, int amount, bool _isInfinite) 
    {
        if (!recipeIdList.Contains(recipeId))
            Debug.LogError("Wrong Production call");

        if (currentRecipeId != recipeId)
            CancelProduction();

        itemsToProduce += amount;
        currentRecipeId = recipeId;
        isInfinite = _isInfinite;

        if (isProducing == false)
        {
            isProducing = true;
            StartCoroutine(Produce(CraftingRecipeList.instance.recipes[recipeId]));
        }
    }
    public IEnumerator Produce(Recipe recipe)
    {
        while ((itemsToProduce > 0 || isInfinite))
        {
            yield return new WaitForSeconds(recipe.creationTime);

            if (PlayerInventory.instance.Buy(recipe)) 
            {
                ItemSpawner.instance.SpawnItem(transform.position + Vector3.down, recipe.resultItemId, 1);
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
    public void InitializeProductionBuilding(int[] ids)
    {
        recipeIdList = new List<int>();
        foreach (int id in ids)
            recipeIdList.Add(id);
    }
}
