using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Sprite itemIcon;
    public RuntimeAnimatorController worldAnimator;
    public float colliderRadius;
    public string itemName;
    public int id; // used to collect dropped item

    public bool destroysAfterTime;
    public float selfDestructTime;

    public bool isPickaxe;
    public bool isHammer;
    public bool isWeapon;
    public float attackInterval;
    public int2 damageRange;
}
