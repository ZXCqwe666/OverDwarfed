using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;

    public void InitializeItem(ItemData _itemData)
    {
        id = _itemData.id;

        if (_itemData.worldAnimator != null)
            GetComponent<Animator>().runtimeAnimatorController = _itemData.worldAnimator;
        else GetComponent<SpriteRenderer>().sprite = _itemData.itemIcon;

        GetComponent<CircleCollider2D>().radius = _itemData.colliderRadius;

        if(_itemData.destroysAfterTime)
        Destroy(gameObject, _itemData.selfDestructTime);
    }
}
