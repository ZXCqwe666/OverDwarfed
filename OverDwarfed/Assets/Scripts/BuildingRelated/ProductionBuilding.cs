using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ProductionBuilding : MonoBehaviour
{
    public List<int> recipeIdList;
    private int itemsToProduce, currentRecipeId;
    private bool isProducing, isInfinite;

    public void StartProduction(Recipe recipe, bool single, bool half, bool max, bool infinite) 
    {
        if (currentRecipeId != recipe.id) CancelProduction();
        if (half || max || infinite) itemsToProduce = 0;

        currentRecipeId = recipe.id;
        isInfinite = infinite;

        int canBuy = PlayerInventory.instance.CanBuyAmount(recipe);
        if (half) { itemsToProduce = Mathf.CeilToInt(canBuy / 2f); }
        if (single) itemsToProduce += 1;
        if (max) itemsToProduce = canBuy;

        if (isProducing == false)
        {
            isProducing = true;
            StartCoroutine(Produce(recipe));
        }
    }
    public IEnumerator Produce(Recipe recipe)
    {
        while (itemsToProduce > 0 || isInfinite)
        {
            yield return new WaitForSeconds(recipe.creationTime);

            if (PlayerInventory.instance.Buy(recipe)) 
            {
                ItemSpawner.instance.SpawnItem(transform.position + Vector3.down, recipe.resultItem, recipe.resultItemAmount);
                if(isInfinite == false) itemsToProduce -= 1;
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
