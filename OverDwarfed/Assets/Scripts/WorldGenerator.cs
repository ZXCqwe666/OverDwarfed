using UnityEngine.Tilemaps;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private Tilemap blockTilemap;
    public TileBase stoneTile;

    private void Start()
    {
        InitializeWorldGenerator();
        GenerateMap(100, 100);
    }
    private void InitializeWorldGenerator()
    {
        blockTilemap = transform.Find("blocks").GetComponent<Tilemap>();
    }
    private void GenerateMap(int sizeX, int sizeY)
    {
        for(int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                blockTilemap.SetTile(new Vector3Int(x, y, 0), stoneTile);
            }
        }
        MiningManager.instance.InitializeBlockData(100, 100);
    }
}
