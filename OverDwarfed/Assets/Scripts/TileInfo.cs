using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu]
public class TileInfo : ScriptableObject
{
    public TileBase[] destructionStages = new TileBase[breakStages];
    public const int breakStages = 4;

    public int maxHealth;
    public ItemData[] dropPool;
    public int[] dropChances;
}
