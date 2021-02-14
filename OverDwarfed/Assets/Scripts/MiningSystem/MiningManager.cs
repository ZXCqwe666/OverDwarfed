using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using UnityEngine;
using PathFinder;

public class MiningManager : MonoBehaviour
{
    public static MiningManager instance;
    public static Dictionary<Block, TileInfo> blocks;

    private const int breakStages = 4;
    private Tilemap blockTilemap;
    private BlockHp[,] blockData;
    private int2 mapSize;

    public void Mine(Vector3 position, int damage)
    {
        Vector3Int pos = blockTilemap.WorldToCell(position);
        pos = new Vector3Int(Mathf.Clamp(pos.x, 0, mapSize.x), Mathf.Clamp(pos.y, 0, mapSize.y), 0);

        if (blockData[pos.x, pos.y].block != Block.empty)
        {
            blockData[pos.x, pos.y].health -= damage;
            BlockHp tile = blockData[pos.x, pos.y];

            if (tile.health <= 0)
            {
                blockTilemap.SetTile(pos, null);
                SetBlock(Block.empty, pos.x, pos.y);
                Pathfinding.pathGrid.ChangeNode(new int2(pos.x, pos.y), true);
                ItemSpawner.instance.SpawnLootTable(pos + Utility.halfVector,
                blocks[tile.block].itemsDropped, blocks[tile.block].itemDropChances);
            }
            else
            {
                int breakIndex = Mathf.CeilToInt(tile.health / (float)blocks[tile.block].maxHealth * breakStages);
                blockTilemap.SetTile(pos, blocks[tile.block].destructionStages[breakIndex - 1]);
            }
        }
    }
    public void InitializeBlockData(int2 _mapSize)
    {
        mapSize = _mapSize;
        blockData = new BlockHp[_mapSize.x, _mapSize.y];
    }
    public void SetBlock(Block block, int x, int y)
    {
        if(block != Block.empty)
        blockTilemap.SetTile(new Vector3Int(x, y, 0), blocks[block].destructionStages[3]);
        blockData[x, y] = new BlockHp(block);
    }
    private struct BlockHp
    {
        public Block block;
        public int health;
        public BlockHp(Block _block)
        {
            health = (_block == Block.empty) ? 0 : blocks[_block].maxHealth;
            block = _block;
        }
    }
    #region Initialization
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeMiningManager();
    }
    private void InitializeMiningManager()
    {
        blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
        blocks = new Dictionary<Block, TileInfo>();

        for (int i = 0; i < 100; i++)
        {
            TileInfo loadedBlock = Resources.Load<TileInfo>("Blocks/" + i.ToString());
            if (loadedBlock != null)
                blocks.Add((Block)(i + 1), loadedBlock);
            else break;
        }
    }
    #endregion
}
