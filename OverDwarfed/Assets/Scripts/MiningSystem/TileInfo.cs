using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu]
public class TileInfo : ScriptableObject
{
    public TileBase[] destructionStages = new TileBase[breakStages];
    public const int breakStages = 4;
    public string blockName;

    public int maxHealth;
    public Item[] itemsDropped;
    public int[] itemDropChances;
}
