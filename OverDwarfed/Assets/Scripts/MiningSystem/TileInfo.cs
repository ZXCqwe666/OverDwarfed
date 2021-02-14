using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu]
public class TileInfo : ScriptableObject
{
    public Block block;

    private const int breakStages = 4;
    public TileBase[] destructionStages = new TileBase[breakStages];

    public int maxHealth;
    public Item[] itemsDropped;
    public int[] itemDropChances;
}
public enum Block
{
    empty,
    stone,
    coal,
    ironOre,
    goldOre,
    crystalOre,
}
