using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnaceTest : MonoBehaviour
{
    public int[] ids;
    public int[] itemCost;

    private Recipe goldenIngot;
    private Cost goldenIngotCost;
    private List<Cost> costList;
    private void Start()
    {
        goldenIngotCost = new Cost(0, 1);
        costList = new List<Cost>();
        costList.Add(goldenIngotCost);

        goldenIngot = new Recipe(costList, 1, 5);
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(Produce(goldenIngot));
        }
    }
    public IEnumerator Produce(Recipe recipe)
    {
        while(PlayerInventory.instance.CanBuy(goldenIngot) )
        {
            yield return new WaitForSeconds(recipe.creationTime);
            PlayerInventory.instance.Buy(recipe);
            ItemSpawner.instance.SpawnItem(transform.position + Vector3.down, recipe.resultItemId);
        }
    }
}
public struct Cost
{
    public int itemCostId;
    public int itemCostAmount;
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
    public Recipe (List<Cost> _CostList, int _resultItemId, float _creationTime)
    {
            CostList = _CostList;
            resultItemId = _resultItemId;
            creationTime = _creationTime;
    }
}
