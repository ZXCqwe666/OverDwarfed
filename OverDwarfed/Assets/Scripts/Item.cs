using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;

    public void InitializeItem(ItemData _itemData)
    {
        id = _itemData.id;
        GetComponent<Animator>().runtimeAnimatorController = _itemData.worldAnimator;
        GetComponent<CircleCollider2D>().radius = _itemData.colliderRadius;
    }
}
