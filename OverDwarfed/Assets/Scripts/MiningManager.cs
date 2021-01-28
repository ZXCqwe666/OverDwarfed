using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using UnityEngine;

public class MiningManager : MonoBehaviour
{
    public static MiningManager instance;

    private Tilemap blockTilemap;
    private static List<TileInfo> tileTypes;
    private BlockHp[,] blockData;
    private const int breakStages = 4;

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
        tileTypes = new List<TileInfo>();

        for (int i = 0; i < 100; i++)
        {
            TileInfo loadedBlock = Resources.Load<TileInfo>("Blocks/" + i.ToString());
            if (loadedBlock != null)
                tileTypes.Add(loadedBlock);
            else break;
        }
    }
    public void Mine(Vector3 position, int damage)
    {
        Vector3Int tilePosition = blockTilemap.WorldToCell(position);
        int2 index = new int2(tilePosition.x, tilePosition.y);

        blockData[index.x, index.y].health -= damage;

        if(blockData[index.x, index.y].health <= 0)
        {
            blockTilemap.SetTile(tilePosition, null); // SPAWN ITEMS ON DROP 
            ItemSpawner.instance.SpawnLootTable(tilePosition + new Vector3(0.5f, 0.5f, 0f), 
                tileTypes[blockData[index.x, index.y].tileTypeId].itemDropIds, tileTypes[blockData[index.x, index.y].tileTypeId].itemDropChances); 
        }
        else
        {
            int breakIndex = Mathf.CeilToInt(blockData[index.x, index.y].health / (float)tileTypes[blockData[index.x, index.y].tileTypeId].maxHealth * breakStages);
            blockTilemap.SetTile(tilePosition, tileTypes[blockData[index.x, index.y].tileTypeId].destructionStages[breakIndex - 1]);
        }
    }
    public void InitializeBlockData(int sizeX, int sizeY)
    {
        blockData = new BlockHp[sizeX, sizeY];
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                if(blockTilemap.GetTile(new Vector3Int(x, y, 0)) != null) // remove != null later
                {
                    blockData[x, y] = new BlockHp(0);
                }
            }
        }
    }
    internal struct BlockHp
    {
        public int health, tileTypeId;
        public BlockHp(int _tileTypeId)
        {
            health = tileTypes[_tileTypeId].maxHealth;
            tileTypeId = _tileTypeId;
        }
    }
}
