using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class MiningManager : MonoBehaviour
{
    public static MiningManager instance;

    private static List<TileInfo> tileTypes;
    private Tilemap blockTilemap;
    private BlockHp[,] blockData;
    private int2 blockDataArraySize;
    private const int breakStages = 4;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitializeMiningManager();
    }
    public void Mine(Vector3 position, int damage)
    {
        Vector3Int tilePosition = blockTilemap.WorldToCell(position);
        tilePosition = new Vector3Int(Mathf.Clamp(tilePosition.x, 0, blockDataArraySize.x), Mathf.Clamp(tilePosition.y, 0, blockDataArraySize.y), 0);
        int2 index = new int2(tilePosition.x, tilePosition.y);

        if(blockData[index.x, index.y].isEmpty == false)
        {
            blockData[index.x, index.y].health -= damage;
            if (blockData[index.x, index.y].health <= 0)
            {
                if(blockTilemap.GetTile(tilePosition) != null)
                {
                    ItemSpawner.instance.SpawnLootTable(tilePosition + new Vector3(0.5f, 0.5f, 0f),
                    tileTypes[blockData[index.x, index.y].tileTypeId].itemDropIds, tileTypes[blockData[index.x, index.y].tileTypeId].itemDropChances);
                    blockTilemap.SetTile(tilePosition, null);
                }
            }
            else
            {
                int breakIndex = Mathf.CeilToInt(blockData[index.x, index.y].health / (float)tileTypes[blockData[index.x, index.y].tileTypeId].maxHealth * breakStages);
                blockTilemap.SetTile(tilePosition, tileTypes[blockData[index.x, index.y].tileTypeId].destructionStages[breakIndex - 1]);
            }
        }
    }
    public void InitializeBlockData(int sizeX, int sizeY)
    {
        blockData = new BlockHp[sizeX, sizeY];
        blockDataArraySize = new int2(sizeX - 1, sizeY - 1);

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                TileBase sampledTile = blockTilemap.GetTile(new Vector3Int(x, y, 0));
                if (sampledTile != null)
                {
                    List<TileInfo> matchingName = tileTypes.Where(tile => tile.blockName == sampledTile.name).ToList();
                    if(matchingName.Count > 0)
                    blockData[x, y] = new BlockHp(tileTypes.IndexOf(matchingName[0]));
                    else blockData[x, y] = new BlockHp(-1);
                }
                else blockData[x, y] = new BlockHp(-1); // -1 means EmptyTime
            }
        }
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
    private struct BlockHp
    {
        public int health, tileTypeId;
        public bool isEmpty;
        public BlockHp(int _tileTypeId)
        {
            isEmpty = (_tileTypeId < 0) ? true : false;
            health = (isEmpty) ? 0 : tileTypes[_tileTypeId].maxHealth;
            tileTypeId = _tileTypeId;
        }
    }
}
