using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public Item item; // used to collect dropped item
    public string itemName;
    public Sprite itemSprite, itemIcon;
    public int stackSize;
    public string description;

    public bool isPickaxe;
    public bool isHammer;
    public bool isWeapon;
    public float attackInterval;
    public int2 damageRange;
}
