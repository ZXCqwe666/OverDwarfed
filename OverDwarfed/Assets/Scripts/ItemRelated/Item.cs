using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item : MonoBehaviour
{
    private const int itemLayerIndex = 9;
    public LayerMask itemLayer;
    public SpriteRenderer amountSprite;
    public int id, amount;

    public void InitializeItem(ItemData _itemData, int _amount)
    {
        id = _itemData.id;
        amount = _amount;

        if (_itemData.worldAnimator != null)
            GetComponent<Animator>().runtimeAnimatorController = _itemData.worldAnimator;
        else GetComponent<SpriteRenderer>().sprite = _itemData.itemIcon;

        GetComponent<CircleCollider2D>().radius = _itemData.colliderRadius;

        if(_itemData.destroysAfterTime)
        Destroy(gameObject, _itemData.selfDestructTime);

        Merge();
    }
    private void Merge()
    {
        int stackSize = ItemSpawner.instance.items[id].stackSize;
        if (amount != stackSize)
        {
            Collider2D[] itemsAroundColliders = Physics2D.OverlapCircleAll(transform.position, 0.75f, itemLayer);
            List<Item> itemsAround = new List<Item>();
            List<float> amounts = new List<float>();

            foreach (Collider2D coll in itemsAroundColliders)
            {
                Item item = coll.GetComponent<Item>();
                if (item.id == id)
                {
                    itemsAround.Add(item);
                    amounts.Add(item.amount);
                }
            }
            if (amounts.Count > 0)
            {
                List<float> amountsLowerOrEqual = amounts.Where(am => am <= amount).ToList();

                while (amount < stackSize && amountsLowerOrEqual.Count > 0) //collecting
                {
                    int index = amounts.IndexOf(amountsLowerOrEqual.Min());
                    int indexInLOE = amountsLowerOrEqual.IndexOf(amountsLowerOrEqual.Min());
                    int canTake = stackSize - amount;

                    if(itemsAround[index].amount > canTake)
                    {
                        amount += canTake;
                        itemsAround[index].amount -= canTake;
                        itemsAround[index].UpdateAmountDisplay();
                        amounts[index] -= canTake;
                    }
                    else
                    {
                        amount += itemsAround[index].amount;
                        Destroy(itemsAround[index].gameObject);
                        amounts[index] = 0;
                    }
                    amountsLowerOrEqual.RemoveAt(indexInLOE);
                }

                List<float> amountsHigher = amounts.Where(am => am > amount && am < stackSize).ToList();

                while (amount > 0 && amountsHigher.Count > 0) //distributing
                {
                    int index = amounts.IndexOf(amountsHigher.Max());
                    int indexInHigher = amountsHigher.IndexOf(amountsHigher.Max());
                    int fitAmount = stackSize - itemsAround[index].amount;

                    if (amount > fitAmount)
                    {
                        itemsAround[index].amount = stackSize;
                        amount -= fitAmount;
                        amounts[index] = stackSize;
                    }
                    else
                    {
                        itemsAround[index].amount += amount;
                        amounts[index] += amount;
                        amount = 0;
                    }
                    itemsAround[index].UpdateAmountDisplay();
                    amountsHigher.RemoveAt(indexInHigher);
                }
                if (amount == 0) // полностью истощен
                {
                    Destroy(gameObject, Time.deltaTime);
                    return;
                }
            }
        }
        gameObject.layer = itemLayerIndex;
        UpdateAmountDisplay();
    }
    public void UpdateAmountDisplay()
    {
        if (amount > 1)
            amountSprite.sprite = NumbersSprites.instance.numbers[amount - 1];
        else amountSprite.sprite = null;
        transform.localScale = Vector3.one * (1f + 0.025f * Mathf.Clamp(amount, 0f, 10f) - 0.025f);
    }
}
