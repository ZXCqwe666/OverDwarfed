using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite itemIcon;
    public RuntimeAnimatorController worldAnimator;
    public float colliderRadius;
    public string itemName;
    public int id;
}
